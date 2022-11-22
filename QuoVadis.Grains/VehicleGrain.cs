using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.Transactions.Abstractions;
using QuoVadis.Common;
using QuoVadis.Common.Exceptions;
using QuoVadis.Common.Extensions;
using QuoVadis.Common.ValueObjects;
using QuoVadis.Contracts;
using QuoVadis.Contracts.Events;
using QuoVadis.GrainInterfaces;
using QuoVadis.Grains.State;

namespace QuoVadis.Grains
{
    // Reentrancy is required for grains that support ACID transactions
    [Reentrant]
    public class VehicleGrain : IVehicleGrain, IGrainBase
    {
        private const int DefaultKilometersDebitLock = 5;

        private readonly IGrainFactory grainFactory;
        private readonly IPersistentState<VehicleState> vehicleState;
        private readonly ITransactionalState<VehicleRentState> rentState;

        public IGrainContext GrainContext { get; }

        public VehicleGrain(
            IGrainContext grainContext, 
            IGrainFactory grainFactory,
            [PersistentState(Constants.VehicleStateName, Constants.RegularStorage)] IPersistentState<VehicleState> vehicleState,
            [TransactionalState(Constants.VehicleRentStateName, Constants.TransactionalStorage)] ITransactionalState<VehicleRentState> rentState)
        {
            GrainContext = grainContext;

            this.grainFactory = grainFactory;
            this.vehicleState = vehicleState;
            this.rentState = rentState;
        }

        public async Task RegisterVehicle(string model, decimal costPerKilometer, double latitude, double longitude)
        {
            var registrationNumber = this.GetGrainId().Key.ToString()!;

            if (vehicleState.RecordExists)
                throw new VehicleAlreadyRegisteredException(registrationNumber);

            var areaResolverGrain = grainFactory.GetGrain<IAreaResolverStatelessWorkerGrain>(0);
            var location = new Location(latitude, longitude);
            var area = await areaResolverGrain.GetAreaForLocation(location);

            if (area is null)
                throw new VehicleOutsideBoundsException();

            var areaName = area.GetGrainId().Key.ToString()!;

            vehicleState.State = new VehicleState()
            {
                Model = model,
                RegistrationNumber = registrationNumber,
                Location = location,
                LastArea = areaName,
                RatePerKilometer = costPerKilometer
            };

            await vehicleState.WriteStateAsync();

            // Register our car for people to rent
            await RegisterInTheArea();

            // Notify the real-time monitor of our location
            await NotifyAreaMonitor(areaName);
        }

        public Task<VehicleInfo> GetVehicleInfo()
        {
            if (!vehicleState.RecordExists)
                throw new VehicleDoesntExistException();

            // The following line is required only if the state could be modified externally
            // await vehicleState.ReadStateAsync();

            return Task.FromResult(GetVehicleInfoInternal());
        }

        public Task<Location> GetLocation()
            => Task.FromResult(vehicleState.State.Location);

        public async Task<IUserGrain?> GetCurrentRentee()
            => await rentState.PerformRead(s => s.CurrentRentee);

        public async Task BeginRent(IUserGrain user)
        {
            CheckIfExists();

            // Create a lock on the user's account - if there are insufficient funds this will rollback our transaction!
            var account = await user.GetAccount();
            var lockId = await account.CreateBalanceLock(vehicleState.State.RatePerKilometer * DefaultKilometersDebitLock);

            // Unregister our vehicle from the area to make it unavailable to others
            await UnregisterFromArea();

            // Set the rent inforamtion transactionally
            await rentState.PerformUpdate(s =>
            {
                if (s.CurrentRentee is not null)
                    throw new VehicleAlreadyRentedExeption();

                s.CurrentRentee = user;
                s.PayingAccount = account;
                s.AccountLockId = lockId;
                s.StartingLocation = vehicleState.State.Location;
                s.LastLocation = vehicleState.State.Location;
                s.DistanceDrivenMeters = 0.0M;
            });
        }

        public async Task<decimal> EndRent()
        {
            CheckIfExists();

            var rent = await rentState.PerformRead(s => s);

            if (rent.CurrentRentee is null)
                throw new VehicleNotRentedException();

            // Charge our client
            var outstandingAmount = vehicleState.State.RatePerKilometer * (rent.DistanceDrivenMeters ?? 0.0M) / 1000M;
            await rent.PayingAccount!.CommitDebit((Guid)rent.AccountLockId!, outstandingAmount);

            // Mark this vehicle as free
            await rentState.PerformUpdate(s => s.CurrentRentee = null);

            // Update vehicle location
            await UpdateVehicleLocationInternal(vehicleState.State.Location); // this is where we should query the IRL vehicle for current position

            // Register the car in the area for others to rent
            await RegisterInTheArea();

            return outstandingAmount;
        }

        public async Task UpdateLocation(double latitude, double longitude)
        {
            CheckIfExists();

            var newLocation = new Location(latitude, longitude);

            // Update location locally
            await UpdateVehicleLocationInternal(newLocation);

            // Update transactional state if the car is rented
            await rentState.PerformUpdate(s =>
            {
                if (s.CurrentRentee is null)
                    return;

                s.DistanceDrivenMeters = (decimal)s.DistanceDrivenMeters! + (decimal)s.LastLocation!.CalculateDistanceToInMeters(newLocation);
                s.LastLocation = newLocation;
            });
        }

        private async Task UpdateVehicleLocationInternal(Location location)
        {
            var areaResolverGrain = grainFactory.GetGrain<IAreaResolverStatelessWorkerGrain>(0);
            var previousArea = vehicleState.State.LastArea;
            var newArea = (await areaResolverGrain.GetAreaForLocation(location)).GetPrimaryKeyString();

            vehicleState.State.Location = location;
            vehicleState.State.LastArea = newArea;

            await vehicleState.WriteStateAsync();

            // Notify monitor(s) of our location
            await NotifyAreaMonitor(newArea, previousArea);
        }

        private async Task NotifyAreaMonitor(string currentArea, string? previousArea = null)
        {
            var streamProvider = this.GetStreamProvider(Constants.MemoryStreamProvider);

            // If we switched areas notify the previous area monitor of the change
            if (previousArea is not null && previousArea != currentArea)
            {
                var previousAreaStream = streamProvider.GetStream<RemoveVehicleFromMonitorEvent>(StreamId.Create(Constants.AreaMonitorRemovalsStreamNamespace, previousArea));
                await previousAreaStream.OnNextAsync(new RemoveVehicleFromMonitorEvent(vehicleState.State.RegistrationNumber));
            }

            // Announce ourselves in the area monitor
            var areaStream = streamProvider.GetStream<UpdateVehiclePositionEvent>(StreamId.Create(Constants.AreaMonitorUpdatesStreamNamespace, currentArea));
            await areaStream.OnNextAsync(new UpdateVehiclePositionEvent(vehicleState.State.RegistrationNumber, vehicleState.State.Location));
        }

        private async Task RegisterInTheArea()
        {
            var areaResolver = grainFactory.GetGrain<IAreaResolverStatelessWorkerGrain>(0);
            var area = await areaResolver.GetAreaForLocation(vehicleState.State.Location);

            if (area is null)
                throw new VehicleOutsideBoundsException();

            await area.RegisterVehicle(GetVehicleInfoInternal());
        }

        private async Task UnregisterFromArea()
        {
            var areaResolver = grainFactory.GetGrain<IAreaResolverStatelessWorkerGrain>(0);
            var area = await areaResolver.GetAreaForLocation(vehicleState.State.Location);

            if (area is null)
                return;

            await area.UnregisterVehicle(this);
        }

        private void CheckIfExists()
        {
            if (!vehicleState.RecordExists)
                throw new VehicleDoesntExistException();
        }

        private VehicleInfo GetVehicleInfoInternal()
            => new VehicleInfo(
                vehicleState.State.Model,
                vehicleState.State.RegistrationNumber,
                vehicleState.State.RatePerKilometer,
                vehicleState.State.Location);
    }
}

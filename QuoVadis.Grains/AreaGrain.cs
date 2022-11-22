using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.Streams;
using Orleans.Streams.Core;
using Orleans.Transactions.Abstractions;
using QuoVadis.Common;
using QuoVadis.Contracts;
using QuoVadis.GrainInterfaces;
using QuoVadis.Grains.State;
using System.Collections.Immutable;

namespace QuoVadis.Grains
{
    [Reentrant]
    public class AreaGrain : IAreaGrain, IGrainBase
    {
        private readonly ITransactionalState<AreaState> areaState;

        public IGrainContext GrainContext { get; }

        public AreaGrain(
            IGrainContext grainContext,
            [TransactionalState(Constants.AreaStateName, Constants.RegularStorage)] ITransactionalState<AreaState> areaState)
        {
            GrainContext = grainContext;

            this.areaState = areaState;
        }

        public async Task<ImmutableList<VehicleInfo>> GetVehicles()
        {
            var vehicles = await areaState.PerformRead(s => s.Vehicles);

            if (vehicles is null)
                return ImmutableList<VehicleInfo>.Empty;

            return vehicles.Select(v => v.Value).ToImmutableList();
        }

        public Task RegisterVehicle(VehicleInfo vehicle)
            => areaState.PerformUpdate(s =>
            {
                if (s.Vehicles is null)
                    s.Vehicles = new Dictionary<string, VehicleInfo>();

                if (s.Vehicles.ContainsKey(vehicle.RegistrationNumber))
                    return;

                s.Vehicles.Add(vehicle.RegistrationNumber, vehicle);
            });

        public Task UnregisterVehicle(IVehicleGrain vehicle)
            => areaState.PerformUpdate(s =>
            {
                if (s.Vehicles is null)
                    s.Vehicles = new Dictionary<string, VehicleInfo>();

                var vehicleKey = vehicle.GetPrimaryKeyString();

                if (!s.Vehicles.ContainsKey(vehicleKey))
                    return;

                s.Vehicles.Remove(vehicleKey);
            });
    }
}

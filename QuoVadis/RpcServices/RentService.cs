using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Orleans.Transactions;
using QuoVadis.Common.Exceptions;
using QuoVadis.Extensions;
using QuoVadis.GrainInterfaces;
using QuoVadis.Proto;
using System.Globalization;

namespace QuoVadis.RpcServices
{
    public class RentService : Rent.RentBase
    {
        private readonly IGrainFactory grainFactory;
        private readonly ITransactionClient transactionClient;

        public RentService(IGrainFactory grainFactory, ITransactionClient transactionClient)
        {
            this.grainFactory = grainFactory;
            this.transactionClient = transactionClient;
        }

        [Authorize(Roles = "User")]
        public override async Task<BeginRentResponse> BeginRent(BeginRentRequest request, ServerCallContext context)
        {
            var username = context.GetIdentityUsername()
                ?? throw new InvalidDataException();

            var userGrain = grainFactory.GetGrain<IUserGrain>(username);

            try
            {
                await transactionClient.RunTransaction(TransactionOption.Create, async () =>
                {
                    await userGrain.RentVehicle(request.RegistrationNumber);
                });
            }
            catch (OrleansTransactionAbortedException ex)
            {
                return new BeginRentResponse
                {
                    Status = ex.InnerException switch
                    {
                        UserAlreadyRentingVehicleException => BeginRentStatus.RentInProgress,
                        InsufficientFundsException => BeginRentStatus.InsufficientFunds,
                        VehicleDoesntExistException => BeginRentStatus.VehicleDoesntExist,
                        VehicleAlreadyRentedExeption => BeginRentStatus.VehicleInUse
                    }
                };
            }

            return new BeginRentResponse { Status = BeginRentStatus.SuccessfullyRented };
        }

        public override async Task<EndRentResponse> EndRent(EndRentRequest request, ServerCallContext context)
        {
            var username = context.GetIdentityUsername()
                ?? throw new InvalidDataException();

            var userGrain = grainFactory.GetGrain<IUserGrain>(username);

            decimal paidAmount;

            try
            {
                // we can omit the ITransactionClient altogether if the called method supports creating transactions
                paidAmount = await userGrain.ReturnVehicle();
            }
            catch (OrleansTransactionAbortedException ex) when (ex.InnerException is NoVehicleCurrentlyRentedException)
            {
                return new EndRentResponse { Status = EndRentStatus.NoVehicleRented };
            }
            catch (OrleansTransactionAbortedException ex) when (ex.InnerException is VehicleOutsideBoundsException)
            {
                return new EndRentResponse { Status = EndRentStatus.OutsideArea };
            }

            return new EndRentResponse { Status = EndRentStatus.SuccessfullyEndedRent, PaidAmount = paidAmount.ToString("F2", CultureInfo.InvariantCulture) };
        }

        public override async Task<GetAreasResponse> GetAreas(GetAreasRequest request, ServerCallContext context)
        {
            // stateless worker grains don't have a separate identity
            var resolverGrain = grainFactory.GetGrain<IAreaResolverStatelessWorkerGrain>(0);
            var areas = await resolverGrain.GetAllAreas();
            var response = new GetAreasResponse();

            response.Areas.AddRange(areas);

            return response;
        }

        public override async Task<GetVehiclesResponse> GetVehicles(GetVehiclesRequest request, ServerCallContext context)
        {
            var areaGrain = grainFactory.GetGrain<IAreaGrain>(request.Area);
            var vehicles = await areaGrain.GetVehicles();
            var response = new GetVehiclesResponse();

            response.Vehicles.AddRange(vehicles
                .OrderBy(v => v.RegistrationNumber)
                .Select(v => new VehicleInfo()
                {
                    Registration = v.RegistrationNumber,
                    Model = v.Model,
                    CostPerKilometer = v.CostPerKilometer.ToString(),
                    Location = new Location
                    {
                        Latitude = v.Location.Latitude,
                        Longitude = v.Location.Longitude
                    }
                }));

            return response;
        }

        public override async Task<GetCurrentlyRentedVehicleResponse> GetCurrentlyRentedVehicle(GetCurrentlyRentedVehicleRequest request, ServerCallContext context)
        {
            var username = context.GetIdentityUsername()
                ?? throw new InvalidDataException();

            var userGrain = grainFactory.GetGrain<IUserGrain>(username);
            var currentlyRented = await userGrain.GetCurrentlyRentedVehicle();

            if (currentlyRented is null)
                return new GetCurrentlyRentedVehicleResponse();

            return new GetCurrentlyRentedVehicleResponse
            {
                Vehicle = new VehicleInfo()
                {
                    Registration = currentlyRented.RegistrationNumber,
                    Model = currentlyRented.Model,
                    CostPerKilometer = currentlyRented.CostPerKilometer.ToString(),
                    Location = new Location
                    {
                        Latitude = currentlyRented.Location.Latitude,
                        Longitude = currentlyRented.Location.Longitude
                    }
                }
            };
        }
    }
}

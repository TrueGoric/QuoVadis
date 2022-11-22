using Grpc.Core;
using QuoVadis.GrainInterfaces;
using QuoVadis.Proto;

namespace QuoVadis.RpcServices
{
    public class VehicleService : Vehicle.VehicleBase
    {
        private readonly IGrainFactory grainFactory;

        public VehicleService(IGrainFactory grainFactory)
        {
            this.grainFactory = grainFactory;
        }

        public override async Task<UpdatePositionResponse> UpdatePosition(UpdatePositionRequest request, ServerCallContext context)
        {
            var vehicleGrain = grainFactory.GetGrain<IVehicleGrain>(request.VehicleRegistration);

            await vehicleGrain.UpdateLocation(request.Location.Latitude, request.Location.Longitude);

            return new UpdatePositionResponse();
        }
    }
}

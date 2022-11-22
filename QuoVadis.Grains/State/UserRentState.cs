using QuoVadis.GrainInterfaces;

namespace QuoVadis.Grains.State
{
    [GenerateSerializer]
    public class UserRentState
    {
        [Id(0)]
        public IVehicleGrain? CurrentlyRentedVehicle { get; set; }
    }
}

using QuoVadis.Common.ValueObjects;
using QuoVadis.GrainInterfaces;

namespace QuoVadis.Grains.State
{
    [GenerateSerializer]
    public class VehicleRentState
    {
        [Id(0)]
        public IUserGrain? CurrentRentee { get; set; }

        [Id(1)]
        public IAccountGrain? PayingAccount { get; set; }

        [Id(2)]
        public Guid? AccountLockId { get; set; }

        [Id(3)]
        public Location? StartingLocation { get; set; }

        [Id(4)]
        public Location? LastLocation { get; set; }

        [Id(5)]
        public decimal? DistanceDrivenMeters { get; set; }
    }
}

using QuoVadis.Common.ValueObjects;

namespace QuoVadis.Grains.State
{
    [GenerateSerializer]
    public class VehicleState
    {
        [Id(0)]
        public string Model { get; set; }

        [Id(1)]
        public string RegistrationNumber { get; set; }

        [Id(2)]
        public Location Location { get; set; }

        [Id(3)]
        public string LastArea { get; set; }

        [Id(4)]
        public decimal RatePerKilometer { get; set; }
    }
}

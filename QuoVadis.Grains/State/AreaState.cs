using QuoVadis.Contracts;

namespace QuoVadis.Grains.State
{
    [GenerateSerializer]
    public class AreaState
    {
        [Id(0)]
        public Dictionary<string, VehicleInfo> Vehicles { get; set; }
    }
}

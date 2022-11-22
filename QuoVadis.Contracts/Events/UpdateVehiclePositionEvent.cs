using QuoVadis.Common.ValueObjects;

namespace QuoVadis.Contracts.Events
{
    [GenerateSerializer]
    public record UpdateVehiclePositionEvent(string RegistrationNumber, Location Location);
}

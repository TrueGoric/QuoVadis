using QuoVadis.Common.ValueObjects;

namespace QuoVadis.Observers
{
    public record AreaMonitorEvent(string RegistrationNumber, Location? Location);
}

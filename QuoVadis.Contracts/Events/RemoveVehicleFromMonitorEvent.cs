namespace QuoVadis.Contracts.Events
{
    [GenerateSerializer]
    public record RemoveVehicleFromMonitorEvent(string RegistrationNumber);
}

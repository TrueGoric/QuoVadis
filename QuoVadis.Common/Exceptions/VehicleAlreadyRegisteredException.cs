namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    public class VehicleAlreadyRegisteredException : DomainException
    {
        public override string Message => $"Vehicle with registration number {VehicleRegistration} is already registered!";

        public string VehicleRegistration { get; }

        public VehicleAlreadyRegisteredException(string vehicleRegistration)
        {
            VehicleRegistration = vehicleRegistration;
        }
    }
}

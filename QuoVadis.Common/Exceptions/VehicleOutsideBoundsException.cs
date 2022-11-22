namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    public class VehicleOutsideBoundsException : DomainException
    {
        public override string Message => "Vehicle must be stored within a municipal boundary!";
    }
}

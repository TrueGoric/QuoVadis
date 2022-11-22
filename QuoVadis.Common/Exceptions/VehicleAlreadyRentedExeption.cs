namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    public class VehicleAlreadyRentedExeption : DomainException
    {
        public override string Message => "Vehicle is already rented!";
    }
}

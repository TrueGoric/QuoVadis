namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    public class VehicleNotRentedException : DomainException
    {
        public override string Message => "This vehicle isn't rented - it can't be returned!";
    }
}

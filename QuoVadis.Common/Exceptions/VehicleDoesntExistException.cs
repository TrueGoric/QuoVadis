namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    public class VehicleDoesntExistException : DomainException
    {
        public override string Message => "There's no such vehicle!";
    }
}

namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    public class NoVehicleCurrentlyRentedException : DomainException
    {
        public override string Message => "The user is not currently renting a vehicle!";
    }
}

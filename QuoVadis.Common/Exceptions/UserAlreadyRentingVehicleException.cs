namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    public class UserAlreadyRentingVehicleException : DomainException
    {
        public override string Message => "User is already renting a vehicle!";
    }
}

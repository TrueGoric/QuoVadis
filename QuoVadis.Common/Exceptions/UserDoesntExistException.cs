namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    public class UserDoesntExistException : DomainException
    {
        public override string Message => "User doesn't exist!";
    }
}

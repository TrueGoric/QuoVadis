namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    public class UsernameAlreadyExistsException : DomainException
    {
        public override string Message => "Provided username already exists!";
    }
}

namespace QuoVadis.Contracts
{
    [GenerateSerializer]
    public record UserRegistrationData(string Username, string Password);
}

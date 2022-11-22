namespace QuoVadis.Contracts
{
    [GenerateSerializer]
    public record UserLoginData(string Username, string Password);
}

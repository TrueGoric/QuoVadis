namespace QuoVadis.Contracts
{
    [GenerateSerializer]
    public record AccountBalance(decimal Total, decimal Available);
}

namespace QuoVadis.Grains.State
{
    [GenerateSerializer]
    public class AccountState
    {
        [Id(0)]
        public decimal Balance { get; set; }

        [Id(1)]
        public Dictionary<Guid, decimal> Locks { get; set; }
    }
}

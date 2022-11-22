namespace QuoVadis.Grains.State
{
    [GenerateSerializer]
    public class UserState
    {
        [Id(0)]
        public string PasswordHash { get; set; }

        [Id(1)]
        public string PasswordSalt { get; set; }

        [Id(2)]
        public Guid AccountId { get; set; }
    }
}

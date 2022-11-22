namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    public class InsufficientFundsException : DomainException
    {
        public decimal MinimumBalance { get; }

        public override string Message => $"You must have at least {MinimumBalance} to rent this vehicle!";

        public InsufficientFundsException(decimal minimumBalance)
        {
            MinimumBalance = minimumBalance;
        }
    }
}

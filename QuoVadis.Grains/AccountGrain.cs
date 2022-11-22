using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.Transactions.Abstractions;
using QuoVadis.Common;
using QuoVadis.Common.Exceptions;
using QuoVadis.Contracts;
using QuoVadis.GrainInterfaces;
using QuoVadis.Grains.State;

namespace QuoVadis.Grains
{
    [Reentrant]
    public class AccountGrain : IAccountGrain, IGrainBase
    {
        private readonly ITransactionalState<AccountState> accountState;

        public IGrainContext GrainContext { get; }

        public AccountGrain(
            IGrainContext grainContext,
            [TransactionalState(Constants.AccountStateName, Constants.TransactionalStorage)] ITransactionalState<AccountState> accountState)
        {
            GrainContext = grainContext;

            this.accountState = accountState;
        }

        public async Task<AccountBalance> GetBalance()
        {
            var balance = await accountState.PerformRead(s => s.Balance);
            var locks = await accountState.PerformRead(s => s.Locks);

            return new AccountBalance(balance, CalculateAvailableBalance(balance, locks));
        }

        public async Task AddBalance(decimal amount)
            => await accountState.PerformUpdate(s =>
            {
                s.Balance += amount;
            });

        public async Task CommitDebit(Guid lockId, decimal debit)
            => await accountState.PerformUpdate(s =>
            {
                if (s.Locks is null || !s.Locks.Remove(lockId))
                    throw new KeyNotFoundException($"Given {nameof(lockId)} doesn't exist!");

                s.Balance -= debit;
            });

        public async Task<Guid> CreateBalanceLock(decimal minimumBalance)
            => await accountState.PerformUpdate(s =>
            {
                if (s.Locks is null)
                    s.Locks = new Dictionary<Guid, decimal>();

                if (CalculateAvailableBalance(s.Balance, s.Locks) < minimumBalance)
                    throw new InsufficientFundsException(minimumBalance);

                var lockId = Guid.NewGuid();

                s.Locks.Add(lockId, minimumBalance);

                return lockId;
            });

        private static decimal CalculateAvailableBalance(decimal balance, Dictionary<Guid, decimal>? locks)
            => balance - (locks?.Sum(l => l.Value) ?? 0.0M);
    }
}

using QuoVadis.Contracts;

namespace QuoVadis.GrainInterfaces;

public interface IAccountGrain : IGrainWithGuidKey
{
    [Transaction(TransactionOption.Join)]
    Task<Guid> CreateBalanceLock(decimal minimumBalance);

    [Transaction(TransactionOption.Join)]
    Task CommitDebit(Guid lockId, decimal debit);

    [Transaction(TransactionOption.CreateOrJoin)]
    Task AddBalance(decimal amount);

    [Transaction(TransactionOption.CreateOrJoin)]
    Task<AccountBalance> GetBalance();
}
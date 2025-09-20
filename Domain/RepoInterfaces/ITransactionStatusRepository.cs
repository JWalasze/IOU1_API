using Domain.Entities;

namespace Domain.RepoInterfaces;

public interface ITransactionStatusRepository
{
    Task<TransactionStatus> GetPendingStatus();
}

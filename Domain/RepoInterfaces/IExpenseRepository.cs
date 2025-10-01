using Domain.Entities;

namespace Domain.RepoInterfaces;

public interface IExpenseRepository : IRepository
{
    Task AddAsync(Expense expense, CancellationToken cancellationToken = default);
    Task<Expense?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Expense>> GetByGroupIdAsync(long groupId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

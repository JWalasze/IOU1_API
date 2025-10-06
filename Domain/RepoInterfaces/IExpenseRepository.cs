using IOU1.Domain.Entities;
using IOU1.Domain.RepoInterfaces;

namespace Domain.RepoInterfaces;

public enum SortingOptions
{
    Oldest,
    Newest
}

public interface IExpenseRepository : IRepository
{
    Task AddAsync(Expense expense, CancellationToken cancellationToken = default);
    Task<Expense?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Expense>> GetByGroupIdAsync(long groupId, CancellationToken cancellationToken = default);
    IQueryable<Expense> GetByGroupIdQuery(long groupId);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

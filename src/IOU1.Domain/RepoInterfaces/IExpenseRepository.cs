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
    Task<Expense?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Expense>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);
    IQueryable<Expense> GetByGroupIdQuery(int groupId);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

using Domain.RepoInterfaces;
using IOU1.Domain.Entities;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace IOU1.Infrastructure.Repositories;

public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    private readonly IOU1Context _context;

    public ExpenseRepository(IOU1Context context) : base(context)
    {
        _context = context;
    }

    public async Task AddAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        // Add Expense with related Transactions
        await _context.Expenses.AddAsync(expense, cancellationToken);
    }

    public async Task<IEnumerable<Expense>> GetByGroupIdAsync(long groupId, CancellationToken cancellationToken = default)
    {
        return await _context.Expenses
            .Include(e => e.Transactions)
                .ThenInclude(t => t.Group)
            .Include(e => e.Transactions)
                .ThenInclude(t => t.Buyer)
            .Include(e => e.Transactions)
                .ThenInclude(t => t.Borrower)
            .Include(e => e.Transactions)
                .ThenInclude(t => t.Currency)
            .Include(e => e.Group)
            .Include(e => e.Buyer)
            .Include(e => e.Currency)
            .Where(e => e.Group.Id == groupId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Expense?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Expenses
            .Include(e => e.Transactions)   // eager load related transactions
            .Include(e => e.Group)
            .Include(e => e.Buyer)
            .Include(e => e.Currency)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public IQueryable<Expense> GetByGroupIdQuery(long groupId)
    {
        return _context.Expenses
            .Include(e => e.Transactions)
                .ThenInclude(t => t.Group)
            .Include(e => e.Transactions)
                .ThenInclude(t => t.Buyer)
            .Include(e => e.Transactions)
                .ThenInclude(t => t.Borrower)
            .Include(e => e.Transactions)
                .ThenInclude(t => t.Currency)
            .Include(e => e.Group)
            .Include(e => e.Buyer)
            .Include(e => e.Currency)
            .Where(e => e.Group.Id == groupId)
            .AsQueryable();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

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
        // Add Expense with related ExpenseShares
        await _context.Expenses.AddAsync(expense, cancellationToken);
    }

    public async Task<IEnumerable<Expense>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return await _context.Expenses
            .Include(e => e.Shares)
                .ThenInclude(es => es.Member)
                    .ThenInclude(m => m.User)
            .Include(e => e.Group)
            .Include(e => e.Payer)
                .ThenInclude(p => p.User)
            .Where(e => e.Group.Id == groupId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Expense?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Expenses
            .Include(e => e.Shares)   // eager load related expense shares
                .ThenInclude(es => es.Member)
                    .ThenInclude(m => m.User)
            .Include(e => e.Group)
            .Include(e => e.Payer)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public IQueryable<Expense> GetByGroupIdQuery(int groupId)
    {
        return _context.Expenses
            .Include(e => e.Shares)
                .ThenInclude(es => es.Member)
                    .ThenInclude(m => m.User)
            .Include(e => e.Group)
            .Include(e => e.Payer)
                .ThenInclude(p => p.User)
            .Where(e => e.Group.Id == groupId)
            .AsQueryable();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

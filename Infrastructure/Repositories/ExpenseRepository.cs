using Domain.Entities;
using Domain.RepoInterfaces;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly IOU1Context _context;

    public ExpenseRepository(IOU1Context context)
    {
        _context = context;
    }

    public async Task AddAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        // Add Expense with related Transactions
        await _context.Expenses.AddAsync(expense, cancellationToken);
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

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

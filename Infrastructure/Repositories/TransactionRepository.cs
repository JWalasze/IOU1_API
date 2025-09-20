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

public class TransactionRepository : ITransactionRepository
{
    private readonly IOU1Context _context;

    public TransactionRepository(IOU1Context context)
    {
        _context = context;
    }
    public async Task<List<Transaction>> AddRangeAsync(List<Transaction> transactions)
    {
        await _context.Transactions.AddRangeAsync(transactions);
        await _context.SaveChangesAsync();
        return transactions;
    }

    public async Task<List<Transaction>> GetGroupTransactionsAsync(long groupId)
    {
        return await _context.Transactions
            .Include(t => t.Group)
            .Include(t => t.Currency)
            .Include(t => t.Borrower)
            .Include(t => t.Buyer)
            .Include(t => t.Status)
            .Where(t => t.Group.Id == groupId)
            .ToListAsync();
    }
}

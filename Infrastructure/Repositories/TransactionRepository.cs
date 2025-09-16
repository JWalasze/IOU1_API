using Domain.Entities;
using Domain.RepoInterfaces;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}

using IOU1.Domain.Entities;
using IOU1.Domain.RepoInterfaces;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly IOU1Context _context;

    public CurrencyRepository(IOU1Context context)
    {
        _context = context;
    }
    public async Task<Currency> GetDefaultCurrency()
    {
        return await _context.Currencies.FirstAsync(s => s.Key == "USD");
    }
}

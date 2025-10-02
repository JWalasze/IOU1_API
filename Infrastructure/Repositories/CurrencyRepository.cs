using Domain.Entities;
using Infrastructure.Context;
using IOU1.Domain.RepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly IOU1Context _context;

    public CurrencyRepository(IOU1Context context)
    {
        _context = context;
    }
    public async Task<Currency> GetDefaultCurrency()
    {
        return await _context.Currencies.FirstAsync(s => s.Name == "USD");
    }
}

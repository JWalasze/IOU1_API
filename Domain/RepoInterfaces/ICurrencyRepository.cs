using Domain.Entities;
using Domain.RepoInterfaces;

namespace IOU1.Domain.RepoInterfaces;

public interface ICurrencyRepository : IRepository
{
    Task<Currency> GetDefaultCurrency();
}

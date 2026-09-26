using IOU1.Domain.Entities;

namespace IOU1.Domain.RepoInterfaces;

public interface ICurrencyRepository : IRepository
{
    Task<Currency> GetDefaultCurrency();
}

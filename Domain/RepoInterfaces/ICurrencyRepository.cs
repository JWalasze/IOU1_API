using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepoInterfaces;

public interface ICurrencyRepository
{
    Task<Currency> GetDefaultCurrency();
}

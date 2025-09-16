using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepoInterfaces;

public interface ITransactionRepository
{
    Task<List<Transaction>> AddRangeAsync(List<Transaction> transactions);
}

using IOU1.Domain.Entities;

namespace IOU1.Domain.Services.Splits;

public interface ISplitStrategy
{
    IEnumerable<Transaction> Split(Expense expense);
}

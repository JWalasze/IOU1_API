using IOU1.Domain.Entities;

namespace IOU1.Domain.Services.Splits;

public interface ISplitStrategy
{
    IEnumerable<ExpenseShare> Split(Expense expense);
}

using Domain.Entities;

namespace IOU1.Application.Strategy;

public abstract class BaseSplitStrategy(Expense expense) : ISplitStrategy
{
    protected readonly Expense _expense = expense;

    public abstract void Split();
}

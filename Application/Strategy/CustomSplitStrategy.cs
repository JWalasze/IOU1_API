using Domain.Entities;

namespace IOU1.Application.Strategy;

public class CustomSplitStrategy(Expense expense) : BaseSplitStrategy(expense)
{
    public override void Split()
    {
        throw new NotImplementedException();
    }
}

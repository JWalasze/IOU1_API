using IOU1.Application.Features.Expenses.GetExpenses.Models;
using IOU1.Domain.Models;

namespace IOU1.Application.Features.Expenses.GetExpenses;

public static class GetExpensesMapper
{
    public static GetExpensesResponse ToGetExpensesResponse(this GroupExpenseSummary? summary)
    {
        var data = summary!;

        return new()
        {
            GroupId = data.GroupId,
            Expenses = data.GroupDebts.Select(gd => new GetExpensesDetailsResponse
            {
                ExpenseId = 0,
                Description = $"{gd.DebtorName} owes {gd.CreditorName}",
                Amount = gd.Amount,
                Date = DateTime.MinValue,
            }),
        };
    }
}

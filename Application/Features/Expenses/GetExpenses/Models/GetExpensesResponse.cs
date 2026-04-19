namespace IOU1.Application.Features.Expenses.GetExpenses.Models;

public class GetExpensesResponse
{
    public long GroupId { get; set; }

    public IEnumerable<GetExpensesDetailsResponse> Expenses { get; set; } = [];
}

public class GetExpensesDetailsResponse
{
    public long ExpenseId { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
}

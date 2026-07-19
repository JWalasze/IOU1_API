namespace IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Models;

public sealed record ExpenseOption(
    int Id,
    string Key,
    string Value,
    string? IconKey)
{
    public const string ExpenseCategory = "EXPENSE_CATEGORY";
    public const string ExpenseSplit = "EXPENSE_SPLIT";
}

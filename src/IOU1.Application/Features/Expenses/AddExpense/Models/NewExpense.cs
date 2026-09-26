using IOU1.Domain.Models;

namespace IOU1.Application.Features.Expenses.AddExpense.Models;

public sealed record NewExpense(
    string Title,
    string? Description,
    int PayerId,
    int GroupId,
    decimal Amount,
    int SplitTypeId,
    int CategoryId,
    IEnumerable<Split> Splits);

using IOU1.Domain.Enums;
using IOU1.Domain.Models;

namespace IOU1.Application.Features.Expenses.AddExpense.Models;

public sealed record NewExpense(
    long BuyerId,
    long GroupId,
    decimal Amount,
    string Title,
    string? Description,
    ExpenseSplitType SplitType,
    IEnumerable<Split> Splits);

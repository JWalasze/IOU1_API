namespace IOU1.Application.Features.Expenses.AddExpense.Models.Request;

public sealed record AddExpenseRequest(
    string Title,
    string? Description,
    int PayerId,
    int GroupId,
    decimal Amount,
    int SplitTypeId,
    int CategoryId,
    IEnumerable<SplitRequest> Splits);

public sealed record SplitRequest(
    int MemberId,
    decimal Amount,
    decimal? Percentage);

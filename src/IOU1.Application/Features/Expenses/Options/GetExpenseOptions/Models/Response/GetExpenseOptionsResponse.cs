namespace IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Models.Response;

public sealed record GetExpenseOptionsResponse(
    int Id,
    string Key,
    string Name,
    string? IconKey);

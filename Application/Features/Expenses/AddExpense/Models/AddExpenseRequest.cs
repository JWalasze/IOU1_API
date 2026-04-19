using IOU1.Domain.Enums;
using System.Text.Json.Serialization;

namespace IOU1.Application.Features.Expenses.AddExpense.Models;

public sealed record AddExpenseRequest
{
    public required long BuyerId { get; init; }
    public required long GroupId { get; init; }
    public required decimal Amount { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ExpenseSplitType SplitType { get; init; }

    public IEnumerable<SplitRequest> Splits { get; init; } = [];
}

public sealed record SplitRequest(
    long MemberId,
    decimal Amount);

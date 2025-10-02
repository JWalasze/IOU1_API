using Application.Mediator;

namespace IOU1.Application.Features.Transactions.AddTransaction.Request;

public record AddTransactionRequest(
    long BuyerId,
    long GroupId,
    decimal Amount,
    string Title,
    string? Description,
    IEnumerable<SplitRequest> Splits) : IRequest;

public record SplitRequest(long MemberId, decimal Amount);

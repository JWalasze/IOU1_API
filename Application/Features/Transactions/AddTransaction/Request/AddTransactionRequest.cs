using Application.Mediator;
using Domain.Enums;

namespace Application.Features.Transactions.AddTransaction.Request;

public record AddTransactionRequest(
    long BuyerId,
    long GroupId,
    decimal Amount,
    string Title,
    string? Description,
    IEnumerable<SplitRequest> Splits,
    ExpenseType Type) : IRequest;

public record SplitRequest(long MemberId, decimal Amount);

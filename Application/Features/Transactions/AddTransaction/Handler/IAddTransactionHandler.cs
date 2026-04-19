using Application.Features.Transactions.AddTransaction.Response;
using IOU1.Application.Features.Transactions.AddTransaction.Request;

namespace IOU1.Application.Features.Transactions.AddTransaction.Handler;

public interface IAddTransactionHandler
{
    //AddTransactionRequest, AddTransactionResponse
    Task<AddTransactionResponse> Handle(AddTransactionRequest request, CancellationToken cancellationToken = default);
}

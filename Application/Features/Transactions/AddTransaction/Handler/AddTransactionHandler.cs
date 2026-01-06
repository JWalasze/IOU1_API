using Application.Features.Transactions.AddTransaction.Response;
using Application.Service;
using FluentValidation;
using IOU1.Application.Features.Transactions.AddTransaction.Request;
using IOU1.Application.Mediator;
using IOU1.Domain.Interfaces;
using MapsterMapper;

namespace IOU1.Application.Features.Transactions.AddTransaction.Handler;

public class AddTransactionHandler(
    IExpenseService expenseService,
    IValidator<AddTransactionRequest> validator,
    IMapper mapper) : RequestHandler<AddTransactionRequest, AddTransactionResponse>(validator, mapper)
{
    private readonly IExpenseService _expenseService = expenseService;

    protected override async Task<IResult> Do(AddTransactionRequest request, CancellationToken cancellationToken = default)
    {
        return await _expenseService.AddExpense(request);
    }
}

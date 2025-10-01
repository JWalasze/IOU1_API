using Application.Features.Transactions.AddTransaction.Request;
using Application.Features.Transactions.AddTransaction.Response;
using Application.Mediator;
using Application.Service;
using FluentValidation;

namespace Application.Features.Transactions.AddTransaction.Handler;

public class AddTransactionHandler(IValidator<AddTransactionRequest> validator, IExpenseService expenseService) : IRequestHandler<AddTransactionRequest, AddTransactionResponse>
{
    private readonly IValidator<AddTransactionRequest> _validator = validator;
    private readonly IExpenseService _expenseService = expenseService;

    public async Task<AddTransactionResponse> Handle(AddTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.First().ErrorMessage,
            };
        }

        var result = await _expenseService.AddExpense(request);
        if (!result.IsSuccess)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = result.ErrorMessage,
            };
        }

        return new()
        {
            IsSuccess = true,
        };
    }
}

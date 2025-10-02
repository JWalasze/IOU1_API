using FluentValidation;
using IOU1.Application.Features.Transactions.AddTransaction.Request;

namespace Application.Features.Transactions.AddTransaction.Validator;

public class AddTransactionValidator : AbstractValidator<AddTransactionRequest>
{
    public AddTransactionValidator()
    {

    }
}

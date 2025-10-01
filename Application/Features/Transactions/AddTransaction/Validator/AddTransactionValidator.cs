using Application.Features.Transactions.AddTransaction.Request;
using FluentValidation;

namespace Application.Features.Transactions.AddTransaction.Validator;

public class AddTransactionValidator : AbstractValidator<AddTransactionRequest>
{
    public AddTransactionValidator()
    {

    }
}

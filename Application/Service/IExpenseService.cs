using Application.Features.Transactions.AddTransaction.Dto;
using Application.Features.Transactions.AddTransaction.Request;
using Domain.Models;

namespace Application.Service;

public interface IExpenseService
{
    Task<Result<AddedExpenseDto>> AddExpense(AddTransactionRequest request);
}

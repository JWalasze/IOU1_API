using Application.Features.Transactions.AddTransaction.Dto;
using IOU1.Application.Features.Transactions.AddTransaction.Request;
using IOU1.Domain.Models;

namespace Application.Service;

public interface IExpenseService
{
    Task<Result<AddedExpenseDto?>> AddExpense(AddTransactionRequest request);
}

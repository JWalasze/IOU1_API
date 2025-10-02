using Application.Features.Transactions.AddTransaction.Dto;
using Domain.Models;
using IOU1.Application.Features.Transactions.AddTransaction.Request;

namespace Application.Service;

public interface IExpenseService
{
    Task<Result<AddedExpenseDto?>> AddExpense(AddTransactionRequest request);
}

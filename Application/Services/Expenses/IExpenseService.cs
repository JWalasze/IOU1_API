using Application.Features.Transactions.AddTransaction.Dto;
using IOU1.Application.Features.Transactions.AddTransaction.Request;
using IOU1.Domain.Models;

namespace IOU1.Application.Services.Expenses;

public interface IExpenseService
{
    Task<Result<AddedExpenseDto?>> AddExpense(AddTransactionRequest request);
}

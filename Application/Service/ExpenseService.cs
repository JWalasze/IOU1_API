using Application.Features.Transactions.AddTransaction.Dto;
using Application.Features.Transactions.AddTransaction.Request;
using Domain.Models;
using Domain.RepoInterfaces;
using Domain.UnitOfWork;

namespace Application.Service;

public class ExpenseService(IUnitOfWork unit) : IExpenseService
{
    private readonly IUnitOfWork _unit = unit;

    public Task<Result<AddedExpenseDto>> AddExpense(AddTransactionRequest request)
    {
        var expenseRepository = _unit.Get<IExpenseRepository>();
        var groupRepository = _unit.Get<IGroupRepository>();
        var userRepository = _unit.Get<IUserRepository>();

        throw new Exception();
    }
}

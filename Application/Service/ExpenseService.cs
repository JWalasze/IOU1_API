using Application.Features.Transactions.AddTransaction.Dto;
using Application.Service;
using Domain.Entities;
using Domain.Models;
using Domain.RepoInterfaces;
using Domain.UnitOfWork;
using IOU1.Application.Features.Transactions.AddTransaction.Request;
using IOU1.Application.Strategy;
using IOU1.Domain.RepoInterfaces;

namespace IOU1.Application.Service;

public class ExpenseService(IUnitOfWork unit) : IExpenseService
{
    private readonly IUnitOfWork _unit = unit;

    public async Task<Result<AddedExpenseDto?>> AddExpense(AddTransactionRequest request)
    {
        var expenseRepository = _unit.Get<IExpenseRepository>();
        var groupRepository = _unit.Get<IGroupRepository>();
        var userRepository = _unit.Get<IUserRepository>();
        var currencyRepository = _unit.Get<ICurrencyRepository>();

        var buyer = await userRepository.GetByIdAsync(request.BuyerId);
        if (buyer is null)
        {
            return Result<AddedExpenseDto?>.Failure("Buyer not found.");
        }

        var group = await groupRepository.GetByIdAsync(request.GroupId);
        if (group is null)
        {
            return Result<AddedExpenseDto?>.Failure("Group not found.");
        }

        var currency = await currencyRepository.GetDefaultCurrency();
        var expense = new Expense(request.Amount, request.Title, request.Description, group, buyer, currency);

        var strategy = ChooseStrategy(request, expense);
        strategy.Split();

        await expenseRepository.AddAsync(expense);
        await _unit.SaveChanges();

        return Result<AddedExpenseDto?>.Success(null);
    }

    private static ISplitStrategy ChooseStrategy(AddTransactionRequest request, Expense expense)
    {
        if (request.Splits.Any())
        {
            return new CustomSplitStrategy(expense);
        }

        return new EqualSplitStrategy(expense);
    }
}

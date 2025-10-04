using Application.Features.Transactions.AddTransaction.Dto;
using Application.Service;
using Domain.Models;
using Domain.RepoInterfaces;
using IOU1.Application.Features.Transactions.AddTransaction.Mapper;
using IOU1.Application.Features.Transactions.AddTransaction.Request;
using IOU1.Application.Strategy;
using IOU1.Domain.Entities;
using IOU1.Domain.RepoInterfaces;
using IOU1.Domain.UnitOfWork;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Service;

public class ExpenseService(IUnitOfWork unit, IOU1Context context) : IExpenseService
{
    private readonly IUnitOfWork _unit = unit;

    private readonly IOU1Context _context = context;

    public async Task<Result<AddedExpenseDto?>> AddExpense(AddTransactionRequest request)
    {
        var expenseRepository = _unit.Get<IExpenseRepository>();
        var groupRepository = _unit.Get<IGroupRepository>();
        var userRepository = _unit.Get<IUserRepository>();
        var currencyRepository = _unit.Get<ICurrencyRepository>();


        //Use methods to retrive data which are missing
        var buyer = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.BuyerId);
        if (buyer is null)
        {
            return Result<AddedExpenseDto?>.Failure("Buyer not found.");
        }

        var group = await _context.Groups
            .Include(g => g.Members)
                .ThenInclude(m => m.User)
            .Include(g => g.Members)
            .FirstOrDefaultAsync();

        if (group is null)
        {
            return Result<AddedExpenseDto?>.Failure("Group not found.");
        }

        var currency = await currencyRepository.GetDefaultCurrency();
        var expense = new Expense(request.Amount, request.Title, request.Description, group, buyer, currency, request.Splits.MapToDto());

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

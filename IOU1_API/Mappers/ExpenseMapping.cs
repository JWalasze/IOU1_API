using Domain.Entities;
using IOU1_API.DTOs;

namespace IOU1_API.Mappers;

public static class ExpenseMapping
{
    public static ExpenseDto ToDto(this Expense expense)
    {
        return new ExpenseDto(
            Id: expense.Id,
            TotalAmount: expense.TotalAmount,
            Title: expense.Title,
            Description: expense.Description,
            GroupId: expense.Group.Id,
            GroupName: expense.Group.Description,
            BuyerId: expense.Buyer.Id,
            BuyerName: $"{expense.Buyer.FirstName}",
            CurrencyCode: expense.Currency.Name,
            Transactions: expense.Transactions.ToDtoList()
        );
    }

    public static List<ExpenseDto> ToDtoList(this IEnumerable<Expense> txs)
        => [.. txs.Select(t => t.ToDto())];
}

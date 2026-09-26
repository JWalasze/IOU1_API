using IOU1.Domain.Entities;
using IOU1.Domain.Enums;

namespace IOU1.Application.Services.Settlements;

public interface ISettlementService
{
    Task<List<ExpenseShareSettlement>> SettleBasedOnNewExpense(
        SearchExpense searchExpense,
        Expense newExpense,
        CancellationToken cancellationToken = default);
}

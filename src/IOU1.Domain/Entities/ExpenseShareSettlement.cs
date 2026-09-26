using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public class ExpenseShareSettlement : Entity
{
    public int ExpenseShareId { get; private set; }
    public ExpenseShare ExpenseShare { get; private set; } = null!;

    public int SettlementId { get; private set; }
    public Settlement Settlement { get; private set; } = null!;

    #region Ctor
    private ExpenseShareSettlement() { }

    private ExpenseShareSettlement(ExpenseShare expenseShare, Settlement settlement)
    {
        ArgumentNullException.ThrowIfNull(expenseShare, nameof(expenseShare));
        ArgumentNullException.ThrowIfNull(settlement, nameof(settlement));
    }
    #endregion

    #region Factories
    public static ExpenseShareSettlement Create(
        ExpenseShare expenseShare, Settlement settlement)
        => new(expenseShare, settlement);
    #endregion
}

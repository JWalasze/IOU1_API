namespace IOU1.Domain.Models;

public class GroupExpenseSummary
{
    public long GroupId { get; set; }
    public IEnumerable<Debt> GroupDebts { get; } = [];

    public GroupExpenseSummary(long groupId, IEnumerable<Debt> groupDebts)
    {
        GroupId = groupId;
        GroupDebts = groupDebts;
    }
};

public class Debt
{
    public long DebtorId { get; set; }
    public string DebtorName { get; set; }
    public long CreditorId { get; set; }
    public string CreditorName { get; set; }
    public decimal Amount { get; set; }

    public Debt(
        long debtorId,
        string debtorName,
        long creditorId,
        string creditorName,
        decimal amount)
    {
        DebtorId = debtorId;
        DebtorName = debtorName;
        CreditorId = creditorId;
        CreditorName = creditorName;
        Amount = amount;
    }
};

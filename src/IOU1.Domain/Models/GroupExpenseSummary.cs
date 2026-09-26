namespace IOU1.Domain.Models;

public class GroupExpenseSummary
{
    public int GroupId { get; set; }
    public IEnumerable<Debt> GroupDebts { get; } = [];

    public GroupExpenseSummary(int groupId, IEnumerable<Debt> groupDebts)
    {
        GroupId = groupId;
        GroupDebts = groupDebts;
    }
};

public class Debt
{
    public int DebtorId { get; set; }
    public string DebtorName { get; set; }
    public int CreditorId { get; set; }
    public string CreditorName { get; set; }
    public decimal Amount { get; set; }

    public Debt(
        int debtorId,
        string debtorName,
        int creditorId,
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

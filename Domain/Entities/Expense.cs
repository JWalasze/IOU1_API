using IOU1.Domain.Base;
using IOU1.Domain.Constants.Expense;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Models;
using IOU1.Domain.Services.Splits;
using IOU1.Domain.Utils;

namespace IOU1.Domain.Entities;

public sealed class Expense : Entity
{
    public const int MaxTitleLength = 50;
    public const int MaxDescriptionLength = 255;

    public string Title { get; } = null!;
    public string? Description { get; } = null!;
    public decimal Amount { get; }

    public DateTime CreatedAt { get; }
    public bool IsDeleted { get; private set; }
    public bool IsSettled { get; private set; }

    public int GroupId { get; }
    public Group Group { get; } = null!;

    public int PayerId { get; }
    public GroupMember Payer { get; } = null!;

    public int CategoryId { get; }
    public ExpenseCategory Category { get; } = null!;

    public int SplitId { get; }
    public ExpenseSplit Split { get; } = null!;

    private readonly List<ExpenseShare> _shares = [];
    public IReadOnlyCollection<ExpenseShare> Shares => _shares;

    private readonly List<Split> _splits = [];
    public IReadOnlyCollection<Split> Splits => _splits;

    private Expense() { }

    private Expense(
        string title,
        string? description,
        decimal amount,
        Group group,
        GroupMember payer,
        IEnumerable<Split> splits,
        ISplitStrategy splitStrategy,
        int categoryId,
        int splitId)
    {
        GuardString.ForPresence<CreatingExpenseException>(title,
            "Title for a expense cannot be empty.");
        GuardString.ForMaxlength<CreatingExpenseException>(title, MaxTitleLength,
            "Title for a expense is too long!");
        Title = title;

        if (description is not null)
            GuardString.ForMaxlength<CreatingExpenseException>(description, MaxDescriptionLength,
                @$"Description for a expense is too long! Max length: {MaxDescriptionLength}");
        Description = description;

        GuardDecimal.ForPositive<CreatingExpenseException>(amount,
            "Amount for a expense must be greater than 0.");
        Amount = amount;
        CreatedAt = DateTime.UtcNow;

        Group = group;
        GroupId = group.Id;
        Payer = payer;
        PayerId = payer.Id;

        GuardIntId.ForPresence<CreatingExpenseException>(categoryId,
            "Category for a expense must be provided and greater than 0.");
        CategoryId = categoryId;

        GuardIntId.ForPresence<CreatingExpenseException>(splitId,
            "Split for a expense must be provided and greater than 0.");
        SplitId = splitId;

        Validate(splits);
        _splits = [.. splits];

        var expenses = splitStrategy.Split(this);

        Validate(expenses);
        _shares = [.. expenses];
    }

    #region Public Methods
    public void Delete()
    {
        if (IsDeleted)
            throw new InvalidOperationException($"Expense with id {Id} is already deleted.");

        IsDeleted = true;
    }

    public void MarkAsSettled()
    {
        if (IsSettled)
            throw new InvalidOperationException($"Expense with id {Id} is already settled.");

        IsSettled = true;
    }
    #endregion

    #region Private Methods
    private void Validate(IEnumerable<Split> splits)
    {
        if (Group.Members.Count == 0)
            throw new CreatingExpenseException($"Group members for id {Group.Id} are missing!");

        foreach (var member in splits)
            if (!Group.Members.Any(m => m.Id == member.MemberId))
                throw new InvalidOperationException($"Member {member.MemberId} doesn't belong to the group {Group.Id}");

        if (SplitId == SplitTypeId.Percentage)
        {
            var totalPercentage = splits.Sum(s => s.Percentage);
            if (totalPercentage != 100)
                throw new CreatingExpenseException($"Total percentage of splits must be 100%. Current total: {totalPercentage}%");
        }

        var totalAmount = splits.Sum(s => s.Amount);
        if (totalAmount != Amount)
            throw new CreatingExpenseException($"Total amount of splits must equal the expense amount. Current total: {totalAmount}, Expense amount: {Amount}");
    }

    private void Validate(IEnumerable<ExpenseShare> shares)
    {
        if (Group.Members.Count == 0)
            throw new CreatingExpenseException($"Group members for id {Group.Id} are missing!");

        foreach (var share in shares)
        {
            if (!Group.Members.Any(m => m.Id == share.MemberId))
                throw new InvalidOperationException($"Member {share.MemberId} doesn't belong to the group {Group.Id}");
        }
    }
    #endregion

    #region Factories
    public static Expense Create(
        string title,
        string? description,
        decimal amount,
        Group group,
        GroupMember payer,
        IEnumerable<Split> splits,
        ISplitStrategy splitStrategy,
        int categoryId,
        int splitId) =>
            new(title, description, amount, group, payer, splits, splitStrategy, categoryId, splitId);
    #endregion
}

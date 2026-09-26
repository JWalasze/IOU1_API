using IOU1.Domain.Base;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Utils;

namespace IOU1.Domain.Entities;

public class ExpenseSplit : Entity
{
    private const int _maxTitleLength = 20;
    private const int _maxDescriptionLength = 255;
    private const int _maxIconKeyLength = 20;

    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string? IconKey { get; private set; }

    public DateTime CreatedAt { get; }
    public bool IsDeletd { get; private set; }

    private ExpenseSplit() { }

    public ExpenseSplit(
        string title,
        string description,
        string? iconKey)
    {
        GuardString.ForPresence<CreatingExpenseSplitException>(title, "Title for expense split cannot be empty.");
        GuardString.ForMaxlength<CreatingExpenseSplitException>(title, _maxTitleLength,
            "Title for a expense split is too long!");

        Title = title;

        GuardString.ForMaxlength<CreatingExpenseSplitException>(description, _maxDescriptionLength,
            @$"Description for a expense split is too long! Max length: {_maxDescriptionLength}");

        Description = description;

        if (iconKey is not null)
            GuardString.ForMaxlength<CreatingExpenseSplitException>(description, _maxIconKeyLength,
            @$"Description for a expense split is too long! Max length: {_maxIconKeyLength}");

        IconKey = iconKey;
        IsDeletd = false;
    }

    public void Delete()
    {
        if (IsDeletd)
            throw new DeletingExpenseSplitException($"Expense split with id: {Id} is already deleted!");

        IsDeletd = true;
    }
}

using IOU1.Domain.Base;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Utils;

namespace IOU1.Domain.Entities;

public class ExpenseCategory : Entity
{
    private const int _maxTitleLength = 20;
    private const int _maxDescriptionLength = 255;
    private const int _maxIconKeyLength = 20;

    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public string? IconKey { get; private set; }

    public int? GroupId { get; }
    public Group? Group { get; }

    public DateTime CreatedAt { get; }
    public bool IsDeletd { get; private set; }

    private ExpenseCategory() { }

    public ExpenseCategory(
        string title,
        string description,
        string? iconKey,
        int groupId)
    {
        GuardString.ForPresence<CreatingExpenseCategoryException>(title,
            "Title for expense category cannot be empty.");
        GuardString.ForMaxlength<CreatingExpenseCategoryException>(title, _maxTitleLength,
            "Title for a expense category is too long!");

        Title = title;

        GuardString.ForMaxlength<CreatingExpenseCategoryException>(description, _maxDescriptionLength,
            @$"Description for a expense category is too long! Max length: {_maxDescriptionLength}");

        Description = description;

        GuardIntId.ForPresence<CreatingExpenseCategoryException>(groupId,
            "GroupId for a expense category cannot be empty.");

        GroupId = groupId;

        if (iconKey is not null)
            GuardString.ForMaxlength<CreatingExpenseCategoryException>(description, _maxIconKeyLength,
            @$"Description for a expense category is too long! Max length: {_maxIconKeyLength}");

        IconKey = iconKey;
        IsDeletd = false;
    }

    public void Delete()
    {
        if (IsDeletd)
            throw new DeletingExpensecategoryException($"Expense category with id: {Id} is already deleted!");

        IsDeletd = true;
    }

    public void SetTitle(string newTitle)
    {
        GuardString.ForPresence<CreatingExpenseCategoryException>(newTitle, "Title cannot be empty.");
        GuardString.ForMaxlength<CreatingExpenseCategoryException>(newTitle, _maxTitleLength,
            "Title for a expense category is too long!");

        Title = newTitle;
    }

    public void SetDescription(string newDescription)
    {
        GuardString.ForMaxlength<CreatingExpenseCategoryException>(newDescription, _maxDescriptionLength,
            @$"Description for a expense category is too long! Max length: {_maxDescriptionLength}");

        Description = newDescription;
    }
}

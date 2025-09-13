using Domain.Base;

namespace Domain.Entities;

public class Currency : Entity
{
    public long Id { get; }

    public string Name { get; } = null!;

    private Currency() { }

    public Currency(long id, string name)
    {
        Id = id;
        Name = name;
    }
}

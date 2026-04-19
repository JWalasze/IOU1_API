using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public class Currency : Entity
{
    public string Key { get; } = null!;

    public Currency(string key)
    {
        Key = key;
    }

    private Currency() { }
}

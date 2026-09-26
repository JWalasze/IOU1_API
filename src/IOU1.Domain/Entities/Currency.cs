namespace IOU1.Domain.Entities;

public class Currency
{
    public string Key { get; } = null!;
    public byte[] Version { get; private set; } = null!;

    public Currency(string key)
    {
        Key = key;
    }

    private Currency() { }
}

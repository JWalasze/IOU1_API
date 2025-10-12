using Domain.Base;

namespace IOU1.Domain.ValueObjects;

public record InvitationKey : ValueObject
{
    public string Key { get; }
    
    public InvitationKey(string key)
    {
        if (!string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Invitation key cannot be null or empty!");

        Key = key;
    }

    public InvitationKey()
    {
        Key = Guid.NewGuid().ToString("N");
    }
}

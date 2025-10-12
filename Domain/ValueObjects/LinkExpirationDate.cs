using Domain.Base;

namespace IOU1.Domain.ValueObjects;

public record LinkExpirationDate : ValueObject
{
    public DateTime ExpirationDate { get; }

    public LinkExpirationDate(DateTime expirationDate)
    {
        if (expirationDate <= DateTime.Now)
            throw new ArgumentException("Expiration date must be in the future.", nameof(expirationDate));

        ExpirationDate = expirationDate;
    }

    public LinkExpirationDate(TimeSpan timeToExpire)
    {
        if (timeToExpire <= TimeSpan.Zero)
            throw new ArgumentException("Time to expire must be greater than zero.", nameof(timeToExpire));

        var expirationDate = DateTime.Now.Add(timeToExpire);
        if (expirationDate <= DateTime.Now)
            throw new ArgumentException("Calculated expiration date must be in the future.", nameof(timeToExpire));

        ExpirationDate = expirationDate;
    }

    public bool IsExpired() => DateTime.Now > ExpirationDate;
}

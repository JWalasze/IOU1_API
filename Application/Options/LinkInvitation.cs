namespace IOU1.Application.Options;

public sealed record LinkInvitation
{
    public TimeSpan ExpirationTime { get; init; }

    public bool Enabled { get; init; }
}

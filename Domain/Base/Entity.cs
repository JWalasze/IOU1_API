namespace IOU1.Domain.Base;

public abstract class Entity
{
    public int Id { get; protected set; }
    public byte[] Version { get; protected set; } = null!;
}

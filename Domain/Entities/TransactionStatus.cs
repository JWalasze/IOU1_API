using Domain.Base;

namespace Domain.Entities;

public class TransactionStatus : Entity
{
    public long Id { get; }
    public string Name { get; } = null!;

    private TransactionStatus() { }

    public TransactionStatus(long id, string name)
    {
        Id = id;
        Name = name;
    }
}

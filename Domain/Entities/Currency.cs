using IOU1.Domain.Base;
using System.Text.RegularExpressions;

namespace IOU1.Domain.Entities;

public class Currency : Entity
{
    public long Id { get; }

    public string Name { get; } = null!;

    public Group? Group { get; }

    private Currency() { }
}

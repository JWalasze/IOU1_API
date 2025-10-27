namespace IOU1.Domain.Models;

public record Problem
{
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required string StatusCode { get; init; }

    public required IEnumerable<ProblemItem> Errors { get; init; } = [];
}

public record ProblemItem
{
    public required string Code { get; init; }

    public required string Message { get; init; }
}

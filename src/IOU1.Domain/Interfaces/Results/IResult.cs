namespace IOU1.Domain.Interfaces.Results;

public interface IResult
{
    bool IsSuccess { get; }

    string? ErrorMessage { get; }

    string? ErrorCode { get; }
}

namespace IOU1.Domain.Interfaces;

public interface IResult
{
    bool IsSuccess { get; }

    string? ErrorMessage { get; }

    string? ErrorCode { get; }
}

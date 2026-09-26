using IOU1.Domain.Interfaces;

namespace IOU1.Domain.Models.Results;

public class Result : IResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public IEnumerable<ProblemDetails> Errors { get; } = [];

    public string? ErrorCode => throw new NotImplementedException();

    private Result(bool isSuccess, string? errorMessage)
    {
        if (isSuccess && !string.IsNullOrWhiteSpace(errorMessage))
            throw new InvalidResultState($"{nameof(Result)} cannot be successed with errorMessage: {errorMessage}");

        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string errorMessage) => new(false, errorMessage);
}

public class InvalidResultState(string errorMessage) : Exception(errorMessage);

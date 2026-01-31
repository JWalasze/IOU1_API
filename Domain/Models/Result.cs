using IOU1.Domain.Interfaces;

namespace IOU1.Domain.Models;

public class Result<T> : IResult<T> where T : class?
{
    public T? Data { get; init; }

    public bool IsSuccess { get; init; }

    public IEnumerable<ProblemDetails> Errors { get; } = [];

    //TODO To be removed
    public string? ErrorCode => null;

    //TODO To be removed
    public string? ErrorMessage { get; init; }


    private Result(T data, bool isSuccess, string? errorMessage)
    {
        Data = data;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    private Result(IEnumerable<ProblemDetails> errors)
    {
        Errors = errors;
    }

    public static Result<T> Success(T data) => new(data, true, null);
    public static Result<T> Failure(string errorMessage) => new(default!, false, errorMessage);
    public static Result<T> Failure(IEnumerable<ProblemDetails> errors) => new(errors);
}

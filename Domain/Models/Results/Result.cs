using FluentValidation.Results;
using IOU1.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace IOU1.Domain.Models.Results;

public class Result<T> : IResult<T> where T : class?
{
    public T Data { get; init; }

    public bool IsSuccess { get; init; }

    public IEnumerable<ProblemDetails> Errors { get; } = [];

    [JsonIgnore]
    public Exception? Exception { get; init; }

    public string? ErrorCode => Errors.FirstOrDefault()?.Code;
    public string? ErrorMessage => Errors.FirstOrDefault()?.Message;

    private Result(T data, bool isSuccess, string? errorMessage)
    {
        Data = data;
        IsSuccess = isSuccess;
        if (!string.IsNullOrWhiteSpace(errorMessage))
            Errors = [new ProblemDetails(null!, errorMessage)];
    }

    private Result(T data, bool isSuccess, string errorCode, string errorMessage)
    {
        Data = data;
        IsSuccess = isSuccess;
        Errors = [new ProblemDetails(errorCode, errorMessage)];
    }

    private Result(T data, bool isSuccess, string? errorMessage, Exception ex)
    {
        Data = data;
        IsSuccess = isSuccess;
        if (!string.IsNullOrWhiteSpace(errorMessage))
            Errors = [new ProblemDetails(null!, errorMessage)];
        Exception = ex;
    }

    private Result(IEnumerable<ProblemDetails> errors)
    {
        Errors = errors;
    }

    public static implicit operator bool(Result<T> result) => result.IsSuccess && result.Data is not null;

    public static Result<T> Success(T data) => new(data, true, null);
    public static Result<T> Failure(string errorMessage) => new(default!, false, errorMessage);
    public static Result<T> Failure(string errorCode, string errorMessage) => new(default!, false, errorCode, errorMessage);
    public static Result<T> Failure(Exception ex, string errorMessage) => new(default!, false, errorMessage, ex);
    public static Result<T> Failure(IEnumerable<ProblemDetails> errors) => new(errors);
    public static Result<T> Failure(IEnumerable<ValidationFailure> errors)
    {
        var problemDetails = errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode));
        return Result<T>.Failure(problemDetails);
    }
}

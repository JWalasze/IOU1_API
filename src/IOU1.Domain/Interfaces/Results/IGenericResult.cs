namespace IOU1.Domain.Interfaces.Results;

public interface IResult<T> : IResult
{
    T Data { get; init; }
}

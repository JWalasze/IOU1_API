namespace IOU1.Domain.Interfaces;

public interface IResult<T> : IResult
{
    T Data { get; init; }
}

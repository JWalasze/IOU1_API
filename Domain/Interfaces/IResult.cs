using IOU1.Domain.Models;

namespace IOU1.Domain.Interfaces;

public interface IResult
{
    //TODO Data property?

    bool IsSuccess { get; }

    string? ErrorMessage { get; }
}

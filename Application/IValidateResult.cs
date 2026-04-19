using IOU1.Domain.Models.Results;

namespace IOU1.Application;

public interface IValidateResult
{
    bool IsValid { get; }

    ICollection<ProblemDetails> Errors { get; }
}

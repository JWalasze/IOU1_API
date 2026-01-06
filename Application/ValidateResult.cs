using IOU1.Domain.Models;

namespace IOU1.Application;

public class ValidateResult : IValidateResult
{
    public bool IsValid => Errors.Count == 0;

    public ICollection<ProblemDetails> Errors { get; } = [];
}

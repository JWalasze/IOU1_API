using Application.Mediator;
using FluentValidation.Results;
using IOU1.Domain.Interfaces;

namespace IOU1.Application.Mediator;

public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest where TResponse : IResponse
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await Validate(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return MapFailureValidationResult(validationResult);
        }

        var result = await Do(request);
        return (result is not null && result.IsSuccess) switch
        {
            true => MapSuccess(result),
            false => MapFailure(result)
        };
    }

    //TODO Validate should't be coupled with ValidationResult but with some abstraction
    protected abstract Task<ValidationResult> Validate(TRequest request, CancellationToken cancellationToken = default); //TODO Should have base implementation but more tricky,  add a virtual keyword

    protected abstract TResponse MapFailureValidationResult(ValidationResult result); //TODO Should have base implementation, add a virtual keyword

    protected abstract TResponse MapSuccess(IResult result); //TODO Should have base implementation, add a virtual keyword

    protected abstract TResponse MapFailure(IResult? result); //TODO Should have base implementation,  add a virtual keyword

    protected abstract Task<IResult> Do(IRequest request); //Stays abstract
}

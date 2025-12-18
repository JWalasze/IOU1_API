using Application.Mediator;
using FluentValidation;
using FluentValidation.Results;
using IOU1.Domain.Interfaces;
using IOU1.Domain.Models;
using MapsterMapper;

namespace IOU1.Application.Mediator;

public abstract class RequestHandler<TRequest, TResponse>(IValidator<TRequest> validator, IMapper mapper) : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest where TResponse : IResponse, new()
{
    protected readonly IValidator<TRequest> _validator = validator;
    protected readonly IMapper _mapper = mapper;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await Validate(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return MapFailure(validationResult);
        }

        var result = await Do(request, cancellationToken);
        return result.IsSuccess switch
        {
            true => MapSuccess(result),
            false => MapFailure(result)
        };
    }

    //TODO Validate should't be coupled with ValidationResult but with some abstraction
    protected virtual async Task<ValidationResult> Validate(TRequest request, CancellationToken cancellationToken = default)
    {
        return await _validator.ValidateAsync(request, cancellationToken);
    }

    protected virtual TResponse MapFailure(ValidationResult result)
    {
        return new()
        {
            Errors = [..result.Errors.Select(e => new ProblemDetails(e.ErrorCode, e.ErrorMessage))]
        };
    }

    protected virtual TResponse MapSuccess(IResult result)
    {
        return _mapper.Map<TResponse>(result);
    }

    protected virtual TResponse MapFailure(IResult result)
    {
        return _mapper.Map<TResponse>(result);
    }

    protected abstract Task<IResult> Do(TRequest request, CancellationToken cancellationToken = default);
}

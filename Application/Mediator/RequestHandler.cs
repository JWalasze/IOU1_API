using FluentValidation;
using IOU1.Domain.Interfaces;
using IOU1.Domain.Models.Results;
using Mapster;
using MapsterMapper;

namespace IOU1.Application.Mediator;

public abstract class RequestHandler<TRequest, TResponse>(IValidator<TRequest> validator, IMapper mapper)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : class, IRequest
    where TResponse : class, IResponse
{
    protected readonly IValidator<TRequest> _validator = validator;
    protected readonly IMapper _mapper = mapper;

    public async Task<IHandlerResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = Validate(request);
        if (!validationResult.IsValid)
        {
            //Za dużo mapowania
            return MapFailure(validationResult);
        }

        var result = await Do(request, cancellationToken);
        //Za dużo mapowania
        return result.IsSuccess switch
        {
            true => MapSuccess(result),
            false => MapFailure(result)
        };
    }


    //TODO ValidateAsync to overhead zbędny nakład pracy żeby opakować w Task
    protected virtual IValidateResult Validate(TRequest request)
    {
        var validationResult = _validator.Validate(request);
        return new ValidateResult()
        {
            Errors = [.. validationResult.Errors.Select(e => new ProblemDetails(e.ErrorCode, e.ErrorMessage))],
        };
    }

    protected virtual IHandlerResponse<TResponse> MapFailure(IValidateResult result)
    {
        return new HandlerResponse<TResponse>
        {
            Data = null,
            Errors = result.Errors
        };
    }

    protected virtual IHandlerResponse<TResponse> MapSuccess(IResult result)
    {
        var handlerResponseData = result.Adapt<TResponse>();
        return new HandlerResponse<TResponse>
        {
            Data = handlerResponseData,
            Errors = []
        };
    }

    protected virtual IHandlerResponse<TResponse> MapFailure(IResult result)
    {
        return new HandlerResponse<TResponse>
        {
            Data = null,
            Errors = [new ProblemDetails
            (
                result.ErrorCode ?? "UNKNOWN_ERROR",
                result.ErrorMessage ?? "An unknown error occurred."
            )]
        };
    }

    protected abstract Task<IResult> Do(TRequest request, CancellationToken cancellationToken = default);
}

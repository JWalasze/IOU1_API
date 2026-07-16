using FluentValidation;
using IOU1.Application.Features.Groups.GetGroupSummary.Models.Dto;
using IOU1.Application.Features.Groups.GetGroupSummary.Models.Request;
using IOU1.Application.Services.Groups.Summary;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Groups.GetGroupSummary.Handler;

public sealed class GetGroupSummaryHandler(
    IValidator<GetGroupSummaryRequest> validator,
    IGroupSummaryService groupService) :
    IGetGroupSummaryHandler
{
    private readonly IValidator<GetGroupSummaryRequest> _validator = validator;
    private readonly IGroupSummaryService _groupService = groupService;

    public async Task<Result<GetGroupSummaryResponse?>> Handle(GetGroupSummaryRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<GetGroupSummaryResponse?>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }



        throw new NotImplementedException();
    }
}

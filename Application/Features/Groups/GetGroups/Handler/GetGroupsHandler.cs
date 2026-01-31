using FluentValidation;
using IOU1.Application.Features.Groups.GetGroups.Models.Dto;
using IOU1.Application.Features.Groups.GetGroups.Models.Request;
using IOU1.Application.Features.Groups.GetGroups.Query;
using IOU1.Domain.Models;

namespace IOU1.Application.Features.Groups.GetGroups.Handler;

public class GetGroupsHandler(
    IGetGroupsQuery repository,
    IValidator<GetGroupsRequest> validator,
    IAuthUser authUser) : IGetGroupsHandler
{
    private readonly IAuthUser _authUser = authUser;
    private readonly IValidator<GetGroupsRequest> _validator = validator;
    private readonly IGetGroupsQuery _repository = repository;

    public async Task<Result<ICollection<GetGroupsDto>>> Handle(GetGroupsRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<ICollection<GetGroupsDto>>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }

        var groups = await _repository.GetGroups(_authUser.Id, cancellationToken);
        return Result<ICollection<GetGroupsDto>>.Success(groups);
    }
}

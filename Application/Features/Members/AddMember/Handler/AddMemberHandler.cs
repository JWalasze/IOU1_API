using FluentValidation;
using IOU1.Application.Features.Members.AddMember.Models;
using IOU1.Application.Services.Members;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Models.Results;
using IOU1.Domain.UnitOfWork;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Features.Members.AddMember.Handler;

public class AddMemberHandler(
    IValidator<AddMemberRequest> validator,
    IAuthUser authUser,
    IMemberService memberService,
    IUnitOfWork unit,
    IOU1Context context) : IAddMemberHandler
{
    private readonly IValidator<AddMemberRequest> _validator = validator;
    private readonly IAuthUser _authUser = authUser;
    private readonly IMemberService _memberService = memberService;
    private readonly IUnitOfWork _unit = unit;
    private readonly IOU1Context _context = context;

    public async Task<Result<AddMemberDto?>> Handle(AddMemberRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<AddMemberDto?>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }

        var group = await _context.Groups
            .Include(g => g.Members)
            .Where(g => g.Id == request.GroupId)
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            return Result<AddMemberDto?>.Failure(
                $"Group with ID: {request.GroupId} doesn't exist.");
        }

        var isMemberOfGroup = await _memberService.IsMemberOfGroup(
            request.GroupId,
            _authUser.Id,
            cancellationToken);

        if (!isMemberOfGroup)
        {
            return Result<AddMemberDto?>.Failure(
                $"{_authUser.Id} is not a member of group {request.GroupId} so new user cannot be added: {request.UserId}");
        }

        var addedMemberResult = await _memberService.AddMember(
            request.GroupId,
            request.UserId,
            cancellationToken);

        if (!addedMemberResult.IsSuccess)
        {
            var errorMessage = addedMemberResult.ErrorMessage
                ?? $"Error occured while adding new member {request.GroupId} to the group {request.GroupId}.";

            return Result<AddMemberDto?>.Failure(errorMessage);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<AddMemberDto?>.Success(new(
            request.UserId,
            request.GroupId,
            addedMemberResult.Data!.Id));
    }
}

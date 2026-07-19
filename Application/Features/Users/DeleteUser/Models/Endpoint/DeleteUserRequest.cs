using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Users.DeleteUser.Models.Endpoint;

public record DeleteUserRequest(int UserId) : IRequest;

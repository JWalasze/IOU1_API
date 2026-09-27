using IOU1.Application.Features.Auth;
using IOU1.Application.Persistance;
using IOU1.Application.Services.Balances;
using IOU1.Application.Services.Expenses;
using IOU1.Application.Services.Groups;
using IOU1.Application.Services.Groups.Summary;
using IOU1.Application.Services.Invitations.General;
using IOU1.Application.Services.Members;
using IOU1.Application.Services.Notifications;
using IOU1.Application.Services.Settlements;
using IOU1.Application.Services.Users;
using IOU1.Application.Services.Users.Checker;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Services.Users;
using IOU1.Infrastructure.Auth;
using IOU1.Infrastructure.Notifications;
using IOU1.Infrastructure.UnitOfWork;

namespace IOU1.Api.Pipelines;

public static class ScopedServicesPipeline
{
    public static void AddScopedServicesPipeline(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IExpenseService, ExpenseService>();
        builder.Services.AddScoped<IGroupService, GroupService>();
        builder.Services.AddScoped<IInvitationLinkService, InvitationLinkService>();

        builder.Services.AddScoped<IBalanceService, BalanceService>();
        builder.Services.AddScoped<IMemberService, MemberService>();
        builder.Services.AddScoped<ISettlementService, SettlementService>();
        builder.Services.AddScoped<IGroupSummaryService, GroupSummaryService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();

        builder.Services.AddScoped<IAuthUser, AuthUser>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserChecker, UserChecker>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        //builder.Services.AddScoped<IServiceBus, ServiceBus>();
    }
}

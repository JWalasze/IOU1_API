using Domain.RepoInterfaces;
using FluentAssertions;
using IOU1.Application.Services.Groups;
using IOU1.Application.Services.Members;
using IOU1.Domain.RepoInterfaces;
using IOU1.Infrastructure.Repositories;
using IOU1.Infrastructure.UnitOfWork;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Tests;
using Xunit.Abstractions;

namespace IOU1.Tests;

public class TempTests
{
    private readonly IOU1Context _context;
    private readonly IServiceProvider _serviceProvider;

    private readonly ITestOutputHelper _output;

    public TempTests(ITestOutputHelper output)
    {
        _output = output;

        var config = TestConfig.InitConfiguration();
        var connectionString = config.GetConnectionString("DefaultConnection");

        var options = new DbContextOptionsBuilder<IOU1Context>()
            .UseSqlServer(connectionString)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .LogTo(_output.WriteLine,
                   [
                       DbLoggerCategory.Database.Command.Name,
                       DbLoggerCategory.Update.Name
                   ],
                   LogLevel.Information,
                   DbContextLoggerOptions.UtcTime)
            .Options;

        _context = new IOU1Context(options);
        var serviceProviderMock = new Mock<IServiceProvider>();
        _serviceProvider = serviceProviderMock.Object;

        var services = new ServiceCollection();
        services.AddScoped<IUserRepository>(x => new UserRepository(_context));
        services.AddScoped<IGroupRepository>(x => new GroupRepository(_context));
        services.AddScoped<IExpenseRepository>(x => new ExpenseRepository(_context));
        services.AddScoped<ICurrencyRepository>(x => new CurrencyRepository(_context));

        _serviceProvider = services.BuildServiceProvider();
    }

    //VERY TEMP...
    [Fact]
    public async Task EfCoreConfigurations_TempTest_ReturnsProperDataFromDB()
    {
        var xx = await _context.Users.Include(u => u.OwnedGroups).ToListAsync();
        var zz = await _context.GroupMembers.Include(gm => gm.User).Include(gm => gm.Group).ToListAsync();
        var xxx = await _context.Users.Include(u => u.MemberGroups).ThenInclude(gm => gm.Group).ToListAsync();
        var yy = await _context.Groups.Include(g => g.Members).ThenInclude(gm => gm.User).ToListAsync();
        var a = await _context.Currencies.ToListAsync();
        var c = await _context.ExpenseShares.ToListAsync();
        var cc = await _context.ExpenseShares
            .Include(es => es.Member)
            .Include(es => es.Expense)
            .ToListAsync();
    }

    [Fact]
    public async Task GroupService_AddGroup_CreatesGroup()
    {
        //Arrange
        var unit = new UnitOfWork(_context, _serviceProvider);
        var sut = new GroupService(_context, null);

        //Act
        var result = await sut.AddGroup([1, 2, 3], 1, "Nazwa", "TextTextText", "PLN");

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Data?.GroupId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CheckHowEFAnyAsyncQueryLooksLike()
    {
        var memberService = new MemberService(_context);
        await memberService.IsMemberOfGroup(1, 1);
    }
}
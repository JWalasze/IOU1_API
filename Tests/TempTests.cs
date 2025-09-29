using Application.Features.AddGroup.Service;
using Domain.Entities;
using Domain.RepoInterfaces;
using FluentAssertions;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using System;
using Xunit.Abstractions;

namespace Tests;

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
            .EnableSensitiveDataLogging()   // include parameter values (tests only)
            .EnableDetailedErrors()
            .LogTo(_output.WriteLine,       // send to xUnit output
                   new[]
                   {
                       DbLoggerCategory.Database.Command.Name, // SQL + params
                       DbLoggerCategory.Update.Name            // INSERT/UPDATE details
                   },
                   LogLevel.Information,
                   DbContextLoggerOptions.SingleLine |
                   DbContextLoggerOptions.UtcTime) // show @p0 = 123, etc.
            .Options;

        _context = new IOU1Context(options);
        var serviceProviderMock = new Mock<IServiceProvider>();
        _serviceProvider = serviceProviderMock.Object;

        var services = new ServiceCollection();
        services.AddScoped<IUserRepository>(x => new UserRepository(_context));
        services.AddScoped<IGroupRepository>(x => new GroupRepository(_context));

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
        var c = await _context.Transactions.ToListAsync();
        var cc = await _context.Transactions
            .Include(t => t.Borrower)
            .Include(t => t.Currency)
            .Include(t => t.Buyer)
            .ToListAsync();
    }

    [Fact]
    public async Task GroupService_AddGroup_CreatesGroup()
    {
        //Arrange
        var unit = new UnitOfWork(_context, _serviceProvider);
        var sut = new GroupService(unit);

        //Act
        var result = await sut.AddGroup([1,2,3], 1, "TextTextText");

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Data?.GroupId.Should().BeGreaterThan(0);
    }
}
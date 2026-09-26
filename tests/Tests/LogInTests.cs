using Domain.RepoInterfaces;
using FluentAssertions;
using IOU1.Application.Features.Auth;
using IOU1.Application.Options;
using IOU1.Domain.Entities;
using IOU1.Domain.Models.Auth;
using IOU1.Infrastructure.Auth;
using IOU1.Infrastructure.Repositories;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using Tests;
using Xunit.Abstractions;

namespace IOU1.Tests;

public class LogInTests
{
    private readonly IOU1Context _context;
    private readonly DbContextOptions<IOU1Context> _options;
    private readonly IServiceProvider _serviceProvider;

    private readonly Mock<IOptions<JwtConfig>> _jwtOptionsMock = new();

    private readonly ITestOutputHelper _output;

    public LogInTests(ITestOutputHelper output)
    {
        _output = output;

        var config = TestConfig.InitConfiguration();
        var connectionString = config.GetConnectionString("DefaultConnection");

        _options = new DbContextOptionsBuilder<IOU1Context>()
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

        _context = new IOU1Context(_options);
        var serviceProviderMock = new Mock<IServiceProvider>();
        _serviceProvider = serviceProviderMock.Object;

        var services = new ServiceCollection();
        services.AddScoped<IUserRepository>(x => new UserRepository(_context));

        _serviceProvider = services.BuildServiceProvider();

        _jwtOptionsMock.Setup(x => x.Value).Returns(Options.Create(new JwtConfig
        {
            Audience = "aud",
            ExpiryInMinutes = 60,
            Issuer = "Iss",
            SecretKey = "random-string-for-test-purpose-1"
        }).Value);
    }

    [Fact]
    public async Task LogIn_ValidCredentials_ReturnToken()
    {
        //Arrange
        var users = new List<User>
        {
            User.Create(
                firstName: "Kuba",
                lastName: "Walaszek",
                email: new Domain.ValueObjects.Email("walaszek@gmail.com"),
                login: "Rachet1234",
                passwordHash: "IYnB6dlEQMnzKC2gXTIXEFOQ/esVgQhCKWoh4Xi1AikrlD7rb6ZSUajIjpNkxhGJSKRjnDaqDoSFFm5iJ0JXwQ==",
                passwordSalt: "/K9gHMM8KTEXp47qFUFf5Y+WfKZzHw0hVMf9Ihm1lH/uIqqP2bB+P6QwGBJHBtGz9TP3cK2R8vkI+hRY99IP7w==",
                createdAt: DateTime.Now)
        };

        var ctxMock = new Mock<IOU1Context>(_options);

        ctxMock.Setup(x => x.Users)
            .ReturnsDbSet(users);

        ctxMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new AuthService(
            ctxMock.Object,
            new JwtTokenProvider(_jwtOptionsMock.Object),
            new PasswordHasher(),
            new PasswordComparer(),
            null);

        //Act
        var result = await sut.LogIn(new Credentials("Rachet1234", "moje_silne_haslo"));

        //Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data.Value.Should().NotBeNullOrWhiteSpace();
    }
}

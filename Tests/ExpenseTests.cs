using Domain.RepoInterfaces;
using FluentAssertions;
using IOU1.Application.Strategy;
using IOU1.Domain.Entities;
using IOU1.Domain.RepoInterfaces;
using IOU1.Domain.ValueObjects;
using IOU1.Infrastructure.Repositories;
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

public class ExpenseTests
{
    private static readonly Currency Pln = new("PLN");

    private readonly IOU1Context _context;
    private readonly IServiceProvider _serviceProvider;

    private readonly ITestOutputHelper _output;

    public ExpenseTests(ITestOutputHelper output)
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

    [Fact]
    public void SplitStrategy_MemberIsNotInGroup_ThrowsException()
    {
        //Arrange
        var alice = User(1, "Alice", "Nowak");
        var bob = User(2, "Bob", "Kowalski");
        var carol = User(3, "Carol", "Wiśniewska");

        var group = new Group("Nazwa", "Weekend trip to Mazury", alice, Pln, new List<User>());

        alice.OwnedGroups.Add(group);

        var gmBob = new GroupMember(group, bob);
        var gmCarol = new GroupMember(group, carol);

        group.AddNewMembers([gmBob, gmCarol]);
        bob.MemberGroups.Add(gmBob);
        carol.MemberGroups.Add(gmCarol);

        var aliceMember = group.Members.First(m => m.UserId == alice.Id);

        //Act
        var action = () =>
        {
            var sut = new Expense(100, "Tytul", "Description", group, aliceMember,
            [
                new()
                {
                    Amount = 100,
                    MemberId = 1
                },

            ], null!);
        };

        //Assert
        action.Should().Throw<Exception>("Member 1 doesn't belong to the group 15");
    }

    [Fact]
    public void SplitStrategy_SplitsAreValid_CreatesProperExpenseShares()
    {
        //Arrange
        var alice = User(1, "Alice", "Nowak");
        var bob = User(2, "Bob", "Kowalski");
        var carol = User(3, "Carol", "Wiśniewska");

        var group = new Group("Nazwa", "Weekend trip to Mazury", alice, Pln, new List<User>());

        alice.OwnedGroups.Add(group);

        var gmBob = new GroupMember(11, group, bob);
        var gmCarol = new GroupMember(12, group, carol);
        var gmAlice = new GroupMember(13, group, alice);

        group.AddNewMembers([gmBob, gmCarol, gmAlice]);
        bob.MemberGroups.Add(gmBob);
        carol.MemberGroups.Add(gmCarol);
        alice.MemberGroups.Add(gmAlice);

        var aliceMember = group.Members.First(m => m.UserId == alice.Id);

        var sut = new Expense(200, "Tytul", "Description", group, aliceMember,
        [
            new()
            {
                Amount = -100,
                MemberId = 2
            },
            new()
            {
                Amount = -50,
                MemberId = 3
            },
            new()
            {
                Amount = -50,
                MemberId = 1
            }
        ], new CustomSplitStrategy());

        //Assert
        sut.Should().NotBeNull();
        sut.ExpenseShares.Should().NotBeNullOrEmpty();
        sut.ExpenseShares.Should().NotContainNulls();
        sut.ExpenseShares.Count.Should().Be(3);
        sut.ExpenseShares.Count(es => es.Member.UserId == alice.Id).Should().Be(1);
        sut.ExpenseShares.Count(es => es.Member.UserId == bob.Id).Should().Be(1);
        sut.ExpenseShares.Count(es => es.Member.UserId == carol.Id).Should().Be(1);

        var aliceShare = sut.ExpenseShares.First(es => es.Member.UserId == alice.Id);
        var bobShare = sut.ExpenseShares.First(es => es.Member.UserId == bob.Id);
        var carolShare = sut.ExpenseShares.First(es => es.Member.UserId == carol.Id);

        aliceShare.Amount.Should().Be(50);
        bobShare.Amount.Should().Be(100);
        carolShare.Amount.Should().Be(50);
    }

    public static Email EmailOf(string local) => new($"{local}@test.local");

    public static User User(int id, string first, string last, string? login = null)
        => Domain.Entities.User.Create(
            firstName: first,
            lastName: last,
            email: EmailOf($"{first.ToLower()}.{last.ToLower()}"),
            login: login ?? $"{first.ToLower()}.{last.ToLower()}",
            passwordHash: $"HASH-{id}",
            passwordSalt: "SALT",
            DateTime.Now);

    public static (Group Group, User Owner, User[] Members) BasicGroup()
    {
        var alice = User(1, "Alice", "Nowak");
        var bob = User(2, "Bob", "Kowalski");
        var carol = User(3, "Carol", "Wiśniewska");

        var group = new Group("Nazwa", "Weekend trip to Mazury", alice, Pln, new List<User>());

        alice.OwnedGroups.Add(group);

        var gmBob = new GroupMember(group, bob);
        var gmCarol = new GroupMember(group, carol);

        group.AddNewMembers(new[] { gmBob, gmCarol });
        bob.MemberGroups.Add(gmBob);
        carol.MemberGroups.Add(gmCarol);

        return (group, alice, new[] { bob, carol });
    }

    public static List<User> NUsers(int n, int startId = 1)
    {
        var list = new List<User>(n);
        for (int i = 0; i < n; i++)
        {
            var id = startId + i;
            var first = $"User{id}";
            var last = "Test";
            list.Add(User(id, first, last));
        }
        return list;
    }

    public static (List<Group> Groups, List<User> Users) GroupsDataset()
    {
        var users = new[]
        {
            User(10, "Alice", "Nowak"),
            User(11, "Bob", "Kowalski"),
            User(12, "Carol", "Wiśniewska"),
            User(13, "Dave", "Zieliński"),
            User(14, "Eve", "Wójcik")
        }.ToList();

        Group G(string desc, User owner, params User[] members)
        {
            var g = new Group("Nazwa", desc, owner, Pln, new List<User>());
            owner.OwnedGroups.Add(g);
            var links = members.Select(u => new GroupMember(g, u)).ToArray();
            g.AddNewMembers(links);
            foreach (var (u, link) in members.Zip(links))
                u.MemberGroups.Add(link);
            return g;
        }

        var g1 = G("Weekend trip", users[0], users[1], users[2]);          // Alice owns; Bob, Carol members
        var g2 = G("Team lunch", users[1], users[0], users[3], users[4]); // Bob owns; Alice, Dave, Eve
        var g3 = G("Hackathon", users[2], users[1], users[4]);           // Carol owns; Bob, Eve

        return (new List<Group> { g1, g2, g3 }, users);
    }
}

using Domain.RepoInterfaces;
using IOU1.Domain.RepoInterfaces;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tests;
using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IOU1.Infrastructure.Repositories;
using Domain.Entities;
using IOU1.Application.Strategy;
using IOU1.Domain.Entities;
using FluentAssertions;
using Domain.ValueObjects;

namespace IOU1.Tests;

public class ExpenseTests
{
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
    public async Task SplitStrategy_MemberIsNotInGroup_ThrowsException()
    {
        //Arrange
        var alice = User(1, "Alice", "Nowak");
        var bob = User(2, "Bob", "Kowalski");
        var carol = User(3, "Carol", "Wiśniewska");

        var group = new Group(15, "Weekend trip to Mazury", alice);

        alice.OwnedGroups.Add(group);

        var gmBob = new GroupMember(group, bob);
        var gmCarol = new GroupMember(group, carol);

        group.AddNewMembers([gmBob, gmCarol]);
        bob.MemberGroups.Add(gmBob);
        carol.MemberGroups.Add(gmCarol);

        //Act
        var action = () => 
        {
            var sut = new Expense(100, "Tytul", "Description", group, alice, null,
            [
                new()
                {
                    Amount = 100,
                    MemberId = 1
                },

            ]);
        };

        //Assert
        action.Should().Throw<Exception>("Member 1 doesn't belong to the group 15");
    }

    [Fact]
    public async Task SplitStrategy_SplitsAreValid_CreatesProperTransactions()
    {
        //Arrange
        var alice = User(1, "Alice", "Nowak");
        var bob = User(2, "Bob", "Kowalski");
        var carol = User(3, "Carol", "Wiśniewska");

        var group = new Group(15, "Weekend trip to Mazury", alice);

        alice.OwnedGroups.Add(group);

        var gmBob = new GroupMember(11, group, bob);
        var gmCarol = new GroupMember(12, group, carol);
        var gmAlice = new GroupMember(13, group, alice);

        group.AddNewMembers([gmBob, gmCarol, gmAlice]);
        bob.MemberGroups.Add(gmBob);
        carol.MemberGroups.Add(gmCarol);
        alice.MemberGroups.Add(gmAlice);

        var sut = new Expense(200, "Tytul", "Description", group, alice, null,
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
        ]);

        var strategy = new CustomSplitStrategy(sut);

        //Act
        strategy.Split();

        //Assert
        sut.Should().NotBeNull();
        sut.Transactions.Should().NotBeNullOrEmpty();
        sut.Transactions.Should().NotContainNulls();
        sut.Transactions.Count.Should().Be(4);
        //ContainsSingle, Contain, AllSatisfy, OnlyContain
        //sut.Transactions.Should().BeEquivalentTo(new List<Transaction>()
        //{
        //    new(-100, DateTime.UtcNow, sut, )
        //});
    }

    public static Email EmailOf(string local) => new Email($"{local}@test.local");

    public static User User(long id, string first, string last, string? login = null)
        => new(
            id: id,
            firstName: first,
            lastName: last,
            email: EmailOf($"{first.ToLower()}.{last.ToLower()}"),
            login: login ?? $"{first.ToLower()}.{last.ToLower()}",
            hashedPassword: $"HASH-{id}");

    /// <summary>
    /// Creates a group owned by Alice with Bob & Carol as members (fully wired).
    /// </summary>
    public static (Group Group, User Owner, User[] Members) BasicGroup()
    {
        var alice = User(1, "Alice", "Nowak");
        var bob = User(2, "Bob", "Kowalski");
        var carol = User(3, "Carol", "Wiśniewska");

        var group = new Group("Weekend trip to Mazury", alice);

        // Back-references (collections are mutable)
        alice.OwnedGroups.Add(group);

        var gmBob = new GroupMember(group, bob);
        var gmCarol = new GroupMember(group, carol);

        group.AddNewMembers(new[] { gmBob, gmCarol });
        bob.MemberGroups.Add(gmBob);
        carol.MemberGroups.Add(gmCarol);

        return (group, alice, new[] { bob, carol });
    }

    /// <summary>
    /// Creates N users with deterministic data.
    /// </summary>
    public static List<User> NUsers(int n, long startId = 1)
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

    /// <summary>
    /// Creates multiple groups with overlapping membership.
    /// </summary>
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
            var g = new Group(desc, owner);
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

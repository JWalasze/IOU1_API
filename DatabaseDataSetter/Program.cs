using IOU1.Application.Strategy;
using IOU1.Domain.Entities;
using IOU1.Domain.Models;
using IOU1.Domain.Services.Crypto;
using IOU1.Domain.Services.Splits;
using IOU1.Domain.ValueObjects;
using IOU1.Infrastructure.Auth;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var services = new ServiceCollection();
services.AddDbContext<IOU1Context>(opt => opt.UseSqlServer(connectionString));
services.AddSingleton<IPasswordHasher, PasswordHasher>();

var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<IOU1Context>();
var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

if (await context.Users.AnyAsync())
{
    Console.WriteLine("Database already contains users. Skipping seed.");
    return;
}

Console.WriteLine("Seeding database...");

// --- Currencies ---
var existingKeys = await context.Currencies.Select(c => c.Key).ToListAsync();
var currencyKeys = new[] { "PLN", "EUR", "USD", "GBP" };
var newCurrencies = currencyKeys
    .Where(k => !existingKeys.Contains(k))
    .Select(k => new Currency(k))
    .ToList();

if (newCurrencies.Count > 0)
{
    context.Currencies.AddRange(newCurrencies);
    await context.SaveChangesAsync();
}

var currencies = await context.Currencies.ToDictionaryAsync(c => c.Key);
Console.WriteLine($"Currencies ready: {string.Join(", ", currencies.Keys)}");

// --- Users ---
var users = new List<User>
{
    User.Create("Alice",   "Smith",  new Email("alice@example.com"),   "alice",   "Password123!", hasher),
    User.Create("Bob",     "Jones",  new Email("bob@example.com"),     "bob",     "Password123!", hasher),
    User.Create("Charlie", "Brown",  new Email("charlie@example.com"), "charlie", "Password123!", hasher),
    User.Create("Diana",   "Prince", new Email("diana@example.com"),   "diana",   "Password123!", hasher),
    User.Create("Eve",     "Miller", new Email("eve@example.com"),     "eve",     "Password123!", hasher),
};

context.Users.AddRange(users);
await context.SaveChangesAsync();
Console.WriteLine($"Users seeded: {string.Join(", ", users.Select(u => u.Login))}");

// --- Groups ---
// "Roommates" — Alice owns, Alice + Bob + Charlie are members
var roommates = new Group(
    name: "Roommates",
    description: "Shared apartment expenses",
    owner: users[0],
    currency: currencies["PLN"],
    members: [users[0], users[1], users[2]]);

// "Weekend Trip" — Bob owns, Bob + Charlie + Diana + Eve are members
var trip = new Group(
    name: "Weekend Trip",
    description: "Prague trip 2025",
    owner: users[1],
    currency: currencies["EUR"],
    members: [users[1], users[2], users[3], users[4]]);

// "Office Lunch" — Diana owns, Diana + Alice are members
var lunch = new Group(
    name: "Office Lunch",
    description: null,
    owner: users[3],
    currency: currencies["PLN"],
    members: [users[3], users[0]]);

context.Groups.AddRange(roommates, trip, lunch);
await context.SaveChangesAsync();
Console.WriteLine("Groups seeded: Roommates, Weekend Trip, Office Lunch");

// --- Expenses ---
// Reload groups with members so navigation properties are fully populated
roommates = await context.Groups
    .Include(g => g.Members).ThenInclude(m => m.User)
    .SingleAsync(g => g.Id == roommates.Id);

trip = await context.Groups
    .Include(g => g.Members).ThenInclude(m => m.User)
    .SingleAsync(g => g.Id == trip.Id);

lunch = await context.Groups
    .Include(g => g.Members).ThenInclude(m => m.User)
    .SingleAsync(g => g.Id == lunch.Id);

ISplitStrategy equal = new EqualSplitStrategy();

static GroupMember PayerOf(Group group, User user) => group.Members.First(m => m.UserId == user.Id);

List<Expense> expenses =
[
    // Roommates group
    new(
        totalAmount: 180.00m,
        title: "Groceries",
        description: "Monthly grocery run",
        group: roommates,
        payer: PayerOf(roommates, users[0]),
        splits: roommates.Members.Select(m => new Split { MemberId = m.UserId, Amount = 0 }),
        splitStrategy: equal),

    new(
        totalAmount: 90.00m,
        title: "Internet bill",
        description: null,
        group: roommates,
        payer: PayerOf(roommates, users[1]),
        splits: roommates.Members.Select(m => new Split { MemberId = m.UserId, Amount = 0 }),
        splitStrategy: equal),

    new(
        totalAmount: 60.00m,
        title: "Cleaning supplies",
        description: null,
        group: roommates,
        payer: PayerOf(roommates, users[2]),
        splits: roommates.Members.Select(m => new Split { MemberId = m.UserId, Amount = 0 }),
        splitStrategy: equal),

    // Weekend Trip group
    new(
        totalAmount: 480.00m,
        title: "Hotel",
        description: "2 nights in Prague",
        group: trip,
        payer: PayerOf(trip, users[1]),
        splits: trip.Members.Select(m => new Split { MemberId = m.UserId, Amount = 0 }),
        splitStrategy: equal),

    new(
        totalAmount: 160.00m,
        title: "Train tickets",
        description: "Round trip",
        group: trip,
        payer: PayerOf(trip, users[3]),
        splits: trip.Members.Select(m => new Split { MemberId = m.UserId, Amount = 0 }),
        splitStrategy: equal),

    new(
        totalAmount: 200.00m,
        title: "Restaurants",
        description: "Meals during the trip",
        group: trip,
        payer: PayerOf(trip, users[2]),
        splits: trip.Members.Select(m => new Split { MemberId = m.UserId, Amount = 0 }),
        splitStrategy: equal),

    // Office Lunch group
    new(
        totalAmount: 55.00m,
        title: "Team lunch",
        description: "Italian restaurant",
        group: lunch,
        payer: PayerOf(lunch, users[3]),
        splits: lunch.Members.Select(m => new Split { MemberId = m.UserId, Amount = 0 }),
        splitStrategy: equal),
];

context.Expenses.AddRange(expenses);
await context.SaveChangesAsync();
Console.WriteLine($"Expenses seeded: {expenses.Count} expenses, {expenses.Sum(e => e.ExpenseShares.Count)} expense shares");

Console.WriteLine("Database seeding completed successfully!");

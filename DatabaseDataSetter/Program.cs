using IOU1.Application.Services.Balances;
using IOU1.Domain.Entities;
using IOU1.Domain.Services.Crypto;
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
services.AddScoped<IBalanceService, BalanceService>();

var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<IOU1Context>();
var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
var balanceService = scope.ServiceProvider.GetRequiredService<IBalanceService>();

Console.WriteLine("Seeding database...");

//Expense categories
await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO ExpenseCategory (Title,IconKey) values('Jedzenie', 'SYSTEM_FOOD')");
await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO ExpenseCategory (Title,IconKey) values('Transport', 'SYSTEM_TRANSPORT')");
await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO ExpenseCategory (Title,IconKey) values('Nocleg', 'SYSTEM_ACCOMODATION')");
await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO ExpenseCategory (Title,IconKey) values('Rozrywka', 'SYSTEM_FUN')");
await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO ExpenseCategory (Title,IconKey) values('Rachunki', 'SYSTEM_BILLS')");
await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO ExpenseCategory (Title,IconKey) values('Rachunki', 'SYSTEM_DIFFERENT')");

//Expense split types
await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO ExpenseSplit (Title,IconKey) values('Równo', 'EQUAL')");
await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO ExpenseSplit (Title,IconKey) values('Nierówno', 'CUSTOM')");
await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO ExpenseSplit (Title,IconKey) values('Procentowo', 'PERCENTAGE')");
await context.SaveChangesAsync();

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
    User.Create("Jakub",      "Walaszek",        new Email("jakub@example.com"),      "jakub",      "123456", hasher),
    User.Create("Julia",      "Jaśkielewicz",    new Email("julia@example.com"),      "julia",      "123456", hasher),
    User.Create("Krzysztof",  "Kwas",            new Email("krzysztof@example.com"),  "krzysztof",  "123456", hasher),
    User.Create("Kacper",     "Mejsner",         new Email("kacper@example.com"),     "kacper",     "123456", hasher),
};

context.Users.AddRange(users);
await context.SaveChangesAsync();
Console.WriteLine($"Users seeded: {string.Join(", ", users.Select(u => u.Login))}");

// --- Groups ---
// "Wydatki w Krakowie" — Jakub owns, Jakub is the only member
var krakow = new Group(
    name: "Wydatki w Krakowie",
    description: "Grupa do zarządzania wydatkami w Krakowie w paczce znajomych :)",
    owner: users[0],
    currency: currencies["PLN"],
    members: [users[0]]);

context.Groups.Add(krakow);
await context.SaveChangesAsync();
Console.WriteLine("Groups seeded: Wydatki w Krakowie");

// Reload group with members so navigation properties are fully populated
krakow = await context.Groups
    .Include(g => g.Members).ThenInclude(m => m.User)
    .SingleAsync(g => g.Id == krakow.Id);

// --- Initial member balances ---
foreach (var member in krakow.Members)
{
    var addedBalancesResult = await balanceService.AddInitialBalancesFor(member);
    if (!addedBalancesResult.IsSuccess)
        throw new InvalidOperationException(
            addedBalancesResult.ErrorMessage ?? $"Error occured while adding initial balances for member {member.Id}.");

    // Zapis po każdym członku, żeby kolejne wywołania widziały już utworzone pary sald
    // i nie tworzyły duplikatów w ramach tej samej partii seedowania.
    await context.SaveChangesAsync();
}
Console.WriteLine("Initial member balances seeded.");

Console.WriteLine("Database seeding completed successfully!");

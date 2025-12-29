using Domain.Entities;
using IOU1.Domain.Entities;
using IOU1.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Persistance.Context;

public class IOU1Context(DbContextOptions<IOU1Context> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<InvitationLink> InvitationLinks { get; set; }

    //Runs per new instance of the context
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    //Runs one time and then model data is cached
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ensure Email value object is treated as owned type globally so EF can materialize it for constructor binding
        modelBuilder.Owned<Email>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IOU1Context).Assembly);

        // Debug: list mapped properties and navigations for User to help diagnose constructor binding
        var userEntity = modelBuilder.Model.FindEntityType(typeof(User));
        if (userEntity != null)
        {
            Console.WriteLine($"EF Model - Entity: {userEntity.Name}");
            Console.WriteLine("Properties:");
            foreach (var prop in userEntity.GetProperties())
            {
                Console.WriteLine($" - {prop.Name} ({prop.ClrType.Name})");
            }

            Console.WriteLine("Navigations:");
            foreach (var nav in userEntity.GetNavigations())
            {
                Console.WriteLine($" - {nav.Name} ({nav.ClrType.Name})");
            }

            Console.WriteLine("Declared constructors on CLR type:");
            foreach (var ctor in typeof(User).GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {
                var parameters = string.Join(", ", ctor.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name));
                Console.WriteLine($" - {typeof(User).Name}({parameters})");
            }
        }
    }
}

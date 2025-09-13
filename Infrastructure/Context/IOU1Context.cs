using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class IOU1Context(DbContextOptions<IOU1Context> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<TransactionStatus> TransactionStatuses { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    //Runs per new instance of the context
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    //Runs one time and then model data is cached
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IOU1Context).Assembly);
    }
}

using IOU1.Domain.Entities;
using IOU1.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Persistance.Context;

public class IOU1Context(DbContextOptions<IOU1Context> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
    public DbSet<MemberBalance> MemberBalances => Set<MemberBalance>();

    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ExpenseShare> ExpenseShares => Set<ExpenseShare>();
    public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();
    public DbSet<ExpenseSplit> ExpenseSplits => Set<ExpenseSplit>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Settlement> Settlements => Set<Settlement>();
    public DbSet<ExpenseShareSettlement> ExpenseShareSettlements => Set<ExpenseShareSettlement>();

    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<InvitationLink> InvitationLinks => Set<InvitationLink>();

    public DbSet<Notification> Notifications => Set<Notification>();

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

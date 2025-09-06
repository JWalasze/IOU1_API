using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class IOU1Context(DbContextOptions<IOU1Context> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IOU1Context).Assembly);
    }
}

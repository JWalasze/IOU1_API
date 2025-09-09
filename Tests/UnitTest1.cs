using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class UnitTest1
{
    private readonly IOU1Context _context;

    public UnitTest1()
    {
        var options = new DbContextOptionsBuilder<IOU1Context>()
            .UseSqlServer("Server=JWALASZEK;Database=IOU1;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        _context = new IOU1Context(options); 
    }

    [Fact]
    public async Task Test1()
    {
        var x = await _context.Users.ToListAsync();
    }
}
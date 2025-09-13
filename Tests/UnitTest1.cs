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

    //VERY TEMP...
    [Fact]
    public async Task EfCoreConfigurations_TempTest_ReturnsProperDataFromDB()
    {
        //var x = await _context.Users.ToListAsync();
        //var y = await _context.Groups.ToListAsync();
        //var z = await _context.GroupMembers.ToListAsync();

        var xx = await _context.Users.Include(u => u.OwnedGroups).ToListAsync();
        var zz = await _context.GroupMembers.Include(gm => gm.User).Include(gm => gm.Group).ToListAsync();
        var xxx = await _context.Users.Include(u => u.MemberGroups).ThenInclude(gm => gm.Group).ToListAsync();
        var yy = await _context.Groups.Include(g => g.Members).ThenInclude(gm => gm.User).ToListAsync();
        var a = await _context.Currencies.ToListAsync();
        var b = await _context.TransactionStatuses.ToListAsync();
        var c = await _context.Transactions.ToListAsync();
    }
}
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests;

public class TempTests
{
    private readonly IOU1Context _context;

    public TempTests() {
        var config = TestConfig.InitConfiguration();
        var connectionString = config.GetConnectionString("DefaultConnection");

        var options = new DbContextOptionsBuilder<IOU1Context>()
            .UseSqlServer(connectionString)
            .Options;

        _context = new IOU1Context(options);
    }

    //VERY TEMP...
    [Fact]
    public async Task EfCoreConfigurations_TempTest_ReturnsProperDataFromDB()
    {
        var xx = await _context.Users.Include(u => u.OwnedGroups).ToListAsync();
        var zz = await _context.GroupMembers.Include(gm => gm.User).Include(gm => gm.Group).ToListAsync();
        var xxx = await _context.Users.Include(u => u.MemberGroups).ThenInclude(gm => gm.Group).ToListAsync();
        var yy = await _context.Groups.Include(g => g.Members).ThenInclude(gm => gm.User).ToListAsync();
        var a = await _context.Currencies.ToListAsync();
        var b = await _context.TransactionStatuses.ToListAsync();
        var c = await _context.Transactions.ToListAsync();
        var cc = await _context.Transactions
            .Include(t => t.Status)
            .Include(t => t.Borrower)
            .Include(t => t.Currency)
            .Include(t => t.Buyer)
            .Include(t => t.Status)
            .ToListAsync();
    }
}
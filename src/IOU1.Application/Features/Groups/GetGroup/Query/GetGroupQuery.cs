using IOU1.Application.Features.Groups.GetGroup.Models.Dto;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Features.Groups.GetGroup.Query;

public class GetGroupQuery(IOU1Context context) : IGetGroupQuery
{
    private readonly IOU1Context _context = context;

    //It's usefull to place here AsNoTracking() and return Task without async/await.
    //Thank's to that we won't be creating additional state machine for async method and we won't be tracking entities in the context.
    //We follow: task passthrough principle.
    public Task<bool> IsThatYourGroup(int userId, int groupId, CancellationToken cancellationToken = default)
    {
        return _context
            .Groups
            .AsNoTracking()
            .Include(g => g.Members)
            .AnyAsync(g =>
                g.Id == groupId &&
                g.Members.Any(m => m.UserId == userId), cancellationToken);
    }

    public Task<GroupDto?> GetGroup(int groupId, CancellationToken cancellationToken = default)
    {
        return _context
            .Groups
            .AsNoTracking()
            .Include(g => g.Members)
                .ThenInclude(m => m.User)
            .Where(g => g.Id == groupId)
            .Select(g => new GroupDto
            {
                GroupId = g.Id,
                Title = g.Name,
                Description = g.Description,
                Currency = g.CurrencyKey,
                Members = g.Members
                    .Select(m => new MemberDto(
                        m.Id,
                        m.User.FullName))
                    .ToList()
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<List<ExpenseDto>> GetLastExpenses(int groupId, int lastExpenseCount, CancellationToken cancellationToken = default)
    {
        return _context
            .Expenses
            .AsNoTracking()
            .Include(e => e.Shares)
            .Include(e => e.Group)
            .Where(e => e.GroupId == groupId)
            .Take(lastExpenseCount)
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => new ExpenseDto(
                e.Id,
                e.PayerId,
                e.Amount,
                e.CreatedAt,
                e.Shares
                    .Select(es => new ExpenseShareDto(
                        es.Id,
                        es.MemberId,
                        es.Amount))
                    .ToList()))
            .ToListAsync(cancellationToken);
    }

    public Task<List<MemberBalanceDto>> GetBalances(int groupId, CancellationToken cancellationToken = default)
    {
        return _context
            .MemberBalances
            .AsNoTracking()
            .Include(b => b.Member)
            .Include(b => b.CounterpartyMember)
            .Where(b =>
                b.Member.GroupId == groupId &&
                b.CounterpartyMember.GroupId == groupId)
            .Select(b => new MemberBalanceDto(
                b.MemberId,
                b.CounterpartyMemberId,
                b.Amount))
            .ToListAsync(cancellationToken);
    }
}

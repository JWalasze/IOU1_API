using IOU1.Application.Features.Groups.GetGroup.Models.Dto;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Features.Groups.GetGroup.Query;

public class GetGroupQuery(IOU1Context context) : IGetGroupQuery
{
    private readonly IOU1Context _context = context;

    public async Task<IEnumerable<GetGroupExpenseDto>> GetLastExpenses(long groupId, int lastExpenseCount, CancellationToken cancellationToken = default)
    {
        return await _context
            .Expenses
            .Include(e => e.Transactions)
            .Where(e => e.GroupId == groupId)
            .Select(e => new GetGroupExpenseDto
            {
                ExpenseId = e.Id,
                BuyerId = e.BuyerId,
                Amount = e.TotalAmount,
                Currency = "PLN",
                Transactions = e.Transactions
                    .Select(t => new GetGroupTransactionDto
                    {
                        Amount = t.Amount,
                        BorrowerId = t.BorrowerId,
                        TransactionId = t.Id,
                        Currency = "PLN"
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<GetGroupDto?> GetGroup(long groupId, CancellationToken cancellationToken = default)
    {
        return await _context
            .Groups
            .Include(g => g.Members)
                .ThenInclude(m => m.User)
            .Where(g => g.Id == groupId)
            .Select(g => new GetGroupDto
            {
                GroupId = g.Id,
                OwnerName = g.Owner.FullName,
                Description = g.Description,
                Members = g.Members
                    .Select(m => new GetGroupMemberDto
                    {
                        UserId = m.Id,
                        UserName = m.User.FullName,
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

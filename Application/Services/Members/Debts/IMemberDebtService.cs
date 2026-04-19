using IOU1.Domain.Entities;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Services.Members.Debts;

public interface IMemberDebtService
{
    Task<Result> UpdateMemberDebts(Expense expense, CancellationToken cancellationToken = default);
}

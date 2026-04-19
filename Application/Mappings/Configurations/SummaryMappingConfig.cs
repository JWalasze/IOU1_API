using IOU1.Application.Features.Expenses.GetExpenses.Models;
using IOU1.Domain.Models;
using Mapster;

namespace IOU1.Application.Mappings.Configurations
{
    internal class SummaryMappingConfig : IMappingConfiguration
    {
        public void Init()
        {
            TypeAdapterConfig<GroupExpenseSummary, GetExpensesResponse>
                .NewConfig()
                .Map(dest => dest.GroupId, src => src.GroupId)
                .Map(dest => dest.Expenses, src => src.GroupDebts.Select(gd => new GetExpensesDetailsResponse
                {
                    ExpenseId = 0,
                    Description = $"{gd.DebtorName} owes {gd.CreditorName}",
                    Amount = gd.Amount,
                    Date = DateTime.MinValue
                }));
        }
    }
}

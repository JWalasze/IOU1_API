using IOU1.Application.Features.Transactions.AddTransaction.Request;
using IOU1.Domain.Models;
using Mapster;

namespace IOU1.Application.Mappings.Configurations;

public class TransactionMappingConfig : IMappingConfiguration
{
    public void Init()
    {
        TypeAdapterConfig<SplitRequest, Split>
            .NewConfig()
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.MemberId, src => src.MemberId);

        TypeAdapterConfig<IEnumerable<SplitRequest>, IEnumerable<Split>>
            .NewConfig()
            .Map(dest => dest, src => src.Select(s => s.Adapt<Split>()));
    }
}

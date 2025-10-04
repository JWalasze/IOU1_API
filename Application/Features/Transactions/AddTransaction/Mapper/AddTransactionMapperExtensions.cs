using IOU1.Application.Features.Transactions.AddTransaction.Dto;
using IOU1.Application.Features.Transactions.AddTransaction.Request;

namespace IOU1.Application.Features.Transactions.AddTransaction.Mapper;

public static class AddTransactionMapperExtensions
{
    public static Split MapToDto(this SplitRequest source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return new()
        {
            Amount = source.Amount,
            MemberId = source.MemberId
        };
    }

    public static IEnumerable<Split> MapToDto(this IEnumerable<SplitRequest> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.Select(s => s.MapToDto());
    }
}

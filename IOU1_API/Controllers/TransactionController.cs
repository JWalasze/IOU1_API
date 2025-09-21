using IOU1_API.DTOs;
using IOU1_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace IOU1_API.Controllers;

public record MemberAmountPair(long MemberId, decimal Amount);

public record GroupTransactionRequest(
    long BuyerId,
    long GroupId,
    decimal AmountTotal,
    string Title,
    string? Description,
    bool DivideEqually,
    IEnumerable<MemberAmountPair>? Splits,
    IEnumerable<long>? MemberIds
);

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;
    private readonly GroupService _groupService;

    public TransactionController(TransactionService transactionService, GroupService groupService)
    {
        _transactionService = transactionService;
        _groupService = groupService;
    }

    /*
    Example 1
    {
      "buyerId": 1,
      "groupId": 1,
      "amountTotal": 100,
      "divideEqually": true,
      "splits": null,
      "memberIds": [1,2,3,4]
    }

    Example 2
    {
      "buyerId": 1,
      "groupId": 1,
      "amountTotal": 100,
      "divideEqually": false,
      "splits": [
        {
          "memberId": 1,
          "amount": 30
        }
      ],
      "memberIds": null
    }
    */
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] GroupTransactionRequest request)
    {
        var result = await _transactionService
            .CreateGroupTransactionsAsync(request);

        return CreatedAtAction(
            nameof(GetGroupTransactions),
            new { groupId = request.GroupId },
            result
        );
    }

    [HttpGet("{groupId:long}")]
    public async Task<IActionResult> GetGroupTransactions(long groupId)
    {
        var transactions = await _transactionService
            .GetTransactionByGroupIdAsync(groupId);

        return Ok(transactions);
    }
}

using IOU1.Domain.Entities;
using IOU1.Domain.Enums;
using IOU1.Persistance.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IOU1.Application.Services.Settlements;

public sealed class SettlementService(IOU1Context context) : ISettlementService
{
    private readonly IOU1Context _context = context;

    public async Task<List<ExpenseShareSettlement>> SettleBasedOnNewExpense(
        SearchExpense searchExpense,
        Expense newExpense,
        CancellationToken cancellationToken = default)
    {
        return searchExpense switch
        {
            SearchExpense.ByOldest => await SettleByOldest(newExpense, cancellationToken),
            SearchExpense.ByNewest => throw new NotImplementedException(),
            SearchExpense.ByMostExpensive => throw new NotImplementedException(),
            _ => throw new NotImplementedException()
        };

        throw new NotImplementedException();
    }

    private async Task<List<ExpenseShareSettlement>> SettleByOldest(
        Expense newExpense,
        CancellationToken cancellationToken = default)
    {
        var table = new DataTable();
        table.Columns.Add("FirstId", typeof(int));
        table.Columns.Add("SecondId", typeof(int));
        foreach (var share in newExpense.Shares)
            table.Rows.Add(newExpense.PayerId, share.MemberId);

        var param = new SqlParameter("@MemberPairs", table)
        {
            TypeName = "DoubleIds",
            SqlDbType = SqlDbType.Structured
        };

        //Bierzemy gdzie payer jest rozny od memberId
        var foundExpenseShares = await _context.ExpenseShares
            .FromSqlRaw($@"
                SELECT es.*
                FROM ExpenseShare es
                JOIN Expense e ON e.id = es.ExpenseId
                JOIN @MemberPairs p ON es.MemberId = p.FirstId AND e.PayerId = p.SecondId
                WHERE e.IsSettled = 0",
                param)
            .Include(es => es.Expense)
            .Include(es => es.ExpenseShareSettlements)
            .OrderBy(es => es.Expense.CreatedAt)
            .ToListAsync(cancellationToken);

        var foundExpenseSharesLookup = foundExpenseShares
            .GroupBy(fes => (fes.MemberId, fes.Expense.PayerId))
            .ToDictionary(fes => fes.Key, v => v.ToList());

        var createdExpenseShareSettlements = new List<ExpenseShareSettlement>();

        //Musimy przejść po kazdym nowym expenseShare i zobaczyć czy jest ono w  stanie skompensować jakąś starą ExpenseShare
        //Wszystkie expenseShare muszą zostać spersystowane - dodamy za to settlementy oraz asocjacyjną miedzy settlement a expense
        //Jak zostanie dodany Settlement musimy uruchomić konfigurowanie balanców
        //Może do balansów przekaże expensyShare nowy ORAZ dodane Settlementy aby wiedział żeby sfiftować a pozniej w kolejnej pętli sfitować
        //na podstawie Settlementów
        //I jeszcze trzeba sprawdzać czy tamten Expense będzie settled po naszej operacji...
        foreach (var newExpenseShare in newExpense.Shares.Where(s => s.Expense.PayerId != s.MemberId))
        {
            var previousExpenseShareGroup = foundExpenseSharesLookup.GetValueOrDefault(
                (newExpenseShare.MemberId, newExpenseShare.Expense.PayerId));

            if (previousExpenseShareGroup is null || previousExpenseShareGroup.Count == 0)
                continue;

            var isSatisfied = false;
            var index = 0;

            while (!isSatisfied)
            {
                var previousExpenseShare = previousExpenseShareGroup.ElementAtOrDefault(index);
                if (previousExpenseShare is null)
                {
                    isSatisfied = true;
                    continue;
                }

                if (Math.Abs(previousExpenseShare.Amount) <= Math.Abs(newExpenseShare.Amount))
                {
                    var settlement = Settlement.Create(
                        newExpense.Group,
                        newExpenseShare.Member,
                        newExpense.Payer,
                        Math.Abs(newExpenseShare.Amount) - Math.Abs(previousExpenseShare.Amount));

                    var expenseShareSettlement = ExpenseShareSettlement.Create(
                        previousExpenseShare,
                        settlement);

                    //Jak się doda settlement trzeba jeszcze sprawdzić czy settlement rozlicza cały expense...
                    //Juz includuje sobie więc...jedynie podliczyć
                    isSatisfied = true;
                    createdExpenseShareSettlements.Add(expenseShareSettlement);
                    _context.ExpenseShareSettlements.Add(expenseShareSettlement);
                }
                else
                {
                    var settlement = Settlement.Create(
                        newExpense.Group,
                        newExpenseShare.Member,
                        newExpense.Payer,
                        Math.Abs(newExpenseShare.Amount));

                    var expenseShareSettlement = ExpenseShareSettlement.Create(
                        previousExpenseShare,
                        settlement);

                    //ExpenseShareSettlements można do expense od razu dodawać przecież...
                    //Nie bo to jest nowy a my będziemy dodawać do starego
                    createdExpenseShareSettlements.Add(expenseShareSettlement);
                    _context.ExpenseShareSettlements.Add(expenseShareSettlement);
                }
            }
        }

        return createdExpenseShareSettlements;
    }
}

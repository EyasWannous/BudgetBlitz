using BudgetBlitz.Domain.Models;
using BudgetBlitz.Presentation.DTO.Expense;

namespace BudgetBlitz.Presentation.Mappers;

public static class ExpenseMapper
{
    public static OutgoingExpenseDTO Outgoing(this Expense expense)
    {
        return new OutgoingExpenseDTO
        {
            Id = expense.Id,
            Amount = expense.Amount,
            Date = expense.Date,
            UserId = expense.UserId,
            CategoryId = expense.CategoryId
        };
    }

    public static Expense Incoming(this IncomingExpenseDTO incomingExpenseDTO)
    {
        return new Expense
        {
            Amount = incomingExpenseDTO.Amount,
            Date = incomingExpenseDTO.Date,
            CategoryId = incomingExpenseDTO.CategoryId
        };
    }
}

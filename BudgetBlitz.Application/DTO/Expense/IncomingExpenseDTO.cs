namespace BudgetBlitz.Application.DTO.Expense;

public class IncomingExpenseDTO
{
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public int CategoryId { get; set; }

}

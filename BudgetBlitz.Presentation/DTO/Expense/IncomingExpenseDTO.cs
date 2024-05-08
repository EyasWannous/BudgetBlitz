namespace BudgetBlitz.Presentation.DTO.Expense;

public class IncomingExpenseDTO
{
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }

}

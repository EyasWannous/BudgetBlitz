namespace BudgetBlitz.Application.DTO.Expense;

public class OutgoingExpenseDTO
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
}

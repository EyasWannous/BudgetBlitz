namespace BudgetBlitz.Domain.Models;

public class Expense : BaseModel
{
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public User User { get; set; }
    public Category Category { get; set; }
}

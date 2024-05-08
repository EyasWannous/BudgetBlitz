namespace BudgetBlitz.Domain.Models;

public class Income : BaseModel
{
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

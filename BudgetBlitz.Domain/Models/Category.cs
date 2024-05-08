namespace BudgetBlitz.Domain.Models;

public class Category : BaseModel
{
    public required string Name { get; set; }
    public List<Expense> Expenses { get; set; } = [];
}

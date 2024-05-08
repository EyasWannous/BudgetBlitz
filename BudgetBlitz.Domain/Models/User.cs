using Microsoft.AspNetCore.Identity;

namespace BudgetBlitz.Domain.Models;

public class User : IdentityUser<int>
{
    public List<Income> Incomes { get; set; } = [];
    public List<Expense> Expenses { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

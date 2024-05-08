namespace BudgetBlitz.Domain.Models;

public class BaseModel
{
    public int Id { get; set; }
    //public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

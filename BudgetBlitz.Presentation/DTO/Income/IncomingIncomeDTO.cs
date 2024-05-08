namespace BudgetBlitz.Presentation.DTO.Income;

public class IncomingIncomeDTO
{
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public int UserId { get; set; }
}

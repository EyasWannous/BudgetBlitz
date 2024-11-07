namespace BudgetBlitz.Application.DTO.Income;

public class OutgoingIncomesDTO
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public int UserId { get; set; }
}

namespace BudgetBlitz.Domain.Models;

public class UserDevice : BaseModel
{
    public int UsertId { get; set; }
    public User User { get; set; }
    public string DeviceToken { get; set; }
}

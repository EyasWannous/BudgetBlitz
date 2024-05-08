using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetBlitz.Domain.Models;

public class RefreshToken : BaseModel
{
    public required string Token { get; set; }
    public required string JwtId { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime ExpireDate { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

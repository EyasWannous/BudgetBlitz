using System.Security.Claims;

namespace BudgetBlitz.Application.IServices;

public interface IFirebaseService
{
    Task<bool> RegisterDeviceTokenAsync(string deviceToken, ClaimsPrincipal principal);
    Task<string> SendMessageAsync(string title, string body, string deviceToken, ClaimsPrincipal principal);
}

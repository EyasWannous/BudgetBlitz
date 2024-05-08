using BudgetBlitz.Domain.Models;

namespace BudgetBlitz.Application.IServices;

public interface IUserService
{
    Task<(string Message, bool IsSuccess, string[] Errors, User? User)> RegisterUserAsync(string email, string userName, string password);
    Task<(string Message, bool IsSuccess, User? User)> LoginUserUsingUserNameAsync(string userName, string password);
    Task<(string Message, bool IsSuccess, User? User)> LoginUserUsingEmailAsync(string email, string password);
    Task<(string Message, bool IsSuccess, string[] Errors)> ConfirmEmailAsync(string userId, string token);
    Task<(string Message, bool IsSuccess)> ForgetPasswordAsync(string email);
    Task<(string Message, bool IsSuccess, string[] Errors)> ResetPasswordAsync(string email, string token, string newPassword);
}

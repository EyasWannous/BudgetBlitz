using BudgetBlitz.Application.Helper;
using BudgetBlitz.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BudgetBlitz.Application.Services;

public class PasswordHasher : IPasswordHasher<User>
{
    const char Delimiter = ';';
    public string HashPassword(User user, string password)
    {
        var mergedPassword = $"{user.UserName}_{password}_{user.Email}";
        var hasedPassword = PasswordHashingSalting.HashPasword(mergedPassword, out byte[] salt);

        return string.Join(Delimiter, Convert.ToHexString(salt), hasedPassword);
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        var elements = hashedPassword.Split(Delimiter);
        var salt = Convert.FromHexString(elements[0]);
        var hash = elements[1];

        var mergedPassword = $"{user.UserName}_{providedPassword}_{user.Email}";

        return PasswordHashingSalting.VerifyPassword(mergedPassword, hash, salt)
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }
}

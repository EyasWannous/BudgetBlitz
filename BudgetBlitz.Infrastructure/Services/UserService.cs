using BudgetBlitz.Application.Helper;
using BudgetBlitz.Application.IServices;
using BudgetBlitz.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BudgetBlitz.Infrastructure.Services;

public class UserService(UserManager<User> userManager, IConfiguration configuration, IMailService mailService) : IUserService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly IMailService _mailService = mailService;


    public async Task<(string Message, bool IsSuccess, string[] Errors)> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return (Message: "User not found.", IsSuccess: false, Errors: []);

        var normalToken = TokenEncoderDecoder.Decode(token);

        var result = await _userManager.ConfirmEmailAsync(user, normalToken);
        if (!result.Succeeded)
            return
            (
                Message: "Email didn't confirmed.",
                IsSuccess: false,
                Errors: result.Errors.Select(e => e.Description).ToArray()
            );

        return (Message: "Email Confirmed Successfully!", IsSuccess: true, Errors: []);
    }


    public async Task<(string Message, bool IsSuccess)> ForgetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return (Message: "No user associated with this Email.", IsSuccess: false);

        _ = Task.Run(() => SendResetPasswordEmail(user));

        return (Message: "Reset Password URL has been sent to the email Successfully!", IsSuccess: true);
    }


    public async Task<(string Message, bool IsSuccess, User? User)> LoginUserUsingEmailAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return (Message: "There is a user with that Email.", IsSuccess: false, User: null);

        var result = await _userManager.CheckPasswordAsync(user, password);
        if (!result)
            return (Message: "Email or Password is not correct.", IsSuccess: false, User: null);

        return (Message: "Logged in Successfully!", IsSuccess: true, User: user);
    }


    public async Task<(string Message, bool IsSuccess, User? User)> LoginUserUsingUserNameAsync(string userName, string password)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return (Message: "There is a user with that UserName.", IsSuccess: false, User: null);

        var result = await _userManager.CheckPasswordAsync(user, password);
        if (!result)
            return (Message: "UserName or Password is not correct.", IsSuccess: false, User: null);

        return (Message: "Logged in Successfully!", IsSuccess: true, User: user);
    }


    public async Task<(string Message, bool IsSuccess, string[] Errors, User? User)> RegisterUserAsync(string email, string userName, string password)
    {
        var checkEmail = await _userManager.FindByEmailAsync(email);
        var checkUserName = await _userManager.FindByNameAsync(userName);
        if (checkEmail is not null || checkUserName is not null)
            return
            (
                Message: "UserName or Email is already taken, UserName and Email must be unique.",
                IsSuccess: false,
                Errors: [],
                User: null
            );

        var user = new User
        {
            UserName = userName,
            Email = email,
        };

        // -----------------------------------------------------------------
        // Don't do this:
        //var hashedPassword = _passwordHasher.HashPassword(user, password);
        //var result = await _userManager.CreateAsync(user, hashedPassword);
        // -----------------------------------------------------------------

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return
            (
                Message: "User dosen't created",
                IsSuccess: false,
                Errors: result.Errors.Select(e => e.Description).ToArray(),
                User: null
            );

        _ = Task.Run(() => SendConfirmRequest(user));

        return (Message: "User created Successfully!", IsSuccess: true, Errors: [], User: user);
    }


    public async Task<(string Message, bool IsSuccess, string[] Errors)> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return (Message: "No user associated with this email.", IsSuccess: false, Errors: []);

        var normalToken = TokenEncoderDecoder.Decode(token);

        var result = await _userManager.ResetPasswordAsync(user, normalToken, newPassword);
        if (!result.Succeeded)
            return
            (
                Message: "Something went wrong.",
                IsSuccess: false,
                Errors: result.Errors.Select(e => e.Description).ToArray()
            );

        return (Message: "Password has been reset Successfully!", IsSuccess: true, Errors: []);
    }


    private async Task SendConfirmRequest(User user)
    {
        var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var validEmailToken = TokenEncoderDecoder.Encode(confirmEmailToken);

        string url = $"{_configuration["AppUrl"]}/api/auth/confirmemail?userId={user.Id}&token={validEmailToken}";

        await _mailService.SendEmailAsync
        (
            user.Email!,
            "Confirm your email",
            "<h1>Welcom to BudgetBlitz</h1>"
            + "<p>Please confirm your email by "
            + $"<a href='{url}'>"
            + "Clicking here</a></p>"
        );
    }


    private async Task SendResetPasswordEmail(User user)
    {
        var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var validResetToken = TokenEncoderDecoder.Encode(passwordResetToken);

        string url = $"{_configuration["AppUrl"]}/api/auth/resetpassword?email={user.Email}&token={validResetToken}";

        await _mailService.SendEmailAsync
        (
            user.Email!,
            "Reset Password",
            "<h1>Follow the instructions to reset the password</h1>"
            + "<p>To reset your password "
            + $"<a href='{url}'>"
            + "Clicking here</a></p>"
        );
    }
}

using BudgetBlitz.Application.IServices;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Presentation.DTO.Account;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBlitz.Presentation.Controllers;

public class AuthConroller(IUnitOfWork unitOfWork, IUserService userService, ITokenService tokenService) : BaseContoller(unitOfWork)
{
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] IncomingUserRegisterationDTO incomingUserRegisterationDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (incomingUserRegisterationDTO.EmailIsNullOrWhiteSpace)
            return NotFound("Invalid Email");

        if (incomingUserRegisterationDTO.CheckPasswords)
            return BadRequest(new OutgoingUserRegisterationDTO
            {
                Message = "Confirm Password doesn't match the Password",
                IsSuccess = false,
            });

        var result = await _userService.RegisterUserAsync(
            incomingUserRegisterationDTO.Email,
            incomingUserRegisterationDTO.UserName,
            incomingUserRegisterationDTO.Password
        );
        if (!result.IsSuccess)
            return BadRequest(new OutgoingUserRegisterationDTO
            {
                Message = result.Message,
                IsSuccess = false,
                Errors = result.Errors,
            });

        var (JwtToken, RefreshToken, ExpireDate) = await _tokenService.GenerateJwtTokenAsync(result.User!);

        return Ok(new OutgoingUserRegisterationDTO
        {
            Message = result.Message,
            IsSuccess = true,
            JwtToken = JwtToken,
            RefreshToken = RefreshToken,
            ExpireDate = ExpireDate
        });
    }


    [HttpPost("loginUserName")]
    public async Task<IActionResult> LoginUserName([FromBody] IncomingUserLoginUsingUserNameDTO incomingUserLoginUsingUserNameDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.LoginUserUsingUserNameAsync(
            incomingUserLoginUsingUserNameDTO.UserName,
            incomingUserLoginUsingUserNameDTO.Password
        );
        if (!result.IsSuccess)
            return BadRequest(new OutgoingUserLoginDTO
            {
                Message = result.Message,
                IsSuccess = false,
            });

        var (JwtToken, RefreshToken, ExpireDate) = await _tokenService.GenerateJwtTokenAsync(result.User!);



        //await _mailService.SendEmailAsync
        //(
        //    "eyaswannous@gmail.com",
        //    "New Login",
        //    "<h1>Hey!, new login to your account noticed</h1><p>New login to your account at" + DateTime.UtcNow + "</p>"
        //);

        return Ok(new OutgoingUserLoginDTO
        {
            Message = result.Message,
            IsSuccess = true,
            JwtToken = JwtToken,
            RefreshToken = RefreshToken,
            ExpireDate = ExpireDate
        });
    }


    [HttpPost("loginEmail")]
    public async Task<IActionResult> LoginEmail([FromBody] IncomingUserLoginUsingEmailDTO incomingUserLoginUsingEmailDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.LoginUserUsingEmailAsync(
            incomingUserLoginUsingEmailDTO.Email,
            incomingUserLoginUsingEmailDTO.Password
        );
        if (!result.IsSuccess)
            return BadRequest(new OutgoingUserLoginDTO
            {
                Message = result.Message,
                IsSuccess = false,
            });

        var (JwtToken, RefreshToken ,ExpireDate) = await _tokenService.GenerateJwtTokenAsync(result.User!);



        //await _mailService.SendEmailAsync
        //(
        //    "eyaswannous@gmail.com",
        //    "New Login",
        //    "<h1>Hey!, new login to your account noticed</h1><p>New login to your account at" + DateTime.UtcNow + "</p>"
        //);

        return Ok(new OutgoingUserLoginDTO
        {
            Message = result.Message,
            IsSuccess = true,
            JwtToken = JwtToken,
            RefreshToken = RefreshToken,
            ExpireDate = ExpireDate
        });
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] IncomingRefreshTokenDTO incomingRefreshTokenDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (incomingRefreshTokenDTO.JwtTokenOrRefreshTokenIsNullOrWhiteSpace)
            return NotFound();

        var (Message, IsSuccess) = await _tokenService.VerfiyTokenAsync(incomingRefreshTokenDTO.JwtToken, incomingRefreshTokenDTO.RefreshToken);
        if (!IsSuccess)
            return BadRequest(new OutgoingRefreshTokenDTO
            {
                Message = Message,
                IsSuccess = false
            });

        var result = await _tokenService.MakeNewRefreshTokenAsync(incomingRefreshTokenDTO.RefreshToken);
        if (!result.IsSuccess)
            return BadRequest(new OutgoingRefreshTokenDTO
            {
                Message = Message,
                IsSuccess = false
            });

        return Ok(new OutgoingRefreshTokenDTO
        {
            Message = result.Message,
            JwtToken = result.JwtToken,
            RefreshToken = result.RefreshToken,
            IsSuccess = true
        });
    }



    [HttpGet("confirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromBody] IncomingUserConfirmationEmailDTO incomingUserConfirmationEmailDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (incomingUserConfirmationEmailDTO.UserIdOrTokenIsNullOrWhiteSpace)
            return NotFound();

        var result = await _userService.ConfirmEmailAsync(incomingUserConfirmationEmailDTO.UserId, incomingUserConfirmationEmailDTO.Token);
        if(!result.IsSuccess)
            return BadRequest(new OutgoingUserConfirmationEmailDTO
            {
                Message = result.Message,
                IsSuccess = false,
                Errors = result.Errors
            });

        return Ok(new OutgoingUserConfirmationEmailDTO
        {
            Message = result.Message,
            IsSuccess = true
        });
    }


    [HttpPost("forgetPassword")]
    public async Task<IActionResult> ForgetPassword([FromBody] IncomingForgetPasswordDTO incomingForgetPasswordDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (incomingForgetPasswordDTO.EmailIsNullOrWhiteSpace)
            return NotFound();

        var result = await _userService.ForgetPasswordAsync(incomingForgetPasswordDTO.Email);
        if (!result.IsSuccess)
            return BadRequest(new OutgoingUserConfirmationEmailDTO
            {
                Message = result.Message,
                IsSuccess = false
            });

        return Ok(new OutgoingForgetPasswordDTO
        {
            Message = result.Message,
            IsSuccess = true
        });
    }


    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] IncomingResetPasswordDTO incomingResetPasswordDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (incomingResetPasswordDTO.EmailIsNullOrWhiteSpace)
            return NotFound("Invalid Email");

        if (incomingResetPasswordDTO.CheckPasswords)
            return BadRequest(new OutgoingForgetPasswordDTO
            {
                Message = "Confirm Password doesn't match the Password",
                IsSuccess = false,
            });

        var result = await _userService.ResetPasswordAsync(incomingResetPasswordDTO.Email,incomingResetPasswordDTO.Token,incomingResetPasswordDTO.NewPasswrod);
        if (!result.IsSuccess)
            return BadRequest(new OutgoingForgetPasswordDTO
            {
                Message = result.Message,
                IsSuccess = false
            });

        return Ok(new OutgoingForgetPasswordDTO
        {
            Message = result.Message,
            IsSuccess = true
        });
    }
}

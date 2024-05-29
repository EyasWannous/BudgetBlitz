using BudgetBlitz.Application.IServices;
using BudgetBlitz.Domain.Abstractions;
using BudgetBlitz.Domain.Models;
using BudgetBlitz.Application.Helper;
using BudgetBlitz.Application.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BudgetBlitz.Infrastructure.Services;

public class TokenService(IOptionsMonitor<JwtOptions> options,IUnitOfWork unitOfWork,
    UserManager<User> userManager, TokenValidationParameters tokenValidationParameters) : ITokenService
{
    private readonly JwtOptions _options = options.CurrentValue;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<User> _userManager = userManager;
    private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;

    public async Task<(string JwtToken, string RefreshToken, DateTime ExpireDate)> GenerateJwtTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var signingKey = Encoding.ASCII.GetBytes(_options.SigningKey);

        List<Claim> claims =
        [
            new("Id", user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.GivenName, user.UserName!),
            new(JwtRegisteredClaimNames.Sub, user.Email!), // unique id
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // used by refresh token
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_options.ExpireTime),
            Audience = _options.ValidAudience,
            Issuer = _options.ValidIssuer,
            SigningCredentials = new SigningCredentials
            (
                new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        //var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        var jwtToken = tokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken
        {
            //Token = $"{user.UserName}_{Guid.NewGuid()}_{user.Email}",
            Token = $"{Guid.NewGuid()}",
            UserId = user.Id,
            IsRevoked = false,
            IsUsed = false,
            JwtId = token.Id,
            ExpireDate = DateTime.UtcNow.AddMonths(1),
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
        await _unitOfWork.CompleteAsync();

        return (JwtToken: jwtToken, RefreshToken : refreshToken.Token, ExpireDate: token.ValidTo);
    }

    public async Task<(string Message, bool IsSuccess)> VerfiyTokenAsync(string jwtToken, string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claimPrincipal = tokenHandler.ValidateToken(jwtToken, _tokenValidationParameters, out var validatedToken);
        if (validatedToken is not JwtSecurityToken jwtSecurityToken)
            return (Message: "Token validation falid.", IsSuccess: false);

        var checkAlgorithm = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature);
        if (!checkAlgorithm)
            return (Message: "Token validation falid.", IsSuccess: false);

        var didParsed = long.TryParse
        (
            claimPrincipal.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp)?.Value,
            out long utcExpiryDate
        );
        if (!didParsed)
            return (Message: "Token validation falid.", IsSuccess: false);

        var expDate = await UnixConverter.UnixTimeStampToDateTimeAsync(utcExpiryDate);
        if (expDate > DateTime.UtcNow)
            return (Message: "Jwt Token has not expired.", IsSuccess: false);

        var userRefreshToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
        if (userRefreshToken is null)
            return (Message: "Invalid Refresh Token.", IsSuccess: false);

        if (userRefreshToken.IsUsed)
            return (Message: "Refresh Token has been used, it cannot be reused.", IsSuccess: false);

        if (userRefreshToken.IsRevoked)
            return (Message: "Refresh Token has been revoked, it cannot be reused.", IsSuccess: false);

        var jtiId = claimPrincipal.Claims.SingleOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;
        if (jtiId is null)
            return (Message: "Token validation falid", IsSuccess: false);

        if (userRefreshToken.JwtId != jtiId)
            return (Message: "Refresh Token refrence dose not match the jwt token.", IsSuccess: false);

        return (Message: "Refresh Token is Valid.", IsSuccess: true);
    }

    public async Task<(string Message, string? JwtToken, string? RefreshToken, bool IsSuccess)> MakeNewRefreshTokenAsync(string refreshToken)
    {
        var userRefreshToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
        if (userRefreshToken is null) 
            return (Message: "Error processing request.", JwtToken: null, RefreshToken: null, IsSuccess: false);

        var updateResult = await _unitOfWork.RefreshTokens.MakeRefreshTokenAsUsedAsync(userRefreshToken);
        if (!updateResult)
            return (Message: "Error processing request.", JwtToken: null, RefreshToken: null, IsSuccess: false);

        await _unitOfWork.CompleteAsync();

        var dbUser = await _userManager.FindByIdAsync(userRefreshToken.UserId.ToString());
        if (dbUser is null)
            return (Message: "Error processing request.", JwtToken: null, RefreshToken: null, IsSuccess: false);

        var newTokens = await GenerateJwtTokenAsync(dbUser);

        return 
        (
            Message: "RefreshToken Maked Used Successfully!", 
            newTokens.JwtToken,
            newTokens.RefreshToken,
            IsSuccess: true
        );
    }
}

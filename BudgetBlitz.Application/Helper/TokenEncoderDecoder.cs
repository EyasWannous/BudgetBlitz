using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BudgetBlitz.Application.Helper;

public static class TokenEncoderDecoder
{
    public static string Encode(string token) 
    {
        var encodedToken = Encoding.UTF8.GetBytes(token);
        var validToken = Base64UrlEncoder.Encode(encodedToken);

        return validToken;
    }
    public static string Decode(string token) 
    {
        var decodedToken = Base64UrlEncoder.DecodeBytes(token);
        var normalToken = Encoding.UTF8.GetString(decodedToken);

        return normalToken;
    }
}

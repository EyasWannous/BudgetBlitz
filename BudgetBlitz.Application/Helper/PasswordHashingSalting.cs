using System.Security.Cryptography;
using System.Text;

namespace BudgetBlitz.Application.Helper;

public static class PasswordHashingSalting
{
    const int keySize = 512 / 8;
    const int iterations = 350000;
    readonly static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public static string HashPasword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(keySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2
        (
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize
        );

        return Convert.ToHexString(hash);
    }

    public static bool VerifyPassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2
        (
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize
        );

        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}

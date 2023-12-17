using System.Security.Cryptography;
using System.Text;

namespace Web2Prosjektoppgave.api.Utilities;

public class PasswordUtility
{
    public static string HashPassword(string password, string saltBase64)
    {
        const int iterations = 10000;
        const int hashLength = 32;
        var salt = Convert.FromBase64String(saltBase64);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] hashBytes = pbkdf2.GetBytes(hashLength);

        string base64Hash = Convert.ToBase64String(hashBytes);

        return base64Hash;
    }

    public static string GenerateSaltBase64()
    {
        byte[] salt = new byte[32];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        return Convert.ToBase64String(salt);
    }

    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {

        var base64PasswordHash = HashPassword(password, storedSalt);
        
        return base64PasswordHash == storedHash;
    }
}
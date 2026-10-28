using System.Security.Cryptography;
using GymTurnos.Application.Interfaces.Services;

namespace GymTurnos.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize
        );

        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var partes = passwordHash.Split('.');

        if (partes.Length != 3)
            return false;

        if (!int.TryParse(partes[0], out int iterations))
            return false;

        byte[] salt = Convert.FromBase64String(partes[1]);
        byte[] hashGuardado = Convert.FromBase64String(partes[2]);

        byte[] hashIngresado = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            hashGuardado.Length
        );

        return CryptographicOperations.FixedTimeEquals(
            hashGuardado,
            hashIngresado
        );
    }
}
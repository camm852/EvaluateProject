using EvaluationProjects.Interfaces;
using System.Security.Cryptography;

namespace EvaluationProjects.Helpers.Auth
{
    
    public class HashingPassword : IHashingPassword
    {
        private const int SaltSize = 16; // Tamaño del salt en bytes
        private const int KeySize = 32; // Tamaño del hash resultante en bytes
        private const int Iterations = 10000; // Número de iteraciones para PBKDF2

        public string HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);

                var hash = new Rfc2898DeriveBytes(password, salt, Iterations);
                var hashBytes = hash.GetBytes(KeySize);

                var hashWithSaltBytes = new byte[SaltSize + KeySize];
                Array.Copy(salt, 0, hashWithSaltBytes, 0, SaltSize);
                Array.Copy(hashBytes, 0, hashWithSaltBytes, SaltSize, KeySize);

                return Convert.ToBase64String(hashWithSaltBytes);
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var hashWithSaltBytes = Convert.FromBase64String(hashedPassword);

            var salt = new byte[SaltSize];
            Array.Copy(hashWithSaltBytes, 0, salt, 0, SaltSize);

            var hash = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hashBytes = hash.GetBytes(KeySize);

            for (int i = 0; i < KeySize; i++)
            {
                if (hashWithSaltBytes[i + SaltSize] != hashBytes[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}

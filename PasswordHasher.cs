using System.Security.Cryptography;

namespace Diplom
{
    public class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Пароль не может быть пустым");

            byte[] salt = GenerateRandomSalt();
            byte[] hash = GenerateHash(password, salt);
            byte[] hashBytes = CombineSaltAndHash(salt, hash);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            if (hashBytes.Length != SaltSize + HashSize)
                return false;

            byte[] salt = ExtractSalt(hashBytes);
            byte[] hash = GenerateHash(password, salt);

            return CompareHashes(hashBytes, hash);
        }

        private byte[] GenerateRandomSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);
            return salt;
        }

        private byte[] GenerateHash(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256);

            return pbkdf2.GetBytes(HashSize);
        }

        private static byte[] CombineSaltAndHash(byte[] salt, byte[] hash)
        {
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, HashSize);
            return hashBytes;
        }

        private byte[] ExtractSalt(byte[] hashBytes)
        {
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);
            return salt;
        }

        private bool CompareHashes(byte[] originalHashBytes, byte[] computedHash)
        {
            for (int i = 0; i < HashSize; i++)
            {
                if (originalHashBytes[i + SaltSize] != computedHash[i])
                    return false;
            }
            return true;
        }
    }
}

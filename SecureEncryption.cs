using System.Security.Cryptography;

public class SecureEncryption
{
    private const string KeyFilePath = "encryption_key.txt";
    private const int KeySize = 32;
    private const int NonceSize = 12;
    private const int TagSize = 16;

    private readonly byte[] _key;

    public static byte[] GetOrCreateKey()
    {
        if (File.Exists(KeyFilePath))
        {
            string base64Key = File.ReadAllText(KeyFilePath);
            return Convert.FromBase64String(base64Key);
        }

        byte[] key = new byte[KeySize];
        RandomNumberGenerator.Fill(key);

        string directory = Path.GetDirectoryName(KeyFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(KeyFilePath, Convert.ToBase64String(key));
        return key;
    }

    public SecureEncryption() : this(GetOrCreateKey())
    {
    }

    public SecureEncryption(byte[] key)
    {
        if (key == null || key.Length != KeySize)
            throw new ArgumentException($"Ключ должен быть {KeySize} байт (256 бит)");
        _key = key;
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return plainText;

        byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        byte[] cipherText = new byte[plainBytes.Length];
        byte[] tag = new byte[TagSize];
        byte[] nonce = new byte[NonceSize];

        RandomNumberGenerator.Fill(nonce);

        using (var aesGcm = new AesGcm(_key))
        {
            aesGcm.Encrypt(nonce, plainBytes, cipherText, tag);
        }

        byte[] result = new byte[nonce.Length + cipherText.Length + tag.Length];
        Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
        Buffer.BlockCopy(cipherText, 0, result, nonce.Length, cipherText.Length);
        Buffer.BlockCopy(tag, 0, result, nonce.Length + cipherText.Length, tag.Length);

        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
            return cipherText;

        byte[] encryptedData = Convert.FromBase64String(cipherText);
        if (encryptedData.Length < NonceSize + TagSize)
            throw new ArgumentException("Некорректный зашифрованный текст");

        byte[] nonce = new byte[NonceSize];
        byte[] tag = new byte[TagSize];
        byte[] cipherBytes = new byte[encryptedData.Length - nonce.Length - tag.Length];

        Buffer.BlockCopy(encryptedData, 0, nonce, 0, nonce.Length);
        Buffer.BlockCopy(encryptedData, nonce.Length, cipherBytes, 0, cipherBytes.Length);
        Buffer.BlockCopy(encryptedData, nonce.Length + cipherBytes.Length, tag, 0, tag.Length);

        byte[] plainBytes = new byte[cipherBytes.Length];

        using (var aesGcm = new AesGcm(_key))
        {
            aesGcm.Decrypt(nonce, cipherBytes, tag, plainBytes);
        }

        return System.Text.Encoding.UTF8.GetString(plainBytes);
    }
}

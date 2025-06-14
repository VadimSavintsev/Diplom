using System.Security.Cryptography;
using System.Text;

public class PasswordGenerator : IDisposable
{
    private static readonly char[] _specialChars = "!@#$%^&*()-_=+[]{};:'\",.<>/?".ToCharArray();
    private readonly RandomNumberGenerator _rng;
    private bool _disposed = false;

    public PasswordGenerator()
    {
        _rng = RandomNumberGenerator.Create();
    }

    public string GeneratePassword(
        int length = 12,
        bool requireDigit = true,
        bool requireLowercase = true,
        bool requireUppercase = true,
        bool requireSpecialChar = true)
    {
        if (length < 8)
            throw new ArgumentException("Пароль должен быть не менее 8 символов", nameof(length));

        var password = new StringBuilder();
        var charSets = new List<char[]>();

        if (requireDigit) charSets.Add("0123456789".ToCharArray());
        if (requireLowercase) charSets.Add("abcdefghijklmnopqrstuvwxyz".ToCharArray());
        if (requireUppercase) charSets.Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
        if (requireSpecialChar) charSets.Add(_specialChars);

        foreach (var charSet in charSets)
        {
            password.Append(GetRandomChar(charSet));
        }

        var allChars = charSets.SelectMany(c => c).ToArray();
        for (int i = password.Length; i < length; i++)
        {
            password.Append(GetRandomChar(allChars));
        }

        return Shuffle(password.ToString());
    }

    private char GetRandomChar(char[] charSet)
    {
        byte[] randomByte = new byte[1];
        _rng.GetBytes(randomByte);
        return charSet[randomByte[0] % charSet.Length];
    }

    private string Shuffle(string str)
    {
        var chars = str.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            byte[] randomByte = new byte[1];
            _rng.GetBytes(randomByte);
            int j = randomByte[0] % chars.Length;
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }
        return new string(chars);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _rng.Dispose();
            }
            _disposed = true;
        }
    }

    ~PasswordGenerator()
    {
        Dispose(false);
    }
}

using System.Text;

public class LoginGenerator
{
    private static readonly Random _random = new Random();

    public string GenerateLogin(int length = 8)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var sb = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            sb.Append(validChars[_random.Next(validChars.Length)]);
        }

        return sb.ToString();
    }
}

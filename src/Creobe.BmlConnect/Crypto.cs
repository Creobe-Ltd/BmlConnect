/* using System.Security.Cryptography;

namespace Creobe.BmlConnect;

internal sealed class Crypto
{
    public static string Md5(string value)
    {
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(value);
        byte[] hashBytes = MD5.HashData(inputBytes);

        return Convert.ToBase64String(hashBytes).ToLower();
    }

    public static string Sha1(string value)
    {
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(value);
        byte[] hashBytes = SHA1.HashData(inputBytes);

        return Convert.ToHexString(hashBytes).ToLower();
    }
} */
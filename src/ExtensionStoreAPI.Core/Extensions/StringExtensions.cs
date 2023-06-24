using System.Security.Cryptography;
using System.Text;

namespace ExtensionStoreAPI.Core.Extensions;

public static class StringExtensions
{
    public static bool EqualsCryptographically(this string str1, string str2)
    {
        var hash1 = SHA256.HashData(Encoding.UTF8.GetBytes(str1));
        var hash2 = SHA256.HashData(Encoding.UTF8.GetBytes(str2));

        if (hash1.Length != hash2.Length)
        {
            return false;
        }

        return !hash1.Where((t, i) => t != hash2[i]).Any();
    }
}
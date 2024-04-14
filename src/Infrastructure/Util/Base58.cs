using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Infrastructure.Util;
public static class Base58
{
    public static readonly string ALPHABET_STRING = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
    private static readonly char[] ALPHABET = ALPHABET_STRING.ToCharArray();

    public static string randomString(int length)
    {
        char[] result = new char[length];

        for (int i = 0; i < length; ++i)
        {
            byte[] randomNumber = new byte[1];
            RandomNumberGenerator.Fill(randomNumber);
            int position = randomNumber[0] % ALPHABET.Length;
            result[i] = ALPHABET[position];
        }

        return new string(result);
    }

    public static string randomString()
    {
        return randomString(32);
    }
}

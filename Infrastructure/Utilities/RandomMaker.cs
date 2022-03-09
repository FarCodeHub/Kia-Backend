using System;
using System.Security.Cryptography;
namespace Infrastructure.Utilities
{
    public static class RandomMaker
    {
        public static string GenerateRandomString(int maxChar)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
         
            
            for (var i = 0; i < stringChars.Length; i++)
            {

                var randomGenerator = RandomNumberGenerator.Create(); // Compliant for security-sensitive use cases
                byte[] data = new byte[16];
                randomGenerator.GetBytes(data);
                stringChars[i] = BitConverter.ToChar(data);

            }

            var finalString = "-" +  new string(stringChars);

            if (maxChar < 9)
            {
                finalString = finalString[..maxChar];
            }

            return finalString;
        }

    }
}

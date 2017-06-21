using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PowerControlDemo.Helper
{
    public class Utility
    {
        public static string GetHashedString(HashType type, string str, bool isLower = false)
        {
            return GetHashedString(type, str, Encoding.UTF8, isLower);
        }

        public static string GetHashedString(HashType type, string str, Encoding encoding, bool isLower = false)
        {
            HashAlgorithm algorithm;
            switch (type)
            {
                case HashType.MD5:
                    algorithm = MD5.Create();
                    break;

                case HashType.SHA1:
                    algorithm = SHA1.Create();
                    break;

                case HashType.SHA256:
                    algorithm = SHA256.Create();
                    break;

                case HashType.SHA384:
                    algorithm = SHA384.Create();
                    break;

                case HashType.SHA512:
                    algorithm = SHA512.Create();
                    break;

                default:
                    algorithm = MD5.Create();
                    break;
            }
            byte[] bytes = encoding.GetBytes(str);
            byte[] hashedBytes = algorithm.ComputeHash(bytes);
            algorithm.Dispose();
            StringBuilder sbText = new StringBuilder();
            if (isLower)
            {
                foreach (byte b in hashedBytes)
                {
                    sbText.Append(b.ToString("x2"));
                }
            }
            else
            {
                foreach (byte b in hashedBytes)
                {
                    sbText.Append(b.ToString("X2"));
                }
            }
            return sbText.ToString();
        }
    }

    /// <summary>
    /// Hash 类型
    /// </summary>
    public enum HashType
    {
        MD5 = 0,
        SHA1 = 1,
        SHA256 = 2,
        SHA384 = 3,
        SHA512 = 4
    }
}
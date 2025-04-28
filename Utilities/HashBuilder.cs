using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Kuro.UnityUtils
{
    public class HashBuilder : MonoBehaviour
    {
        public static class HashHelper
        {
            public static int ComputeSha256Hash(string rawData)
            {
                using var sha256 = SHA256.Create();
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToInt32(bytes, 0);
            }
        }
    }
}

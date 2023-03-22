using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace VitroCore
{
    public class EncodeHashManager
    {
        public string EncodeHash(string texto)
        {
            using(SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();
                foreach (byte h in hash)
                {
                    builder.Append(h.ToString("X2"));
                }
                return builder.ToString();
            }
        }
    }
}

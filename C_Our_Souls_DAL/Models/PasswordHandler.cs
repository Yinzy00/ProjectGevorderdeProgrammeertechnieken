using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public class PasswordHandler
    {
        public string Key { get; set; } = Guid.NewGuid().ToString();
        public const Int32 bytePermutation1 = 0x19;
        public const Int32 bytePermutation2 = 0x59;
        public const Int32 bytePermutation3 = 0x17;
        public const Int32 bytePermutation4 = 0x41;
        public PasswordHandler()
        {

        }
        public PasswordHandler(string key)
        {
            Key = key;
        }

        // encoding
        public string Encrypt(string password)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(password)));
            // reference https://msdn.microsoft.com/en-us/library/ds4kkd55(v=vs.110).aspx

        }


        // decoding
        public string Decrypt(string password)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(password)));
            // reference https://msdn.microsoft.com/en-us/library/system.convert.frombase64string(v=vs.110).aspx

        }

        // encrypt
        private byte[] Encrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(Key,
            new byte[] { bytePermutation1,
                         bytePermutation2,
                         bytePermutation3,
                         bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }

        // decrypt
        private byte[] Decrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(Key,
            new byte[] { bytePermutation1,
                         bytePermutation2,
                         bytePermutation3,
                         bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }
    }
}

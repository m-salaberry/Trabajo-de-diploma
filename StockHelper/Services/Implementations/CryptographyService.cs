using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public static class CryptographyService
    {
        /// <summary>
        /// Computes the MD5 hash of the specified input string and returns the result as a hexadecimal string.
        /// </summary>
        /// <remarks>The input string is encoded using Unicode (UTF-16) before hashing. The returned hash
        /// is always 32 characters long. This method is not suitable for cryptographic security purposes.</remarks>
        /// <param name="input">The input string to hash. Cannot be null.</param>
        /// <returns>A lowercase hexadecimal string representing the MD5 hash of the input.</returns>
        public static string HashMd5(string input)
        {
            StringBuilder sb = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                byte[] retVal = md5.ComputeHash(Encoding.Unicode.GetBytes(input));
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }

        private static string encryptionKey = Environment.GetEnvironmentVariable("STOCK_HELPER_SECRET_KEY")!;
        /// <summary>
        /// Encrypts the specified plain text string using AES encryption and returns the result as a Base64-encoded
        /// string.
        /// </summary>
        /// <remarks>The encryption uses a predefined key and initialization vector derived from a
        /// password and salt. The output can be decrypted only by a compatible decryption method using the same key and
        /// salt. The method is not intended for secure password storage.</remarks>
        /// <param name="clearText">The plain text string to encrypt. Cannot be null.</param>
        /// <returns>A Base64-encoded string containing the encrypted representation of the input text.</returns>
        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(clearBytes, 0, clearBytes.Length);
                        csEncrypt.Close();
                    }
                    clearText = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            return clearText;
        }
        /// <summary>
        /// Decrypts the specified Base64-encoded cipher text using a predefined encryption key.
        /// </summary>
        /// <remarks>The decryption uses a static encryption key and a fixed salt value. The method
        /// expects the input to be a valid Base64-encoded string produced by the corresponding encryption method. If
        /// the input is not valid or was not encrypted with the matching key and salt, decryption will fail.</remarks>
        /// <param name="cipherText">The Base64-encoded string representing the encrypted data to decrypt. Spaces will be replaced with plus
        /// signs before decoding.</param>
        /// <returns>The decrypted plain text string.</returns>
        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            cipherText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return cipherText;
        }
    }
}

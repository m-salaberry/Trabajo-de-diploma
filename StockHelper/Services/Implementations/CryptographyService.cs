using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Services.Contracts.CustomsException;

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

        private const string FieldPrefix = "enc:v1:";

        private static readonly byte[] _keySalt =
            { 0x53, 0x74, 0x6f, 0x63, 0x6b, 0x48, 0x65, 0x6c, 0x70, 0x65, 0x72, 0x53, 0x61, 0x6c, 0x74 };

        /// <summary>
        /// Derives a 32-byte AES key and a 32-byte HMAC key from the configured master secret.
        /// </summary>
        /// <returns>A tuple with the AES key and the HMAC key.</returns>
        private static (byte[] aesKey, byte[] hmacKey) DeriveKeys()
        {
            if (string.IsNullOrEmpty(encryptionKey))
            {
                throw new MySystemException(
                    "Encryption key (STOCK_HELPER_SECRET_KEY) is not configured.", "Services");
            }

            using (var pdb = new Rfc2898DeriveBytes(encryptionKey, _keySalt, 10000, HashAlgorithmName.SHA256))
            {
                return (pdb.GetBytes(32), pdb.GetBytes(32));
            }
        }

        /// <summary>
        /// Encrypts a sensitive field for storage, tagging it with an integrity code (HMAC).
        /// Returns null unchanged so nullable columns stay null.
        /// </summary>
        /// <param name="plainText">The plaintext value to protect.</param>
        /// <returns>The protected representation, or null when the input is null.</returns>
        public static string ProtectField(string plainText)
        {
            if (plainText == null)
            {
                return null;
            }

            var (aesKey, hmacKey) = DeriveKeys();

            byte[] iv;
            byte[] cipher;
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.GenerateIV();
                iv = aes.IV;
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cipher = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
            }

            byte[] ivPlusCipher = new byte[iv.Length + cipher.Length];
            Buffer.BlockCopy(iv, 0, ivPlusCipher, 0, iv.Length);
            Buffer.BlockCopy(cipher, 0, ivPlusCipher, iv.Length, cipher.Length);

            byte[] mac;
            using (var hmac = new HMACSHA256(hmacKey))
            {
                mac = hmac.ComputeHash(ivPlusCipher);
            }

            return FieldPrefix + Convert.ToBase64String(ivPlusCipher) + ":" + Convert.ToBase64String(mac);
        }

        /// <summary>
        /// Reverses <see cref="ProtectField"/>, verifying the integrity code before decrypting.
        /// Values without the protection prefix are returned unchanged (legacy/plaintext data).
        /// </summary>
        /// <param name="storedValue">The stored value to unprotect.</param>
        /// <returns>The original plaintext, or null when the input is null.</returns>
        /// <exception cref="MySystemException">Thrown when the integrity check fails (tampered/corrupt data).</exception>
        public static string UnprotectField(string storedValue)
        {
            if (storedValue == null)
            {
                return null;
            }

            if (!storedValue.StartsWith(FieldPrefix, StringComparison.Ordinal))
            {
                return storedValue;
            }

            string[] parts = storedValue.Substring(FieldPrefix.Length).Split(':');
            if (parts.Length != 2)
            {
                throw new MySystemException("El sistema no puede verificar la integridad", "Services");
            }

            byte[] ivPlusCipher;
            byte[] mac;
            try
            {
                ivPlusCipher = Convert.FromBase64String(parts[0]);
                mac = Convert.FromBase64String(parts[1]);
            }
            catch (FormatException)
            {
                throw new MySystemException("El sistema no puede verificar la integridad", "Services");
            }

            var (aesKey, hmacKey) = DeriveKeys();

            byte[] expectedMac;
            using (var hmac = new HMACSHA256(hmacKey))
            {
                expectedMac = hmac.ComputeHash(ivPlusCipher);
            }
            if (!CryptographicOperations.FixedTimeEquals(mac, expectedMac))
            {
                throw new MySystemException("El sistema no puede verificar la integridad", "Services");
            }

            byte[] iv = new byte[16];
            byte[] cipher = new byte[ivPlusCipher.Length - iv.Length];
            Buffer.BlockCopy(ivPlusCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(ivPlusCipher, iv.Length, cipher, 0, cipher.Length);

            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = iv;
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
        }
    }
}

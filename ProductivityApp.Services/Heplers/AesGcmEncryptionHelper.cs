using System.Security.Cryptography;
using System.Text;

namespace ProductivityApp.Services.Heplers
{
    public class AesGcmEncryptionHelper : IAesGcmEncryptionHelper
    {
        private const int KeySize = 32; 
        private const int NonceSize = 12; 
        private const int TagSize = 16; 

        private byte[] GenerateKey(string userId)
        {
            using var sha = SHA256.Create();
            var keyBytes = Encoding.UTF8.GetBytes(userId + "YourStaticPepperHere");
            return sha.ComputeHash(keyBytes); 
        }

        public string Encrypt(string plainText, string userId)
        {
            var key = GenerateKey(userId);
            var nonce = RandomNumberGenerator.GetBytes(NonceSize);
            var plaintextBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = new byte[plaintextBytes.Length];
            var tag = new byte[TagSize];

            using var aes = new AesGcm(key);
            aes.Encrypt(nonce, plaintextBytes, cipherBytes, tag);

            var result = new byte[NonceSize + TagSize + cipherBytes.Length];
            Buffer.BlockCopy(nonce, 0, result, 0, NonceSize);
            Buffer.BlockCopy(tag, 0, result, NonceSize, TagSize);
            Buffer.BlockCopy(cipherBytes, 0, result, NonceSize + TagSize, cipherBytes.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string cipherText, string userId)
        {
            var key = GenerateKey(userId);
            var fullCipher = Convert.FromBase64String(cipherText);

            var nonce = new byte[NonceSize];
            var tag = new byte[TagSize];
            var cipherBytes = new byte[fullCipher.Length - NonceSize - TagSize];

            Buffer.BlockCopy(fullCipher, 0, nonce, 0, NonceSize);
            Buffer.BlockCopy(fullCipher, NonceSize, tag, 0, TagSize);
            Buffer.BlockCopy(fullCipher, NonceSize + TagSize, cipherBytes, 0, cipherBytes.Length);

            var plainBytes = new byte[cipherBytes.Length];
            using var aes = new AesGcm(key);
            aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}

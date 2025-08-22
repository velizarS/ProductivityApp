using System.Security.Cryptography;
using System.Text;

namespace ProductivityApp.Services.Heplers
{
    public class AesGcmEncryptionHelper : IAesGcmEncryptionHelper
    {
        private const int KeySize = 32; // 256 bit
        private const int NonceSize = 12; // AES-GCM standard nonce size
        private const int TagSize = 16; // authentication tag size

        // Генериране на ключ на база userId + salt/pepper (може да е static string)
        private byte[] GenerateKey(string userId)
        {
            using var sha = SHA256.Create();
            // може да добавиш допълнителен "pepper" за сигурност
            var keyBytes = Encoding.UTF8.GetBytes(userId + "YourStaticPepperHere");
            return sha.ComputeHash(keyBytes); // връща 32 байта за AES-256
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

            // комбинираме nonce + tag + ciphertext и връщаме Base64
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

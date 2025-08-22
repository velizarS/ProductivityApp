namespace ProductivityApp.Services.Heplers
{
    public interface IAesGcmEncryptionHelper
    {
        string Encrypt(string plainText, string userId);
        string Decrypt(string cipherText, string userId);
    }
}

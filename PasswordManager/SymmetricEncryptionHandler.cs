using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Utils;

public class SymmetricEncryptionHandler
{
    private readonly string SECRET_KEY;

    public SymmetricEncryptionHandler()
    {
        DotNetEnv.Env.Load();

        SECRET_KEY = Environment.GetEnvironmentVariable("SECRET_STRING")!;
    }

    public string Encrypt(string text)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(SECRET_KEY);
        aes.IV = iv;
        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using MemoryStream ms = new MemoryStream();
        using CryptoStream cryptoStream = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write);
        using StreamWriter sw = new StreamWriter((Stream) cryptoStream);
        sw.Write(text);

        array = ms.ToArray();

        return Convert.ToBase64String(array);
    }

    public string Decrypt(string text)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(text);
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(text);
        aes.IV = iv;
        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using MemoryStream ms = new MemoryStream(buffer);
        using CryptoStream cryptoStream = new CryptoStream((Stream) ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cryptoStream);

        return sr.ReadToEnd();
    }
}





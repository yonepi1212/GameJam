using System.IO;
using System.Security.Cryptography;

/*  AseIVUtility
    暗号化
*/
public class AseIVUtility
{
    private const string ASE_IV = "1234567890123456";
    private const string ASE_KEY = "abcdefghijklmnop";

    //文字列をAESで暗号化
    public static string Encrypt(string text)
    {
        RijndaelManaged aes = new RijndaelManaged();
        aes.BlockSize = 128;
        aes.KeySize = 128;
        aes.Padding = PaddingMode.Zeros;
        //aes.Mode = CipherMode.ECB;
        aes.Mode = CipherMode.CBC;

        aes.Key = System.Text.Encoding.UTF8.GetBytes(ASE_KEY);
        // CBCモードを利用する場合は設定する.
        aes.IV = System.Text.Encoding.UTF8.GetBytes(ASE_IV);

        ICryptoTransform encrypt = aes.CreateEncryptor();
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);

        byte[] text_bytes = System.Text.Encoding.UTF8.GetBytes(text);

        cryptStream.Write(text_bytes, 0, text_bytes.Length);
        cryptStream.FlushFinalBlock();

        byte[] encrypted = memoryStream.ToArray();

        //Debug.Log( "byte :" + encrypted[0] );
        return (System.Convert.ToBase64String(encrypted));
    }
    //文字列をAESで複合化
    public static string Decrypt(string text)
    {
        RijndaelManaged aes = new RijndaelManaged();
        aes.BlockSize = 128;
        aes.KeySize = 128;
        aes.Padding = PaddingMode.Zeros;
        //aes.Mode = CipherMode.ECB;
        aes.Mode = CipherMode.CBC;  // CBCモードを利用する場合は設定をこちらに.

        aes.Key = System.Text.Encoding.UTF8.GetBytes(ASE_KEY);
        // CBCモードを利用する場合はIVの設定を行う.
        aes.IV = System.Text.Encoding.UTF8.GetBytes(ASE_IV);

        ICryptoTransform decryptor = aes.CreateDecryptor();

        byte[] encrypted = System.Convert.FromBase64String(text);
        byte[] planeText = new byte[encrypted.Length];

        MemoryStream memoryStream = new MemoryStream(encrypted);
        CryptoStream cryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        cryptStream.Read(planeText, 0, planeText.Length);

        return (System.Text.Encoding.UTF8.GetString(planeText)).TrimEnd('\0');
    }
}

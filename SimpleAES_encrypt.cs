using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

// Token: 0x020001DC RID: 476
public static class SimpleAES
{
	// Token: 0x06000EF8 RID: 3832 RVA: 0x00045384 File Offset: 0x00043584
	public static string Decrypt(string encrypted)
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(SimpleAES.key, SimpleAES.vector);
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(SimpleAES.Decrypt(Convert.FromBase64String(encrypted), decryptor));
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x000453CC File Offset: 0x000435CC
	public static byte[] Decrypt(byte[] buffer, ICryptoTransform decryptor)
	{
		return SimpleAES.Transform(buffer, decryptor);
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x00045348 File Offset: 0x00043548
	public static string Encrypt(string unencrypted)
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(SimpleAES.key, SimpleAES.vector);
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return Convert.ToBase64String(SimpleAES.Encrypt(uTF8Encoding.GetBytes(unencrypted), encryptor));
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x000453C0 File Offset: 0x000435C0
	public static byte[] Encrypt(byte[] buffer, ICryptoTransform encryptor)
	{
		return SimpleAES.Transform(buffer, encryptor);
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x000453D8 File Offset: 0x000435D8
	public static byte[] Transform(byte[] buffer, ICryptoTransform transform)
	{
		MemoryStream memoryStream = new MemoryStream();
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
		{
			cryptoStream.Write(buffer, 0, buffer.Length);
		}
		return memoryStream.ToArray();
	}

	// Token: 0x04000A40 RID: 2624
	private static byte[] key = new byte[]
	{
		12,
		128,
		45,
		11,
		24,
		26,
		14,
		6,
		12,
		184,
		4,
		162,
		37,
		112,
		18,
		209
	};

	// Token: 0x04000A41 RID: 2625
	private static byte[] vector = new byte[]
	{
		146,
		12,
		6,
		111,
		4,
		2,
		101,
		45,
		97,
		121,
		18,
		14,
		79,
		32,
		114,
		156
	};
}

public class TwoDotsCrypt
{
	static public void Main (string[] args)
	{
		string s;
		while ((s = Console.ReadLine()) != null)
		{
			s = s.Replace( "\r", "").Replace( "\n", "" );
			Console.WriteLine (SimpleAES.Encrypt(s));
		}

	}
}


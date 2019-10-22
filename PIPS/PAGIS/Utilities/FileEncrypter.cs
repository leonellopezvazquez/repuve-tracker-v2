using System;
using System.Security.Cryptography;
using System.IO;

// ***WARNING*** this class is a duplicate - it also resides in the boss project as PIPS.Utilities.FileEncrypter
// in the future this class should be moved to PIPS.General.Util.Encryption 
// -- Gregory Ostermayr TODO fix this 
namespace PIPS.Utilities
{
	/// <summary>
	/// Summary description for FileEncrypter.
	/// </summary>
	public class FileEncrypter
	{
		private static string HeaderText = "PAGIS ENCRYPTED";
		private static string KeyString = "PIPSTECH";

		public static StreamWriter GetStreamWriter(Stream stream) {
			DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
			DES.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(KeyString);
			DES.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(KeyString);
			ICryptoTransform desencrypt = DES.CreateEncryptor();
			CryptoStream cryptostream = new CryptoStream(stream, desencrypt, CryptoStreamMode.Write);
			try {
				StreamWriter sw = new StreamWriter(cryptostream);
				try {
					sw.WriteLine(HeaderText);
					return sw;
				} catch {
					sw.Close();
					throw;
				}
			} catch {
				cryptostream.Close();
				throw;
			}
		}
		public static StreamWriter GetStreamWriter(string filename) {
			FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
			try {
				return GetStreamWriter(stream);
			} catch {
				stream.Close();
				throw;
			}
		}

		public static StreamReader GetStreamReader(string filename) {
			FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
			try {
				DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
				DES.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(KeyString);
				DES.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(KeyString);
				ICryptoTransform desdecrypt = DES.CreateDecryptor();
				CryptoStream cryptostream = new CryptoStream(stream, desdecrypt, CryptoStreamMode.Read);
				try {
					StreamReader sr = new StreamReader(cryptostream);
					try {
						string check = sr.ReadLine();
						if(!check.Equals(HeaderText))
							throw new IOException("File was not properly encrypted, PAGIS header is either not present or invalid.");
						return sr;
					} catch {
						sr.Close();
						throw;
					}
				} catch {
					cryptostream.Close();
					throw;
				}
			} catch {
				stream.Close();
				throw;
			}
		}

		public static void EncryptFile(string input, string output) {
			StreamReader sr = new StreamReader(input);
			try {
				StreamWriter sw = GetStreamWriter(output);
				try {
					string line;
					while((line = sr.ReadLine()) != null) {
						sw.WriteLine(line);
					}
				} finally {
					sw.Close();
				}
			} finally {
				sr.Close();
			}
		}

		public static bool IsEncrypted(string filename) {
			bool isEnc = false;
			FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
			try {
				DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
				DES.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(KeyString);
				DES.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(KeyString);
				ICryptoTransform desdecrypt = DES.CreateDecryptor();
				CryptoStream cryptostream = new CryptoStream(stream, desdecrypt, CryptoStreamMode.Read);
				try {
					StreamReader sr = new StreamReader(cryptostream);
					try {
						string check = sr.ReadLine();
						if(check.Equals(HeaderText))
							isEnc = true;
						//throw new IOException("File was not properly encrypted, PAGIS header is either not present or invalid.");
					} finally {
						sr.Close();
					}
				} finally {
					cryptostream.Close();
				}
			} catch {
			} finally {
				stream.Close();
			}
			return isEnc;
		}
	}
}

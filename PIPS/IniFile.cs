using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace PIPS {
	/// <summary>
	/// Create a New INI file to store or load data
	/// </summary>
	public class IniFile : MarshalByRefObject {
		public string path;

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section,string key,string val,string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section,string key,string def,StringBuilder retVal,int size,string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section,string key,string def,byte[] retVal,int size,string filePath);
		[DllImport("kernel32")]
		private static extern int WritePrivateProfileSection(string section, string strData, string filePath);

		/*public IniFile() {
			path = ".\\";
		}*/

		public IniFile(string path) {
			this.path = path;
		}

		//private static IniFile m_instance = new IniFile();

		/*public static IniFile Instance {
			get {
				return m_instance;
			}
		}*/

		/*public void setPath(string path) {
			this.path = path;
		}

		public string getPath() {
			return path;
		}*/

		public string Path {
			get {
				return this.path;
			}
			set {
				this.path = value;
			}
		}

		public void WriteDateTime(string section, string key, DateTime val) {
			this.WriteInt64(section, key, val.Ticks);
		}
		public void WriteInt64(string section, string key, long val) {
			this.WriteString(section, key, val.ToString(System.Globalization.CultureInfo.InvariantCulture));
		}
		public void WriteInt32(string section, string key, int val) {
			this.WriteString(section, key, val.ToString(System.Globalization.CultureInfo.InvariantCulture));
		}
		public void WriteBoolean(string section, string key, bool val) {
			this.WriteString(section, key, val.ToString());
		}

		public DateTime ReadDateTime(string section, string key, DateTime def) {
			return new DateTime(this.ReadInt64(section, key, def.Ticks));
		}
		public long ReadInt64(string section, string key, long def) {
			try {
				return long.Parse(this.ReadString(section, key, def.ToString(System.Globalization.CultureInfo.InvariantCulture)), System.Globalization.CultureInfo.InvariantCulture);
			} catch {}
			return def;
		}
		public int ReadInt32(string section, string key, int def) {
			try {
				return int.Parse(this.ReadString(section, key, def.ToString(System.Globalization.CultureInfo.InvariantCulture)), System.Globalization.CultureInfo.InvariantCulture);
			} catch {}
			return def;
		}
        /// <summary>
        /// Reads a boolean value from ini file
        /// </summary>
        /// <param name="section">ini section</param>
        /// <param name="key">ini key</param>
        /// <param name="def">The default key value returned if not present in the ini file</param>
        /// <returns></returns>
		public bool ReadBoolean(string section, string key, bool def) {
			try {
				return bool.Parse(this.ReadString(section, key, def.ToString()));
			} catch {}
			return def;
		}

		public void WriteString(string Section,string Key,string Value) {
			WriteString(Section, Key, Value, this.path);
		}

		private void WriteString(string Section,string Key,string Value, string FileName) {
			if(FileName == "")
				return;
			WritePrivateProfileString(Section,Key,Value,FileName);
		}
		
		public string ReadString(string Section,string Key,string Default) {
			return ReadString(Section, Key, Default, this.path);
		}

		private string ReadString(string Section,string Key,string Default, string FileName) {
			if(FileName == "") {
				return "";
			}
			StringBuilder result = new StringBuilder(16384);
			int i = GetPrivateProfileString(Section,Key,Default,result,16384,FileName);
			if (i == 16383) throw new ApplicationException("IniFile: Key value did not fit into buffer!");
			return result.ToString();
		}

		public bool IsSection(string Section) {
			return IsSection(Section, this.path);
		}

		public string[] GetKeys(string category) {
			byte[] returnString = new byte[65536];
			GetPrivateProfileString(category, null, null, returnString, 65536, this.Path);
			byte[] key = new byte[1024];
			ArrayList result = new ArrayList();
			int keyPos = 0;
			for (int i = 0; i < returnString.Length; i++) {
				if (returnString[i] == '\0') {
					if (keyPos == 0) break;
					result.Add(Encoding.ASCII.GetString(key, 0, keyPos));
					keyPos = 0;
					continue;
				}
				key[keyPos++] = returnString[i];
			}
			return (string[])result.ToArray(typeof(string));		
		}

		private bool IsSection(string Section, string FileName) {
			if(FileName == "") {
				return false;
			}
			StringBuilder temp = new StringBuilder(16384);
			int i = GetPrivateProfileString(Section,null,"",temp,16384,FileName);
			if (i == 16383) throw new ApplicationException("IniFile: Key value did not fit into buffer!");
			return (i>0);
		}

		public void WriteSection(string Section, string Data) {
			WriteSection(Section, Data, this.path);
		}

		private void WriteSection(string Section, string Data, string FileName) {
			if(FileName == "") {
				return;
			}
			WritePrivateProfileSection(Section, Data, FileName);
		}

		public void DeleteSection(string Section) {
			DeleteSection(Section, this.path);
		}

		private void DeleteSection(string Section, string FileName) {
			WritePrivateProfileString(Section, null, null, FileName);
		}

	}
}

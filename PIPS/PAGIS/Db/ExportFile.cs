using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
//NOTE: duplicate file warning
namespace PIPS.PAGIS.Db {
	/// <summary>
	/// Exports db table to xml
	/// </summary>
	public class ExportFile : IDisposable {
		private string filename;
		private FileStream file;
		private bool isNew;
        private const String DISK_SYNC_FILE_VERSION_BEING_WRITTEN = "3.0.0";

		public ExportFile(string filename) {
			this.filename = filename;
			this.isNew = !File.Exists(filename);
			this.file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
			if(this.isNew)
                this.WriteString(String.Format("<pips disk_sync_file_version='{0}' source='{1}'>", DISK_SYNC_FILE_VERSION_BEING_WRITTEN, Environment.MachineName));
		}

		public string FileName {
			get {
				return filename;
			}
		}

		#region GetVolumeSerial("C");

		public string GetVolumeSerial(string strDriveLetter) {
			try {
				uint serNum = 0;
				uint maxCompLen = 0;
				StringBuilder VolLabel = new StringBuilder(256); // Label
				UInt32 VolFlags = new UInt32();
				StringBuilder FSName = new StringBuilder(256); // File System Name
				long Ret = GetVolumeInformation(strDriveLetter, VolLabel, (UInt32)VolLabel.Capacity, ref serNum, ref maxCompLen, ref VolFlags, FSName, (UInt32)FSName.Capacity);
				return Convert.ToString(serNum);
			} catch {
				return DateTime.Now.Ticks.ToString();
			}
		}

		[DllImport("kernel32.dll")]
		private static extern long GetVolumeInformation(string PathName, StringBuilder VolumeNameBuffer, UInt32 VolumeNameSize, ref UInt32 VolumeSerialNumber, ref UInt32 MaximumComponentLength, ref UInt32 FileSystemFlags, StringBuilder FileSystemNameBuffer, UInt32 FileSystemNameSize);

		#endregion

		private void WriteString(string data) {
			byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(data);
			this.file.Write(buf, 0, buf.Length);
		}

		private void Close() {	
			if(this.file != null) {
				if(this.isNew) {
					this.WriteString("</pips>");
				}
				try {
					this.file.Close();
				} catch {}
			}
		}

		public void Save(XmlDocument xml) {
			lock(file) {
				this.file.Seek(0, SeekOrigin.End);
				xml.Save(this.file);
			}
		}

		public void EnumerateFiles(XmlNodeHandler xmlCallback) {
			lock(file) {
				file.Seek(0, SeekOrigin.Begin);
				XmlDocument doc = new XmlDocument();
				doc.Load(file);
				foreach(XmlNode node in doc.FirstChild.ChildNodes)
					xmlCallback(node);
			}
		}

		#region IDisposable Members

		public void Dispose() {
			if(this.file != null) {
				try {
					this.Close();
				} catch {}
				this.file = null;
			}
		}

		#endregion
	}
}

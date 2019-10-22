using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using PIPS.PAGIS.Db;

namespace PIPS.PAGIS.Db.SQLite
{
	/// <summary>
	/// Summary description for DataRepositoryBase.
	/// </summary>
	public class DataRepositoryBase : IDisposable {
		private string _dir;
		private DataFileCollection dataFiles;
		
		public DataRepositoryBase(string dir) {
			this._dir = dir;
			this.dataFiles = new DataFileCollection();
		}

		public DataFileCollection DataFiles {
			get {
				return this.dataFiles;
			}
		}

		public string DataDirectory {
			get {
				if(!Directory.Exists(_dir))
					Directory.CreateDirectory(_dir);
				return _dir;
			}
		}

		public void Initialize() {
            foreach (DataFileBase file in this.dataFiles)
            {
                try
                {
                    file.Initialize(this);
                }
                catch (Exception ex)
                {
                    const string format =
                      "ERROR: Unable to create or write to local database files. \nThis is most likely due to lack of permissions on the repository directory '{0}'. \nException has been written to log. \n\nPAGIS will now close. ";
                    var msg = string.Format(format + "\nException: \n{1}", _dir, ex);
                    //Logger.Exception(LoggerPriority.High, ex);
                    msg = string.Format(format, _dir);
                    //Logger.WriteLine(msg);
                    MessageBox.Show(msg, "PAGIS - Error creating/writing to local database files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    break;
                }
            }
		}

		#region IDisposable Members

		public virtual void Dispose() {
			if(this.dataFiles != null) {
				try {
					foreach(DataFileBase file in this.dataFiles) {
						try {
							file.Dispose();
						} catch {}
					}
					this.dataFiles.Clear();
				} catch {}
				this.dataFiles = null;
			}
		}

		#endregion

	}
}

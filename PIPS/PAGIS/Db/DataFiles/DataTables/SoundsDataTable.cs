using System;
using PIPS.Devices.Sound.Output;
using PIPS.PAGIS.Db.SQLite;
using PIPS.Utilities;

namespace PIPS.PAGIS.Db.DataFiles.DataTables {
	public enum SoundEvents : int {
		CameraDetection = 0,
		AlarmLow,
		AlarmMedium,
		AlarmHigh,
		NoMatchInDatabase,
		LoginFailed,
		Menu,
		NoData,
		ShuttingDown,
		Saved,
		Welcome,
		Goodbye,
		CaptureOn,
		CaptureOff
	}

	/// <summary>
	/// Summary description for SoundsDataTable.
	/// </summary>
	public class SoundsDataTable : DataTableBase {
		public SoundsDataTable() {
			this.Columns.Add(new StringDataColumn("eventname"));
			this.Columns.Add(new StringDataColumn("filename"));
			this.Columns.Add(new BooleanDataColumn("isenabled"));
		}

		public override string Name {
			get {
				return "t_sounds";
			}
		}

		public override int IndexColumn {
			get {
				return 0;
			}
		}

		private string CreateFileString(string lang, string file) {
			return string.Format("{0}\\sounds\\{1}_{2}_Female.wav", System.Environment.CurrentDirectory, file, lang);
		}

		private void SaveSoundsDataEvent(SoundEvents sound, string lang, string file) {
			SoundsDataEvent ev = this.CreateSoundsDataEvent();
			ev.IsEnabled = true;
			ev.ID = -1;
			ev.File = this.CreateFileString(lang, file);
			ev.Event = sound.ToString();
			this.Save(ev);
		}

		public void PlaySound(SoundEvents sound) {
			this.PlaySound(sound.ToString());
		}
		public void PlaySound(string sound) {
			long[] ids = this.SelectIDsByIndexColumn(sound);
			if((ids != null) && (ids.Length > 0)) {
				//PIPS.Logger.WriteLine("SoundsDataTable.PlaySound({0})", sound);
				SoundsDataEvent ev = this.SelectByID(ids[0]) as SoundsDataEvent;
				if((ev != null) && ev.IsEnabled)
					ev.Play();
			} else if(System.IO.File.Exists(sound)) {
				try {
					System.IO.Stream stream = System.IO.File.OpenRead(sound);
					if ( stream != null ) {
						WavePlayback playback = new WavePlayback(stream);
						playback.Play();
					}
				} catch (Exception ex) {
					//Logger.Exception(ex);
					try {
						WaveBasic.PlaySoundAsync(sound);
					} catch(Exception ex2) {
						//PIPS.Logger.Exception(ex2);
					}
				}
			}
		}

		protected override void OnInitialized() {
			long[] ids = this.SelectIDs();
			if((ids == null) || (ids.Length <= 0)) {
				string culture = System.Globalization.CultureInfo.CurrentUICulture.Name;
				string lang = "UK";
				if(culture == "en-US")
					lang = "US";
				SoundsDataEvent ev = this.CreateSoundsDataEvent();
				ev.IsEnabled = true;
				ev.ID = -1;
				ev.File = string.Format("{0}\\sounds\\Beep.wav", System.Environment.CurrentDirectory);
				ev.Event = SoundEvents.CameraDetection.ToString();
				this.Save(ev);
				this.SaveSoundsDataEvent(SoundEvents.AlarmLow, lang, "LowAlert");
				this.SaveSoundsDataEvent(SoundEvents.AlarmMedium, lang, "MediumAlert");
				this.SaveSoundsDataEvent(SoundEvents.AlarmHigh, lang, "HighAlert");
				this.SaveSoundsDataEvent(SoundEvents.CaptureOn, lang, "CapResuming");
				this.SaveSoundsDataEvent(SoundEvents.CaptureOff, lang, "CapOff");
				this.SaveSoundsDataEvent(SoundEvents.Welcome, lang, "Welcome");
				this.SaveSoundsDataEvent(SoundEvents.Goodbye, lang, "Goodbye");
				this.SaveSoundsDataEvent(SoundEvents.LoginFailed, lang, "LoginFail");
				this.SaveSoundsDataEvent(SoundEvents.ShuttingDown, lang, "ShutDown");
				this.SaveSoundsDataEvent(SoundEvents.Menu, lang, "Menu");
				this.SaveSoundsDataEvent(SoundEvents.NoData, lang, "NoData");
				this.SaveSoundsDataEvent(SoundEvents.NoMatchInDatabase, lang, "NoMatchInDB");
				this.SaveSoundsDataEvent(SoundEvents.Saved, lang, "Saved");
				
			}
			base.OnInitialized ();
		}

		public SoundsDataEvent CreateSoundsDataEvent() {
			return new SoundsDataEvent(this);
		}

		public override DataEvent CreateDataEvent() {
			return this.CreateSoundsDataEvent();
		}
	}
}

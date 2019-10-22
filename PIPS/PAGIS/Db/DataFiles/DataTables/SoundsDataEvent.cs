using System;
using PIPS.Devices.Sound.Output;
using PIPS.PAGIS.Db.SQLite;
using PIPS.Utilities;

namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
    public class SoundsDataEvent : DataEvent {

        public SoundsDataEvent(SoundsDataTable table) : base(table) {}

        public void Play() {
            //if(this.File != null) {
            try {
                //PIPS.Logger.WriteLine(false, "SoundsDataEvent.Play({0})", this.File);
                System.IO.Stream stream = System.IO.File.OpenRead(this.File);
                if ( stream != null ) {
                    WavePlayback playback = new WavePlayback(stream);
                    playback.Play();
                }
            } catch (Exception ex) {
                //Logger.Exception(ex);
            }
            //}
        }

        public string Event {
            get {
                return (string)this[0];
            }
            set {
                this[0] = value;
            }
        }

        public string File {
            get {
                return (string)this[1];
            }
            set {
                this[1] = value;
            }
        }

        public bool IsEnabled {
            get {
                return (bool)this[2];
            }
            set {
                this[2] = value;
            }
        }
    }
}
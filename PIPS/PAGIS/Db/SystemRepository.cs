using PIPS.PAGIS.Db.DataFiles;
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db
{
	/// <summary>
	/// Summary description for DataRepository.
	/// </summary>
	public class SystemRepository : DataRepositoryBase
	{
		public SystemRepository() : this(System.Environment.CurrentDirectory + @"\data"){}
		public SystemRepository(string dir) : base(dir) {

            //This line initializes the Synchronizer ini, and needs to be called...even though it
            //looks like it isn't doing anything. (Mark McKnight)
            this.DataFiles.Add(new AuditsDataFile());
            this.DataFiles.Add(new EventsDataFile());
            this.DataFiles.Add(new LoginsDataFile(false));
            this.DataFiles.Add(new TargetsDataFile());
            this.DataFiles.Add(new HotLists.HotListsDataFile());
            this.DataFiles.Add(new SoundsDataFile());
            this.Initialize();
		}

        public AuditsDataFile Audits
        {
            get
            {
                return this.DataFiles[0] as AuditsDataFile;
            }
        }

        public EventsDataFile Events
        {
            get
            {
                return this.DataFiles[1] as EventsDataFile;
            }
        }

        public LoginsDataFile Logins
        {
            get
            {
                return this.DataFiles[2] as LoginsDataFile;
            }
        }

        public TargetsDataFile Targets
        {
            get
            {
                return this.DataFiles[3] as TargetsDataFile;
            }
        }

        public HotLists.HotListsDataFile HotLists
        {
            get
            {
                return this.DataFiles[4] as HotLists.HotListsDataFile;
            }
        }

        public SoundsDataFile Sounds
        {
            get
            {
                return this.DataFiles[5] as SoundsDataFile;
            }
        }

    }
}

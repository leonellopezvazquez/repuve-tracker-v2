using System.Data.SQLite;

namespace PIPS.PAGIS.Db.SQLite
{
    public abstract class SqlDataCommandBase : DataCommandBase
    {
        //private SQLiteCommand cmd;
        private string cmdText;
        private SQLiteParameter[] parameters;

        public SqlDataCommandBase(string cmdText, params SQLiteParameter[] parameters) : this(cmdText, false, parameters) { }
        public SqlDataCommandBase(string cmdText, bool dispose, params SQLiteParameter[] parameters)
            : base(dispose)
        {
            this.cmdText = cmdText;
            this.parameters = parameters;
        }

        protected override void InternalExecute(DataFileBase datafile)
        {
            SQLiteCommand cmd = datafile.GetCommand(cmdText, parameters);
            this.InternalExecute(cmd);
        }

        protected abstract void InternalExecute(SQLiteCommand cmd);
    }
}

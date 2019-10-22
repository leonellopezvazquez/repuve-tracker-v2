using System.Data.SQLite;

namespace PIPS.PAGIS.Db.SQLite
{
    public class InsertDataCommand : SqlDataCommandBase
    {
        private long rowid;

        public InsertDataCommand(string cmdText, params SQLiteParameter[] parameters) : this(cmdText, false, parameters) { }
        public InsertDataCommand(string cmdText, bool dispose, params SQLiteParameter[] parameters)
            : base(cmdText, dispose, parameters)
        {//SQLiteCommand cmd) : base(cmd) {
            rowid = -1;
        }

        protected override void InternalExecute(SQLiteCommand cmd)
        {
            cmd.ExecuteNonQuery();

            try
            {
                //-----------------------------------------------------------
                // The lines below are used to replace the statement:
                //  rowid = cmd.Connection.LastInsertRowID();
                //-----------------------------------------------------------
                const string sql = "SELECT last_insert_rowid()";
                var getIdCommand = new SQLiteCommand(sql, cmd.Connection);
                var obj = getIdCommand.ExecuteScalar();
                rowid = (long)obj;
                //-----------------------------------------------------------
            }
            catch { }
        }

        public override object Result
        {
            get
            {
                return rowid;
            }
        }

    }
}
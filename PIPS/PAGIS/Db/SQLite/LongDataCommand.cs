using System.Data.SQLite;

namespace PIPS.PAGIS.Db.SQLite
{
    public class LongDataCommand : SqlDataCommandBase
    {
        private long result;
        public LongDataCommand(string cmdText, params SQLiteParameter[] parameters) : base(cmdText, parameters) { }//SQLiteCommand cmd) : base(cmd) {}

        protected override void InternalExecute(SQLiteCommand cmd)
        {
            result = (long)cmd.ExecuteScalar();
        }

        public override object Result
        {
            get
            {
                return result;
            }
        }
    }
}
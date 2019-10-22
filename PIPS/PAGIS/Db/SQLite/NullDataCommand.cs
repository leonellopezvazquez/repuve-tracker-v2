using System.Data.SQLite;

namespace PIPS.PAGIS.Db.SQLite
{
    public class NullDataCommand : SqlDataCommandBase
    {
        public NullDataCommand(string cmdText, params SQLiteParameter[] parameters) : base(cmdText, parameters) { }//SQLiteCommand cmd) : base(cmd) {}

        protected override void InternalExecute(SQLiteCommand cmd)
        {
            cmd.ExecuteNonQuery();
        }

        public override object Result
        {
            get
            {
                return null;
            }
        }
    }
}
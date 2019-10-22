using System.Collections;
using System.Data;
using System.Data.SQLite;

namespace PIPS.PAGIS.Db.SQLite
{
    public class LongsDataCommand : SqlDataCommandBase
    {
        private long[] result;
        public LongsDataCommand(string cmdText, params SQLiteParameter[] parameters) : base(cmdText, parameters) { }//SQLiteCommand cmd) : base(cmd) {}

        protected override void InternalExecute(SQLiteCommand cmd)
        {
            using (IDataReader rdr = cmd.ExecuteReader())
            {
                ArrayList ids = new ArrayList();
                while (rdr.Read())
                {
                    ids.Add(rdr.GetInt64(0));
                }
                result = (long[])ids.ToArray(typeof(long));
            }
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
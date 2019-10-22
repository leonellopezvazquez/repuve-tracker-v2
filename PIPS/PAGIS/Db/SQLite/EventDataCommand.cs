using System;
using System.Data;
using System.Data.SQLite;

namespace PIPS.PAGIS.Db.SQLite
{
    public class EventDataCommand : SqlDataCommandBase
    {
        private DataEvent result;
        public EventDataCommand(DataEvent ev, string cmdText, params SQLiteParameter[] parameters)
            : base(cmdText, parameters)
        {
            this.result = ev;
        }

        protected override void InternalExecute(SQLiteCommand cmd)
        {
            using (IDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    for (int i = 0; i < result.Table.Columns.Count; i++)
                    {
                        try
                        {
                            result[i] = result.Table.Columns[i].GetValue(rdr, i + 1);
                        }
                        catch (InvalidCastException)
                        {
                            //Swallow exceptions for InvalidCastException
                            //Console.WriteLine("****InvalidCastException for {0}.{1}", result.Table.Name, result.Table.Columns[i].Name);
                        }
                    }
                }
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace PIPS.PAGIS.Db.SQLite
{
    public class InitializeCommand : DataCommandBase
    {
        private string dir;
        private SQLiteConnection conn;

        public InitializeCommand(string dir)
        {
            this.dir = dir;
            this.conn = null;
        }

        private SQLiteConnection Connect(string filename, int version)
        {
            bool isNew = !File.Exists(filename);
            SQLiteConnection c = new SQLiteConnection(string.Format("Data Source={0};New={1};Compress=False;Synchronous=Normal;Version={2};AutoVacuum=False;UTF16Encoding=False;UTF8Encoding=False", filename, isNew, version));
            try
            {
                c.Open();
                if (c.State == ConnectionState.Open)
                {
                    ArrayList tables = new ArrayList();
                    using (SQLiteCommand cmd = c.CreateCommand())
                    {
                        cmd.CommandText = "select name from sqlite_master where type = 'table'";
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                string name = rdr.GetString(0);
                                tables.Add(name);
                                //PIPS.Logger.WriteLine("InitializeCommand.SqliteMaster({0}, {1})", filename, name);
                            }
                        }
                    }
                    foreach (string table in tables)
                    {
                        using (SQLiteCommand cmd2 = c.CreateCommand())
                        {
                            cmd2.CommandText = "select rowid from " + table;
                            object o = cmd2.ExecuteScalar();
                            //PIPS.Logger.WriteLine("InitializeCommand.Counting({0}, {1})", table, o);
                        }
                    }
                    //PIPS.Logger.WriteLine(false, "InitializeCommand.Connected({0})", filename);
                    return c;
                }
            }
            catch (Exception ex)
            {
                //PIPS.Logger.Exception(ex);
            }
            try
            {
                c.Close();
            }
            catch { }
            return null;
        }

        private SQLiteConnection ConnectForce(DataFileBase file, string direc)
        {
            string filename = direc + @"\" + file.FileName + ".db";
            SQLiteConnection c = this.Connect(filename, file.SQLiteVersion);
            if (c == null)
            {
                try
                {
                    if (File.Exists(filename))
                        File.Delete(filename);
                }
                catch (Exception ex)
                {
                    //PIPS.Logger.Exception(ex);
                }
                c = this.Connect(filename, file.SQLiteVersion);
            }
            return c;
        }

        protected override void InternalExecute(DataFileBase datafile)
        {
            DateTime start = DateTime.Now;
            //PIPS.Logger.WriteLine(false, "DataFileBase.Initialize({0})", datafile.FileName);
            this.conn = this.ConnectForce(datafile, dir);
            //PIPS.Logger.WriteLine(false, "DataFileBase.Initialize.Complete({0}, {1})", datafile.FileName, (DateTime.Now - start).TotalMilliseconds);
        }

        public override object Result
        {
            get
            {
                return this.conn;
            }
        }

    }
}

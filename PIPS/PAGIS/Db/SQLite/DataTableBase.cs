using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace PIPS.PAGIS.Db.SQLite
{
    /// <summary>
    /// Summary description for DataTableBase.
    /// </summary>
    public abstract class DataTableBase : IDisposable
    {
        //private long lastId;
        private readonly DataColumnCollection _columns;
        private readonly DataTableCollection _linkedTables;
        private DataFileBase _file;
        private string _insertCommand;
        private bool _isOpen;

        protected DataTableBase()
        {
            _isOpen = false;
            _columns = new DataColumnCollection();
            _linkedTables = new DataTableCollection();
        }

        //private SQLiteParameter[] insertParameters;

        public DataColumnCollection Columns
        {
            get { return _columns; }
        }

        public virtual int IndexColumn
        {
            get { return -1; }
        }

        public virtual int SortColumn
        {
            get { return -1; }
        }

        public abstract string Name { get; }

        public DataTableCollection LinkedTables
        {
            get { return _linkedTables; }
        }

        public DataFileBase DataFile
        {
            get { return _file; }
        }

        protected virtual bool IsCountInitialized
        {
            get { return true; }
        }

        private bool TableExists
        {
            get
            {
                string cmdText =
                    string.Format("SELECT COUNT(*) FROM SQLITE_MASTER WHERE TYPE = 'table' AND NAME = '{0}'", Name);
                using (DataCommandBase dcb = new LongDataCommand(cmdText))
                {
                    _file.AddCommand(dcb);
                    dcb.Wait();
                    var res = (long)dcb.Result;
                    return (res == 1);
                }
            }
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
        }

        #endregion

        public event DataEventHandler Inserted, Updated, Deleted, Inserting, Updating, Deleting;

        public virtual void Initialize(DataFileBase dataFileBase)
        {
            createTable(dataFileBase);
        }

        /// <summary>
        /// sqlite commands which build the sql table
        /// </summary>
        /// <param name="dataFileBase"></param>
        protected void createTable(DataFileBase dataFileBase)
        {
            if (_isOpen)
                return;
            _file = dataFileBase;
            var bld = new StringBuilder();
            if (!TableExists)
            {
                bld.Append("create table ").Append(Name).Append("(id integer primary key");
                foreach (DataColumn col in _columns)
                    bld.Append(", ").Append(col.Name).Append(" ").Append(col.Type);
                bld.Append(")");
                //cmd.CommandText = bld.ToString();
                using (DataCommandBase dcb = new NullDataCommand(bld.ToString()))
                {
                    _file.AddCommand(dcb);
                    dcb.Wait();
                }

                //Logger.WriteLine("DataTableBase.CreatedTable({0})", Name);
                if (IndexColumn >= 0)
                {
                    string cmdText = string.Format("create index i_{0} on {0}({1})", Name, _columns[IndexColumn].Name);
                    using (DataCommandBase dcb = new NullDataCommand(cmdText))
                    {
                        _file.AddCommand(dcb);
                        dcb.Wait();
                    }
                    //Logger.WriteLine("DataTableBase.CreatedIndex({0}, {1})", Name, _columns[IndexColumn].Name);
                }

                //add the maneual entry default here to hotlists.db because the zero folder does not get added.
                if (Name == "t_hotlists")
                {
                    var cmdText =
                        "insert into t_hotlists (name, priority, timestamp, color, covert, alarm, active, file, whitelist, bossid, sound) " +
                        "values ('Manual Reports',500, " + DateTime.Now.Ticks + ", -16776961,'False','MED','True','','False','','')";
                    using (DataCommandBase dcb = new NullDataCommand(cmdText))
                    {
                        _file.AddCommand(dcb);
                        dcb.Wait();
                    }
                }
            }
            //adds unique constraint on hotlist tables
            if (Name == "t_hotlist")
            {
                /////////**************///////////
                /// this is here to check the file dir is equal to 1 so we do not add the Unique Index to the Manual Entry data file.
                var f = _file as HotLists.HotListDataFile;
                if (f != null)
                {
                    var path = f.Dir;
                    var folderNum = path.Substring(path.LastIndexOf('\\'));

                    var ini = new IniFile(Environment.CurrentDirectory + @"\Synchronizer.ini");
                    string type = ini.ReadString("Synchronizer", "Type", "BOF2").ToUpper();
                    if (folderNum != "\\0")
                    {
                        if (folderNum != "\\1" && type != "BOF2")
                            //excluding uniqueness because InStation does not have a BOSS ID
                        {
                            try
                            {
                                string cmdText = string.Format("create unique index iu_{0} on {0} (", Name);
                                cmdText += string.Format("{0} asc", Columns[8].Name);
                                cmdText += string.Format(")");
                                using (DataCommandBase dcb = new NullDataCommand(cmdText))
                                {
                                    _file.AddCommand(dcb);
                                    dcb.Wait();
                                }
                                //Logger.WriteLine("DataTableBase.CreatedUniqueIndex({0})", Name);
                            }
                            catch (Exception ex)
                            {
                                //Logger.WriteLine("Data Table {0} already has Unique Index", Name);
                            }

                        }
                        ////////*****************///////////
                    }
                }
            }

            //this.lastId = this.GetLastId();
            foreach (DataTableBase table in _linkedTables)
                table.Initialize(DataFile);

            _isOpen = true;
            count = IsCountInitialized ? GetCount(null) : 0;
            bld.Length = 0;
            bld.Append("insert into ");
            bld.Append(Name).Append(" (");

            if (Columns.Count > 0)
                bld.Append(Columns[0].Name);

            for (int i = 1; i < Columns.Count; i++)
                bld.Append(", ").Append(Columns[i].Name);

            bld.Append(") values (");
            if (Columns.Count > 0)
                bld.Append("?");

            for (int i = 1; i < Columns.Count; i++)
                bld.Append(", ?");

            bld.Append(")");
            _insertCommand = bld.ToString();

            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

        public void Reindex()
        {
            //var bld = new StringBuilder();
            if (TableExists && IndexColumn >= 0)
            {
                string cmdText = string.Format("reindex {0}", Name);
                using (DataCommandBase dcb = new NullDataCommand(cmdText))
                {
                    _file.AddCommand(dcb);
                    dcb.Wait();
                }
                //Logger.WriteLine("DataTableBase.Reindex({0}, {1})", Name, _columns[IndexColumn].Name);
            }
            OnReindexed();
        }

        protected virtual void OnReindexed()
        {
        }

        public abstract DataEvent CreateDataEvent();



        #region Select IDs

        public bool DoesBossIDExist(long id)
        {
            bool ret;
            long count = 0;
            string cmdText = string.Format("select COUNT(*) from {0} where bossid = {1}", Name, id);
            using (DataCommandBase dtc = new LongDataCommand(cmdText))
            {
                _file.AddCommand(dtc);
                dtc.Wait();
                count = (long)dtc.Result;
            }
            return count > 0;
        }

        public DataEvent SelectByBossID(long id)
        {

            string cmdText = string.Format("select * from {0} where bossid = {1}", Name, id);
            DataEvent ev = CreateDataEvent();
            using (DataCommandBase dtc = new LongDataCommand(string.Format("select id from {0} where bossid = {1}", Name, id)))
            {
                _file.AddCommand(dtc);
                dtc.Wait();
                ev.ID = (long)dtc.Result;
            }
            using (DataCommandBase dtc = new EventDataCommand(ev, cmdText))
            {
                _file.AddCommand(dtc);
                dtc.Wait();
            }
            foreach (DataTableBase linkedTable in LinkedTables)
            {
                long[] linkids = linkedTable.SelectIDsByIndexColumn(id);
                if (linkids != null)
                {
                    foreach (long linkid in linkids)
                    {
                        ev.LinkedDataEvents(linkedTable).Add(linkedTable.SelectByID(linkid));
                    }
                }
            }
            return ev;
        }

        public virtual DataEvent SelectByID(long id)
        {
            //using(SQLiteCommand cmd = this.file.CreateCommand()) {
            string cmdText = string.Format("select * from {0} where id = {1}", Name, id);
            //cmd.CommandText = sel;

            DataEvent ev = CreateDataEvent();
            ev.ID = id;
            using (DataCommandBase dtc = new EventDataCommand(ev, cmdText))
            {
                _file.AddCommand(dtc);
                dtc.Wait();
            }
            foreach (DataTableBase linkedTable in LinkedTables)
            {
                long[] linkids = linkedTable.SelectIDsByIndexColumn(id);
                if (linkids != null)
                {
                    foreach (long linkid in linkids)
                    {
                        ev.LinkedDataEvents(linkedTable).Add(linkedTable.SelectByID(linkid));
                    }
                }
            }
            return ev;
        }

        public long[] SelectIDsByLinkedTable(DataTableBase linkedTable)
        {
            string cmd =
                CreateSelectIDsCommand(
                    string.Format("id in (select {0} from {1})", linkedTable.Columns[linkedTable.IndexColumn].Name,
                                  linkedTable.Name), "id");
            return SelectIDs(cmd);

        }

        public long[] SelectIDsByLinkedTable(int index)
        {
            return SelectIDsByLinkedTable(LinkedTables[index]);
        }

        public long[] SelectIDsByLinkedTableWhere(int index, string where)
        {
            return SelectIDsByLinkedTableWhere(LinkedTables[index], where);
        }

        public long[] SelectIDsByLinkedTableWhere(DataTableBase linkedTable, string where)
        {
            string cmd =
                CreateSelectIDsCommand(
                    string.Format("id in (select {0} from {1}) and {2}",
                                  linkedTable.Columns[linkedTable.IndexColumn].Name, linkedTable.Name, where), "id");

            return SelectIDs(cmd);
        }

        public virtual long[] SelectIDs()
        {
            return SelectIDsWhere(null);
        }

        public long[] SelectIDsWhere(string where)
        {
            return SelectIDsWhere(where, null);
        }

        public long[] SelectIDsWhere(string where, string orderBy, bool asc)
        {
            string cmd = CreateSelectIDsCommand(where, orderBy, asc);
            return SelectIDs(cmd);
        }

        public long[] SelectIDsWhere(string where, string orderBy)
        {
            string cmd = CreateSelectIDsCommand(where, orderBy);
            return SelectIDs(cmd);
        }

        private string CreateSelectIDsCommand(string where, string orderBy, bool asc = true)
        {
            string order = string.Empty;
            if (!string.IsNullOrEmpty(orderBy))
                order = string.Format(" order by {0} {1}", orderBy, asc ? "asc" : "desc");
            if ((order == string.Empty) && (SortColumn >= 0))
                order = string.Format(" order by {0} {1}", _columns[SortColumn].Name, asc ? "asc" : "desc");
            //SQLiteCommand cmd = this.file.CreateCommand();
            string wh = string.Empty;
            if (!string.IsNullOrEmpty(where))
                wh = string.Format(" where {0}", where);
            return string.Format("select id from {0}{1}{2}", Name, wh, order);
        }

        protected long[] SelectIDs(string cmd, params SQLiteParameter[] parameters)
        {
            using (DataCommandBase dtc = new LongsDataCommand(cmd, parameters))
            {
                _file.AddCommand(dtc);
                dtc.Wait();
                return (long[])dtc.Result;
            }
        }

        protected long[] SelectIDsCustom(string cmd)
        {
            using (DataCommandBase dtc = new LongsDataCommand(cmd))
            {
                _file.AddCommand(dtc);
                dtc.Wait();
                return (long[])dtc.Result;
            }
        }

        public long[] SelectIDsByIndexColumn(object val)
        {
            return SelectIDsByIndexColumn(val, null);
        }

        public long[] SelectIDsByIndexColumn(object val, string orderBy)
        {
            if (IndexColumn < 0)
                return null;

            string cond = "=";
            if ((_columns[IndexColumn].Type.ToUpper() == "TEXT") && (val is string))
            {
                var search = (string)val;
                search = search.Replace("*", "%").Replace("?", "_");
                if (search.IndexOf("(") >= 0)
                {
                    var ids = SelectIDsWhere(string.Format("{0} in {1}", _columns[IndexColumn].Name, search), orderBy);
                    if (ids != null && ids.Length > 0)
                        return ids;
                }
                if (search.IndexOfAny(new[] { '_', '%', '[' }) >= 0)
                    cond = "like";
                val = search;
            }

            string where = string.Format("{0} {1} ?", _columns[IndexColumn].Name, cond);
            //string where = string.Format("{0} {1} "+val+"", _columns[IndexColumn].Name, cond);
            string cmdText = CreateSelectIDsCommand(where, orderBy);
            //PIPS.Logger.WriteLine("DataTableBase.Select({0})", cmdText);
            var param = new SQLiteParameter();

            _columns[IndexColumn].SetValue(param, val);
            return SelectIDs(cmdText, param);
        }

        #endregion

        #region Insert / Update / Delete

        public void Save(DataEvent ev)
        {
            if (ev != null)
            {
                if (ev.ID < 0)
                    Insert(ev);
                else
                    Update(ev);
            }
        }

        protected virtual void OnUpdating(DataEvent ev)
        {
            if (Updating != null)
                Updating(ev);
        }

        private void Update(DataEvent ev)
        {
            OnUpdating(ev);
            //using(SQLiteCommand cmd = this.file.CreateCommand()) {
            var bld = new StringBuilder("update ");
            bld.Append(Name).Append(" set ");
            if (Columns.Count > 0)
                bld.Append(Columns[0].Name).Append(" = ?");
            for (int i = 1; i < Columns.Count; i++)
                bld.Append(", ").Append(Columns[i].Name).Append(" = ?");
            bld.Append(" where id = ?");
            var parameters = new SQLiteParameter[Columns.Count + 1];
            for (int i = 0; i < Columns.Count; i++)
            {
                parameters[i] = new SQLiteParameter();
                Columns[i].SetValue(parameters[i], ev[i]);
            }
            parameters[Columns.Count] = new SQLiteParameter { DbType = DbType.Int64, Value = ev.ID };
            using (DataCommandBase dtc = new NullDataCommand(bld.ToString(), parameters))
            {
                _file.AddCommand(dtc);
                dtc.Wait();
            }
            //}
            foreach (DataTableBase linkedTable in LinkedTables)
            {
                foreach (DataEvent linkedEvent in ev.LinkedDataEvents(linkedTable))
                {
                    linkedEvent[linkedTable.IndexColumn] = ev.ID;
                    linkedTable.Save(linkedEvent);
                }
            }
            OnUpdated(ev);
        }

        protected virtual void OnUpdated(DataEvent ev)
        {
            if (Updated != null)
                Updated(ev);
        }

        protected virtual void OnInserting(DataEvent ev)
        {
            if (Inserting != null)
                Inserting(ev);
        }

        /*public void BulkInsert(DataEvent ev) {
			SQLiteParameter[] parameters = new SQLiteParameter[this.Columns.Count];
			for(int i = 0; i < this.Columns.Count; i++) {
				parameters[i] = new SQLiteParameter();
				this.Columns[i].SetValue(parameters[i], ev[i]);
			}
			this.file.AddCommand(new InsertDataCommand(this.insertCommand, true, parameters));
			count++;
		}*/

        private void Insert(DataEvent ev)
        {
            //DateTime start = DateTime.Now;
            OnInserting(ev);
            if (ev.AbortTransaction) return;
            //PIPS.Logger.WriteLine("DataTableBase.Insert({0}, {1})", this.columns.Count, (DateTime.Now - start).TotalMilliseconds);
            var parameters = new SQLiteParameter[Columns.Count];
            for (int i = 0; i < Columns.Count; i++)
            {
                parameters[i] = new SQLiteParameter();
                try
                {
                    Columns[i].SetValue(parameters[i], ev[i]);
                }
                catch (Exception x)
                {
                    //Logger.Exception(x);
                }
            }
            using (DataCommandBase dtc = new InsertDataCommand(_insertCommand, parameters))
            {
                _file.AddCommand(dtc);
                dtc.Wait();
                //PIPS.Logger.WriteLine("DataTableBase.InsertItem({0})", (DateTime.Now - start).TotalMilliseconds);
                ev.ID = (long)dtc.Result;
            }
            //}
            foreach (DataTableBase linkedTable in LinkedTables)
            {
                foreach (DataEvent linkedEvent in ev.LinkedDataEvents(linkedTable))
                {
                    linkedEvent[linkedTable.IndexColumn] = ev.ID;
                    linkedTable.Save(linkedEvent);
                }
            }
            count++;
            //PIPS.Logger.WriteLine("DataTableBase.InsertLinked({0})", (DateTime.Now - start).TotalMilliseconds);
            OnInserted(ev);
            //PIPS.Logger.WriteLine("DataTableBase.InsertComplete({0})", (DateTime.Now - start).TotalMilliseconds);
        }

        protected virtual void OnInserted(DataEvent ev)
        {
            if (Inserted != null)
            {
                //foreach(Delegate del in this.Inserted.GetInvocationList()) {
                //	PIPS.Logger.WriteLine("OnInsert.Delegate({0}, {1})", del.Method, del.Target);
                //}
                Inserted(ev);
            }
        }

        protected virtual void OnDeleting(DataEvent ev)
        {
            if (Deleting != null)
                Deleting(ev);
        }

        // Just bypass all this stuff so we can actually process things in a reasonable time.
        public void BulkDelete(string[] bossIDs)
        {
            try
            {
                // String.Join will throw an out of memory exception if our array is too large, so chunk it out into sub arrays.
                List<string[]> chunkedIDs = new List<string[]>();
                for (int i = 0; i < bossIDs.Length; i += 100000)
                {
                    int end = i + 99999;
                    if (end > bossIDs.Length) end = bossIDs.Length;
                    var newArray = bossIDs.Skip(i).Take(end).ToArray();
                    chunkedIDs.Add(newArray);
                }

                foreach (string[] sub in chunkedIDs)
                {
                    string cmdText = String.Format("delete from {0} where bossID in ({1})", Name, String.Join(",", sub));
                    using (DataCommandBase dtc = new NullDataCommand(cmdText))
                    {
                        _file.AddCommand(dtc);
                        dtc.Wait();
                    }
                }
            }
            catch (Exception e)
            {

            }


        }

        public void Delete(DataEvent ev)
        {
            if ((ev != null) && (ev.ID >= 0))
            {
                OnDeleting(ev);
                foreach (DataTableBase linkedTable in LinkedTables)
                {
                    foreach (DataEvent linkedEvent in ev.LinkedDataEvents(linkedTable))
                    {
                        linkedTable.Delete(linkedEvent);
                    }
                }
                //using(SQLiteCommand cmd = this.file.CreateCommand()) {
                string cmdText = string.Format("delete from {0} where id = {1}", Name, ev.ID);

                //SQLiteParameter parameter = new SQLiteParameter();
                //parameter.DbType = DbType.Int64;
                //parameter.Value = ev.ID;);
                using (DataCommandBase dtc = new NullDataCommand(cmdText))
                {
                    _file.AddCommand(dtc);
                    dtc.Wait();
                }
                //}
                ev.ID = -1;
                count--;
                OnDeleted(ev);
            }
        }

        protected virtual void OnDeleted(DataEvent ev)
        {
            if (Deleted != null)
                Deleted(ev);
        }

        #endregion

        #region Count

        private long count;

        public virtual long Count
        {
            get
            {
                return count;
            }
        }

        public long LinkedCount(DataTableBase linkedTable)
        {
            return
                GetCount(string.Format("id in (select {0} from {1})", linkedTable.Columns[linkedTable.IndexColumn].Name,
                                       linkedTable.Name));
        }

        public long LinkedCount(int index)
        {
            return LinkedCount(LinkedTables[index]);
        }

        protected long GetCount(string where)
        {
            try
            {
                string cmdText = "select count(*) from " + Name;
                if (!string.IsNullOrEmpty(where))
                    cmdText += " where " + where;

                using (DataCommandBase dtc = new LongDataCommand(cmdText))
                {
                    _file.AddCommand(dtc);
                    dtc.Wait();
                    return (long)dtc.Result;
                }
            }
            catch
            {
                return 0;
            }
        }

        protected long GetCountCustom(string query)
        {
            try
            {
                string cmdText = "select count(*) from " + Name;
                if (!string.IsNullOrEmpty(query))
                    cmdText += query;

                using (DataCommandBase dtc = new LongDataCommand(cmdText))
                {
                    _file.AddCommand(dtc);
                    dtc.Wait();
                    return (long)dtc.Result;
                }
            }
            catch
            {
                return 0;
            }
        }

        #endregion
    }
}
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
    public interface IReadsDataTable
    {
        HitsDataTable Hits { get; }
        int FilteredHitsCount { get; }
        string Name { get; }
        int IndexColumn { get; }
        long SyncedCount { get; }
        long MisreadsCount { get; }
        long ManaulsCount { get; }
        DataColumnCollection Columns { get; }
        int SortColumn { get; }
        DataTableCollection LinkedTables { get; }
        DataFileBase DataFile { get; }
        long Count { get; }
        ReadsDataEvent Save(string vrm);
        ReadsDataEvent Save(string vrm, bool permit);
        void Dispose();
        ReadsDataEvent CreateReadsDataEvent(SystemRepository systemRepository);
        DataEvent CreateDataEvent();
        long[] SelectManualIDs();
        long[] SelectNonSyncedIDs();
        long[] SelectSyncedIDs();
        long[] SelectMisreadIDs();

        /// <summary>
        /// Gets a list of read id's which are not misreads
        /// and have associated hits
        /// </summary>
        /// <returns></returns>
        long[] SelectIDsWithFilteredHits();

        event DataEventHandler Inserted;
        event DataEventHandler Updated;
        event DataEventHandler Deleted;
        event DataEventHandler Inserting;
        event DataEventHandler Updating;
        event DataEventHandler Deleting;
        void Initialize(DataFileBase dataFileBase);
        void Reindex();
        DataEvent SelectByID(long id);
        long[] SelectIDsByLinkedTable(DataTableBase linkedTable);
        long[] SelectIDsByLinkedTable(int index);
        long[] SelectIDsByLinkedTableWhere(int index, string where);
        long[] SelectIDsByLinkedTableWhere(DataTableBase linkedTable, string where);
        long[] SelectIDs();
        long[] SelectIDsWhere(string where);
        long[] SelectIDsWhere(string where, string orderBy, bool asc);
        long[] SelectIDsWhere(string where, string orderBy);
        long[] SelectIDsByIndexColumn(object val);
        long[] SelectIDsByIndexColumn(object val, string orderBy);
        void Save(XmlPackets.ReadHit readPacket, SystemRepository systemRepository);
        void Save(DataEvent ev);
        void Delete(DataEvent ev);
        long LinkedCount(DataTableBase linkedTable);
        long LinkedCount(int index);
    }
}
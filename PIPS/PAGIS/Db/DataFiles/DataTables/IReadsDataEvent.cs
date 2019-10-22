using System;
using System.Collections.Generic;
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
    public interface IReadsDataEvent
    {
        string VRM { get; set; }
        string Login { get; set; }
        DateTime Timestamp { get; set; }
        string Location { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        string Camera { get; set; }
        byte[] Patch { get; set; }
        byte[] Overview { get; set; }
        string PatchLocation { get; set; }
        string OverviewLocation { get; set; }
        int Confidence { get; set; }
        bool IsMisread { get; set; }
        bool IsManual { get; set; }
        bool IsSynced { get; set; }
        int BossID { get; set; }
        int CameraID { get; set; }
        string Syntax { get; set; }
        bool Resync { get; set; }
        bool Permit { get; set; }
        List<HitsDataEvent> Hits { get; }
        List<HitsDataEvent> HitsFiltered { get; }
        DataTableBase Table { get; }
        long ID { get; set; }
        int ColumnCount { get; }
        bool AbortTransaction { get; set; }
        System.Drawing.Image CreateOverview();
        System.Drawing.Image CreatePatch();
        void Delete();
        void Save();
        void SaveAndQueueForSync();
        DataEventCollection LinkedDataEvents(DataTableBase linkedTable);
        DataEventCollection LinkedDataEvents(int index);
        object this[int index] { get; set; }
    }
}
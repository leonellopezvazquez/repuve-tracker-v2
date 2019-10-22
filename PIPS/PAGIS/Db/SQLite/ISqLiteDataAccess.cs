using System;

namespace PIPS.PAGIS.Db.SQLite
{
    public interface ISqLiteDataAccess : IDisposable
    {
        void PurgeReads(int timeFrameInMinutes);
        void PurgeHits(int timeFrameInMinutes);
        void PurgeHotlists(int timeFrameInMinutes);
        void RemoveTargets();
    }
}
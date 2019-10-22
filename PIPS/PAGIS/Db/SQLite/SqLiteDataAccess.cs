using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using PIPS.PAGIS.Db.DataFiles.DataTables;
using PIPS.PAGIS.Db.HotLists;
using PIPS.PAGIS.Db.SQLite.DataModels;

namespace PIPS.PAGIS.Db.SQLite
{
    /// <summary>
    /// Pretty much useless untill we no longer have to rely on having to use SystemRepository
    /// </summary>
    public class SqLiteDataAccess : IDisposable//, ISqLiteDataAccess
    {
        private SystemRepository _repository;

        #region Constructors
        public SqLiteDataAccess(SystemRepository systemRepository)
        {
            this._repository = systemRepository;
        }

        public SqLiteDataAccess()
        {
        }
        #endregion Constructors

        #region Private Methods
        private string createConnectionString(string dbFile)
        {
            string dirString = System.Environment.CurrentDirectory + @"\data\" + dbFile;
            return string.Format("Data Source={0};New=False;Compress=False;Synchronous=Normal;Version=3;AutoVacuum = False;", dirString);
        }
        #endregion Private Methods

        #region Public Methods
        public void Dispose()
        {
            // throw new NotImplementedException();
        }
        #endregion Public Methods

        public string GetReadIdsAsString(string criteria, DateTime purgeDate)
        {
            var ret = new List<Read>();
            var stringReadList = "";
            using (var conn = new SQLiteConnection(createConnectionString("Events.db")))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * from t_reads " + criteria;
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var read1 = new Read()
                        {
                            BossId = reader.GetInt64(reader.GetOrdinal("bossid")),
                            Camera = reader.GetString(reader.GetOrdinal("camera")),
                            CameraId = reader.GetInt32(reader.GetOrdinal("cameraid")),
                            Confidence = reader.GetInt32(reader.GetOrdinal("confidence")),
                            Id = reader.GetInt64(reader.GetOrdinal("id")),
                            Latitude = reader.GetDouble(reader.GetOrdinal("latitude")),
                            Location = reader.GetString(reader.GetOrdinal("location")),
                            Login = reader.GetString(reader.GetOrdinal("login")),
                            Longitude = reader.GetDouble(reader.GetOrdinal("longitude")),
                            Misread = Boolean.Parse(reader.GetString(reader.GetOrdinal("misread"))),
                            Synced = Boolean.Parse(reader.GetString(reader.GetOrdinal("synced"))),
                            Timestamp = new DateTime((reader.GetInt64(reader.GetOrdinal("timestamp")))),
                            Vrm = reader.GetString(reader.GetOrdinal("vrm")),
                            OverviewLocation = reader.GetString(reader.GetOrdinal("overview")),
                            PatchLocation = reader.GetString(reader.GetOrdinal("patch"))
                        };
                        ret.Add(read1);
                    }
                    var list1 = ret.FindAll(o => o.Timestamp < purgeDate);
                    stringReadList = string.Join(",", list1.Select(o => o.Id.ToString()).ToArray());
                }
            }
            return stringReadList;
        }

        #region HotPlates

        //public void BulkInsertHotPlates(List<HotPlateRestAPIV1> plates, long hlID)
        //{
        //    try
        //    {
        //        using (var conn = new SQLiteConnection(createConnectionString(hlID.ToString() + @"\HotList_0.db")))
        //        {
        //            conn.Open();
                   
        //            using (var cmd = conn.CreateCommand())
        //            {
        //                using (var transaction = conn.BeginTransaction())
        //                {
        //                    foreach (var hp in plates)
        //                    {

        //                        cmd.CommandText = string.Format(
        //                            "insert into t_hotlist (VRM, field1, field2, field3, field4, field5, pncid, information, bossID) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', {8});",
        //                            hp.VRM, hp.Field1, hp.Field2, hp.Field3, hp.Field4, hp.Field5, hp.PNCID,
        //                            hp.Information,
        //                            hp.BossID);

        //                        cmd.ExecuteNonQuery();
        //                    }
        //                    transaction.Commit();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.WriteLine(e.ToString());
        //    }
        //}

        public void AddHotPlate(HotPlate hp)
        {
            try
            {
                using (
                    var conn =
                        new SQLiteConnection("Data Source=" + hp.FilePath +
                                             ";New=False;Compress=False;Synchronous=Normal;Version=3;AutoVacuum = False;")
                    )
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText =
                            String.Format(
                                "insert into t_hotlist (VRM, field1, field2, field3, field4, field5, pncid, information, bossID) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', {8})",
                                hp.VRM, hp.Field1, hp.Field2, hp.Field3, hp.Field4, hp.Field5, hp.PNCID, hp.Information,
                                hp.BossID);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException e)
            {
                UpdateHotPlate(hp);
            }
        }

        public void UpdateHotPlate(HotPlate hp)
        {
            try
            {
                using (
                    var conn =
                        new SQLiteConnection("Data Source=" + hp.FilePath +
                                             ";New=False;Compress=False;Synchronous=Normal;Version=3;AutoVacuum = False;")
                    )
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText =
                            String.Format(
                                "update t_hotlist set VRM = '{0}', field1 = '{1}', field2 = '{2}', field3 = '{3}', field4 = '{4}', field5 = '{5}', pncid = '{6}', information = '{7}' where bossID = {8}",
                                hp.VRM, hp.Field1, hp.Field2, hp.Field3, hp.Field4, hp.Field5, hp.PNCID, hp.Information,
                                hp.BossID);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException e)
            {
                //Logger.WriteLine("Oops");
                throw;
            }
        }
        #endregion HotPlates

        #region HotLists



        public List<Hotlists> GetHotlists()
        {
            var hotListList = new List<Hotlists>();

            using (var conn = new SQLiteConnection(createConnectionString("HotLists.db")))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * from t_hotlists ";
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            long BossID;
                            Int64.TryParse(reader.GetValue(reader.GetOrdinal("bossid")).ToString(), out BossID);

                            var hotList = new Hotlists()
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Priority = reader.GetInt32(reader.GetOrdinal("priority")),
                                Timestamp = new DateTime((reader.GetInt64(reader.GetOrdinal("timestamp")))),
                                Color = reader.GetInt64(reader.GetOrdinal("color")),
                                Covert = Boolean.Parse(reader.GetString(reader.GetOrdinal("covert"))),
                                Alarm = reader.GetString(reader.GetOrdinal("alarm")),
                                Active = Boolean.Parse(reader.GetString(reader.GetOrdinal("active"))),
                                File = reader.GetString(reader.GetOrdinal("file")),
                                WhiteList = Boolean.Parse(reader.GetString(reader.GetOrdinal("whitelist"))),
                                BossId = BossID
                            };
                            hotListList.Add(hotList);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }

            return hotListList;
        }

        #endregion HotLists

    }
}

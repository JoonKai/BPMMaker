using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HellsysLibrary.Helpers;

namespace BPMMaker.DataBase
{
    public class DBManager
    {
        #region 데이터베이스멤버
        private static string SECTION1 = "DATABASE";
        private static string DbName = "MyDB,db";
        private static string MyDBPath = System.IO.Path.Combine(Helper.EPIIni.Read(SECTION1, "DBPath"), DbName);
        private static string conString = "Data Source=" + MyDBPath;

        private static SQLiteConnection connection = new SQLiteConnection(conString);
        private static SQLiteCommand command = new SQLiteCommand("", connection);
        private static string sql;

        #endregion
        public DBManager()
        {

        }
        public void Create_DB()
        {
            if (!System.IO.File.Exists(MyDBPath))
            {
                SQLiteConnection.CreateFile(MyDBPath);
                using (var sqlite = new SQLiteConnection(conString))
                {
                    sqlite.Open();
                    string sql = "create table Vendor(" +
                        "vendorcode INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL," +
                        "vendorname TEXT NOT NULL," +
                        "vendorperson TEXT," +
                        "vendorpersoncellphone TEXT," +
                        "vendorproduct TEXT," +
                        "vendornote TEXT)";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                Console.WriteLine("Database cannot crate");
                return;
            }
        }
        
    }
}

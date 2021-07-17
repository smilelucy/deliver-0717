using PULI.Models.DataInfo;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Services.SQLite
{
    public class DoggsyDatabase
    {
        static object locker = new object();

        public string DBPath { get; set; }

        SQLiteConnection database;

        public DoggsyDatabase()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = database.DatabasePath;
            // create the tables
            database.CreateTable<SQLiteInfo>();
        }

        public IEnumerable<SQLiteInfo> GetItems()
        {
            lock (locker)
            {
                return (from i in database.Table<SQLiteInfo>() select i).ToList();
            }
        }

        public IEnumerable<SQLiteInfo> GetItemsNotDone()
        {
            lock (locker)
            {
                return database.Query<SQLiteInfo>("SELECT * FROM [SQLiteInfo] WHERE [Done] = 0");
            }
        }

        public SQLiteInfo GetItem(int id)
        {
            lock (locker)
            {
                return database.Table<SQLiteInfo>().FirstOrDefault(x => x.ID == id);
            }
        }

        public int SaveItem(SQLiteInfo item)
        {
            lock (locker)
            {
                if (item.ID != 0)
                {
                    database.Update(item);
                    return item.ID;
                }
                else
                {
                    return database.Insert(item);
                }
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return database.Delete<SQLiteInfo>(id);
            }
        }

        public void DeleteAll()
        {
            var fooItems = GetItems().ToList();
            foreach (var item in fooItems)
            {
                DeleteItem(item.ID);
            }
        }
    }
}
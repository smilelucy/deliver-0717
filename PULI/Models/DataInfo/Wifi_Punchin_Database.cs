using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class Wifi_Punchin_Database
    {
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database_wifi_punchin;

        public Wifi_Punchin_Database()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database_wifi_punchin = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database_wifi_punchin.DatabasePath;
            _database_wifi_punchin.CreateTable<Wifi_Punchin>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        public IEnumerable<Wifi_Punchin> GetAccountAsync()
        {
            lock (locker)
            {
                return (from i in _database_wifi_punchin.Table<Wifi_Punchin>() select i).ToList();
            }
        }

        public List<Wifi_Punchin> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database_wifi_punchin.Table<Wifi_Punchin>() select i).ToList();
            }
        }

        //public IEnumerable<Wifi_Punchin> GetItemsTwo()
        //{
        //    lock (locker)
        //    {
        //        return _database_wifi_punchin.Query<Wifi_Punchin>("SELECT * FROM [TempAccount] WHERE [ID] = 2");
        //    }
        //}

        public Wifi_Punchin GetItem(int id)
        {
            lock (locker)
            {
                return _database_wifi_punchin.Table<Wifi_Punchin>().FirstOrDefault(x => x.ID == id);
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(Wifi_Punchin tmp)
        {
            lock (locker)
            {
                return _database_wifi_punchin.Insert(tmp);
                //if (tmp.ID != 0)
                //{
                //    _database2.Update(tmp);
                //    return tmp.ID;
                //}
                //else
                //{
                //    return _database2.Insert(tmp);
                //}
            }
        }


        //public int UpdateAccountAsync(Punch tmp)
        //{
        //    lock (locker)
        //    {
        //        //_database2.Update(tmp);
        //        //return tmp.ID;
        //        return _database2.Execute("UPDATE [TempAccount] SET [wqb99] = wqb99  WHERE [ID] = id");
        //        //return _database2.Query<TempAccount>("UPDATE * FROM [TempAccount] WHERE [ID] = 2");
        //        //_database2.Update(tmp);
        //        //return tmp.ID;
        //    }
        //}
        //public Task<int> DeleteAllAccountAsync(Account acc)
        //{
        //    return _database.DeleteAsync(acc);
        //    //_database.DropTableAsync<Account>().Wait(acc); // 清空
        //    //_database.CreateTableAsync<Account>().Wait(); // 創造
        //}



        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return _database_wifi_punchin.Delete<Wifi_Punchin>(id);
            }
        }

        


        public void DeleteAll()
        {
            var fooItems = GetAccountAsync().ToList();

            foreach (var item in fooItems)
            {
                DeleteItem(item.ID);
                Console.WriteLine("PPPPP " + item.name);
            }
        }


    }
}

using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class EntrytxtDatabase
    {
        //readonly SQLiteAsyncConnection _database;
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database_entry;

        public EntrytxtDatabase()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database_entry = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database_entry.DatabasePath;
            _database_entry.CreateTable<Entry_txt>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        //public Task<List<Account>> GetAccountAsync()
        //{
        //    return _database.Table<Account>().ToListAsync();
        //}

        public IEnumerable<Entry_txt> GetAccountAsync(int id)
        {
            lock (locker)
            {
                return (from i in _database_entry.Table<Entry_txt>() select i).ToList();
                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }

        public List<Entry_txt> GetAccountAsyncToList(int id)
        {
            lock (locker)
            {
                return (from i in _database_entry.Table<Entry_txt>() select i).ToList();
                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }

        public IEnumerable<Entry_txt> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database_entry.Table<Entry_txt>() select i).ToList();
            }
        }

        public IEnumerable<Entry_txt> GetItemsName(string name)
        {
            lock (locker)
            {
                return _database_entry.Query<Entry_txt>("SELECT * FROM [TempAccount] WHERE [ClientName] = " + name);
            }
        }

        public Entry_txt GetItem(int id)
        {
            lock (locker)
            {
                return _database_entry.Table<Entry_txt>().FirstOrDefault(x => x.ID == id);
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(Entry_txt tmp)
        {
            lock (locker)
            {
                return _database_entry.Insert(tmp);
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


        public int UpdateAccountAsync(Entry_txt tmp)
        {
            lock (locker)
            {
                //_database2.Update(tmp);
                //return tmp.ID;
                return _database_entry.Execute("UPDATE [TempAccount] SET [wqb99] = wqb99  WHERE [ID] = id");
                //return _database2.Query<TempAccount>("UPDATE * FROM [TempAccount] WHERE [ID] = 2");
                //_database2.Update(tmp);
                //return tmp.ID;
            }
        }
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
                return _database_entry.Delete<Entry_txt>(id);
            }
        }



        public void DeleteAll()
        {
            var fooItems = GetAccountAsync2().ToList();
            foreach (var item in fooItems)
            {
                DeleteItem(item.ID);
                //Console.WriteLine("KLKLKL " + item.account);
            }
        }
    }
}
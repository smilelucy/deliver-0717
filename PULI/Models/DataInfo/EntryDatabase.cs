using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class EntryDatabase
    {
        //readonly SQLiteAsyncConnection _database;
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database3;

        public EntryDatabase()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database3 = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database3.DatabasePath;
            _database3.CreateTable<Entry_DB>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        //public Task<List<Account>> GetAccountAsync()
        //{
        //    return _database.Table<Account>().ToListAsync();
        //}

        public IEnumerable<Entry_DB> GetAccountAsync(int id)
        {
            lock (locker)
            {
                return (from i in _database3.Table<Entry_DB>() select i).ToList();
                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }

        public List<Entry_DB> GetAccountAsyncToList(int id)
        {
            lock (locker)
            {
                return (from i in _database3.Table<Entry_DB>() select i).ToList();
                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }

        public IEnumerable<Entry_DB> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database3.Table<Entry_DB>() select i).ToList();
            }
        }

        public IEnumerable<Entry_DB> GetItemsName(string name)
        {
            lock (locker)
            {
                return _database3.Query<Entry_DB>("SELECT * FROM [TempAccount] WHERE [ClientName] = " + name);
            }
        }

        public Entry_DB GetItem(int id)
        {
            lock (locker)
            {
                return _database3.Table<Entry_DB>().FirstOrDefault(x => x.ID == id);
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(Entry_DB tmp)
        {
            lock (locker)
            {
                return _database3.Insert(tmp);
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


        public int UpdateAccountAsync(Entry_DB tmp)
        {
            lock (locker)
            {
                //_database2.Update(tmp);
                //return tmp.ID;
                return _database3.Execute("UPDATE [TempAccount] SET [wqb99] = wqb99  WHERE [ID] = id");
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
                return _database3.Delete<Entry_DB>(id);
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
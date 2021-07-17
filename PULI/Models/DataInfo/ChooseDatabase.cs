using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class ChooseDatabase
    {
        //readonly SQLiteAsyncConnection _database;
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database22;

        public ChooseDatabase()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database22 = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database22.DatabasePath;
            _database22.CreateTable<Choose>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        //public Task<List<Account>> GetAccountAsync()
        //{
        //    return _database.Table<Account>().ToListAsync();
        //}

        public IEnumerable<Choose> GetAccountAsync(int id)
        {
            lock (locker)
            {
                return (from i in _database22.Table<Choose>() select i).ToList();
                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }

        public List<Choose> GetAccountAsyncToList(int id)
        {
            lock (locker)
            {
                return (from i in _database22.Table<Choose>() select i).ToList();
                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }

        public IEnumerable<Choose> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database22.Table<Choose>() select i).ToList();
            }
        }

        public IEnumerable<Choose> GetItemsName(string name)
        {
            lock (locker)
            {
                return _database22.Query<Choose>("SELECT * FROM [TempAccount] WHERE [ClientName] = " + name);
            }
        }

        public Choose GetItem(int id)
        {
            lock (locker)
            {
                return _database22.Table<Choose>().FirstOrDefault(x => x.ID == id);
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(Choose tmp)
        {
            lock (locker)
            {
                return _database22.Insert(tmp);
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


        public int UpdateAccountAsync(Choose tmp)
        {
            lock (locker)
            {
                //_database2.Update(tmp);
                //return tmp.ID;
                return _database22.Execute("UPDATE [TempAccount] SET [wqb99] = wqb99  WHERE [ID] = id");
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
                return _database22.Delete<Choose>(id);
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
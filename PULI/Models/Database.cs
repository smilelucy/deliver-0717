using Deliver.Models.DataInfo;
using PULI.Models.DataInfo;
using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Deliver.Models
{
    public class Database
    {
        //readonly SQLiteAsyncConnection _database;
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database;

        public Database()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database.DatabasePath;
            _database.CreateTable<Account>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        //public Task<List<Account>> GetAccountAsync()
        //{
        //    return _database.Table<Account>().ToListAsync();
        //}

        public IEnumerable<Account> GetAccountAsync()
        {
            lock (locker)
            {
                return (from i in _database.Table<Account>() select i).ToList();
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(Account acc)
        {
            return _database.Insert(acc);
        }

        //public Task<int> DeleteAllAccountAsync(Account acc)
        //{
        //    return _database.DeleteAsync(acc);
        //    //_database.DropTableAsync<Account>().Wait(acc); // 清空
        //    //_database.CreateTableAsync<Account>().Wait(); // 創造
        //}
        public SQLiteInfo GetItem(int id)
        {
            lock (locker)
            {
                return _database.Table<SQLiteInfo>().FirstOrDefault(x => x.ID == id);
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return _database.Delete<Account>(id);
            }
        }

        public void DeleteItem2(int id)
        {
            var fooItems = GetAccountAsync().ToList();
            foreach (var item in fooItems)
            {
                if(item.ID == id)
                {
                    _database.Delete(item.ID);
                }
                //DeleteItem(item.ID);
                //DeleteItem(item.password);
                //Console.WriteLine("KLKLKL " + item.account);
            }
        }

        public void DeleteAll()
        {
            var fooItems = GetAccountAsync().ToList();
            foreach (var item in fooItems)
            {
                DeleteItem(item.ID);
                //DeleteItem(item.password);
                //Console.WriteLine("KLKLKL " + item.account);
            }
        }
    }
}
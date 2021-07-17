using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class TempAddDatabase_else
    {
        //readonly SQLiteAsyncConnection _database;
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database_add_else;

        public TempAddDatabase_else()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database_add_else = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database_add_else.DatabasePath;
            _database_add_else.CreateTable<TmpAdd>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        //public Task<List<Account>> GetAccountAsync()
        //{
        //    return _database.Table<Account>().ToListAsync();
        //}

        public IEnumerable<TmpAdd> GetAccountAsync(int id)
        {
            lock (locker)
            {
                return (from i in _database_add_else.Table<TmpAdd>() select i).ToList();
                //return (from i in _database_add_else.Table<TmpAdd>() orderby id descending select i).ToList();
            }
        }

        public List<TmpAdd> GetAccountAsyncToList(int id)
        {
            lock (locker)
            {
                return (from i in _database_add_else.Table<TmpAdd>() select i).ToList();
                //return (from i in _database_add_else.Table<TmpAdd>() orderby id descending select i).ToList();
            }
        }

        public IEnumerable<TmpAdd> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database_add_else.Table<TmpAdd>() select i).ToList();
            }
        }

        public IEnumerable<TmpAdd> GetItemsName(string name)
        {
            lock (locker)
            {
                return _database_add_else.Query<TmpAdd>("SELECT * FROM [TmpAdd] WHERE [ClientName] = " + name);
            }
        }

        public TmpAdd GetItem(int id)
        {
            lock (locker)
            {
                return _database_add_else.Table<TmpAdd>().FirstOrDefault(x => x.ID == id);
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(TmpAdd tmp)
        {
            lock (locker)
            {
                return _database_add_else.Insert(tmp);
                //if (tmp.ID != 0)
                //{
                //    _database_add_else.Update(tmp);
                //    return tmp.ID;
                //}
                //else
                //{
                //    return _database_add_else.Insert(tmp);
                //}
            }
        }


        public int UpdateAccountAsync(TmpAdd tmp)
        {
            lock (locker)
            {
                //_database_add_else.Update(tmp);
                //return tmp.ID;
                return _database_add_else.Execute("UPDATE [TmpAdd] SET [wqb99] = wqb99  WHERE [ID] = id");
                //return _database_add_else.Query<TmpAdd>("UPDATE * FROM [TmpAdd] WHERE [ID] = 2");
                //_database_add_else.Update(tmp);
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
                return _database_add_else.Delete<TmpAdd>(id);
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
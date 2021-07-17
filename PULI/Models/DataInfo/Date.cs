using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class Date
    {
        //readonly SQLiteAsyncConnection _databasedate;
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _databasedate;

        public Date()
        {
            //_databasedate = new SQLiteAsyncConnection(dbPath);
            //_databasedate.CreateTableAsync<CheckDate>().Wait();
            _databasedate = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _databasedate.DatabasePath;
            _databasedate.CreateTable<CheckDate>();
            // create the tables
            //_databasedate.CreateTableAsync<CheckDate>().Wait(); // 創造
        }

        //public Task<List<CheckDate>> GetAccountAsync()
        //{
        //    return _databasedate.Table<CheckDate>().ToListAsync();
        //}

        public IEnumerable<CheckDate> GetAccountAsync()
        {
            lock (locker)
            {
                return (from i in _databasedate.Table<CheckDate>() select i).ToList();
            }
        }

        public IEnumerable<CheckDate> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _databasedate.Table<CheckDate>() select i).ToList();
            }
        }
        //public Task<int> SaveAccountAsync(CheckDate acc)
        //{
        //    return _databasedate.InsertAsync(acc);
        //}

        public int SaveAccountAsync(CheckDate date)
        {
            return _databasedate.Insert(date);
        }

        //public Task<int> DeleteAllAccountAsync(CheckDate acc)
        //{
        //    return _databasedate.DeleteAsync(acc);
        //    //_databasedate.DropTableAsync<CheckDate>().Wait(acc); // 清空
        //    //_databasedate.CreateTableAsync<CheckDate>().Wait(); // 創造
        //}
        public SQLiteInfo GetItem(int id)
        {
            lock (locker)
            {
                return _databasedate.Table<SQLiteInfo>().FirstOrDefault(x => x.ID == id);
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return _databasedate.Delete<CheckDate>(id);
            }
        }

        public void DeleteItem2(int id)
        {
            var fooItems = GetAccountAsync().ToList();
            foreach (var item in fooItems)
            {
                if (item.ID == id)
                {
                    _databasedate.Delete(item.ID);
                }
                //DeleteItem(item.ID);
                //DeleteItem(item.password);
                //Console.WriteLine("KLKLKL " + item.CheckDate);
            }
        }

        public void DeleteAll()
        {
            var fooItems = GetAccountAsync().ToList();
            foreach (var item in fooItems)
            {
                DeleteItem(item.ID);
                //DeleteItem(item.password);
                //Console.WriteLine("KLKLKL " + item.CheckDate);
            }
        }
    }
}

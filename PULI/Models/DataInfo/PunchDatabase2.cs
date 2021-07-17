using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class PunchDatabase2
    {
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database4;

        public PunchDatabase2()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database4 = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database4.DatabasePath;
            _database4.CreateTable<Punch2>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        public IEnumerable<Punch2> GetAccountAsync(int id)
        {
            lock (locker)
            {
                return (from i in _database4.Table<Punch2>() select i).ToList();
            }
        }

        public IEnumerable<Punch2> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database4.Table<Punch2>() select i).ToList();
            }
        }

        public IEnumerable<Punch2> GetLastInsertSetnum()
        {
            //return (int)SQLite3.LastInsertRowid(_database4.Handle);
            lock (locker)
            {
                return _database4.Query<Punch2>("SELECT ID FROM [Punch2] ORDER BY DESC LIMIT 1");
            }
        }
        //public IEnumerable<Punch2> GetItemsTwo()
        //{
        //    lock (locker)
        //    {
        //        return _database4.Query<Punch2>("SELECT * FROM [TempAccount] WHERE [ID] = 2");
        //    }
        //}

        public Punch2 GetItem(int id)
        {
            lock (locker)
            {
                return _database4.Table<Punch2>().FirstOrDefault(x => x.ID == id);
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(Punch2 tmp)
        {
            lock (locker)
            {
                return _database4.Insert(tmp);
                
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


        //public int UpdateAccountAsync(Punch2 tmp)
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
                return _database4.Delete<Punch2>(id);
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

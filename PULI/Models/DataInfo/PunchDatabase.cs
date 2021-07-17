using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class PunchDatabase 
    {
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database3;

        public PunchDatabase()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database3 = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database3.DatabasePath;
            _database3.CreateTable<Punch>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        public IEnumerable<Punch> GetAccountAsync(int id)
        {
            lock (locker)
            {
                return (from i in _database3.Table<Punch>() select i).ToList();
            }
        }

        public IEnumerable<Punch> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database3.Table<Punch>() select i).ToList();
            }
        }

        //public IEnumerable<Punch> GetItemsTwo()
        //{
        //    lock (locker)
        //    {
        //        return _database3.Query<Punch>("SELECT * FROM [TempAccount] WHERE [ID] = 2");
        //    }
        //}

        public Punch GetItem(int id)
        {
            lock (locker)
            {
                return _database3.Table<Punch>().FirstOrDefault(x => x.ID == id);
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(Punch tmp)
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
                return _database3.Delete<Punch>(id);
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
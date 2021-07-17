using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class PunchDataBaseTmp
    {
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database4;

        public PunchDataBaseTmp()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database4 = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database4.DatabasePath;
            _database4.CreateTable<PunchTmp>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        public IEnumerable<PunchTmp> GetAccountAsync()
        {
            lock (locker)
            {
                return (from i in _database4.Table<PunchTmp>() select i).ToList();
            }
        }

        public List<PunchTmp> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database4.Table<PunchTmp>() select i).ToList();
            }
        }

        //public IEnumerable<PunchTmp> GetItemsTwo()
        //{
        //    lock (locker)
        //    {
        //        return _database4.Query<PunchTmp>("SELECT * FROM [TempAccount] WHERE [ID] = 2");
        //    }
        //}

        public PunchTmp GetItem(int id)
        {
            lock (locker)
            {
                return _database4.Table<PunchTmp>().FirstOrDefault(x => x.ID == id);
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(PunchTmp tmp)
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
                return _database4.Delete<PunchTmp>(id);
            }
        }

        public int DeleteItem2()
        {
            lock (locker)
            {
                return _database4.Delete<PunchTmp>(from i in _database4.Table<PunchTmp>() select i);
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

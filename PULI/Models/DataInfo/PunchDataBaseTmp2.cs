using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class PunchDataBaseTmp2
    {
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database5;

        public PunchDataBaseTmp2()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database5 = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database5.DatabasePath;
            _database5.CreateTable<PunchTmp2>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        public IEnumerable<PunchTmp2> GetAccountAsync()
        {
            lock (locker)
            {
                return (from i in _database5.Table<PunchTmp2>() select i).ToList();
            }
        }

        public List<PunchTmp2> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database5.Table<PunchTmp2>() select i).ToList();
            }
        }

        //public IEnumerable<PunchTmp2> GetItemsTwo()
        //{
        //    lock (locker)
        //    {
        //        return _database5.Query<PunchTmp2>("SELECT * FROM [TempAccount] WHERE [ID] = 2");
        //    }
        //}

        public PunchTmp2 GetItem(int id)
        {
            lock (locker)
            {
                return _database5.Table<PunchTmp2>().FirstOrDefault(x => x.ID == id);
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(PunchTmp2 tmp)
        {
            lock (locker)
            {
                return _database5.Insert(tmp);
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
                return _database5.Delete<PunchTmp2>(id);
            }
        }

        public int DeleteItem2()
        {
            lock (locker)
            {
                return _database5.Delete<PunchTmp2>(from i in _database5.Table<PunchTmp2>() select i);
            }
        }


        public void DeleteAll()
        {
            var fooItems = GetAccountAsync().ToList();

            foreach (var item in fooItems)
            {
                DeleteItem(item.ID);
                //Console.WriteLine("KLKLKL " + item.account);
            }
        }


    }
}

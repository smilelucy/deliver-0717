using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class PunchYesOrNo
    {
        static object locker = new object();

        public string DBPath { get; set; }
        SQLiteConnection _database6;

        public PunchYesOrNo()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database6 = DependencyService.Get<ISQLite>().GetConnection();
            DBPath = _database6.DatabasePath;
            _database6.CreateTable<PunchYorN>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        public IEnumerable<PunchYorN> GetAccountAsync(int id)
        {
            lock (locker)
            {
                return (from i in _database6.Table<PunchYorN>() select i).ToList();
            }
        }

        public IEnumerable<PunchYorN> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database6.Table<PunchYorN>() select i).ToList();
            }
        }

        //public IEnumerable<PunchYorN> GetItemsTwo()
        //{
        //    lock (locker)
        //    {
        //        return _database6.Query<PunchYorN>("SELECT * FROM [TempAccount] WHERE [ID] = 2");
        //    }
        //}

        public PunchYorN GetItem(int id)
        {
            lock (locker)
            {
                return _database6.Table<PunchYorN>().FirstOrDefault(x => x.ID == id);
            }
        }

        //public Task<int> SaveAccountAsync(Account acc)
        //{
        //    return _database.InsertAsync(acc);
        //}

        public int SaveAccountAsync(PunchYorN tmp)
        {
            lock (locker)
            {
                return _database6.Insert(tmp);
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
                return _database6.Delete<PunchYorN>(id);
            }
        }

        public int DeleteItem2()
        {
            lock (locker)
            {
                return _database6.Delete<PunchYorN>(from i in _database6.Table<PunchYorN>() select i);
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

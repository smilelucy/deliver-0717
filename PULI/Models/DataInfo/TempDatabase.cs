using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class TempDatabase
    {
        //readonly SQLiteAsyncConnection _database;
        static object locker = new object();

        public string DBPath2 { get; set; }
        //public string DBPath3 { get; set; }
        //public string DBPath4 { get; set; }
        SQLiteConnection _database2;
        //SQLiteConnection _database3;
        //SQLiteConnection _database4;

        public TempDatabase()
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Account>().Wait();
            _database2 = DependencyService.Get<ISQLite>().GetConnection();
            //_database3 = DependencyService.Get<ISQLite>().GetConnection();
            //_database4 = DependencyService.Get<ISQLite>().GetConnection();
            DBPath2 = _database2.DatabasePath;
            //DBPath3 = _database3.DatabasePath;
            //DBPath4 = _database4.DatabasePath;

            _database2.CreateTable<TempAccount>(); // type 1
            _database2.CreateTable<Punch>(); // type 2
            _database2.CreateTable<Punch2>(); // type 3
            _database2.CreateTable<PunchTmp>(); // type 4
            _database2.CreateTable<PunchTmp2>(); // type 5
            _database2.CreateTable<Wifi_Punchout>(); // type 6
            _database2.CreateTable<Wifi_Punchin>(); // type 7
            //_database2.CreateTable<Choose>();
            _database2.CreateTable<Reset>();
            _database2.CreateTable<Entry_DB>();
            _database2.CreateTable<Entry_txt>();
            _database2.CreateTable<ResetLabel>();
            // create the tables
            //_database.CreateTableAsync<Account>().Wait(); // 創造
        }

        // TempAccount
        public IEnumerable<TempAccount> GetAccountAsync(int id)
        {
            lock (locker)
            {
                return (from i in _database2.Table<TempAccount>() select i ).ToList();
                
                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }
        public List<TempAccount> GetAccountAsyncToList(int id)
        {
            lock (locker)
            {
                return (from i in _database2.Table<TempAccount>() select i).ToList();
                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }

        public IEnumerable<TempAccount> GetAccountAsync2()
        {
            lock (locker)
            {
                return (from i in _database2.Table<TempAccount>() select i).ToList();
            }
        }

        public IEnumerable<TempAccount> GetItemsName(string name)
        {
            lock (locker)
            {
                return _database2.Query<TempAccount>("SELECT * FROM [TempAccount] WHERE [ClientName] = " + name);
            }
        }

        public TempAccount GetItem(int id)
        {
            lock (locker)
            {
                return _database2.Table<TempAccount>().FirstOrDefault(x => x.ID == id);
            }
        }


        public int SaveAccountAsync(TempAccount tmp)
        {
            lock (locker)
            {
                return _database2.Insert(tmp);
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


        public int UpdateAccountAsync(TempAccount tmp)
        {
            lock (locker)
            {
                //_database2.Update(tmp);
                //return tmp.ID;
                return _database2.Execute("UPDATE [TempAccount] SET [wqb99] = wqb99  WHERE [ID] = id");
                //return _database2.Query<TempAccount>("UPDATE * FROM [TempAccount] WHERE [ID] = 2");
                //_database2.Update(tmp);
                //return tmp.ID;
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return _database2.Delete<TempAccount>(id);
            }
        }

        public void DeleteAll_TempAccount()
        {
            // type1 type2 type3 
            var fooItems = GetAccountAsync2().ToList();
            foreach (var item in fooItems)
            {
                DeleteItem(item.ID);
                //Console.WriteLine("KLKLKL " + item.account);
            }

        }

        // Punch
        public IEnumerable<Punch> GetAccountAsync_Punch(int id)
        {
            lock (locker)
            {
                return (from i in _database2.Table<Punch>() select i).ToList();

                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }
        public IEnumerable<Punch> GetAccountAsync2_Punch()
        {
            lock (locker)
            {
                return (from i in _database2.Table<Punch>() select i).ToList();
            }
        }
        public int SaveAccountAsync_Punch(Punch tmp)
        {
            lock (locker)
            {
                return _database2.Insert(tmp);
                
            }
        }
        public int DeleteItem_Punch(int id)
        {
            lock (locker)
            {
                return _database2.Delete<Punch>(id);
            }
        }
        public void DeleteAll_Punch()
        {
            var fooItems = GetAccountAsync2_Punch().ToList();
            foreach (var item in fooItems)
            {
                DeleteItem_Punch(item.ID);
                //Console.WriteLine("KLKLKL " + item.account);
            }
        }


        // Punch2

        public IEnumerable<Punch2> GetAccountAsync_Punch2(int id)
        {
            lock (locker)
            {
                return (from i in _database2.Table<Punch2>() select i).ToList();

                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }
        public IEnumerable<Punch2> GetAccountAsync2_Punch2()
        {
            lock (locker)
            {
                return (from i in _database2.Table<Punch2>() select i).ToList();
            }
        }
        public int SaveAccountAsync_Punch2(Punch2 tmp)
        {
            lock (locker)
            {
                return _database2.Insert(tmp);

            }
        }
        public int DeleteItem_Punch2(int id)
        {
            lock (locker)
            {
                return _database2.Delete<Punch2>(id);
            }
        }
        public void DeleteAll_Punch2()
        {
            var fooItems = GetAccountAsync2_Punch2().ToList();
            foreach (var item in fooItems)
            {
                DeleteItem_Punch2(item.ID);
                //Console.WriteLine("KLKLKL " + item.account);
            }
        }



        // PunchTmp

        public IEnumerable<PunchTmp> GetAccountAsync_PunchTmp()
        {
            lock (locker)
            {
                return (from i in _database2.Table<PunchTmp>() select i).ToList();

                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }
        public List<PunchTmp> GetAccountAsync2_PunchTmp()
        {
            lock (locker)
            {
                return (from i in _database2.Table<PunchTmp>() select i).ToList();
            }
        }
        public int DeleteItem_PunchTmp(int id)
        {
            lock (locker)
            {
                return _database2.Delete<PunchTmp>(id);
            }
        }
        public void DeleteAll_PunchTmp()
        {
            var fooItems = GetAccountAsync_PunchTmp().ToList();

            foreach (var item in fooItems)
            {
                DeleteItem_PunchTmp(item.ID);
                //Console.WriteLine("PPPPP " + item.name);
            }
        }
        public int SaveAccountAsync_PunchTmp(PunchTmp tmp)
        {
            lock (locker)
            {
                return _database2.Insert(tmp);
            }
        }

        // PunchTmp2

        public IEnumerable<PunchTmp2> GetAccountAsync_PunchTmp2()
        {
            lock (locker)
            {
                return (from i in _database2.Table<PunchTmp2>() select i).ToList();

                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }
        public List<PunchTmp2> GetAccountAsync2_PunchTmp2()
        {
            lock (locker)
            {
                return (from i in _database2.Table<PunchTmp2>() select i).ToList();
            }
        }
        public int SaveAccountAsync_PunchTmp2(PunchTmp2 tmp)
        {
            lock (locker)
            {
                return _database2.Insert(tmp);
                
            }
        }
        public int DeleteItem_PunchTmp2(int id)
        {
            lock (locker)
            {
                return _database2.Delete<PunchTmp2>(id);
            }
        }
        public void DeleteAll_PunchTmp2()
        {
            var fooItems = GetAccountAsync_PunchTmp2().ToList();

            foreach (var item in fooItems)
            {
                DeleteItem_PunchTmp2(item.ID);
                //Console.WriteLine("KLKLKL " + item.account);
            }
        }

        // Wifi_Punchout

        public IEnumerable<Wifi_Punchout> GetAccountAsync_Wifi_Punchout(int id)
        {
            lock (locker)
            {
                return (from i in _database2.Table<Wifi_Punchout>() select i).ToList();

                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }
        public IEnumerable<Wifi_Punchout> GetAccountAsync3_Wifi_Punchout()
        {
            lock (locker)
            {
                return (from i in _database2.Table<Wifi_Punchout>() select i).ToList();
            }
        }
        public List<Wifi_Punchout> GetAccountAsync2_Wifi_Punchout()
        {
            lock (locker)
            {
                return (from i in _database2.Table<Wifi_Punchout>() select i).ToList();
            }
        }
        public int SaveAccountAsync_Wifi_Punchout(Wifi_Punchout tmp)
        {
            lock (locker)
            {
                return _database2.Insert(tmp);
            }
        }
        public int DeleteItem_Wifi_Punchout(int id)
        {
            lock (locker)
            {
                return _database2.Delete<Wifi_Punchout>(id);
            }
        }

        public void DeleteAll_Wifi_Punchout()
        {
            var fooItems = GetAccountAsync3_Wifi_Punchout().ToList();

            foreach (var item in fooItems)
            {
                DeleteItem_Wifi_Punchout(item.ID);
                //Console.WriteLine("PPPPP " + item.name);
            }
        }



        // Wifi_Punchin

        public IEnumerable<Wifi_Punchin> GetAccountAsync_Wifi_Punchin(int id)
        {
            lock (locker)
            {
                return (from i in _database2.Table<Wifi_Punchin>() select i).ToList();

                //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
            }
        }
        public List<Wifi_Punchin> GetAccountAsync2_Wifi_Punchin()
        {
            lock (locker)
            {
                return (from i in _database2.Table<Wifi_Punchin>() select i).ToList();
            }
        }
        public IEnumerable<Wifi_Punchin> GetAccountAsync3_Wifi_Punchin()
        {
            lock (locker)
            {
                return (from i in _database2.Table<Wifi_Punchin>() select i).ToList();
            }
        }
        public int SaveAccountAsync_Wifi_Punchin(Wifi_Punchin tmp)
        {
            lock (locker)
            {
                return _database2.Insert(tmp);
               
            }
        }
        public int DeleteItem_Wifi_Punchin(int id)
        {
            lock (locker)
            {
                return _database2.Delete<Wifi_Punchin>(id);
            }
        }
        public void DeleteAll_Wifi_Punchin()
        {
            var fooItems = GetAccountAsync3_Wifi_Punchin().ToList();

            foreach (var item in fooItems)
            {
                DeleteItem_Wifi_Punchin(item.ID);
                //Console.WriteLine("PPPPP " + item.name);
            }
        }


        // ActivityView

        public int SaveAccountAsync_ActivityView(TotalList tmp)
        {
            lock (locker)
            {
                return _database2.Insert(tmp);

            }
        }

        public IEnumerable<TotalList> GetAccountAsync_ActivityView()
        {
            lock (locker)
            {
                //return (from i in _database2.Table<TotalList>() select i).ToList();
                return (from i in _database2.Table<TotalList>() select i);
            }
        }



        // Choose
        //public IEnumerable<Choose> GetAccountAsync2_Choose()
        //{
        //    lock (locker)
        //    {
        //        return (from i in _database2.Table<Choose>() select i).ToList();
        //    }
        //}

        //public IEnumerable<Choose> GetAccountAsync_Choose(int id)
        //{
        //    lock (locker)
        //    {
        //        return (from i in _database2.Table<Choose>() select i).ToList();
        //        //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
        //    }
        //}

        //public int SaveAccountAsync_Choose(Choose tmp)
        //{
        //    lock (locker)
        //    {
        //        return _database2.Insert(tmp);

        //    }
        //}

        //public int DeleteItem_Choose(int id)
        //{
        //    lock (locker)
        //    {
        //        return _database2.Delete<Choose>(id);
        //    }
        //}

        //public void DeleteAll_Choose()
        //{
        //    var fooItems = GetAccountAsync2_Choose().ToList();
        //    foreach (var item in fooItems)
        //    {
        //        DeleteItem_Choose(item.ID);
        //        //Console.WriteLine("KLKLKL " + item.account);
        //    }
        //}

        // Reset
        //public IEnumerable<Reset> GetAccountAsync2_Reset()
        //{
        //    lock (locker)
        //    {
        //        return (from i in _database2.Table<Reset>() select i).ToList();
        //    }
        //}

        //public IEnumerable<Reset> GetAccountAsync_Reset(int id)
        //{
        //    lock (locker)
        //    {
        //        return (from i in _database2.Table<Reset>() select i).ToList();
        //        //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
        //    }
        //}
        //public int SaveAccountAsync_Reset(Reset tmp)
        //{
        //    lock (locker)
        //    {
        //        return _database2.Insert(tmp);

        //    }
        //}
        //public int DeleteItem_Reset(int id)
        //{
        //    lock (locker)
        //    {
        //        return _database2.Delete<Reset>(id);
        //    }
        //}
        //public void DeleteAll_Reset()
        //{
        //    var fooItems = GetAccountAsync2_Reset().ToList();
        //    foreach (var item in fooItems)
        //    {
        //        DeleteItem_Reset(item.ID);
        //        //Console.WriteLine("KLKLKL " + item.account);
        //    }
        //}

        //// Entry_DB
        //public IEnumerable<Entry_DB> GetAccountAsync2_Entry_DB()
        //{
        //    lock (locker)
        //    {
        //        return (from i in _database2.Table<Entry_DB>() select i).ToList();
        //    }
        //}
        //public IEnumerable<Entry_DB> GetAccountAsync_Entry_DB(int id)
        //{
        //    lock (locker)
        //    {
        //        return (from i in _database2.Table<Entry_DB>() select i).ToList();
        //        //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
        //    }
        //}
        //public int DeleteItem_Entry_DB(int id)
        //{
        //    lock (locker)
        //    {
        //        return _database2.Delete<Entry_DB>(id);
        //    }
        //}
        //public int SaveAccountAsync_Entry_DB(Entry_DB tmp)
        //{
        //    lock (locker)
        //    {
        //        return _database2.Insert(tmp);

        //    }
        //}
        //public void DeleteAll_Entry_DB()
        //{
        //    var fooItems = GetAccountAsync2_Entry_DB().ToList();
        //    foreach (var item in fooItems)
        //    {
        //        DeleteItem_Entry_DB(item.ID);
        //        //Console.WriteLine("KLKLKL " + item.account);
        //    }
        //}

        //// Entry_txt
        //public IEnumerable<Entry_txt> GetAccountAsync2_Entry_txt()
        //{
        //    lock (locker)
        //    {
        //        return (from i in _database2.Table<Entry_txt>() select i).ToList();
        //    }
        //}
        //public IEnumerable<Entry_txt> GetAccountAsync_Entry_txt(int id)
        //{
        //    lock (locker)
        //    {
        //        return (from i in _database2.Table<Entry_txt>() select i).ToList();
        //        //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
        //    }
        //}
        //public int DeleteItem_Entry_txt(int id)
        //{
        //    lock (locker)
        //    {
        //        return _database2.Delete<Entry_txt>(id);
        //    }
        //}
        //public int SaveAccountAsync_Entry_txt(Entry_txt tmp)
        //{
        //    lock (locker)
        //    {
        //        return _database2.Insert(tmp);

        //    }
        //}
        //public void DeleteAll_Entry_txt()
        //{
        //    var fooItems = GetAccountAsync2_Entry_txt().ToList();
        //    foreach (var item in fooItems)
        //    {
        //        DeleteItem_Entry_txt(item.ID);
        //        //Console.WriteLine("KLKLKL " + item.account);
        //    }
        //}

        //// ResetLabel
        //public IEnumerable<ResetLabel> GetAccountAsync2_ResetLabel()
        //{
        //    lock (locker)
        //    {
        //        return (from i in _database2.Table<ResetLabel>() select i).ToList();
        //    }
        //}
        //public IEnumerable<ResetLabel> GetAccountAsync_ResetLabel(int id)
        //{
        //    lock (locker)
        //    {
        //        return (from i in _database2.Table<ResetLabel>() select i).ToList();
        //        //return (from i in _database2.Table<TempAccount>() orderby id descending select i).ToList();
        //    }
        //}
        //public int SaveAccountAsync_ResetLabel(ResetLabel tmp)
        //{
        //    lock (locker)
        //    {
        //        return _database2.Insert(tmp);

        //    }
        //}
        //public int DeleteItem_ResetLabel(int id)
        //{
        //    lock (locker)
        //    {
        //        return _database2.Delete<ResetLabel>(id);
        //    }
        //}
        //public void DeleteAll_ResetLabel()
        //{
        //    var fooItems = GetAccountAsync2_ResetLabel().ToList();
        //    foreach (var item in fooItems)
        //    {
        //        DeleteItem_ResetLabel(item.ID);
        //        //Console.WriteLine("KLKLKL " + item.account);
        //    }
        //}
    }
}
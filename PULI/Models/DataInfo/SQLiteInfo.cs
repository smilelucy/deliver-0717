using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class SQLiteInfo 
    {
        //在這邊定義資料表的table schema
        public SQLiteInfo()
        {
           
        }
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public string UserName { get; set; }
        public string SelectItem { get; set; }
        public bool Done { get; set; }
    }
}
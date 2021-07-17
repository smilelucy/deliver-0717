using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class TempAccount
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string wqh_s_num { get; set; }
        public string qh_s_num { get; set; }
        public string qb_s_num { get; set; }
        public string qb_order { get; set; }
        public string wqb01 { get; set; }
        public string wqb99 { get; set; }
        
    }
    public class Choose
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ClientName { get; set; }
        public bool ischoose { get; set; }
        
    }

    public class Reset
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string wqh_s_num { set; get; }
        public string qb_order { set; get; }
        public string color { set; get; }

    }

    public class ResetLabel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string wqh_s_num { set; get; }
        public string qb_order { set; get; }
        public string yesorno { set; get; }

    }

    public class Entry_DB
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string wqh_s_num { set; get; }
        public string qb_s_num { get; set; }
        public string qb_order { set; get; }

    }

    public class Entry_txt
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string wqh_s_num { set; get; }
        public string entrytxt { set; get; }
    }

    public class TmpAdd
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string wqh_s_num { get; set; }
        public string qb_order { get; set; }
    }

    //public class Temptxt
    //{
    //    public int ID { get; set; }
    //    public string wqb99 { get; set; }
    //}
}
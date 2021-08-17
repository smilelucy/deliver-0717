using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PULI.Models.DataInfo
{
    public class PunchInfo
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string inorout { get; set; }
        public string name { get; set; }
        public string dys_05_type { get; set; }
        public string ct06_telephone { get; set; }
        public string sec06 { get; set; }

        //public string error { get; set; }
        public string dys03 { get; set; }
        public string dys02 { get; set; }
        

    }
}

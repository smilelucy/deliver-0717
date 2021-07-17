using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PULI.Models.DataInfo
{
    public class CheckDate
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string date { get; set; }
        
    }
}

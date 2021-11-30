using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class Punch2 
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int setnum { get; set; }
    }
    
}
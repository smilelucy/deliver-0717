using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Deliver.Models.DataInfo
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public string identity { get; set; }
        public string login_time { get; set; }

        public string time { get; set; }
    }
}
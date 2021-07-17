using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class PunchTmp
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        // MainPage.token, ct_s_num, sec_s_num, mlo_s_num, bn_s_num, position.Latitude, position.Longitude
      
        public string name { get; set; }

        public string time { get; set; }


    }
}
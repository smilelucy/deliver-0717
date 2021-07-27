using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PULI.Models.DataInfo
{
    public class Wifi_Punchin
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        // MainPage.token, ct_s_num, sec_s_num, mlo_s_num, bn_s_num, position.Latitude, position.Longitude

        public string name { get; set; }

        public string time { get; set; }


    }
}

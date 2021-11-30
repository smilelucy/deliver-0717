using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class Punch
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        // MainPage.token, ct_s_num, sec_s_num, mlo_s_num, bn_s_num, position.Latitude, position.Longitude
        public string token { get; set; }
        public string name { get; set; }

        public string punchname { get; set; }
        public string inorout { set; get; }
        public int setnum { get; set; }
        public string ct_s_num { get; set; }
        public string sec_s_num { get; set; }
        public string mlo_s_num { get; set; }
        public string bn_s_num { get; set; }
        public string reh_s_num { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public string time { get; set; }

        public string timeforpost { get; set; }
        public string phl50 { get; set; }

    }

    public class PunchIn
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        // MainPage.token, ct_s_num, sec_s_num, mlo_s_num, bn_s_num, position.Latitude, position.Longitude
        public string token { get; set; }
        public string name { get; set; }

        public string punchname { get; set; }
        public string inorout { set; get; }
        public int setnum { get; set; }
        public string ct_s_num { get; set; }
        public string sec_s_num { get; set; }
        public string mlo_s_num { get; set; }
        public string bn_s_num { get; set; }
        public string reh_s_num { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public string time { get; set; }

        public string timeforpost { get; set; }
        public string phl50 { get; set; }

    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Deliver.Models.DataInfo
{
    public class ClientInfo
    {
        [JsonProperty("ct_s_num")]
        public string ct_s_num { get; set; }

        [JsonProperty("sec_s_num")]
        public string sec_s_num { get; set; }

        [JsonProperty("mlo_s_num")]
        public string mlo_s_num { get; set; }

        [JsonProperty("bn_s_num")]
        public string bn_s_num { get; set; }

        [JsonProperty("ct_name")]
        public string ct_name { get; set; }

        //[JsonProperty("ClientBirthday")]
        //public string ClientBirthday { get; set; }

        [JsonProperty("ct_address")]
        public string ClientAddress { get; set; }

        [JsonProperty("ct16")]
        public string ct16 { get; set; }

        [JsonProperty("ct17")]
        public string ct17 { get; set; }

        //[JsonProperty("MealName")]
        //public string MealName { get; set; }
    }
    public class AllClientInfo
    {
        [JsonProperty("s_num")] // 案主ID
        public string s_num { get; set; }

        [JsonProperty("bn_s_num")] // beacon ID
        public string bn_s_num { get; set; }

        [JsonProperty("ct01")] // 案主姓
        public string ct01 { get; set; }

        [JsonProperty("ct02")] // 案主名
        public string ct02 { get; set; }

        [JsonProperty("name")] // 案主名
        public string name
        {
            get
            {
                return string.Format("{0}{1} ", ct01, ct02);
            }
        }

        [JsonProperty("ct03")] // 案主身分證
        public string ct03 { get; set; }

        [JsonProperty("ct04")] // 案主性別
        public string ct04 { get; set; }

        [JsonProperty("ct05")] // 案主生日
        public string ct05 { get; set; }

        [JsonProperty("ct06_homephone")] // 案主家電
        public string ct06_homephone { get; set; }

        [JsonProperty("ct06_telephone")] // 案主手機
        public string ct06_telephone { get; set; }

        [JsonProperty("ct16")] // 案主家緯度
        public string ct16 { get; set; }

        [JsonProperty("ct16_actual")] // 案主家緯度(現場)
        public double ct16_actual { get; set; }

        [JsonProperty("ct17")] // 案主家經度
        public string ct17 { get; set; }

        [JsonProperty("ct17_actual")] // 案主家經度(現場)
        public double ct17_actual { get; set; }

    }
    public class AllClientInfo2
    {
       

        [JsonProperty("ct01")] // 案主姓
        public string ct01 { get; set; }

        [JsonProperty("ct02")] // 案主名
        public string ct02 { get; set; }

        [JsonProperty("name")] // 案主名
        public string name
        {
            get
            {
                return string.Format("{0}{1} ", ct01, ct02);
            }
        }

        [JsonProperty("ct03")] // 案主身分證
        public string ct03 { get; set; }

        [JsonProperty("ct04")] // 案主性別
        public string ct04 { get; set; }

        [JsonProperty("ct05")] // 案主生日
        public string ct05 { get; set; }

        [JsonProperty("ct06")] // 手機
        public string ct06 { get; set; }

       
    }
}
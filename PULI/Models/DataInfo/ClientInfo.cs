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

    public class AllInfo
    {
        [JsonProperty("bn_s_num")]
        public string bn_s_num { get; set; }

        [JsonProperty("mlo_s_num")]
        public string mlo_s_num { get; set; }

        [JsonProperty("sec_s_num")]
        public string sec_s_num { get; set; }

        [JsonProperty("s_num")]
        public string s_num { set; get; }// 配送單序號

        [JsonProperty("dys08")]
        public string dys08 { set; get; }// 送餐順序

        [JsonProperty("sec06")]
        public string sec06 { set; get; }// 放置點

        [JsonProperty("ct_name")]
        public string ct_name { set; get; }// 案主名稱

        [JsonProperty("ct_address")]
        public string ct_address { set; get; }// 案主聯絡地

        [JsonProperty("ct16")]
        public string ct16 { set; get; }// 經度

        [JsonProperty("ct17")]
        public string ct17 { set; get; }// 緯度

        [JsonProperty("dys02")]
        public string dys02 { set; get; }// 餐別(只顯示中晚餐)

        [JsonProperty("dys03")]
        public string dys03 { set; get; }// 餐點名稱

        [JsonProperty("dys04")]
        public string dys04 { set; get; }// 餐點指示

        private string setcolor = null;
        [JsonProperty("dys05_type")]
        public string dys05_type
        {
            set;
            get;
        }// 代餐種類

        [JsonProperty("ct06_telephone")]
        public string ct06_telephone { set; get; }// 電話




        //private string setcolor = null;

        //[JsonProperty("dys05")]
        //public string dys05
        //{
        //    set
        //    {
        //        if (value == "1")
        //            setcolor = ConsoleColor.Red.ToString();
        //    }
        //    get => setcolor;
        //}// 是否異動(如果1你就顯示紅色)

        [JsonProperty("dys06")]
        public string dys06 { set; get; }// 餐點是否異動

        [JsonProperty("dys13")]
        public string dys13 { set; get; }// 是否自費

        [JsonProperty("ct_mp04")]
        public string ct_mp04 { set; get; }// 代餐是否送達

        [JsonProperty("ct_mp06")]
        public string ct_mp06 { set; get; }// 代餐是否異動
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
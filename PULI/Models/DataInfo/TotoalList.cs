using Newtonsoft.Json;
using PULI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class TotalList
    {
        [JsonProperty("daily_shipment")]
        public List<daily_shipment> daily_shipments { set; get; }

        [JsonProperty("abnormal")]
        public List<abnormal> abnormals { set; get; }
    }
    public class daily_shipment
    {
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
        public string ct17  { set; get; }// 緯度

        [JsonProperty("dys02")]
        public string dys02 { set; get; }// 餐別(只顯示中晚餐)

        [JsonProperty("dys03")]
        public string dys03 { set; get; }// 餐點名稱

        [JsonProperty("dys04")]
        public string dys04 { set; get; }// 餐點指示

        private string setcolor = null;
        [JsonProperty("dys05_type")]
        public string dys05_type {
            set;
            get ; 
        }// 代餐種類

        [JsonProperty("ct06_telephone")]
        public string ct06_telephone { set; get; }// 電話

        [JsonProperty("dys21")]
        public string dys21 { set; get; }// 電話




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

        [JsonProperty("sec_s_num")]
        public string sec_s_num { set; get; }// 

        [JsonProperty("mlo_s_num")]
        public string mlo_s_num { set; get; }// 

        [JsonProperty("ct_s_num")]
        public string ct_s_num { set; get; }// 

        [JsonProperty("reh_s_num")]
        public string reh_s_num { set; get; }// 

        //[JsonProperty("ShipmentOrder")]
        //public string ShipmentOrder { set; get; }


    }

    public class stopname
    {
        [JsonProperty("stop")]
        public string stop { set; get; }// 
    }
    public class restorename
    {
        [JsonProperty("restore")]
        public string restore { set; get; }// 
    }

    public class abnormal
    {
        [JsonProperty("ClientName")]
        public string ClientName { set; get; }// 案主名稱

        //[JsonProperty("different")]
        //public string different { set; get; }// 異動內容(你需要的)

    }

    public class deliver
    {
        [JsonProperty("dys06")]
        public int dys06 { set; get; }// 送餐順序

    }
}
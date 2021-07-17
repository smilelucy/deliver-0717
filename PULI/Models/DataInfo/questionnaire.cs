using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class questionnaire
    {
        string both1;
        [JsonProperty("ClientName")]
        public string ClientName { set; get; }
        //public string both
        //{
        //    get
        //    {
        //        for (int i = 0; i < qbs.Count; i++)
        //        {
        //            return ClientName + wqh_s_num + qbs[i].qb_order;
        //        }
        //        return both1;
        //    }
        //    set
        //    {
        //        both1 = value;
        //    }
        //}
        //public string both
        //{
        //    get
        //    {
        //        if (wqh_s_num != null)
        //            return ClientName + wqh_s_num + qbs[0].qb_order;
        //        else
        //            return both1;
        //    }
        //    set
        //    {
        //        both1 = value;
        //    }
        //}
        //public string both2
        //{
        //    get
        //    {
        //        if (wqh_s_num != null)
        //            return ClientName + wqh_s_num + qbs[1].qb_order;
        //        else
        //            return both1;
        //    }
        //    set
        //    {
        //        both1 = value;
        //    }
        //}

        //string wqh = "工作問卷編號";
        [JsonProperty("wqh_s_num")]
        //public string wqh_s_num { set { wqh += value; } get => wqh; } // 工作問卷編號
        public string wqh_s_num { set; get; } // 工作問卷編號

        string qh = "問卷編號";
        [JsonProperty("qh_s_num")]
        public string qh_s_num { set { qh += value; } get => qh; } // 問卷編號

        [JsonProperty("qb_s_num")]
        public string qb_s_num { set; get; } // 流水號

        [JsonProperty("qh01")]
        public string qh01 { set; get; } // 問卷名稱

        [JsonProperty("qb")]
        public List<qb> qbs { set; get; }

        //public bool ifischeck = false;
        
        
}
    public class qb
    {
        [JsonProperty("qb_s_num")]
        public string qb_s_num { set; get; } // 問題編號


        [JsonProperty("qb_order")]
        public string qb_order { set; get; } // 題號

        [JsonProperty("qb01")]
        public string qb01 { set; get; } // 問題題目

        public string qb02 { set; get; } // 問題題目

        [JsonProperty("qb03")]
        public string[] qb03 { set; get; } // 選項
    }

    public class checkInfo
    {
        [JsonProperty("wqh_s_num")]
        //public string wqh_s_num { set { wqh += value; } get => wqh; } // 工作問卷編號
        public string wqh_s_num { set; get; } // 工作問卷編號

        [JsonProperty("qb_s_num")]
        public string qb_s_num { set; get; } // 問題編號

        [JsonProperty("qh_s_num")]
        public string qh_s_num { set; get; } // 工作問卷編號

        [JsonProperty("qb_order")]
        public string qb_order { set; get; } // 


        [JsonProperty("wqb01")]
        public string wqb01 { set; get; } // 答案

        [JsonProperty("wqb99")]
        public string wqb99 { set; get; } // 問題編號

    }

    public class checkInfomulti
    {
        [JsonProperty("wqh_s_num")]
        //public string wqh_s_num { set { wqh += value; } get => wqh; } // 工作問卷編號
        public string wqh_s_num { set; get; } // 工作問卷編號

        [JsonProperty("qb_s_num")]
        public string qb_s_num { set; get; } // 問題編號

        [JsonProperty("qh_s_num")]
        public string qh_s_num { set; get; } // 工作問卷編號

        [JsonProperty("wqb01")]
        public string[] wqb01 { set; get; } // 答案

        //[JsonProperty("wqb99")]
        //public string wqb99 { set; get; } // 問題編號

    }

    public class work_data
    {
        [JsonProperty("work_data")]
        public List<checkInfo> upload_check { set; get; }
    }

}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class upload_Q
    {
        [JsonProperty("wqh_s_num")]
        public string wqh_s_num { set; get; } //工作問卷編號

        [JsonProperty("qh_s_num")]
        public string qh_s_num { set; get; } //問卷編號

        [JsonProperty("qb_s_num")]
        public string qb_s_num { set; get; } //問題題號

        [JsonProperty("wqb01")]
        public string wqb01 { set; get; } //問題答案
    }

    //public class work_data
    //{
    //    [JsonProperty("work_data")]
    //    public List<upload_Q> upload_Qs { set; get; }
    //    //public IEnumerable<upload_Q> upload_Qs { set; get; }
    //}
}
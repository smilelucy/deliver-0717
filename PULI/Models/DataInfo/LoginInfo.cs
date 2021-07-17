using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Deliver.Models.DataInfo
{
    public class LoginInfo
    {
        [JsonProperty("acc_user")]
        public string acc_user { get; set; }

        [JsonProperty("acc_password")]
        public string acc_password { get; set; }

        [JsonProperty("state")]
        public string state { get; set; }

        [JsonProperty("acc_name")]
        public string acc_name { get; set; }

        [JsonProperty("acc_auth")]
        public string acc_auth { get; set; }

        [JsonProperty("login_time")]
        public string login_time { get; set; }

        [JsonProperty("acc_token")]
        public string acc_token { get; set; }

        [JsonProperty("daily_shipment_nums")]
        public int daily_shipment_nums { get; set; }

    }
}
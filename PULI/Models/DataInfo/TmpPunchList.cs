using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Deliver.Models.DataInfo
{
    public class TmpPunchList
    {
        [JsonProperty("name")]
        public string name { get; set; }

       

    }
}
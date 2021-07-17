using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataInfo
{
    public class Item 
    {
        public string Name { get; set; }
        public string Questionnum { get; set; }
        public string Question { get; set; }

        public bool isChecked { get; set; } // This field indicates whether or not it is selected
    }
}
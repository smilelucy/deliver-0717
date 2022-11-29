using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JSJ.Droid.Services;
using JSJ.Services;
using PULI.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(StopForegoundService))]
namespace JSJ.Droid.Services
{
    class StopForegoundService : IQRCode
    {
        public async Task<string> ScanAsync()
        {
            //ForService ser = new ForService();
            ForService2 ser = new ForService2();
            ser.A();
            return "ABC";
        }
    }
}
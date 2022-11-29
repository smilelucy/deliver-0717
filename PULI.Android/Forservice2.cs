using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PULI.Droid
{
    [Service]
    public class ForService2 : Android.App.Service
    {
        public const int FORSERVICE_NOTIFICATION_ID = 10000;
        public const string MAIN_ACTIVITY_ACTION = "Main_activity";
        public const string PUT_EXTRA = "has_service_been_started";

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        

        public void A()
        {
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(FORSERVICE_NOTIFICATION_ID);
            StopSelf();
            StopForeground(true);


        }
    }
}
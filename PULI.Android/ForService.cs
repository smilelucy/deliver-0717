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
    public class ForService : Android.App.Service
    {
        public const int FORSERVICE_NOTIFICATION_ID = 10000;
        public const string MAIN_ACTIVITY_ACTION = "Main_activity";
        public const string PUT_EXTRA = "has_service_been_started";

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            registerForService();

            //Task.Run(async () =>
            //{
                //await arbitraryMethodInViewModel(); // this method runs an asynchronous timer
            //});

            return StartCommandResult.Sticky;
        }

        private void registerForService()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                ICharSequence derp = new Java.Lang.String("Channel");
                var channel = new NotificationChannel("10000", derp, NotificationImportance.Default)
                {
                    Description = "Foreground Service Channel"
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);

                var notification = new Notification.Builder(this, "10000")
                .SetContentTitle("etc")
                .SetContentText("etc")
                //.SetSmallIcon(Resource.Drawable.ic_stat_name)
                .SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                //.AddAction(BuildRestartTimerAction())
                .AddAction(BuildStopServiceAction())
                .Build();

                // Enlist this instance of the service as a foreground service
                StartForeground(FORSERVICE_NOTIFICATION_ID, notification);
            }
            else
            {
                var notification = new Notification.Builder(this)
                    .SetContentTitle("etc")
                    .SetContentText("etc")
                    //.SetSmallIcon(Resource.Drawable.ic_stat_name)
                    .SetContentIntent(BuildIntentToShowMainActivity())
                    .SetOngoing(true)
                    //.AddAction(BuildRestartTimerAction())
                    .AddAction(BuildStopServiceAction())
                    .Build();

                // Enlist this instance of the service as a foreground service
                StartForeground(FORSERVICE_NOTIFICATION_ID, notification); // 每次啟動Service的時候，就會跳一個通知，告訴使用者：這邊有一個背景任務正在運作中唷
            }
        }

        PendingIntent BuildIntentToShowMainActivity()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.SetAction(MAIN_ACTIVITY_ACTION);
            notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
            notificationIntent.PutExtra(PUT_EXTRA, true);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
            return pendingIntent;
        }

        Notification.Action BuildStopServiceAction()
        {
            var stopServiceIntent = new Intent(this, GetType());
            stopServiceIntent.SetAction(Constants.ACTION_STOP_SERVICE);
            var stopServicePendingIntent = PendingIntent.GetService(this, 0, stopServiceIntent, 0);

            var builder = new Notification.Action.Builder(Android.Resource.Drawable.IcMediaPause,
                                                          GetText(Resource.String.stop_service),
                                                          stopServicePendingIntent);
            //var builder = new Notification.Action.Builder(Android.Resource.Drawable.IcMediaPause,
            //                                             "停止",
            //                                            stopServicePendingIntent);
            return builder.Build();

        }

        public override void OnDestroy()
        {
            // Remove the notification from the status bar.
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(FORSERVICE_NOTIFICATION_ID);
            StopSelf();
            StopForeground(true);

            base.OnDestroy();
        }
    }
}
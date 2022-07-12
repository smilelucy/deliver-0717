using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Android.Support.V4.App;
using Android.Locations;
using Xamarin.Forms;
using Android.Content;
using Plugin.Permissions;
using Messier16.Forms.Android.Controls;

namespace PULI.Droid
{
    [Activity(Label = "弗傳慈心基金會", Icon = "@mipmap/icon2", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Plugin.Media.CrossMedia.Current.Initialize();
            
            CheckboxRenderer.Init();
            global::Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //DisplayAlert("提示", "[弗傳慈心基金會] 會收集位置資料，以便在應用程式關閉或未使用時，也可支援紀錄外送員gps位置以判斷打卡。", "ok");
            AlertDialog ad = new AlertDialog.Builder(this).Create();
            ad.SetMessage("[弗傳慈心基金會] 會收集位置資料，以便在應用程式關閉或未使用時，也可支援紀錄外送員gps位置以判斷打卡。");
            ad.SetButton2("好", delegate
            {
                ActivityCompat.RequestPermissions(this,
               new String[]{

                        Manifest.Permission.AccessFineLocation,
                        Manifest.Permission.AccessCoarseLocation,
                        Manifest.Permission.Camera,
                        Manifest.Permission.WriteExternalStorage,
                        Manifest.Permission.ReadExternalStorage,
                        Manifest.Permission.WriteExternalStorage,
                        Manifest.Permission.Internet,
                        Manifest.Permission.AccessWifiState,
                        Manifest.Permission.Bluetooth,
                        Manifest.Permission.BluetoothAdmin,
                        Manifest.Permission.InstantAppForegroundService,
                        Manifest.Permission.AccessLocationExtraCommands
               },
               100);
            });
            ad.Show();
            LoadApplication(new App());
            

            //Intent startIntent = new Intent(this, typeof(ForService));
            //if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)   // if API >= 26
            //    StartForegroundService(startIntent);
            //else                                // if API <= 25
            //    StartService(startIntent);



            //CrossBadge.Current.SetBadge(9);
            Console.WriteLine("EXECUTE");
            OpenLocationSettings();
        }

        
       
        public void OpenLocationSettings()
        {
            LocationManager LM = (LocationManager)Forms.Context.GetSystemService(Android.Content.Context.LocationService);
            if (LM.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                AlertDialog ad = new AlertDialog.Builder(this).Create();

                ad.SetMessage("請開啟定位服務");
                ad.SetCancelable(false);
                ad.SetCanceledOnTouchOutside(false);
                ad.SetButton("好", delegate
                {
                    Android.Content.Context ctx = Forms.Context;
                    ctx.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                });

                ad.SetButton2("不好", delegate
                {

                });
                ad.Show();
            }
        }
    }
}
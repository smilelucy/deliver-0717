using PULI.Models.DataCell;
using Quick.Xamarin.BLE;
using Quick.Xamarin.BLE.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PULI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class BeaconScan 
    {
        public INavigation Navigation { get; set; }
        public static AdapterConnectStatus BleStatus;
        public static IBle ble;
        public static IDev ConnectDevice = null;
        public static List<BeaconItem> beacons;
        string substr = "HERE_Beacon"; // 每家公司不同
        public static List<string> checkList = new List<string>();
        //public static bool ischeck = false;
        public static bool letpunchin = false;
        public static bool letpunchout = false;
        public static bool beaconin = false;
        public static bool beaconout = false;
        public static string UUID;

        public BeaconScan()
        {
            //Messager();
            //Navigation = navigation;

            ble = CrossBle.Createble();
            Console.WriteLine("issanning~~~" + ble.isScanning);
            ble.OnScanDevicesIn += Ble_OnScanDevicesIn;
            BleStatus = ble.AdapterConnectStatus;

            //Console.WriteLine("blestatus~~~" + BleStatus);
            ble.StartScanningForDevices();
            //when search devices
            //Messager();
            //list = new ObservableCollection<IDevice>();
            beacons = new List<BeaconItem>();
            Console.WriteLine("BEACONPAGE~~~");
            Device.StartTimer(TimeSpan.FromSeconds(2), OnTimerElapsed);
        }

        public bool OnTimerElapsed()
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                RetreiveBLE();

            });
            return true;
        }

        private void RetreiveBLE()
        {
            if (beacons.Count > 0)
            {
                try
                {
                    //list.Clear();
                    //var insertList = new NewIbeConInfo
                    //{
                    //    uid = MainPage.UsrID,
                    //    ibeacons = ibeaconList
                    //};

                    ///*
                    //WebSocket._scc.Emit(WebSocket._emit_channel, insertList, (n, err, d) =>
                    //{
                    //    Console.WriteLine("EmitErr ; " + err);
                    //});*/

                    //ibeaconList.Clear();
                    if(letpunchin == true)
                    {
                        beaconin = true;
                    }
                    if(letpunchout == false)
                    {
                        beaconout = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }


        }

        private void Ble_OnScanDevicesIn(object sender, IDev e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    //Console.WriteLine("BEACON~~~LA~~~");
                    if (e.Name != null)
                    {
                        if (e.Name.Contains(substr))
                        {
                            Console.WriteLine("beacon_in~~~~");
                            Console.WriteLine("Device Name : {0} Rssi : {1} UUID : {2} ", e.Name, calculateDistance(e.Rssi), e.Uuid);
                            //Console.WriteLine("TriggerDistance : " + Int32.Parse(N}avigateView.ibeConDistance));
                            if (calculateDistance(e.Rssi) < 5)
                            {
                                Console.WriteLine("Less5~ " + e.Name);
                                if (!checkList.Contains(e.Name))
                                {
                                    Console.WriteLine("okin~ " + e.Name);
                                    UUID = e.Uuid;
                                    // Console.WriteLine("Device Name : " + MainPage.ibeDic[e.Name] + "Distance : " + calculateDistance(e.Rssi));
                                    checkList.Add(e.Name);
                                    letpunchin = true;
                                    //await Navigation.PushAsync(new ProductDetailView(MainPage.prdDic[e.Name]));
                                    //await Navigation.PushAsync(new ProductDetailView(await WebServices.getprdIDByIbeName(a.Device.Name)));
                                    //var s = await WebService.InsertBonus(MainPage.UsrID, "4");
                                    //MemberVIew.isUserUpdate = true;
                                }
                            }
                            if (calculateDistance(e.Rssi) > 5 && letpunchin == true)
                            {
                                Console.WriteLine("okout~ " + e.Name);
                                letpunchout = true;
                            }
                            //var insertlist = new ibeaconInfo
                            //{
                            //    id = Int32.Parse(MyDic[e.Name]),
                            //    distance = calculateDistance(e.Rssi),
                            //    time = DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss")
                            //};
                            //Console.WriteLine("Name : {0} Distance : {1} Time : {2}", Int32.Parse(MyDic[e.Name]), calculateDistance(e.Rssi), DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss"));
                            //ibeaconList.Add(insertlist);
                        }
                        
                        var beacon = new BeaconItem
                        {
                            Name = e.Name
                        };
                        beacons.Clear();
                        beacons.Add(beacon);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            });
        }
        private double calculateDistance(double rssi) // 算距離
        {
            var txPower = -59; //hard coded power value. Usually ranges between -59 to -65


            if (rssi == 0)
            {
                return -1.0;
            }

            var ratio = rssi * 1.0 / txPower;
            if (ratio < 1.0)
            { 
                return Math.Pow(ratio, 10);
            }
            else
            {
                var distance = (0.89976) * Math.Pow(ratio, 7.7095) + 0.111;
                return distance;
            }
        }

        //private void Messager()
        //{
        //    MessagingCenter.Subscribe<MemberView, bool>(this, "BEACON_SCAN", (sender, arg) =>
        //    {
        //        BeaconScan(Navigation);
        //    });
        //}
    }
}
using Deliver.Models;
using Deliver.Models.DataInfo;
using Deliver.Services;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using PULI.Models.DataInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace PULI.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentPage
    {
        IGeolocator location;
        Plugin.Geolocator.Abstractions.Position position;
        bool isSetView = false, isAlert = false;
        int location_DesiredAccuracy = 20, map_Zoom = 14;

        IEnumerable<ClientInfo> clientList = null;
        //public static List<ClientInfo> cList = new List<ClientInfo>();
        public static List<ClientInfo> cList2 = new List<ClientInfo>();
        //public static List<daily_shipment> totalList2 = new List<daily_shipment>();
        public static TotalList totalList = new TotalList();
        public static TotalList totalListforhelp = new TotalList();
        public static TotalList totalList2 = new TotalList();
        // public static daily_shipment totalListDone= new daily_shipment();
        public static int[] totalListTmp;
        public static int[] trylist;
        // public static daily_shipment shipList = new daily_shipment();
        public static List<AllClientInfo> allclientList = new List<AllClientInfo>(); // for auth = 6 社工
        public static List<AllClientInfo> help_client_list = new List<AllClientInfo>(); // for auth = 6 社工
        public static List<questionnaire> questionnaireslist = new List<questionnaire>();

        WebService web = new WebService();
        ParamInfo param = new ParamInfo();
        public static double lat = 0;
        public static double lot = 0;
        public string home;
        public string Address;
        public string gps;
        public string gps2;
        public string bday;
        public string phone;
        public string cellphone;
        public string gender;
        public static int n = 0;
        public static int m = 0;
        private static int daily_shipment_client_num;
        //bool punch_in = false;
        public static string setName = "";
        public static string setBirthday = "";
        public static string setAddress = "";
        public static string setMealName = "";
        public static string setMobilePhone = "";
        public static string setContact = "";
        public static string setOldLon = "";
        public static string setOldLat = "";
        public static string set_s_num = "";
        public static string set_beacon_s_num = "";
        string ct_s_num = "";
        string sec_s_num = "";
        string mlo_s_num = "";
        string setbeacon_s_num = "";
        string bn_s_num = "";
        public static string name;
        // Dictionary<string, bool> yesnoList2 = new Dictionary<string, bool>();
        public static string punchinmsg;
        public static string punchoutmsg;
        public static List<checkInfo> checkList2 = new List<checkInfo>();
        public static List<TempAccount> checkList3 = new List<TempAccount>();
        Entry entny = new Entry();
        public string Clname;
        public static int which;
        public string NAME;
        public static int FINAL;
        //public static bool isform = false;
        //static string[] punchList;
        public static string[] sortList;
        Dictionary<string, bool> punchList = new Dictionary<string, bool>(); // 判斷簽到+簽退都成功的
        Dictionary<string, bool> punch_in = new Dictionary<string, bool>(); // 判斷簽到成功的
       // Dictionary<string, bool> tmp_punch_in = new Dictionary<string, bool>(); // 
        //Dictionary<string, bool> tmp_punch_out = new Dictionary<string, bool>(); // 
        Dictionary<string, bool> punch_out = new Dictionary<string, bool>(); // 判斷簽退成功的
        Dictionary<string, bool> isform = new Dictionary<string, bool>(); // 判斷跳出問卷的
        Dictionary<string, bool> gomap = new Dictionary<string, bool>(); // 判斷導航的
        Dictionary<string, bool> punchyesorno = new Dictionary<string, bool>(); // 判斷是否進入判斷打卡(無論打卡成功與否)
        public static TempDatabase AccDatabase; // 紀錄問卷的
        public static PunchDatabase PunchDatabase; // 記錄無網路環境打卡的
        public static PunchDatabase2 PunchDatabase2; // 紀錄案主家打卡進度的(setnum)
        public static PunchDataBaseTmp PunchTmp;  // 紀錄無網路環境下，後來自動簽到成功的
        public static PunchDataBaseTmp2 PunchTmp2; // 紀錄無網路環境下，後來自動簽退成功的
        public static Wifi_Punchout_Database Wifi_Punchout_DB; // 紀錄有網路環境下，簽退成功紀錄
        public static Wifi_Punchin_Database Wifi_Punchin_DB; // 紀錄有網路環境下，簽到成功紀錄
        public static PunchYesOrNo PunchYN; // 紀錄是否進入判斷打卡(無論打卡成功與否)
       // public static string entrytxt;
        //public static int number;
        public static bool isSet = false;
        //public static int TmpID;
        //public static string TmpNum;
        //Dictionary<string, int> Temp = new Dictionary<string, int>();
        //public static int id;
        //public static bool ispress = false;
        //public static string homenum;
        //public static string homename;
        private double px;
        private double py;
        private double dx;
        private double dy;
        private double d;
        public static int num = 0;
        public static int setnum;
        public static double NowLon;
        public static double NowLat;
        public static string inorout; // 判斷SQLite為簽到還是簽退
        public static string gendertxt; // for社工地圖?!
        public static List<string> name_list_in = new List<string>(); // 紀錄處理無網路簽到成功
        //public static List<TmpPunchList> name_list_in2 = new List<TmpPunchList>();
        public static List<string> name_list_out = new List<string>(); // 記錄處理無網路簽退成功
        //public static List<TmpPunchList> name_list_out2 = new List<TmpPunchList>();
        //public static List<int> trylist2;
        //public static bool TmpPunch;
        public string btnGPS;
        public static int total_need_to_serve;
        public static List<string> total_reserve_name = new List<string>();
        public static bool DeliverOver = false; // for判斷是否顯示送餐完畢

        public MapView()
        {
            InitializeComponent();
            //Console.WriteLine("here~~11~");
            
            //Console.WriteLine("here~22~~");
            AccDatabase = new TempDatabase();
            PunchDatabase = new PunchDatabase();
            PunchDatabase2 = new PunchDatabase2();
            PunchTmp = new PunchDataBaseTmp();
            PunchTmp2 = new PunchDataBaseTmp2();
            Wifi_Punchin_DB = new Wifi_Punchin_Database();
            Wifi_Punchout_DB = new Wifi_Punchout_Database();
            PunchYN = new PunchYesOrNo();
            //trylist2 = new List<int>();
            //AccDatabase.DeleteAll();
            MapUiSetting();
            //setView(); 
            //if(MainPage.checkdate == true)
            //{
            //    //Console.WriteLine("Deletesetnum");
            //    PunchDatabase2.DeleteAll();
            //}
            //MessagingCenter.Subscribe<MainPage, bool>(this, "Deletesetnum", (sender, arg) =>
            //{
            //    // do something when the msg "UPDATE_BONUS" is recieved
            //    //Console.WriteLine("deletemap~~~");
            //    if (arg)
            //    {
            //        //Console.WriteLine("Deletesetnum~~mapview~~~");

            //    }
            //});
            //Console.WriteLine("GGG~~~" + MainPage.dateDatabase.GetAccountAsync2().Count());
           
            Messager();
            if(MainPage.AUTH == "4") // 外送員(有打卡功能)
            {
                Device.StartTimer(TimeSpan.FromSeconds(10), OnTimerTick);
              
                Console.WriteLine("shipment~~~");
            }
            //else if(MainPage.AUTH == "6" && MainPage.userList.daily_shipment_nums > 0) // 社工幫忙送餐(有打卡功能)
            //{
            //    Device.StartTimer(TimeSpan.FromSeconds(5), OnTimerTick);
            //    Console.WriteLine("helpermixshipment~~~");
            //}
            else
            {
                Device.StartTimer(TimeSpan.FromSeconds(5), OnTimerTick2); // 只有postgps(單純社工，無打卡功能)
            }
            //else // 單純社工(無打卡功能)
            //{
            //    Device.StartTimer(TimeSpan.FromSeconds(5), OnTimerTick2);
            //    //Console.WriteLine("helper");
            //}
            
          
        }

        private async void setView()
        {
            try
            {
                if (clientList == null)
                {
                   
                    questionnaireslist = await web.Get_Questionaire(MainPage.token); // 拿問卷
                    
                    
                   
                    //Console.WriteLine("TOKEN" +MainPage.token);
                   
                    if (MainPage.AUTH == "4" )
                    {

                        //Console.WriteLine("外送員~~~~");
                        MyMap.IsVisible = true;
                        MyMap.IsEnabled = true;
                        //Console.WriteLine("AUTH " + MainPage.AUTH);
                        Console.WriteLine("timemap~~~ " + MainPage._time);
                        if(MainPage._time == "早上") // 早上跟下午用不同api
                        {
                            totalList = await web.Get_Daily_Shipment(MainPage.token);
                        }
                        else
                        {
                            totalList = await web.Get_Daily_Shipment_night(MainPage.token);
                        }
                        Console.WriteLine("nnnn~~~ " + totalList.daily_shipments.Count());
                        
                      
                        clientList = await web.Get_Client(MainPage.token);
                        Console.WriteLine("DATA2~" + clientList.Count());
                        
                        var data = await web.Get_Client2(MainPage.token); // 拿案主資料
                        Console.WriteLine("DATA~" + data.Count());
                        for (int i = 0; i < data.Count; i++)
                        {

                            cList2.Add(data[i]); // 案主資料的list
                            
                            
                            //Console.WriteLine("dataname~~~" + data[i].ct_name);
                            //Console.WriteLine("www~~~" + total_reserve_name.Contains(data[i].ct_name));
                            if (!total_reserve_name.Contains(data[i].ct_name)) // 過濾一個案主有兩筆資料
                            {
                                //Console.WriteLine("add~~~");
                                total_reserve_name.Add(data[i].ct_name); // 過濾完的list
                            }
                            //Console.WriteLine("EEEE~~" + cList2[i].ct_name);
                            //cList2.Reverse();
                            ////Console.WriteLine("WWWW~~" + cList2[i].ct_name);


                        }
                        //Console.WriteLine("cList222222~~~ " + cList2.Count());
                        total_need_to_serve = total_reserve_name.Count(); // 過濾完兩筆訂單的名單後，所有要送餐案主家的數量
                        //Console.WriteLine("total_need_to_serve~~ " + total_reserve_name.Count());
                        //cList2.Reverse();
                        //for (int s = 0; s < cList2.Count(); s++)
                        //{
                            //Console.WriteLine("PPPP" + cList2[s].ct_name);
                        //}
                        //Console.WriteLine("SSSS" + cList2.Count);
                        //totalList = await web.Get_Daily_Shipment(MainPage.token);
                        Console.WriteLine("Data4~ " + totalList.daily_shipments.Count());
                        Console.WriteLine("QQQQ " + totalList.daily_shipments.Count);
                       
                        Console.WriteLine("DATA3~ " + clientList.Count());
                        //if (clientList != null)
                        //{
                        //    //Console.WriteLine("QAQin~~~");
                        //    foreach (var i in clientList)
                        //    {
                        //        cList.Add(i);
                        //       // //Console.WriteLine("QAQ" + cList.Count());
                        //        ////Console.WriteLine("QAQ2~~~" + cList[0].ClientName);
                        //        ////Console.WriteLine("QAQ3~~~" + cList[1].ClientName);
                        //    }
                        //}

                        MyMap.Pins.Clear(); 
                        for (int i = 0; i < totalList.daily_shipments.Count; i++)
                        {
                            //Console.WriteLine("loginway~~" + MainPage.Loginway);
                            // 輸入帳號登入
                            if (MainPage.Loginway == "Enter")
                            {
                                PunchDatabase.DeleteAll(); // 記錄無網路環境打卡的
                                PunchDatabase2.DeleteAll(); // 紀錄案主家打卡進度的(setnum)
                                PunchTmp.DeleteAll(); // 紀錄無網路環境下，後來自動簽到成功的
                                PunchTmp2.DeleteAll(); // 紀錄無網路環境下，後來自動簽退成功的
                                punchList[totalList.daily_shipments[i].ct_name] = false; // 判斷簽到+簽退都成功的
                                punch_in[totalList.daily_shipments[i].ct_name] = false; // 判斷簽到成功的
                                punch_out[totalList.daily_shipments[i].ct_name] = false; // 判斷簽退成功的
                                isform[totalList.daily_shipments[i].ct_name] = false; // 判斷跳出問卷的
                                gomap[totalList.daily_shipments[i].ct_name] = false; // 判斷導航的
                                punchyesorno[totalList.daily_shipments[i].ct_name] = false; // 判斷是否進入判斷打卡(無論打卡成功與否)
                                //setnum = totalList.daily_shipments.Count() - 1;
                                setnum = 0; // 送餐進度
                                //PunchTmp.SaveAccountAsync(new PunchTmp // 把簽退成功紀錄到無網路簽退的SQLite
                                //{
                                //    name = "lab測試_in_2", // 姓名
                                //    time = DateTime.Now.ToShortTimeString() // 時間
                                //});
                                
                                //DateTime myDate = DateTime.Now;
                                //string time = myDate.ToString("yyyy-MM-dd HH:mm:ss");
                                //Console.WriteLine("time~~~ " + time);
                                //PunchSaveToSQLite(MainPage.token, "lab測試_in_2", "in", "1620", "2313", "1", 23.9523305, 120.9280672, time, DateTime.Now.ToShortTimeString());
                                
                                //Console.WriteLine("setnumCC~~" + setnum);
                                //SetIcon(setnum); // 地圖上設案主標點(只存在下一家要送餐的)
                                double lat = Convert.ToDouble(totalList.daily_shipments[i].ct16);
                                Console.WriteLine("LAT" + lat);
                                double lot = Convert.ToDouble(totalList.daily_shipments[i].ct17);
                                Console.WriteLine("LOT" + lot);
                                home = totalList.daily_shipments[i].ct_name + " 的家";
                                ////Console.WriteLine("HOME" + home);
                                gps = lat + "," + lot;
                                //gender = allclientList[i].ct04; // 性別
                                //bday = allclientList[i].ct05; // 生日
                                //phone = allclientList[i].ct06_homephone; // 家裡電話
                                //cellphone = allclientList[i].ct06_telephone; // 手機

                                // 全部ICON都在map上
                                //PinMarker(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);
                                PinMarker3(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);
                                location = CrossGeolocator.Current;
                                location.DesiredAccuracy = location_DesiredAccuracy;
                                position = await location.GetPositionAsync(TimeSpan.FromSeconds(1));
                                NowLon = position.Longitude;
                                NowLat = position.Latitude;
                                //Console.WriteLine("nowlat" + position.Latitude);
                                //Console.WriteLine("nowlot" + position.Longitude);
                                //Console.WriteLine("NoewLon~~~" + NowLon);
                                //Console.WriteLine("NoewLat~~~" + NowLat);
                                CameraPosition cameraPosition = new CameraPosition(new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude), map_Zoom);
                                await MyMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
                            }
                            else
                            {
                                // 自動登入
                                //Console.WriteLine("Auto~~~");
                                if (MainPage.dateDatabase.GetAccountAsync2().Count() != 0) // 如果紀錄登入日期的SQLite裡面有資料，先比對
                                {
                                    Console.WriteLine("old~~ " + MainPage.oldday2);
                                    Console.WriteLine("new~~~ " + MainPage._login_time);
                                    // 判斷上次登入日期跟這次登入日期
                                    // 若不同則刪除SQLite裡面的資料
                                    if (MainPage._login_time != MainPage.oldday2)
                                    {

                                        Console.WriteLine("newdayrecieve~~Mapview~~22~");
                                        AccDatabase.DeleteAll();  // 紀錄問卷的
                                        PunchDatabase.DeleteAll(); // 記錄無網路環境打卡的
                                        PunchDatabase2.DeleteAll(); // 紀錄案主家打卡進度的(setnum)
                                        if(PunchDatabase2.GetAccountAsync2().Count() == 0)
                                        {
                                            Console.WriteLine("new_day~~no_setnum ");
                                        }
                                        PunchTmp.DeleteAll(); // 紀錄無網路環境下，後來自動簽到成功的
                                        PunchTmp2.DeleteAll(); // 紀錄無網路環境下，後來自動簽退成功的
                                        PunchYN.DeleteAll(); // 紀錄是否進入判斷打卡(無論打卡成功與否)
                                        Wifi_Punchin_DB.DeleteAll(); // 有網路簽到記錄
                                        Wifi_Punchout_DB.DeleteAll(); // 有網路簽退紀錄
                                        name_list_in.Clear(); // 紀錄處理無網路簽到成功
                                        name_list_out.Clear(); // 紀錄處理無網路簽退成功
                                        TestView.ChooseDB.DeleteAll(); 
                                        TestView.ResetDB.DeleteAll();
                                        TestView.EntryDB.DeleteAll();
                                        TestView.EntrytxtDB.DeleteAll();

                                        MainPage.checkdate = true;
                                        ////Console.WriteLine("howmany~" + MapView.PunchDatabase2.GetAccountAsync2().Count());
                                        MainPage.dateDatabase.DeleteAll(); // 讓裡面永遠只保持最新的一筆日期紀錄
                                        MainPage.dateDatabase.SaveAccountAsync(new CheckDate // 把新的登入日期紀錄進SQLite
                                        {
                                            date = MainPage._login_time
                                        });

                                    }
                                }
                                else // 裡面還沒有資料
                                {
                                    MainPage.dateDatabase.SaveAccountAsync( // 把新的登入日期紀錄進SQLite
                                    new CheckDate
                                    {
                                        date = MainPage._login_time
                                    });
                                    //Console.WriteLine("date_nodata_save~~");
                                }
                                punchList[totalList.daily_shipments[i].ct_name] = false;  // 判斷簽到+簽退都成功的
                                punch_in[totalList.daily_shipments[i].ct_name] = false;  // 判斷簽到成功的
                                punch_out[totalList.daily_shipments[i].ct_name] = false;  // 判斷簽退成功的
                                isform[totalList.daily_shipments[i].ct_name] = false; // 判斷跳出問卷的
                                gomap[totalList.daily_shipments[i].ct_name] = false; // 判斷導航的
                                punchyesorno[totalList.daily_shipments[i].ct_name] = false; // 判斷是否進入判斷打卡(無論打卡成功與否)
                                // tmp_punch_in[totalList.daily_shipments[i].ct_name] = false;
                                //tmp_punch_out[totalList.daily_shipments[i].ct_name] = false;
                                //foreach (var a in tmp_punch_in)
                                //{
                                //    //Console.WriteLine("AAAA~" + a);
                                //}
                                if (PunchDatabase.GetAccountAsync2().Count() > 0) // 無網路環境下打卡的database裡面有資料
                                {
                                    //Console.WriteLine("WWWW~~~~");
                                    //Console.WriteLine("pp~~" + PunchDatabase.GetAccountAsync2().Count());
                                    for (int b = 0; b < PunchDatabase.GetAccountAsync2().Count(); b++)
                                    {
                                        var c = PunchDatabase.GetAccountAsync(b);


                                        foreach (var TempAnsList in c)
                                        {
                                            //Console.WriteLine("tmpname1111~~~" + TempAnsList.name);
                                           
                                            if (TempAnsList.name == totalList.daily_shipments[i].ct_name)
                                            {
                                                punchList[totalList.daily_shipments[i].ct_name] = true;
                                                punch_in[totalList.daily_shipments[i].ct_name] = true;
                                                punch_out[totalList.daily_shipments[i].ct_name] = true;
                                            }
                                        }
                                        foreach (var a in punchList)
                                        {
                                            //Console.WriteLine("DDDD~" + a);
                                        }
                                        foreach (var a in punch_in)
                                        {
                                            //Console.WriteLine("DDDD~" + a);
                                        }
                                    }
                                }
                                if (Wifi_Punchin_DB.GetAccountAsync2().Count() > 0) // 無網路環境下打卡的database裡面有資料
                                {
                                    //Console.WriteLine("WWWW~~~~");
                                    //Console.WriteLine("pp~~" + PunchDatabase.GetAccountAsync2().Count());
                                    for (int b = 0; b < Wifi_Punchin_DB.GetAccountAsync2().Count(); b++)
                                    {
                                        var c = Wifi_Punchin_DB.GetAccountAsync(b);


                                        foreach (var TempNameList in c)
                                        {
                                            //Console.WriteLine("tmpname1111~~~" + TempAnsList.name);

                                            if (TempNameList.name == totalList.daily_shipments[i].ct_name)
                                            {
                                                //punchList[totalList.daily_shipments[i].ct_name] = true;
                                                punch_in[totalList.daily_shipments[i].ct_name] = true;
                                                //punch_out[totalList.daily_shipments[i].ct_name] = true;
                                            }
                                        }
                                        foreach (var a in punchList)
                                        {
                                            //Console.WriteLine("DDDD~" + a);
                                        }
                                        foreach (var a in punch_in)
                                        {
                                            //Console.WriteLine("DDDD~" + a);
                                        }
                                    }
                                }
                                if (Wifi_Punchout_DB.GetAccountAsync2().Count() > 0) // 無網路環境下打卡的database裡面有資料
                                {
                                    //Console.WriteLine("WWWW~~~~");
                                    //Console.WriteLine("pp~~" + PunchDatabase.GetAccountAsync2().Count());
                                    for (int b = 0; b < Wifi_Punchout_DB.GetAccountAsync2().Count(); b++)
                                    {
                                        var c = Wifi_Punchout_DB.GetAccountAsync(b);


                                        foreach (var TempNameList in c)
                                        {
                                            //Console.WriteLine("tmpname1111~~~" + TempAnsList.name);

                                            if (TempNameList.name == totalList.daily_shipments[i].ct_name)
                                            {
                                                //punchList[totalList.daily_shipments[i].ct_name] = true;
                                                //punch_in[totalList.daily_shipments[i].ct_name] = true;
                                                punch_out[totalList.daily_shipments[i].ct_name] = true;
                                            }
                                        }
                                        foreach (var a in punchList)
                                        {
                                            //Console.WriteLine("DDDD~" + a);
                                        }
                                        foreach (var a in punch_in)
                                        {
                                            //Console.WriteLine("DDDD~" + a);
                                        }
                                    }
                                }
                                if(punch_out[totalList.daily_shipments[i].ct_name] == true && punch_in[totalList.daily_shipments[i].ct_name] == true)
                                {
                                    punchList[totalList.daily_shipments[i].ct_name] = true;
                                }
                                double lat = Convert.ToDouble(totalList.daily_shipments[i].ct16);
                                ////Console.WriteLine("LAT" + lat);
                                double lot = Convert.ToDouble(totalList.daily_shipments[i].ct17);
                                ////Console.WriteLine("LOT" + lot);
                                home = totalList.daily_shipments[i].ct_name + " 的家";
                                ////Console.WriteLine("HOME" + home);
                                gps = lat + "," + lot;
                                //gender = allclientList[i].ct04; // 性別
                                //bday = allclientList[i].ct05; // 生日
                                //phone = allclientList[i].ct06_homephone; // 家裡電話
                                //cellphone = allclientList[i].ct06_telephone; // 手機

                                // 全部ICON都在map上
                                //PinMarker(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);
                                PinMarker3(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);
                                location = CrossGeolocator.Current;
                                location.DesiredAccuracy = location_DesiredAccuracy;
                                position = await location.GetPositionAsync(TimeSpan.FromSeconds(1));
                                NowLon = position.Longitude;
                                NowLat = position.Latitude;
                                //Console.WriteLine("nowlat" + position.Latitude);
                                //Console.WriteLine("nowlot" + position.Longitude);
                                //Console.WriteLine("NoewLon~~~" + NowLon);
                                //Console.WriteLine("NoewLat~~~" + NowLat);
                                CameraPosition cameraPosition = new CameraPosition(new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude), map_Zoom);
                                await MyMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
                                /*
                                if (PunchDatabase2.GetAccountAsync2().Count() == 0)  // 如果紀錄送餐進度的SQLite裡面沒有資料(無送餐進度)
                                {

                                    //setnum = totalList.daily_shipments.Count() - 1;
                                    setnum = 0;
                                    SetIcon(setnum);
                                    //Console.WriteLine("setnumA~~~~" + setnum);
                                }
                                else
                                {
                                    // 如果紀錄送餐進度的SQLite裡面有資料(有送餐進度)
                                    setnum = PunchDatabase2.GetAccountAsync2().Last().setnum + 1;
                                    Console.WriteLine("GG~~~~");
                                    Console.WriteLine("GGsetnum~~~ " + setnum);
                                    //if(setnum < 0)
                                    //{
                                    //    //await DisplayAlert(param.SYSYTEM_MESSAGE, "今日送餐完畢", param.DIALOG_MESSAGE);
                                    //    DeliverEnd.IsVisible = true;
                                    //    Dist.IsVisible = false;
                                    //}
                                    //else
                                    //{
                                    //    SetIcon(setnum);
                                    //}
                                    if (setnum > totalList.daily_shipments.Count() || setnum == totalList.daily_shipments.Count()) // 判斷是否送餐完畢
                                    {
                                        //await DisplayAlert(param.SYSYTEM_MESSAGE, "今日送餐完畢", param.DIALOG_MESSAGE);
                                        Console.WriteLine("setnumAAAA~~~ " + setnum);
                                        Console.WriteLine("shipmentcount~~~~ " + totalList.daily_shipments.Count());
                                        DeliverEnd.IsVisible = true;
                                        Dist.IsVisible = false;
                                    }
                                    else
                                    {
                                        SetIcon(setnum); // 把案主家的圖標顯示在地圖上
                                    }
                                    //Console.WriteLine("setnumB~~~~" + setnum);
                                }
                                */
                            }
                            //Console.WriteLine("setnum1111~~~~" + setnum);
                            //PinMarker(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);

                         
                        }



                    }
                    else // 單純社工(不用打卡)
                    {
                        Console.WriteLine("社工~~~~");
                        //Console.WriteLine("AUTH" + MainPage.AUTH);
                        allclientList = await web.Get_All_Client(MainPage.token);
                       // //Console.WriteLine("ALLCLN~~~" + allclientList[0]);
                      
                        MyMap.Pins.Clear();
                        MyMap.IsEnabled = true;
                        MyMap.IsVisible = true;
                        //DeliverMap.IsVisible = true;
                        //DeliverMap.IsEnabled = true;
                        //buttonhelp.IsVisible = true;
                        //buttonhelp.IsEnabled = true;

                        Dist.IsVisible = false;
                        Dist.IsEnabled = false;
                        ////Console.WriteLine("countAA" + allclientList.Count);
                        for (int i = 0; i < allclientList.Count; i++) // 單純社工身分
                        {
                            ////Console.WriteLine("count" + allclientList.Count);
                            double lat = Convert.ToDouble(allclientList[i].ct16);
                            ////Console.WriteLine("LAT" + lat);
                            double lot = Convert.ToDouble(allclientList[i].ct17);
                            ////Console.WriteLine("LOT" + lot);
                            home = allclientList[i].ct01 + allclientList[i].ct02 + " 的家";
                            ////Console.WriteLine("HOME" + home);
                            gps = lat + "," + lot;
                            gender = allclientList[i].ct04; // 性別
                            bday = allclientList[i].ct05; // 生日
                            phone = allclientList[i].ct06_homephone; // 家裡電話
                            cellphone = allclientList[i].ct06_telephone; // 手機
                            //Console.WriteLine("gender~~" + gender);
                            //Console.WriteLine("bday~~" + bday);
                            //Console.WriteLine("phone~~" + phone);
                            //Console.WriteLine("cellphone~~~" + cellphone);
                            //for (int j = 0; j < totalListforhelp.daily_shipments.Count(); j++)
                            //{
                            //    if (totalListforhelp.daily_shipments[j].ct_name == allclientList[i].ct01 + allclientList[i].ct02)
                            //    {
                            //        gender = allclientList[i].ct04; // 性別
                            //        bday = allclientList[i].ct05; // 生日
                            //        phone = allclientList[i].ct06; // 電話
                            //    }
                            //    //Console.WriteLine("gender~~" + gender);
                            //    //Console.WriteLine("bday~~" + bday);
                            //    //Console.WriteLine("phone~~" + phone);

                            //}

                            // 全部ICON都在map上
                            //PinMarker(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);
                            PinMarker2(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps, gender, bday, phone, cellphone);
                          
                            //punchList[allclientList[i].ct01 + allclientList[i].ct02] = false;
                            //punch_in[allclientList[i].ct01 + allclientList[i].ct02] = false;
                            //punch_out[allclientList[i].ct01 + allclientList[i].ct02] = false;
                            //isform[allclientList[i].ct01 + allclientList[i].ct02] = false;
                            //gomap[allclientList[i].ct01 + allclientList[i].ct02] = false;
                        }
                        location = CrossGeolocator.Current;
                        location.DesiredAccuracy = location_DesiredAccuracy;
                        position = await location.GetPositionAsync(TimeSpan.FromSeconds(1));
                        NowLon = position.Longitude;
                        NowLat = position.Latitude;
                        //Console.WriteLine("nowlat" + position.Latitude);
                        //Console.WriteLine("nowlot" + position.Longitude);
                        //Console.WriteLine("NoewLon~~~" + NowLon);
                        //Console.WriteLine("NoewLat~~~" + NowLat);
                        CameraPosition cameraPosition = new CameraPosition(new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude), map_Zoom);
                        await MyMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
                    }

                    //var totalListDone = await web.Get_Daily_Shipment2(MainPage.token);
                    ////Console.WriteLine("DONE~ " + totalListDone.Count());
                    //for(int i = 0; i < totalListDone.Count; i++)
                    //{
                    //    //Console.WriteLine("INLA~~~");
                    //    totalList2.Add(totalListDone[i]);
                    //    //Console.WriteLine("ghjk~ " + totalListDone[i].dys06);
                    //}
                    ////Console.WriteLine("COUNT~~" + totalList.daily_shipments.Count());
                    //for (int i = 0; i < totalList.daily_shipments.Count; i++)
                    //{

                    //    totalList2.Add(totalList.daily_shipments[i]);
                    //    //Console.WriteLine("WWWW" + cList2[i].ct_name);
                    //    //Console.WriteLine("SSSS" + cList2.Count);

                    //}
                    ////Console.WriteLine("Count " + totalList2.Count());
                    //totalList2.Sort();

                    //for(int a = 0; a < totalList2.Count(); a++)
                    //{
                    //    //Console.WriteLine("name@@ " + totalList2[a].ct_name);
                    //    //Console.WriteLine("dys6@@ " + totalList2[a].dys06);
                    //}

                    //IEnumerable<daily_shipment> sortAscendingQuery =
                    //    from dys6 in totalList2
                    //    orderby dys6 //"ascending" is default
                    //    select dys6;
                    //foreach (var dys6  in sortAscendingQuery)
                    //    //Console.WriteLine("dys~~~" + dys6);
                    //shipList = await web.Get_Daily_Shipment2(MainPage.token);
                   // //Console.WriteLine("QQQQ" + totalList.daily_shipments.Count);
                    ////Console.WriteLine("AAAA" + shipList.ct_name);

                    // for社工
                    
                    


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MAPA");
                Console.WriteLine(ex.ToString());
            }

        }

        public async void SetIcon(int setnum) // for送餐地圖，地圖上只顯示下一家要送餐的案主
        {
            //Console.WriteLine("AUTH " + MainPage.AUTH);
            //Console.WriteLine("countBB " + totalList.daily_shipments.Count);

            //Console.WriteLine("count" + totalList.daily_shipments.Count);
            //Console.WriteLine("SETNUM~~~~" + setnum);
            double lat = double.Parse(totalList.daily_shipments[setnum].ct16); // 案主家經緯度
            //Console.WriteLine("LAT" + lat);
            double lot = double.Parse(totalList.daily_shipments[setnum].ct17); // 案主家經緯度
            //Console.WriteLine("LOT" + lot);
            home = totalList.daily_shipments[setnum].ct_name + " 的家"; // 要出現的字
            //Console.WriteLine("HOME" + home);
            gps = lat + "," + lot;
            //Console.WriteLine("GPS" + gps);
            //Address = allclientList[i].ClientAddress;
            //Console.WriteLine("Address" + Address);
            Console.WriteLine("NAMEEEE~~" + totalList.daily_shipments[setnum].ct_name);

            MyMap.Pins.Clear(); // 要加下一個點之前先把之前的點清掉
            PinMarker3(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);

        }
        public async void SetIcon3(int setnum) 
        {
            //Console.WriteLine("AUTH " + MainPage.AUTH);
            //Console.WriteLine("countBB " + totalList.daily_shipments.Count);

            //Console.WriteLine("count" + totalList.daily_shipments.Count);
            //Console.WriteLine("SETNUM~~~~" + setnum);
            double lat = double.Parse(totalList.daily_shipments[setnum].ct16);
            //Console.WriteLine("LAT" + lat);
            double lot = double.Parse(totalList.daily_shipments[setnum].ct17);
            //Console.WriteLine("LOT" + lot);
            home = totalList.daily_shipments[setnum].ct_name + " 的家";
            //Console.WriteLine("HOME" + home);
            gps = lat + "," + lot;
            //Console.WriteLine("GPS" + gps);
            //Address = allclientList[i].ClientAddress;
            //Console.WriteLine("Address" + Address);
            //Console.WriteLine("NAMEEEE~~" + totalList.daily_shipments[setnum].ct_name);

            DeliverMap.Pins.Clear();
            PinMarker(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);

        }

        //public async void SetIcon2(int setnum)
        //{
        //    //Console.WriteLine("seticon2~~~~");
        //    ////Console.WriteLine("count" + allclientList.Count);
        //    double lat = Convert.ToDouble(totalListforhelp.daily_shipments[setnum].ct16);
        //    ////Console.WriteLine("LAT" + lat);
        //    double lot = Convert.ToDouble(totalListforhelp.daily_shipments[setnum].ct17);
        //    ////Console.WriteLine("LOT" + lot);
        //    home = totalListforhelp.daily_shipments[setnum].ct_name + " 的家";
        //    ////Console.WriteLine("HOME" + home);
        //    gps = lat + "," + lot;
           
        //    ////Console.WriteLine("GPS" + gps);
        //    //Address = allclientList[i].ClientAddress;
        //    ////Console.WriteLine("Address" + Address);
        //    DeliverMap.Pins.Clear();
        //    PinMarker(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);
        //   // PinMarker2(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps, gender, bday, phone);
            
        //}
        private async void buttonhelp_Clicked(object sender, EventArgs e) // 社工身分的社工地圖(有全部案主家)
        {
            MyMap.IsVisible = true;
            MyMap.IsEnabled = true;
            DeliverMap.IsVisible = false;
            DeliverMap.IsEnabled = false;
        }

        // 典籍已預約資訊button
        private async void buttondeliver_Clicked(object sender, EventArgs e) // 社工身分的送餐地圖(幫忙送餐)
        {
            if(MainPage.userList.daily_shipment_nums > 0)
            {
                //Console.WriteLine("delivermap~true~~");
                DeliverMap.IsVisible = true;
                DeliverMap.IsEnabled = true;
            }
            MyMap.IsVisible = false;
            MyMap.IsEnabled = false;
            InfoWindow.IsVisible = false;
        }

        private void Button_OnPressed(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackgroundColor = Color.FromHex("f1ab86");
                btn.TextColor = Color.White;
                buttondeliver.BackgroundColor = Color.White;
                buttondeliver.TextColor = Color.FromHex("f1ab86");
            }

        }
        private void Button2_OnPressed(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackgroundColor = Color.FromHex("f1ab86");
                btn.TextColor = Color.White;
                buttonhelp.BackgroundColor = Color.White;
                buttonhelp.TextColor = Color.FromHex("f1ab86");
            }
        }

        private BitmapDescriptor ResourceToBitmap(string resource)
        {
            ////Console.WriteLine("BITIN");
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            //string resource = "AirPmc.png.map_bicycle.png";
            Stream stream = assembly.GetManifestResourceStream(resource);
            ////Console.WriteLine("BITIN2");
            BitmapDescriptor bitmap = BitmapDescriptorFactory.FromStream(stream);
            ////Console.WriteLine("BITIN3");
            return bitmap;

        }

        private async Task getLocation2()
        {
            location = CrossGeolocator.Current;
            if (location != null)
            {
                location.DesiredAccuracy = location_DesiredAccuracy;
                position = await location.GetPositionAsync(TimeSpan.FromSeconds(5));
                NowLon = position.Longitude;
                NowLat = position.Latitude;

                //Console.WriteLine("nowlat" + position.Latitude);
                //Console.WriteLine("nowlot" + position.Longitude);
                //Console.WriteLine("NoewLon~~~" + NowLon);
                //Console.WriteLine("NoewLat~~~" + NowLat);
                CameraPosition cameraPosition = new CameraPosition(new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude), map_Zoom);
                await MyMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition)); // 地圖上抓取目前位置
                await DeliverMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
            }

        }

        private async Task getLocation()
        {
            try
            {
                //var current = Connectivity.NetworkAccess;
                Console.WriteLine("LOCATION~~~~");
                //Console.WriteLine("INTERNET~~~~" + CrossConnectivity.Current.IsConnected);
                //Console.WriteLine("setnum3333~~~~" + setnum);
                //Console.WriteLine("tmpcount~~" + PunchTmp.GetAccountAsync2().Count());
                //Console.WriteLine("tmpcount2~~" + PunchTmp2.GetAccountAsync2().Count());

                // 判斷有無網路打卡紀錄，有則更新至memberview的listview
                if (PunchTmp.GetAccountAsync().Count() > 0) // 無網路簽到記錄
                {
                    MessagingCenter.Send(this, "Setlist", true);
                    //Console.WriteLine("sendsetlist222~~~");
                }
                if (PunchTmp2.GetAccountAsync().Count() > 0) // 無網路簽退紀錄
                {
                    MessagingCenter.Send(this, "Setlist2", true);
                    //Console.WriteLine("sendsetlist333~~~");
                }
                if (Wifi_Punchin_DB.GetAccountAsync3().Count() > 0) // 有網路簽到記錄
                {
                    MessagingCenter.Send(this, "wifi_Setlist_in", true);
                    //Console.WriteLine("sendsetlist222~~~");
                }
                if (Wifi_Punchout_DB.GetAccountAsync3().Count() > 0) // 有網路簽退紀錄
                {
                    MessagingCenter.Send(this, "wifi_Setlist_out", true);
                    //Console.WriteLine("sendsetlist333~~~");
                }
                //if(setnum == 0 && punchList[totalList.daily_shipments[setnum].ct_name] == true) // 已經送餐完畢
                //{

                //}
                //if (!isSetView) // 還沒setview
                //{
                location = CrossGeolocator.Current;
                Console.WriteLine("location~~ " + location);
                if (location != null)
                {
                    Console.WriteLine("location_in~~~ ");
                    try
                    {
                        d = 0;
                        location.DesiredAccuracy = location_DesiredAccuracy;
                        position = await location.GetPositionAsync(TimeSpan.FromSeconds(10));
                        NowLon = position.Longitude;
                        NowLat = position.Latitude;

                        Console.WriteLine("nowlat" + position.Latitude);
                        Console.WriteLine("nowlot" + position.Longitude);
                        //Console.WriteLine("NoewLon~~~" + NowLon);
                        //Console.WriteLine("NoewLat~~~" + NowLat);
                        CameraPosition cameraPosition = new CameraPosition(new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude), map_Zoom);
                        await MyMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition)); // 地圖上抓取目前位置
                        await DeliverMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
                        //latitude.Text = "Lat" + position.Latitude.ToString();
                        // //Console.WriteLine("@@LAT@@ " + latitude.Text);
                        //longitude.Text = "Lon" + position.Longitude.ToString();
                        ////Console.WriteLine("@@LON@@ " + longitude.Text);
                        //Console.WriteLine("punchdatabase~~~" + PunchDatabase.GetAccountAsync2().Count());
                        //Console.WriteLine("punchdatabase22~~~" + PunchDatabase2.GetAccountAsync2().Count());
                        //isSetView = true;
                        ////Console.WriteLine("CC" + cList.Count);
                        ////Console.WriteLine("CC2" + cList2.Count);
                        // for無網路環境(不會及時跳出打卡成功訊息)
                        // 偵測到網路
                        // 先判斷SQLite有無資料
                        // if有資料判斷將其自動打卡
                        // 全都打卡完之後將SQLite delete all
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            if (PunchDatabase.GetAccountAsync2().Count() > 0) // 記錄無網路環境打卡的database裡面有資料
                            {
                                Console.WriteLine("RRRRRR~~~~");
                                Console.WriteLine("pp~~" + PunchDatabase.GetAccountAsync2().Count());
                                for (int b = 0; b < PunchDatabase.GetAccountAsync2().Count(); b++)
                                {
                                    var c = PunchDatabase.GetAccountAsync(b);


                                    foreach (var TempAnsList in c)
                                    {
                                        Console.WriteLine("tmpname2222~~~" + TempAnsList.name);
                                        //Console.WriteLine("token~~" + TempAnsList.token);
                                        //Console.WriteLine("name~" + TempAnsList.name);
                                        //Console.WriteLine("ct_s_num~~" + TempAnsList.ct_s_num);
                                        //Console.WriteLine("sec_s_num~~" + TempAnsList.sec_s_num);
                                        //Console.WriteLine("mlo_s_num~~" + TempAnsList.mlo_s_num);
                                        //Console.WriteLine("bn_s_num~~" + TempAnsList.bn_s_num);
                                        //Console.WriteLine("lat~~" + TempAnsList.latitude);
                                        //Console.WriteLine("lon~~" + TempAnsList.longitude);
                                            
                                        if (TempAnsList.inorout == "in") // 處理簽到
                                        {
                                            //Console.WriteLine("Tmpname~~~" + TempAnsList.name);
                                            //for(int i = 0; i < tmp_punch_in.Count(); i++)
                                            //{
                                            //    //Console.WriteLine("tmp_pun_in~~" + tmp_punch_in);
                                            //}
                                            //Console.WriteLine("count~~in~" + name_list_in.Count());
                                            //if (name_list_in.Count() != total_need_to_serve)
                                            //{

                                            //}
                                            //else
                                            //{
                                            //    PunchTmp.DeleteAll();
                                            //    MessagingCenter.Send(this, "Setlist", true);
                                            //}
                                            //Console.WriteLine("nameLA~~in~" + TempAnsList, name);
                                            if (TempAnsList.name != null)
                                            {
                                                if (!name_list_in.Contains(TempAnsList.name)) // 判斷還沒處理過這筆無網路打卡
                                                {
                                                    // 自動簽到
                                                    bool web_res2 = await web.Save_Punch_In(TempAnsList.token, TempAnsList.ct_s_num, TempAnsList.sec_s_num, TempAnsList.mlo_s_num, TempAnsList.latitude, TempAnsList.longitude, TempAnsList.time);
                                                    Console.WriteLine("web_resin~~~ " + web_res2);
                                                    if (web_res2 == true)
                                                    {
                                                        // 打卡成功
                                                        name_list_in.Add(TempAnsList.name);
                                                        
                                                        //Console.WriteLine("name_list_in~~~" + name_list_in.Count);
                                                        //name_list_in2.Add(new TmpPunchList
                                                        //{
                                                        //   name = TempAnsList.name
                                                        //});


                                                        //Console.WriteLine("TmpInAdd~~~");
                                                        // //Console.WriteLine("name~~~" + name_list_in2.ElementAt(0));
                                                        //Console.WriteLine("name_in~~" + name_list_in.Count());
                                                        //  tmp_punch_in[TempAnsList.name] = true; // 簽到成功
                                                        // //Console.WriteLine("SQLitepunchin~~~" + tmp_punch_in[TempAnsList.name] + "name " + TempAnsList.name);
                                                        PunchDatabase.DeleteItem(TempAnsList.ID); // 把那筆刪掉
                                                                                                    //formin.IsVisible = true;
                                                                                                    //formin.IsEnabled = true;
                                                                                                    //await Task.Delay(10000); // 等待30秒
                                                                                                    //Messager2();
                                                        PunchTmp.DeleteItem(TempAnsList.ID); // 把那筆刪掉

                                                        MessagingCenter.Send(this, "Setlist", true); // 更新主頁面的吳網路打卡紀錄
                                                                                                     //Console.WriteLine("deletein~~~" + TempAnsList.name);
                                                                                                     //Console.WriteLine("incount111~~~" + PunchDatabase.GetAccountAsync2().Count());
                                                        Wifi_Punchin_DB.SaveAccountAsync(new Wifi_Punchin // 存進有網路簽到成功的SQLite
                                                        {
                                                            name = TempAnsList.name, // 案主姓名
                                                            time = TempAnsList.timeforpost  // 簽到時間
                                                        });
                                                    }
                                                    else
                                                    {
                                                        //await DisplayAlert("FAIL", "打卡失敗in" + setName, "OK");
                                                        //Console.WriteLine("ASQLite簽到失敗");
                                                    }
                                                }
                                                else
                                                {
                                                    // 已經處理過的話就直接刪除SQLite中這筆紀錄
                                                    PunchDatabase.DeleteItem(TempAnsList.ID);
                                                    PunchTmp.DeleteItem(TempAnsList.ID);
                                                }
                                            }
                                               
                                        }
                                        else // 處理簽退
                                        {
                                            //Console.WriteLine("name_list_out~~~" + name_list_out.Count());
                                            //if (name_list_out.Count() != total_need_to_serve)
                                            //{

                                            //}
                                            //else
                                            //{
                                            //    PunchTmp2.DeleteAll();
                                            //    MessagingCenter.Send(this, "Setlist2", true);
                                            //}
                                            //Console.WriteLine("nameLA~~out~" + TempAnsList, name);
                                            if(TempAnsList.name != null)
                                            {
                                                if (!name_list_out.Contains(TempAnsList.name)) // 還沒處理過這筆案主的簽退
                                                {
                                                    // 自動簽退
                                                    bool web_res2 = await web.Save_Punch_Out(TempAnsList.token, TempAnsList.ct_s_num, TempAnsList.sec_s_num, TempAnsList.mlo_s_num, TempAnsList.latitude, TempAnsList.longitude, TempAnsList.time);
                                                    Console.WriteLine("web_res_out~~~ " + web_res2);
                                                    if (web_res2 == true)
                                                    {
                                                        // 打卡成功
                                                        name_list_out.Add(TempAnsList.name);
                                                        
                                                        //Console.WriteLine("name_list_out~~~" + name_list_out.Count);
                                                        //name_list_out2.Add(new TmpPunchList
                                                        //{
                                                        //    name = TempAnsList.name
                                                        //});


                                                        //Console.WriteLine("TmpOutAdd~~~");
                                                        //Console.WriteLine("name_out~~" + name_list_out.Count());
                                                        //  tmp_punch_out[TempAnsList.name] = true; // 簽到成功
                                                        // //Console.WriteLine("SQLitepunchout~~~" + tmp_punch_in[TempAnsList.name] + "name " + TempAnsList.name);
                                                        PunchDatabase.DeleteItem(TempAnsList.ID); // 把那筆刪掉
                                                                                                    //formin.IsVisible = true;
                                                                                                    //formin.IsEnabled = true;
                                                                                                    //await Task.Delay(10000); // 等待30秒
                                                                                                    //Messager2();

                                                        PunchTmp2.DeleteItem(TempAnsList.ID);
                                                        
                                                        MessagingCenter.Send(this, "Setlist2", true); // 更新主頁面的無網路簽退紀錄
                                                        //Console.WriteLine("deleteout~~~" + TempAnsList.name);
                                                        //Console.WriteLine("outcount111~~~" + PunchDatabase.GetAccountAsync2().Count());
                                                        Wifi_Punchout_DB.SaveAccountAsync(new Wifi_Punchout// 把簽退成功紀錄到無網路簽退的SQLite
                                                        {
                                                            name = TempAnsList.name, // 姓名
                                                            time = TempAnsList.timeforpost // 時間
                                                        });

                                                    }
                                                    else
                                                    {
                                                        //await DisplayAlert("FAIL", "打卡失敗in" + setName, "OK");
                                                        //Console.WriteLine("ASQLite簽退失敗");
                                                    }
                                                }
                                                else
                                                {
                                                    // 已經處理過這筆簽退，直接刪除這筆紀錄
                                                    PunchDatabase.DeleteItem(TempAnsList.ID);
                                                    PunchTmp2.DeleteItem(TempAnsList.ID);
                                                }
                                            }
                                               
                                        }
                                    }
                                }
                                //Console.WriteLine("number~~ " + PunchDatabase.GetAccountAsync2().Count());
                                if (PunchDatabase.GetAccountAsync2().Count() == 0) // 判斷是否還有未處理的無網路打卡
                                {
                                    //Console.WriteLine("punchtmpSUCESS");
                                    // 全部刪除，且更新主頁面上的紀錄
                                    PunchTmp.DeleteAll();
                                    PunchTmp2.DeleteAll();
                                    MessagingCenter.Send(this, "Setlist", true);
                                    //Console.WriteLine("sendsetlist~~~");
                                    MessagingCenter.Send(this, "Setlist2", true);
                                    //Console.WriteLine("sendsetlist22~~~");

                                }
                                //if(name_list_in.Count() == total_need_to_serve && name_list_out.Count() == total_need_to_serve) // 判斷是否送餐完畢
                                //{
                                //    DeliverOver = true;
                                //}

                                //if(PunchTmp2.GetAccountAsync().Count() == 0)
                                //{
                                //    //Console.WriteLine("punchtmp2SUCESS");
                                //    PunchTmp2.DeleteAll();
                                //    MessagingCenter.Send(this, "Setlist2", true);
                                //    //Console.WriteLine("sendsetlist2~~~");
                                //}
                                //PunchDatabase.DeleteAll();
                            }
                               
                        }
                        //if (setnum > 0 || setnum == 0)
                        //Console.WriteLine("setnum~~~~" + setnum);
                    //Console.WriteLine("totoal_need_to_serve~~~ " + total_need_to_serve);
                        //if(setnum == 0 || total_need_to_serve > setnum || total_need_to_serve == setnum)
                        //{
                            //Console.WriteLine("setnum~~in~~~");
                        //Console.WriteLine("deliver_over~~ " + DeliverOver);
                            //if (DeliverOver == false)
                            //{
                            //Console.WriteLine("deliver_in~~~ ");
                            //Console.WriteLine("cList2~~~ " + cList2.Count());
                                for (int i = 0; i < cList2.Count(); i++)
                                {
                                    //if (homename == cList2[i].ct_name)
                                    //{

                                    int which = 0;
                                        
                                    //Console.WriteLine("who1" + cList2[i].ct_name);
                                    ////Console.WriteLine("punch1" + punchList[cList[i].ct_name]);
                                    //Console.WriteLine("whoami~~~" + setnum);
                                    // 算目前使用者位置跟案主家的距離
                                    /*
                                         
                                        */
                                    //px = double.Parse(totalList.daily_shipments[setnum].ct16);
                                    px = double.Parse(cList2[i].ct16);
                                    //py = double.Parse(totalList.daily_shipments[setnum].ct17);
                                    py = double.Parse(cList2[i].ct17);
                                    dx = position.Latitude - px > 0 ? position.Latitude - px : px - position.Latitude;
                                    dy = position.Longitude - py > 0 ? position.Longitude - py : py - position.Longitude;
                                    d = Math.Sqrt(dx * 110000 * dx * 110000 + dy * 100000 * dy * 100000);
                                    //Console.WriteLine("d2" + d);
                                    string d2 = d.ToString();
                                    //Console.WriteLine("@@@@@   " + d2);
                                    distance.Text = d2;
                                    Latitude.Text = position.Latitude.ToString();
                                    Longitude.Text = position.Longitude.ToString();
                                //Console.WriteLine("lat~~ " + position.Latitude.ToString());
                                //Console.WriteLine("lot~~~ " + position.Longitude.ToString());
                                    //foreach (var a in punchList)
                                    //{
                                    //    //Console.WriteLine("*****" + a);
                                    //}
                                    ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~///
                                    //if(gomap[totalList.daily_shipments[0].ct_name] == false) // 沒導到googlemap過
                                    //{
                                    //    if (i == 0) // 第一個案主家
                                    //    {
                                    //        gps2 = totalList.daily_shipments[0].ClientLatitude + "," + totalList.daily_shipments[0].ClientLongitude;
                                    //        string uri = "https://www.google.com.tw/maps/place/" + gps2;
                                    //        //Console.WriteLine("URI" + uri);
                                    //        if (await Launcher.CanOpenAsync(uri))
                                    //        {
                                    //            await Launcher.OpenAsync(uri);
                                    //            gomap[totalList.daily_shipments[0].ct_name] = true;
                                    //        }
                                    //        else
                                    //        {
                                    //            await DisplayAlert(param.SYSYTEM_MESSAGE, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                                    //        }
                                    //    }
                                    //}
                                    ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~///
                                    // //Console.WriteLine("setnum4444~~~~" + setnum);
                                    ////Console.WriteLine("WHO~~~~" + totalList.daily_shipments[setnum].ct_name);
                                    ////Console.WriteLine("~~~~" + punchList[totalList.daily_shipments[setnum].ct_name]);

                                    //Console.WriteLine("WHOLAA~~" + totalList.daily_shipments[setnum].ct_name + punchList[totalList.daily_shipments[setnum].ct_name]);
                                    if (punchList[cList2[i].ct_name] == false) // 先判斷有沒有打卡(簽到+簽退)過
                                    {
                                    //Console.WriteLine("name~~~ " + cList2[i].ct_name);
                                    //Console.WriteLine("mlo~~~ " + cList2[i].mlo_s_num); 
                                    //Console.WriteLine("who2>>>>>" + totalList.daily_shipments[setnum].ct_name);
                                        //Console.WriteLine("punch2>>>>" + punchList[totalList.daily_shipments[setnum].ct_name]);
                                        //Console.WriteLine("ddddistance~~" + d);

                                        // GPS 簽到
                                        if (d < 30 && punch_in[cList2[i].ct_name] == false) // 符合簽到距離且尚未簽到過
                                        {
                                            punchyesorno[cList2[i].ct_name] = true;
                                            //Console.WriteLine("who3" + totalList.daily_shipments[setnum].ct_name);
                                            //Console.WriteLine("punch3" + punchList[totalList.daily_shipments[setnum].ct_name]);

                                            //Console.WriteLine("~~~~~~~" + which);
                                            //for (int a = 0; a < cList2.Count(); a++)
                                            //{
                                                //Console.WriteLine("in~~~");
                                                //Console.WriteLine("cListname~~" + cList2[a].ct_name);
                                                ////Console.WriteLine("totalname~~" + totalList.daily_shipments[setnum].ct_name);
                                                //if (cList2[i].ct_name == totalList.daily_shipments[setnum].ct_name)
                                                //{
                                                    which = i;
                                                    //setName = cList2[i].ct_name;
                                                    // 抓取案主資料
                                                    setname.Text = cList2[i].ct_name;
                                                    for (int a = 0; a < totalList.daily_shipments.Count(); a++)
                                                    {
                                                        if(cList2[i].ct_name == totalList.daily_shipments[a].ct_name)
                                                        {
                                                            daily_shipment_client_num = a;
                                                            setname3.Text = totalList.daily_shipments[a].ct_name;
                                                            dys05_type.Text = totalList.daily_shipments[a].dys05_type;
                                                            //Console.WriteLine("dys05~~~ " + dys05_type.Text);
                                                            sec06.Text = totalList.daily_shipments[a].sec06;
                                                            ct06_telephone.Text = totalList.daily_shipments[a].ct06_telephone;
                                                            dys03.Text = totalList.daily_shipments[a].dys03;
                                                            dys02.Text = totalList.daily_shipments[a].dys02;
                                                        }
                                                    }
                                                        
                                                    //Console.WriteLine("name1~~" + setname.Text);
                                                    Clname = cList2[i].ct_name;
                                                    //Console.WriteLine("name1~~~" + Clname);
                                                    ////Console.WriteLine("HOME5" + cList[5].ct_name);
                                                    //string both = questionnaireslist[i].both;
                                                    //string both2 = questionnaireslist[i].both2;
                                                    setname2.Text = cList2[i].ct_name;
                                                    //Console.WriteLine("name2" + setname2.Text);
                                                    ////Console.WriteLine("setName" + Clname);
                                                    //setBirthday = totalList.daily_shipments[i].ClientBirthday;
                                                    ////Console.WriteLine("setBirthday" + setBirthday);
                                                    //setAddress = totalList.daily_shipments[i].ClientAddress;
                                                    //setMealName = totalList.daily_shipments[i].MealName;
                                                    //setclnt_s_num = cList[i].clnt_s_num;
                                                    ct_s_num = cList2[i].ct_s_num;
                                                    //Console.WriteLine("setclnt_s_num>>>>" + ct_s_num);
                                                    //setsrvc_s_num = cList[i].srvc_s_num;
                                                    sec_s_num = cList2[i].sec_s_num;
                                                    //Console.WriteLine("setsrvc_s_num" + sec_s_num);
                                                    //setmealo_s_num = cList[i].mealo_s_num;
                                                    mlo_s_num = cList2[i].mlo_s_num;  // 訂單s_num(
                                                    //Console.WriteLine("setmealo_s_num" + mlo_s_num);
                                                    //setbeacon_s_num = cList[i].beacon_s_num;
                                                    bn_s_num = cList2[i].bn_s_num; //  打卡鄰近的beancon_s_num(beacon id)
                                                    //Console.WriteLine("setbeacon_s_num" + setbeacon_s_num);
                                                    //Console.WriteLine("Lat" + position.Latitude);
                                                    //Console.WriteLine("Lot" + position.Longitude);
                                                    //Console.WriteLine("TOKEN" + MainPage.token);

                                                    if (CrossConnectivity.Current.IsConnected) // 有連到網路
                                                    {
                                        // 自動簽到
                                                        DateTime myDate = DateTime.Now;
                                                        string time = myDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                        Console.WriteLine("time~~~ " + time);
                                                        bool web_res = await web.Save_Punch_In(MainPage.token, ct_s_num, sec_s_num, mlo_s_num, position.Latitude, position.Longitude, time);
                                                        //Console.WriteLine("web_res" + web_res);
                                                        if (web_res == true)
                                                        {
                                                            // 打卡成功
                                                            //Console.WriteLine("name~~~~" + totalList.daily_shipments[setnum].ct_name + punch_in[totalList.daily_shipments[setnum].ct_name]);
                                                            punch_in[cList2[i].ct_name] = true; // 簽到成功
                                                            Wifi_Punchin_DB.SaveAccountAsync(new Wifi_Punchin // 存進有網路簽到成功的SQLite
                                                            {
                                                                name = cList2[i].ct_name, // 案主姓名
                                                                time = DateTime.Now.ToShortTimeString() // 簽到時間
                                                            });
                                                            //Console.WriteLine("punchin~~~gps" + punch_in[totalList.daily_shipments[setnum].ct_name] + "name " + totalList.daily_shipments[setnum].ct_name);
                                                            //Console.WriteLine("true");
                                                            //Console.WriteLine("BEE~~ " + BeaconScan.letpunchin);
                                                            formin_1.IsVisible = true; // 跳出簽到案主家成功訊息
                                                            formin_1.IsEnabled = true;
                                                            formin_2.IsVisible = true; // 跳出案主家相關資訊
                                                            formin_2.IsEnabled = true;
                                                            await Task.Delay(10000); // 等待30秒
                                                            Messager2(); // 訊息消失(自動關閉)

                                                            //punchinmsg = "簽到成功" + setName + "的家";
                                                            ////Console.WriteLine("punchinmsg" + punchinmsg);
                                                            Thread.Sleep(5000); // 等待五秒之後
                                                            fadeformin(); // 簽到成功訊息自動消失
                                                        }
                                                        else
                                                        {
                                                            //await DisplayAlert("FAIL", "打卡失敗in" + setName, "OK");
                                                            //Console.WriteLine("簽到失敗");
                                                        }
                                                    }
                                                    else // 無網路環境下，先將要打卡資料存進SQLite
                                                    {
                                                           
                                                        inorout = "in"; // 簽到
                                                                        ////Console.WriteLine("");
                                                                        // 將簽到資訊存進SQLite
                                                                        //bool web_res = await web.Save_Punch_In(MainPage.token, ct_s_num, sec_s_num, mlo_s_num, position.Latitude, position.Longitude);
                                                        DateTime myDate = DateTime.Now;
                                                        string time = myDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                        Console.WriteLine("time~~~ " + time);
                                                        PunchSaveToSQLite(MainPage.token, Clname, inorout, ct_s_num, sec_s_num, mlo_s_num, position.Latitude, position.Longitude, time, DateTime.Now.ToShortTimeString());
                                                        punch_in[cList2[i].ct_name] = true; // 簽到成功
                                                        PunchTmp.SaveAccountAsync(new PunchTmp // 存進無網路簽到成功的SQLite
                                                        {
                                                            name = cList2[i].ct_name, // 案主姓名
                                                            time = DateTime.Now.ToShortTimeString() // 簽到時間
                                                        });
                                                    }
                                                //}
                                            //}
                                        }
                                        //-------------<<<<<beacon punchin dont delete------>>>>>>>
                                        ////if (BeaconScan.letpunchin == true && punch_in[totalList.daily_shipments[setnum].ct_name] == false)
                                        ////{
                                        ////    if (CrossConnectivity.Current.IsConnected) // 有連到網路
                                        ////    {
                                        ////        bool web_res = await web.Beacon_Punch(MainPage.token, BeaconScan.UUID, 1.ToString()); // 簽到bnl02是1 簽退是2
                                        ////        if (web_res == true)
                                        ////        {
                                        ////            // 打卡成功
                                        ////            //Console.WriteLine("beacon_punch~~~");
                                        ////            //Console.WriteLine("name~~~~" + totalList.daily_shipments[setnum].ct_name + punch_in[totalList.daily_shipments[setnum].ct_name]);
                                        ////            punch_in[totalList.daily_shipments[setnum].ct_name] = true; // 簽到成功
                                        ////            //Console.WriteLine("punchin~~~gps" + punch_in[totalList.daily_shipments[setnum].ct_name] + "name " + totalList.daily_shipments[setnum].ct_name);
                                        ////            //Console.WriteLine("true");
                                        ////            //Console.WriteLine("BEE~~ " + BeaconScan.letpunchin);
                                        ////            formin_1.IsVisible = true;
                                        ////            formin_1.IsEnabled = true;
                                        ////            formin_2.IsVisible = true;
                                        ////            formin_2.IsEnabled = true;
                                        ////            await Task.Delay(10000); // 等待30秒
                                        ////            Messager2();

                                        ////            //punchinmsg = "SUCESS簽到成功in" + setName + "的家";
                                        ////            ////Console.WriteLine("punchinmsg" + punchinmsg);
                                        ////            //Thread.Sleep(5000); // 等待五秒之後
                                        ////            //fadeformin(); // 簽到成功訊息自動消失
                                        ////        }
                                        ////        else
                                        ////        {
                                        ////            //await DisplayAlert("FAIL", "打卡失敗in" + setName, "OK");
                                        ////            //Console.WriteLine("簽到失敗");
                                        ////        }
                                        ////    }
                                        ////    else // 無網路環境下，先將要打卡資料存進SQLite
                                        ////    {
                                        ////        //Console.WriteLine("nowifiadd_in~~~~");
                                        ////        //Console.WriteLine("token~~" + MainPage.token);
                                        ////        //Console.WriteLine("name~" + Clname);
                                        ////        //Console.WriteLine("ct_s_num~~" + ct_s_num);
                                        ////        //Console.WriteLine("sec_s_num~~" + sec_s_num);
                                        ////        //Console.WriteLine("mlo_s_num~~" + mlo_s_num);
                                        ////        //Console.WriteLine("bn_s_num~~" + bn_s_num);
                                        ////        //Console.WriteLine("lat~~" + position.Latitude);
                                        ////        //Console.WriteLine("lon~~" + position.Longitude);
                                        ////        inorout = "in";
                                        ////        ////Console.WriteLine("");
                                        ////        PunchSaveToSQLite(MainPage.token, Clname, inorout, ct_s_num, sec_s_num, mlo_s_num, position.Latitude, position.Longitude);
                                        ////        punch_in[totalList.daily_shipments[setnum].ct_name] = true; // 簽到成功
                                        ////        PunchTmp.SaveAccountAsync(new PunchTmp
                                        ////        {
                                        ////            name = totalList.daily_shipments[setnum].ct_name,
                                        ////            time = DateTime.Now.ToShortTimeString()
                                        ////        });
                                        ////    }
                                                
                                        ////}
                                        // //Console.WriteLine("punchin22~~~" + punch_in[totalList.daily_shipments[setnum].ct_name] + "name " + totalList.daily_shipments[setnum].ct_name);
                                        // 符合簽退距離 & 簽到成功 & 尚未簽退過
                                        if (d > 10 && punch_in[cList2[i].ct_name] == true && punch_out[cList2[i].ct_name] == false)
                                        {
                                                
                                            //Console.WriteLine("ddddistanceout~~~~" + d);
                                            //Console.WriteLine("PUNCH" + punch_in);
                                            //Console.WriteLine("EEEE" + MainPage.token);

                                            //qborder.Text = questionnaireslist[0].qbs[0].qb_order;
                                            //qb01.Text = questionnaireslist[0].qbs[0].qb01;
                                            //qborder2.Text = questionnaireslist[0].qbs[1].qb_order;
                                            //qb02.Text = questionnaireslist[0].qbs[1].qb01;
                                            // //Console.WriteLine("name" + questionnaireslist[0].ct_name);
                                            ////Console.WriteLine("AAA" + questionnaireslist[0].qbs[0].qb_order);
                                            ////Console.WriteLine("DDD" + questionnaireslist[0].qbs[0].qb01);
                                            if (CrossConnectivity.Current.IsConnected) // 有連到網路
                                            {
                                                DateTime myDate = DateTime.Now;
                                                string time = myDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                Console.WriteLine("time~~~ " + time);
                                                // 自動簽退
                                                bool web_res2 = await web.Save_Punch_Out(MainPage.token, ct_s_num, sec_s_num, mlo_s_num, position.Latitude, position.Longitude, time);
                                                //Console.WriteLine("web_res2" + web_res2);
                                                if (web_res2 == true)
                                                {
                                                    // 打卡成功
                                                    //await DisplayAlert("SUCESS", "簽退成功in" + setName + "的家", "OK");
                                                    // 幾秒之後alert自動消失
                                                    // 跳出回饋單
                                                    formout.IsVisible = true; // 跳出簽退成功訊息
                                                    formout.IsEnabled = true;
                                                    Form.IsVisible = true; // 跳出問卷
                                                    Form.IsEnabled = true;
                                                    punch_out[cList2[i].ct_name] = true;  // 簽退成功
                                                                                                                        //PunchSavepunchnameToSQLite(totalList.daily_shipments[setnum].ct_name);
                                                    //Console.WriteLine("punchout~~~gps" + punch_out[totalList.daily_shipments[setnum].ct_name] + "name " + totalList.daily_shipments[setnum].ct_name);
                                                    //punch_in[cList[i].ct_name] = false;
                                                    //which = 0;
                                                    punchList[cList2[i].ct_name] = true; // 打卡完成設為true(簽到+簽退成功)
                                                    Wifi_Punchout_DB.SaveAccountAsync(new Wifi_Punchout// 把簽退成功紀錄到無網路簽退的SQLite
                                                    {
                                                        name = cList2[i].ct_name, // 姓名
                                                        time = DateTime.Now.ToShortTimeString() // 時間
                                                    });
                                                    //Console.WriteLine("punchList~~~" + punchList[totalList.daily_shipments[setnum].ct_name] + "name " + totalList.daily_shipments[setnum].ct_name);
                                                    //------------------------跳出問卷先拿掉----------------------------
                                                    
                                                    if (isform[cList2[i].ct_name] == false)
                                                    {
                                                        try
                                                        {
                                                            if(questionnaireslist != null)
                                                            {
                                                                if(questionnaireslist.Count != 0)
                                                                {
                                                                    setQues(daily_shipment_client_num);
                                                                    isform[cList2[i].ct_name] = true; // 紀錄是否跳出問卷
                                                                }
                                                                    
                                                            }
                                                        }
                                                        catch(Exception ex)
                                                        {
                                                            DisplayAlert("系統訊息", "Error : deliver_mapview_questionnairelist_null", "ok");
                                                        }
                                                    }
                                                    
                                                    //----------------------------------------------------------------------------------
                                                    //trylist2.Add(setnum);
                                                    PunchSavesetnumToSQLite(setnum); // 把送餐進度存進SQLite
                                                    //Console.WriteLine("setnumadd111~~~" + setnum + "count " + trylist2.Count());
                                                    num = num + 1;
                                                    //if (setnum != 0)
                                                    //{

                                                    //    setnum = setnum - 1;
                                                    //    // for該案主同時有兩張單的狀況(只需要打卡一次)
                                                    //    // 判斷是否打過卡，有的話就跳過
                                                    //    foreach(var a in punchList)
                                                    //    {
                                                    //        if (a.Key == totalList.daily_shipments[setnum].ct_name)
                                                    //        {
                                                    //            if(a.Value == true)
                                                    //            {
                                                    //                //Console.WriteLine("key~~~" + a.Key);
                                                    //                PunchSavesetnumToSQLite(setnum);
                                                    //                setnum = setnum - 1;

                                                    //            }

                                                    //        }
                                                    //    }

                                                    //}
                                                    //if (setnum == 0 || total_need_to_serve > setnum)
                                                    //{

                                                        //setnum = setnum + 1;
                                                        // for該案主同時有兩張單的狀況(只需要打卡一次)
                                                        // 判斷是否打過卡，有的話就跳過
                                                        foreach (var a in punchList)
                                                        {
                                                            if (a.Key == cList2[i].ct_name)
                                                            {
                                                                if (a.Value == true)
                                                                {
                                                                    //Console.WriteLine("key~~~" + a.Key);
                                                                    PunchSavesetnumToSQLite(setnum);
                                                                    //setnum = setnum + 1;

                                                                }

                                                            }
                                                        }

                                                    //} 
                                                    /*
                                                    if (MainPage.AUTH == "4") // 外送員
                                                    {
                                                        //Console.WriteLine("setnumLA~~~~" + setnum);
                                                        if (totalList.daily_shipments.Count() > setnum)
                                                        {
                                                            SetIcon(setnum);
                                                        }

                                                        // //Console.WriteLine("ship_setnum~~" + totalList.daily_shipments[setnum]);
                                                    }
                                                    else
                                                    {
                                                        SetIcon3(setnum);
                                                    }
                                                    */
                                                    await Task.Delay(10000); // 等待30秒
                                                    Messager3(); // 簽退成功訊息消失(自動關閉)
                                                    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~`//
                                                    // 自動跳到下一家的google map位置

                                                    //if(gomap[totalList.daily_shipments[setnum].ct_name] == false)
                                                    //{
                                                    //    gps2 = totalList.daily_shipments[setnum].ClientLatitude + "," + totalList.daily_shipments[setnum].ClientLongitude;
                                                    //    string uri = "https://www.google.com.tw/maps/place/" + gps2;
                                                    //    //Console.WriteLine("URI" + uri);
                                                    //    if (await Launcher.CanOpenAsync(uri))
                                                    //    {
                                                    //        await Launcher.OpenAsync(uri);
                                                    //        gomap[totalList.daily_shipments[setnum].ct_name] = true;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        await DisplayAlert(param.SYSYTEM_MESSAGE, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                                                    //    }
                                                    //}

                                                    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~`//
                                                    //punchoutmsg = "SUCESS簽退成功in" + setName + "的家";
                                                    ////Console.WriteLine("punchinmsg" + punchoutmsg);
                                                    //Thread.Sleep(5000); // 等待五秒之後
                                                    //fadeformout(); // 簽退成功訊息自動消失

                                                }
                                                else
                                                {
                                                    //await DisplayAlert("FAIL", "打卡失敗in" + setName, "OK");
                                                    //Console.WriteLine("簽退失敗");
                                                }
                                            }
                                            else // 無網路環境下簽退
                                            {
                                                //把原本要上船的東西存到SQLite
                                                //Console.WriteLine("nowifiadd_out~~~~");
                                                //Console.WriteLine("name~" + Clname);
                                                //Console.WriteLine("ct_s_num~~" + ct_s_num);
                                                //Console.WriteLine("sec_s_num~~" + sec_s_num);
                                                //Console.WriteLine("mlo_s_num~~" + mlo_s_num);
                                                //Console.WriteLine("bn_s_num~~" + bn_s_num);
                                                ////Console.WriteLine("");
                                                inorout = "out"; // 簽退
                                                                 // 把要打卡的資料先存回SQLite
                                                DateTime myDate = DateTime.Now;
                                                string time = myDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                Console.WriteLine("time~~~ " + time);
                                                PunchSaveToSQLite(MainPage.token, Clname, inorout, ct_s_num, sec_s_num, mlo_s_num, position.Latitude, position.Longitude, time, DateTime.Now.ToShortTimeString());
                                                
                                                punch_out[cList2[i].ct_name] = true;  // 謙退成功
                                                punchList[cList2[i].ct_name] = true; // 打卡完成設為true
                                                PunchTmp2.SaveAccountAsync(new PunchTmp2 // 把簽退成功紀錄到無網路簽退的SQLite
                                                {
                                                    name = cList2[i].ct_name, // 姓名
                                                    time = DateTime.Now.ToShortTimeString() // 時間
                                                });
                                                PunchSavesetnumToSQLite(setnum); // 紀錄送餐進度
                                                //trylist2.Add(setnum);
                                                //Console.WriteLine("setnumadd22~~~" + setnum + "count " + trylist2.Count());
                                                //if (setnum != 0)
                                                //{
                                                //    setnum = setnum - 1;
                                                //    // for該案主同時有兩張單的狀況(只需要打卡一次)
                                                //    // 判斷是否打過卡，有的話就跳過
                                                //    foreach (var a in punchList)
                                                //    {
                                                //        if (a.Key == totalList.daily_shipments[setnum].ct_name)
                                                //        {
                                                //            if (a.Value == true)
                                                //            {
                                                //                //Console.WriteLine("key~~~" + a.Key);
                                                //                PunchSavesetnumToSQLite(setnum);
                                                //                setnum = setnum - 1;

                                                //            }

                                                //        }
                                                //    }
                                                //}
                                                //if (setnum == 0 || total_need_to_serve > setnum)
                                                //{

                                                    //setnum = setnum + 1;
                                                    // for該案主同時有兩張單的狀況(只需要打卡一次)
                                                    // 判斷是否打過卡，有的話就跳過
                                                    foreach (var a in punchList)
                                                    {
                                                        if (a.Key == cList2[i].ct_name)
                                                        {
                                                            if (a.Value == true)
                                                            {
                                                                //Console.WriteLine("key~~~" + a.Key);
                                                                PunchSavesetnumToSQLite(setnum);
                                                                //setnum = setnum + 1;

                                                            }

                                                        }
                                                    }

                                                //}
                                                /*
                                                if (MainPage.AUTH == "4")
                                                {
                                                    if (totalList.daily_shipments.Count() > setnum)
                                                    {
                                                        SetIcon(setnum);
                                                    }

                                                }
                                                else
                                                {
                                                    SetIcon3(setnum);
                                                }
                                                */
                                                //Console.WriteLine("setnumnowifipunchout~~~" + setnum);

                                            }
                                        }
                                        //if (BeaconScan.letpunchout == true && punch_in[totalList.daily_shipments[setnum].ct_name] == true && punch_out[totalList.daily_shipments[setnum].ct_name] == false)
                                        //{
                                        //    if (CrossConnectivity.Current.IsConnected) // 有連到網路
                                        //    {
                                        //        bool web_res2 = await web.Beacon_Punch(MainPage.token, BeaconScan.UUID, 1.ToString()); // 簽到bnl02是1 簽退是2
                                        //        if (web_res2 == true)
                                        //        {
                                        //            // 打卡成功
                                        //            //await DisplayAlert("SUCESS", "簽退成功in" + setName + "的家", "OK");
                                        //            // 幾秒之後alert自動消失
                                        //            // 跳出回饋單
                                        //            formout.IsVisible = true;
                                        //            formout.IsEnabled = true;
                                        //            Form.IsVisible = true;
                                        //            Form.IsEnabled = true;
                                        //            punch_out[totalList.daily_shipments[setnum].ct_name] = true;  // 謙退成功
                                        //                                                                             //PunchSavepunchnameToSQLite(totalList.daily_shipments[setnum].ct_name);
                                        //            //Console.WriteLine("punchout~~~gps" + punch_out[totalList.daily_shipments[setnum].ct_name] + "name " + totalList.daily_shipments[setnum].ct_name);
                                        //            //punch_in[cList[i].ct_name] = false;
                                        //            //which = 0;
                                        //            punchList[totalList.daily_shipments[setnum].ct_name] = true; // 打卡完成設為true

                                        //            //Console.WriteLine("punchList~~~" + punchList[totalList.daily_shipments[setnum].ct_name] + "name " + totalList.daily_shipments[setnum].ct_name);
                                        //            if (isform[totalList.daily_shipments[setnum].ct_name] == false)
                                        //            {
                                        //                setQues(setnum);
                                        //                isform[totalList.daily_shipments[setnum].ct_name] = true;
                                        //            }
                                        //            //trylist2.Add(setnum);
                                        //            PunchSavesetnumToSQLite(setnum);
                                        //            //Console.WriteLine("setnumadd111~~~" + setnum + "count " + trylist2.Count());
                                        //            num = num + 1;
                                        //            //if (setnum != 0)
                                        //            //{

                                        //            //    setnum = setnum - 1;
                                        //            //    // for該案主同時有兩張單的狀況(只需要打卡一次)
                                        //            //    // 判斷是否打過卡，有的話就跳過
                                        //            //    foreach(var a in punchList)
                                        //            //    {
                                        //            //        if (a.Key == totalList.daily_shipments[setnum].ct_name)
                                        //            //        {
                                        //            //            if(a.Value == true)
                                        //            //            {
                                        //            //                //Console.WriteLine("key~~~" + a.Key);
                                        //            //                PunchSavesetnumToSQLite(setnum);
                                        //            //                setnum = setnum - 1;

                                        //            //            }

                                        //            //        }
                                        //            //    }

                                        //            //}
                                        //            if (setnum == 0 || total_need_to_serve > setnum)
                                        //            {

                                        //                setnum = setnum + 1;
                                        //                // for該案主同時有兩張單的狀況(只需要打卡一次)
                                        //                // 判斷是否打過卡，有的話就跳過
                                        //                foreach (var a in punchList)
                                        //                {
                                        //                    if (a.Key == totalList.daily_shipments[setnum].ct_name)
                                        //                    {
                                        //                        if (a.Value == true)
                                        //                        {
                                        //                            //Console.WriteLine("key~~~" + a.Key);
                                        //                            PunchSavesetnumToSQLite(setnum);
                                        //                            setnum = setnum + 1;

                                        //                        }

                                        //                    }
                                        //                }

                                        //            }
                                        //            if (MainPage.AUTH == "4")
                                        //            {
                                        //                //Console.WriteLine("setnumLA~~~~" + setnum);
                                        //                if (totalList.daily_shipments.Count() > setnum)
                                        //                {
                                        //                    SetIcon(setnum);
                                        //                }

                                        //                // //Console.WriteLine("ship_setnum~~" + totalList.daily_shipments[setnum]);
                                        //            }
                                        //            else
                                        //            {
                                        //                SetIcon3(setnum);
                                        //            }
                                        //            //Console.WriteLine("setnumwifipunchout~~~" + setnum);

                                        //            ////Console.WriteLine("setnum2222~~~~" + setnum);
                                        //            ////Console.WriteLine("who4" + totalList.daily_shipments[setnum].ct_name);
                                        //            ////Console.WriteLine("punch4" + punchList[totalList.daily_shipments[setnum].ct_name]);
                                        //            ////Console.WriteLine("BEEQQ~~ " + BeaconScan.letpunchout);
                                        //            await Task.Delay(10000); // 30秒
                                        //            Messager3();
                                        //            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~`//
                                        //            // 自動跳到下一家的google map位置

                                        //            //if(gomap[totalList.daily_shipments[setnum].ct_name] == false)
                                        //            //{
                                        //            //    gps2 = totalList.daily_shipments[setnum].ClientLatitude + "," + totalList.daily_shipments[setnum].ClientLongitude;
                                        //            //    string uri = "https://www.google.com.tw/maps/place/" + gps2;
                                        //            //    //Console.WriteLine("URI" + uri);
                                        //            //    if (await Launcher.CanOpenAsync(uri))
                                        //            //    {
                                        //            //        await Launcher.OpenAsync(uri);
                                        //            //        gomap[totalList.daily_shipments[setnum].ct_name] = true;
                                        //            //    }
                                        //            //    else
                                        //            //    {
                                        //            //        await DisplayAlert(param.SYSYTEM_MESSAGE, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                                        //            //    }
                                        //            //}

                                        //            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~`//
                                        //            //punchoutmsg = "SUCESS簽退成功in" + setName + "的家";
                                        //            ////Console.WriteLine("punchinmsg" + punchoutmsg);
                                        //            //Thread.Sleep(5000); // 等待五秒之後
                                        //            //fadeformout(); // 簽退成功訊息自動消失

                                        //        }
                                        //        else
                                        //        {
                                        //            //await DisplayAlert("FAIL", "打卡失敗in" + setName, "OK");
                                        //            //Console.WriteLine("簽退失敗");
                                        //        }
                                        //    }
                                        //    else // 無網路環境下，先將要打卡資料存進SQLite
                                        //    {
                                        //        //Console.WriteLine("nowifiadd_out~~~~");
                                        //        //Console.WriteLine("name~" + Clname);
                                        //        //Console.WriteLine("ct_s_num~~" + ct_s_num);
                                        //        //Console.WriteLine("sec_s_num~~" + sec_s_num);
                                        //        //Console.WriteLine("mlo_s_num~~" + mlo_s_num);
                                        //        //Console.WriteLine("bn_s_num~~" + bn_s_num);
                                        //        ////Console.WriteLine("");
                                        //        inorout = "out";
                                        //        PunchSaveToSQLite(MainPage.token, Clname, inorout, ct_s_num, sec_s_num, mlo_s_num, position.Latitude, position.Longitude);

                                        //        punch_out[totalList.daily_shipments[setnum].ct_name] = true;  // 謙退成功
                                        //        punchList[totalList.daily_shipments[setnum].ct_name] = true; // 打卡完成設為true
                                        //        PunchTmp2.SaveAccountAsync(new PunchTmp2
                                        //        {
                                        //            name = totalList.daily_shipments[setnum].ct_name,
                                        //            time = DateTime.Now.ToShortTimeString()
                                        //        });
                                        //        PunchSavesetnumToSQLite(setnum);
                                        //        trylist2.Add(setnum);
                                        //        //Console.WriteLine("setnumadd22~~~" + setnum + "count " + trylist2.Count());
                                        //        //if (setnum != 0)
                                        //        //{
                                        //        //    setnum = setnum - 1;
                                        //        //    // for該案主同時有兩張單的狀況(只需要打卡一次)
                                        //        //    // 判斷是否打過卡，有的話就跳過
                                        //        //    foreach (var a in punchList)
                                        //        //    {
                                        //        //        if (a.Key == totalList.daily_shipments[setnum].ct_name)
                                        //        //        {
                                        //        //            if (a.Value == true)
                                        //        //            {
                                        //        //                //Console.WriteLine("key~~~" + a.Key);
                                        //        //                PunchSavesetnumToSQLite(setnum);
                                        //        //                setnum = setnum - 1;

                                        //        //            }

                                        //        //        }
                                        //        //    }
                                        //        //}
                                        //        if (setnum == 0 || total_need_to_serve > setnum)
                                        //        {

                                        //            setnum = setnum + 1;
                                        //            // for該案主同時有兩張單的狀況(只需要打卡一次)
                                        //            // 判斷是否打過卡，有的話就跳過
                                        //            foreach (var a in punchList)
                                        //            {
                                        //                if (a.Key == totalList.daily_shipments[setnum].ct_name)
                                        //                {
                                        //                    if (a.Value == true)
                                        //                    {
                                        //                        //Console.WriteLine("key~~~" + a.Key);
                                        //                        PunchSavesetnumToSQLite(setnum);
                                        //                        setnum = setnum + 1;

                                        //                    }

                                        //                }
                                        //            }

                                        //        }
                                        //        if (MainPage.AUTH == "4")
                                        //        {
                                        //            if (totalList.daily_shipments.Count() > setnum)
                                        //            {
                                        //                SetIcon(setnum);
                                        //            }

                                        //        }
                                        //        else
                                        //        {
                                        //            SetIcon3(setnum);
                                        //        }
                                                    
                                        //    }
                                                
                                        //}
                                        //if (setnum == 0 || total_need_to_serve > setnum)
                                        //{
                                            if (punchyesorno[cList2[i].ct_name] == true) // 先確定是有進入判斷，而不是尚未進入判斷
                                            {
                                                // 簽到失敗導致無法簽退，因此簽退也失敗，且已經送完這個案主(遠離案主家)(目前設距離>50m)
                                                // 必須讓他繼續跳到下一個案主家進行判斷
                                                if (d > 50 && punch_in[cList2[i].ct_name] == false) // 簽到失敗且簽退失敗，距離大於10(遠離案主家)
                                                {
                                                    PunchSavesetnumToSQLite(setnum);
                                                    //Console.WriteLine("setnum~~1~~ " + setnum);
                                                    //trylist2.Add(setnum);
                                                    //Console.WriteLine("setnumadd333~~~" + setnum + "count " + trylist2.Count());
                                                    //if (setnum != 0)
                                                    //{
                                                    //    setnum = setnum - 1;
                                                    //    //Console.WriteLine("setnum~~2~~ " + setnum);
                                                    //    // for該案主同時有兩張單的狀況(只需要打卡一次)
                                                    //    // 判斷是否打過卡，有的話就跳過
                                                    //    foreach (var a in punchList)
                                                    //    {
                                                    //        if (a.Key == totalList.daily_shipments[setnum].ct_name)
                                                    //        {
                                                    //            if (a.Value == true)
                                                    //            {
                                                    //                //Console.WriteLine("key~~~" + a.Key);
                                                    //                PunchSavesetnumToSQLite(setnum);
                                                    //                //Console.WriteLine("setnum~~3~~ " + setnum);
                                                    //                setnum = setnum - 1;
                                                    //                //Console.WriteLine("setnum~~4~~ " + setnum);

                                                    //            }

                                                    //        }
                                                    //    }
                                                    //}
                                                    /*
                                                    if (setnum == 0 || total_need_to_serve > setnum)
                                                    {
                                                        setnum = setnum + 1;
                                                        // for該案主同時有兩張單的狀況(只需要打卡一次)
                                                        // 判斷是否打過卡，有的話就跳過
                                                        foreach (var a in punchList)
                                                        {
                                                            if (a.Key == totalList.daily_shipments[setnum].ct_name)
                                                            {
                                                                if (a.Value == true)
                                                                {
                                                                    //Console.WriteLine("key~~~" + a.Key);
                                                                    PunchSavesetnumToSQLite(setnum);
                                                                    setnum = setnum + 1;

                                                                }

                                                            }
                                                        }
                                                    }
                                                    if (MainPage.AUTH == "4")
                                                    {
                                                        SetIcon(setnum);
                                                    }
                                                    else
                                                    {
                                                        SetIcon3(setnum);
                                                    }
                                                    */
                                                    //Console.WriteLine("setnumwifidistance~~~" + setnum);

                                                }
                                                // 簽到成功但簽退失敗，距離大於10(遠離案主家)(目前設距離>50m)
                                                // 簽到成功但是簽退失敗，且已經完成該案主送餐
                                                // 必須讓他繼續跳他下一家進行判斷
                                                if (d > 50 && punch_in[totalList.daily_shipments[setnum].ct_name] == true && punch_out[totalList.daily_shipments[setnum].ct_name] == false) 
                                                {
                                                    PunchSavesetnumToSQLite(setnum);
                                                    //trylist2.Add(setnum);
                                                    //Console.WriteLine("setnumadd444~~~" + setnum + "count " + trylist2.Count());
                                                    //if (setnum != 0)
                                                    //{
                                                    //    setnum = setnum - 1;
                                                    //    // for該案主同時有兩張單的狀況(只需要打卡一次)
                                                    //    // 判斷是否打過卡，有的話就跳過
                                                    //    foreach (var a in punchList)
                                                    //    {
                                                    //        if (a.Key == totalList.daily_shipments[setnum].ct_name)
                                                    //        {
                                                    //            if (a.Value == true)
                                                    //            {
                                                    //                //Console.WriteLine("key~~~" + a.Key);
                                                    //                PunchSavesetnumToSQLite(setnum);
                                                    //                setnum = setnum - 1;

                                                    //            }

                                                    //        }
                                                    //    }
                                                    //}
                                                    /*
                                                    if (setnum == 0 || total_need_to_serve > setnum)
                                                    {
                                                        setnum = setnum + 1;
                                                        // for該案主同時有兩張單的狀況(只需要打卡一次)
                                                        // 判斷是否打過卡，有的話就跳過
                                                        foreach (var a in punchList)
                                                        {
                                                            if (a.Key == totalList.daily_shipments[setnum].ct_name)
                                                            {
                                                                if (a.Value == true)
                                                                {
                                                                    //Console.WriteLine("key~~~" + a.Key);
                                                                    PunchSavesetnumToSQLite(setnum);
                                                                    setnum = setnum + 1;

                                                                }

                                                            }
                                                            else
                                                            {
                                                                if (MainPage.AUTH == "4")
                                                                {
                                                                    SetIcon(setnum);
                                                                }
                                                                else
                                                                {
                                                                    SetIcon3(setnum);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    */
                                                    //Console.WriteLine("setnumwifidistance~~~" + setnum);

                                                }
                                            }
                                        //}

                                        //if(d > 2 && punch_in[totalList.daily_shipments[setnum].ct_name] == false && punch_out[totalList.daily_shipments[setnum].ct_name] == false) // 不管簽到簽退是否成功，單純用距離判斷將地圖上的icon換到下 一位案主
                                        //{
                                        //    if (setnum != 0)
                                        //    {
                                        //        setnum = setnum - 1;
                                        //    }
                                        //    SetIcon(setnum);
                                        //    //Console.WriteLine("setnumwifidistance~~~" + setnum);
                                        //    PunchSavesetnumToSQLite(setnum);
                                        //}
                                    }

                                    //}


                                }
                            //}
                                
                        //}
                        //else
                        //{
                        //    Console.WriteLine("setnumBBB~~~ " + setnum);
                        //    Console.WriteLine("totalneedtoserve~~~ " + total_need_to_serve);
                        //    DeliverEnd.IsVisible = true;
                        //    Dist.IsVisible = false;
                        //}

                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("GET");
                        //Console.WriteLine("ERRORLA~~~" + ex.ToString());
                        if (!isAlert)
                        {
                            isAlert = true;
                            //await DisplayAlert(param.SYSYTEM_MESSAGE, param.LOCATION_ERROR_MESSAGE, param.DIALOG_AGREE_MESSAGE);
                        }
                    }
                }
                else
                {
                    //Console.WriteLine("Location null~~");
                }
                //}
            }
            catch (Exception ex)
            {
                //Console.WriteLine("GETERROR");
                //Console.WriteLine(ex.ToString());
            }
        }

       

        // 產生問卷
        private async void setQues(int which)
        {
            //questionnaireslist = await web.Get_Questionaire(MainPage.token);
            ////Console.WriteLine("GGGGG" + which);
            ////Console.WriteLine("DDDDD" + questionnaireslist[which].ClientName);
            ////Console.WriteLine("C1 :" + questionnaireslist.Count);
            ////Console.WriteLine("BBBBB" + totalList.daily_shipments[which].ClientName);
            ////Console.WriteLine("C2 :" + totalList.daily_shipments.Count);
            quesStack.Children.Clear();
            FINAL = 0;
            for (int i = 0; i < questionnaireslist.Count; i++)
            {
                if (totalList.daily_shipments[which].ct_name == questionnaireslist[i].ClientName)
                {
                    FINAL = i;
                }
            }
            //Console.WriteLine("FINAL" + questionnaireslist[FINAL].ClientName);
            questionView(questionnaireslist[FINAL]);
        }

        private void reset()
        {
            quesStack.Children.Clear();
            questionView(questionnaireslist[FINAL]);
        }

        public StackLayout questionView(questionnaire questionList)
        {
            //StackLayout stack2 = new StackLayout // stacklayout看裡面包什麼寫在children
            //{
            //    Children = { }
            //};

            var label_name = new Label
            {
                Text = questionList.ClientName,
                TextColor = Color.DarkBlue,
                FontSize = 20
            };

            var label_wqh = new Label
            {
                Text = questionList.wqh_s_num
            };
            var label_qh = new Label
            {
                Text = questionList.qh_s_num
            };

            var stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                
                // BackgroundColor = Color.FromHex("eddcd2"),
                Children = { label_name, label_wqh, label_qh } // 標題(人名 + 問卷編號 +)
            };


            foreach (var i in questionList.qbs) // 看選項有幾個就跑幾次
            {
                if(Int32.Parse(i.qb_order) < 3)
                {
                    if (i.qb02 == "1")
                    {
                        var label_que_name = new Label // 問題題號+題目
                        {
                            Text = i.qb_order + " " + i.qb01,
                            FontSize = 20,
                            TextColor = Color.Black
                        };

                        var stack_ques = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal
                        };

                        foreach (var j in i.qb03) // 跑選項的for迴圈(for產生幾個checkbox)
                        {
                            var temp_j = "";
                            var temp_value = "";
                            var temp_ans = "";

                            if (TestView.TmpAnsList.ContainsKey(questionList.wqh_s_num + questionList.ClientName + i.qb_order) && TestView.TmpAnsList[questionList.wqh_s_num + questionList.ClientName + i.qb_order] != "")
                            {
                                //////Console.WriteLine("third~~ ");
                                //////Console.WriteLine("wqh2222~~ " + questionList.wqh_s_num);
                                //////Console.WriteLine("qborder~~~ " + i.qb_order);
                                var _wqhsnum = questionList.wqh_s_num;
                                temp_j = TestView.TmpAnsList[questionList.wqh_s_num + questionList.ClientName + i.qb_order];
                                //////Console.WriteLine("tempj~~ " + temp_j);
                                for (int d = 0; d < i.qb03.Count(); d++)
                                {
                                    //////Console.WriteLine("j00~~ " + j);
                                    //////Console.WriteLine("w00~~~ " + i.qb03[d]);
                                    if (temp_j == i.qb03[d])
                                    {

                                        //////Console.WriteLine("w~~~ " + i.qb03[d]);
                                        ////////Console.WriteLine("check~~ " + checkList2[a].wqb01);
                                        //////Console.WriteLine("qb0311~~ " + qb03_count);
                                        //////Console.WriteLine("j~~ " + j);
                                        //////Console.WriteLine("w~~~ " + i.qb03[d]);
                                        //ANS2 = Convert.ToString(qb03_count);
                                        TestView.ANS2 = d.ToString();
                                        //////Console.WriteLine("jj~~ " + temp_j);
                                        //////Console.WriteLine("ANS2_2~~ " + ANS2);
                                    }

                                    //////Console.WriteLine("qb0322~~ " + qb03_count);
                                }
                                //////Console.WriteLine("wqh3333~~ " + questionList.wqh_s_num);
                                //////Console.WriteLine("qborder~~~ " + i.qb_order);
                                //////Console.WriteLine("why~~ " + TmpAdd_elseList[questionList.wqh_s_num + i.qb_order]);
                                checkList2.RemoveAll(x => x.wqh_s_num == questionList.wqh_s_num && x.qb_order == i.qb_order);
                                var check3 = new checkInfo
                                {
                                    wqh_s_num = questionList.wqh_s_num, // 問卷編號
                                    qh_s_num = questionList.qh_s_num, // 工作問卷編號
                                    qb_s_num = i.qb_s_num, // 問題編號(第幾題)
                                    qb_order = i.qb_order,
                                    wqb01 = TestView.ANS2 // 答案

                                };
                                ////////Console.WriteLine("count1~~ " + checkList2.Count());
                                checkList2.Add(check3); // for save

                            }
                            // 跑選是的reset把checkList抓回來判斷
                            //////Console.WriteLine("checklist2~count3~ " + checkList2.Count());
                            for (int a = 0; a < TestView.checkList.Count(); a++)
                            {
                                //////Console.WriteLine("check11~~ " + checkList[a].wqh_s_num);
                                //////Console.WriteLine("ques11~~~ " + questionList.wqh_s_num);
                                ////////Console.WriteLine("COUNT222~~~~" + MapView.AccDatabase.GetAccountAsync2().Count());
                                if (TestView.checkList[a].wqh_s_num == questionList.wqh_s_num) // 判斷問卷編號
                                {
                                    ////////Console.WriteLine("IMMMM222~~~~");
                                    Console.WriteLine("AAQ~~~ " + questionList.wqh_s_num);
                                    if (TestView.checkList[a].qb_s_num == i.qb_s_num) // 判斷哪一題
                                    {
                                        //////Console.WriteLine("BBQ~~~~ " + i.qb_s_num);

                                        //foreach (var w in i.qb03)
                                        for (int d = 0; d < i.qb03.Count(); d++)
                                        {
                                            //////Console.WriteLine("check00~~ " + checkList[a].wqb01);
                                            //////Console.WriteLine("w00~~~ " + d.ToString());
                                            if (TestView.checkList[a].wqb01 == d.ToString())
                                            {

                                                //////Console.WriteLine("w~~~ " + i.qb03[d]);
                                                //////Console.WriteLine("check~~ " + checkList[a].wqb01);
                                                //////Console.WriteLine("qb0311~~ " + qb03_count);
                                                //////Console.WriteLine("j~~ " + j);
                                                //////Console.WriteLine("w~~~ " + i.qb03[d]);
                                                //ANS2 = Convert.ToString(qb03_count);
                                                temp_j = i.qb03[d]; // 答案
                                                Console.WriteLine("YYYYYY_jj~~ " + temp_j);
                                            }

                                            //////Console.WriteLine("qb0322~~ " + qb03_count);
                                        }
                                        // //////Console.WriteLine("cc~~~ " + p);
                                        //////Console.WriteLine("ANS2~~ " + ANS2);

                                        //temp_value = checkList[a].wqb99; // entry
                                    }
                                }
                            }



                            bool ischeck = (temp_j == j) ? true : false; // 再把剛剛的答案抓回來判斷(如果是就把他勾起來)


                            var check_box = new CheckBox // 產生checkbox
                            {
                                IsChecked = ischeck,
                                Color = Color.FromHex("264653")
                            };
                            //if (j == "是")
                            //{
                            //    entny = new Entry // 產生Entry
                            //    {
                            //        Placeholder = "請說明",
                            //        Text = temp_value,
                            //        IsVisible = isEntry,
                            //        IsEnabled = isEntry


                            //    };



                            //    entny.TextChanged += async (ss, ee) =>  // 點擊Entry
                            //    {
                            //        for (int a = 0; a < checkList2.Count(); a++)
                            //        {
                            //            if (checkList2[a].qb_s_num == i.qb_s_num) // 第幾題
                            //            {
                            //                checkList2[a].wqb99 = ee.NewTextValue;
                            //                entrytxt = ee.NewTextValue;
                            //                //var check2 = new TempAccount
                            //                //{
                            //                //    wqh_s_num = questionList.wqh_s_num, // 問卷編號
                            //                //    qh_s_num = questionList.qh_s_num, // 工作問卷編號
                            //                //    qb_s_num = i.qb_s_num, // 問題編號
                            //                //    wqb01 = j, // 答案
                            //                //    wqb99 = entrytxt
                            //                //};
                            //                //checkList3.Add(check2);
                            //                //ispress = true;
                            //                //reset();
                            //                //AccDatabase.SaveAccountAsync(new TempAccount
                            //                //{

                            //                //    wqh_s_num = questionList.wqh_s_num, // 問卷編號
                            //                //    qh_s_num = questionList.qh_s_num, // 工作問卷編號
                            //                //    qb_s_num = i.qb_s_num, // 問題編號
                            //                //    //wqb01 = j,
                            //                //    wqb99 = entrytxt
                            //                //});
                            //                //Console.WriteLine("ENTRY~~" + entrytxt);
                            //                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                            //                //if(TmpNum == i.qb_s_num)
                            //                //{
                            //                //    id = Temp[TmpNum];
                            //                //    //Console.WriteLine("IDin~~");
                            //                //    //Console.WriteLine("id " + id);
                            //                //    int Q = AccDatabase.SaveAccountAsync(new TempAccount
                            //                //    {
                            //                //        //wqh_s_num = questionList.wqh_s_num, // 問卷編號
                            //                //        //qh_s_num = questionList.qh_s_num, // 工作問卷編號
                            //                //        //qb_s_num = i.qb_s_num, // 問題編號
                            //                //        //wqb01 = j,
                            //                //        ID = id,
                            //                //        wqb99 = entrytxt
                            //                //    });
                            //                //    //Console.WriteLine("QQ " + Q);
                            //                //    //Console.WriteLine("TmpID2 " + id);
                            //                //    //Console.WriteLine("wqb99 " + entrytxt);
                            //                //}

                            //            }
                            //        }
                            //    };
                            //}
                            check_box.CheckedChanged += async (s, e) =>
                            {
                                if (e.Value)
                                {
                                    isSet = true;
                                    //for (int b = 0; b < MapView.AccDatabase.GetAccountAsync2().Count(); b++)
                                    //{
                                    //    var a = MapView.AccDatabase.GetAccountAsync(b);


                                    //    foreach (var TempAnsList in a)
                                    //    {
                                    //        if(TempAnsList.wqh_s_num == questionList.wqh_s_num)
                                    //        {
                                    //            if(TempAnsList.qb_s_num == i.qb_s_num)
                                    //            {
                                    //                //Console.WriteLine("which~~~" + TempAnsList.wqh_s_num);
                                    //                //Console.WriteLine("num~~~~~" + TempAnsList.qb_s_num);
                                    //                AccDatabase.DeleteItem(TempAnsList.ID);
                                    //                //Console.WriteLine("DELETE~~~");
                                    //            }
                                    //        }

                                    //    }
                                    //}
                                    //Console.WriteLine("True!!!");
                                    //Console.WriteLine("NUM~~~" + i.qb_s_num);
                                    //Console.WriteLine("wqh_s_num : " + questionList.wqh_s_num);
                                    //Console.WriteLine("qb_s_num : " + i.qb_s_num);
                                    //Console.WriteLine("qb03 : " + j);
                                    try
                                    {
                                       
                                        for (int a = 0; a < TestView.checkList.Count(); a++)
                                        {
                                            if (TestView.checkList[a].wqh_s_num == questionList.wqh_s_num)
                                            {
                                                if (TestView.checkList[a].qb_s_num == i.qb_s_num)
                                                {
                                                    TestView.checkList.RemoveAt(a);
                                                    //checkList2.RemoveAt(a);
                                                }
                                            }

                                        }
                                        ////////Console.WriteLine("NAME~~~~" + questionList.ClientName);
                                        //if (tmp_name_list.Contains(questionList.ClientName))
                                        //{
                                        //    //////Console.WriteLine("NAME~~~~" + questionList.ClientName);
                                        //    var total = tmp_name_list.Count(b => b == questionList.ClientName);
                                        //    //////Console.WriteLine("a~ " + total);
                                        //    tmp_name_list.Remove(questionList.ClientName);
                                        //    var total2 = tmp_name_list.Count(a => a == questionList.ClientName);
                                        //    //////Console.WriteLine("b~ " + total2);
                                        //}

                                        //if (j == "是")
                                        //{
                                        //    ANS = 0;
                                        //    ANS2 = Convert.ToString(ANS);
                                        //}
                                        //else
                                        //{
                                        //    ANS = 1;
                                        //    ANS2 = Convert.ToString(ANS);
                                        //}
                                        for (int d = 0; d < i.qb03.Count(); d++)
                                        {
                                            //////Console.WriteLine("j00~~ " + j);
                                            //////Console.WriteLine("w00~~~ " + i.qb03[d]);
                                            if (j == i.qb03[d])
                                            {

                                                //////Console.WriteLine("w~~~ " + i.qb03[d]);
                                                ////////Console.WriteLine("check~~ " + checkList2[a].wqb01);
                                                //////Console.WriteLine("qb0311~~ " + qb03_count);
                                                //////Console.WriteLine("j~~ " + j);
                                                //////Console.WriteLine("w~~~ " + i.qb03[d]);
                                                //ANS2 = Convert.ToString(qb03_count);
                                                TestView.ANS2 = d.ToString();
                                                //////Console.WriteLine("jj~~ " + temp_j);
                                                //////Console.WriteLine("ANS2_2~~ " + ANS2);
                                            }

                                            //////Console.WriteLine("qb0322~~ " + qb03_count);
                                        }
                                        if(i.qb_order == "1")
                                        {
                                            if (j == "是" || j == "已發")
                                            {
                                                //Console.WriteLine("G_in~~~ ");
                                                TestView.color = "Green";
                                                TestView.IsResetList[questionList.wqh_s_num + i.qb_order] = true;
                                                TestView.IsGreenOrRed[questionList.wqh_s_num + i.qb_order] = "Green";
                                                TestView.YesOrNoAlreadyChoose[questionList.wqh_s_num + i.qb_order] = j;
                                            }
                                            else
                                            {
                                                //Console.WriteLine("R_in~~~ ");
                                                TestView.color = "Red";
                                                TestView.IsResetList[questionList.wqh_s_num + i.qb_order] = true;
                                                TestView.IsGreenOrRed[questionList.wqh_s_num + i.qb_order] = "Red";
                                                TestView.YesOrNoAlreadyChoose[questionList.wqh_s_num + i.qb_order] = j;
                                            }
                                        }
                                        else
                                        {
                                            if (j == "是" || j == "已發")
                                            {
                                                TestView.color = "Red";
                                                TestView.IsResetList[questionList.wqh_s_num + i.qb_order] = true;
                                                TestView.IsGreenOrRed[questionList.wqh_s_num + i.qb_order] = "Red";
                                                TestView.YesOrNoAlreadyChoose[questionList.wqh_s_num + i.qb_order] = j;
                                            }
                                            else
                                            {
                                                TestView.color = "Green";
                                                TestView.IsResetList[questionList.wqh_s_num + i.qb_order] = true;
                                                TestView.IsGreenOrRed[questionList.wqh_s_num + i.qb_order] = "Green";
                                                TestView.YesOrNoAlreadyChoose[questionList.wqh_s_num + i.qb_order] = j;
                                            }
                                        }
                                       
                                        // 把問題和答案存進SQLite
                                        TestView.TmpAnsList[questionList.wqh_s_num + questionList.ClientName + i.qb_order] = temp_j;
                                        TestView.TmpAnsList_same_wqh[questionList.ClientName + i.qb_order] = questionList.wqh_s_num;
                                        TestView.TmpAnsList_same[questionList.wqh_s_num + i.qb_order] = temp_j;
                                        QuesSaveToSQLite(questionList.wqh_s_num, questionList.qh_s_num, i.qb_s_num, j, questionList.ClientName);
                                        ResetSaveToDB(questionList.wqh_s_num, i.qb_order, TestView.color);
                                        var check = new checkInfo
                                        {
                                            wqh_s_num = questionList.wqh_s_num, // 問卷編號
                                            qh_s_num = questionList.qh_s_num, // 工作問卷編號
                                            qb_s_num = i.qb_s_num, // 問題編號(第幾題)
                                            wqb01 = TestView.ANS2 // 答案

                                        };
                                        checkList2.RemoveAll(x => x.wqh_s_num == questionList.wqh_s_num && x.qb_order == i.qb_order);
                                        var check3 = new checkInfo
                                        {
                                            wqh_s_num = questionList.wqh_s_num, // 問卷編號
                                            qh_s_num = questionList.qh_s_num, // 工作問卷編號
                                            qb_s_num = i.qb_s_num, // 問題編號(第幾題)
                                            qb_order = i.qb_order,
                                            wqb01 = TestView.ANS2 // 答案

                                        };
                                        ////////Console.WriteLine("count1~~ " + checkList2.Count());
                                        checkList2.Add(check3); // for save
                                                                //////Console.WriteLine("i.qb_s_num####~~" + i.qb_s_num);
                                        TestView.checkList.Add(check); // for check
                                        ////Console.WriteLine("CHECK" + checkList[0]);
                                        reset(); // 因為+entry之前畫面已run好，所以要+entry要重run一次再把選項抓回來填進去
                                    }
                                    catch
                                    {
                                        //Console.WriteLine("ERROE~~HERE~~~line: 1194 ");
                                    }


                                }
                                else
                                {

                                    for (int a = 0; a < checkList2.Count(); a++)
                                    {
                                        if (checkList2[a].qb_s_num == i.qb_s_num)
                                        {
                                            checkList2.RemoveAt(a);
                                        }
                                    }
                                }

                                foreach (var b in checkList2)
                                {
                                    //Console.WriteLine("wqh_s_num : " + b.wqh_s_num);
                                    //Console.WriteLine("qb_s_num : " + b.qb_s_num);
                                    //Console.WriteLine("qb03 : " + b.wqb01);
                                    //Console.WriteLine("enrty : " + b.wqb99);
                                }
                            };

                            var label_check = new Label // 選項
                            {
                                Text = j,
                                TextColor = Color.Black,
                                FontSize = 20
                            };



                            var stack_check = new StackLayout // checkbox跟選項
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = { check_box, label_check }
                            };


                            //var ques_all_check = new StackLayout
                            //{
                            //    Orientation = StackOrientation.Horizontal,
                            //    Children = { stack_check, stack }
                            //};


                            stack_ques.Children.Add(stack_check);


                            //var final_stack = new StackLayout
                            //{
                            //    Orientation = StackOrientation.Horizontal,
                            //    Children = { stack_ques, label_que_name }
                            //};
                        }

                        quesStack.Children.Add(stack); // w

                        //quesStack.Children.Add(final_stack);
                        //quesStack.Children.Add(label_que_name);
                        //quesStack.Children.Add(stack_ques);


                        var final_stack = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { label_que_name, stack_ques }
                        };

                        //var lastest_stack = new StackLayout
                        //{
                        //    Orientation = StackOrientation.Vertical,
                        //    Children = { final_stack, more_form }
                        //};

                        Frame frame = new Frame // frame包上面那個stacklayout
                        {
                            Padding = new Thickness(15, 5, 10, 5),

                            BackgroundColor = Color.FromHex("eddcd2"),
                            CornerRadius = 10,
                            HasShadow = false,
                            Content = final_stack
                        };

                        quesStack.Children.Add(frame);
                        //quesStack.Children.Add(more_form);
                        Messager4();

                    }
                }
                
               
            }
            //var more_form = new Button // 更多題目的回饋單
            //{
            //    Text = "更多",
            //    CornerRadius = 60,
            //    BackgroundColor = Color.FromHex("fec89a")

            //};

            //more_form.Clicked += (object sender, EventArgs e) =>
            //{
            //    Navigation.PushAsync(new MoreFormView());
            //};
            //quesStack.Children.Add(more_form);
            return null;
        }



        public void QuesSaveToSQLite(string _wqh_s_num, string _qh_s_num, string _qb_s_num, string _wqb01, string _clnName)
        {
            AccDatabase.SaveAccountAsync(new TempAccount
            {
                ClientName = _clnName,
                wqh_s_num = _wqh_s_num, // 問卷編號
                qh_s_num = _qh_s_num, // 工作問卷編號
                qb_s_num = _qb_s_num, // 問題編號
                wqb01 = _wqb01,
                //wqb99 = entrytxt
            });
        }

        public void ResetSaveToDB(string _wqh_s_num, string _qb_order, string _color)
        {
            TestView.ResetDB.SaveAccountAsync(new Reset
            {
                wqh_s_num = _wqh_s_num,
                qb_order = _qb_order,
                color = _color

            });
        }

        public void PunchSaveToSQLite(string _token, string _name, string _inorout, string _ct_s_num, string _sec_s_num, string _mlo_s_num, double _lat, double _lot, string _time, string _timeforpost)
        {
            // MainPage.token, ct_s_num, sec_s_num, mlo_s_num, bn_s_num, position.Latitude, position.Longitude
            //Console.WriteLine("punchsave~~~");
            PunchDatabase.SaveAccountAsync(new Punch
            {
                token = _token,
                name = _name,
                inorout = _inorout,
                ct_s_num = _ct_s_num,
                sec_s_num = _sec_s_num,
                mlo_s_num = _mlo_s_num,
                latitude = _lat,
                longitude = _lot,
                time = _time,
                timeforpost = _timeforpost
            });
        }

        public void PunchSavesetnumToSQLite(int _setnum)
        {
            //Console.WriteLine("setnumadd~~~");
            PunchDatabase2.SaveAccountAsync(new Punch2
            {
                setnum = _setnum
            });
        }

        public void PunchSavepunchnameToSQLite(string _punchname)
        {
            PunchDatabase.SaveAccountAsync(new Punch
            {
                punchname = _punchname
            });
        }

        public void PunchSavePunchYesOrNoToSQLite(string _punchname)
        {
            PunchYN.SaveAccountAsync(new PunchYorN
            {
                name = _punchname
            });
        }
        //public void noonecheck(object sender, CheckedChangedEventArgs e)
        //{
        //    var t = ((CheckBox)sender).ClassId;
        //    //ClassId = questionnaireslist[0].wqh_s_num;
        //    if (e.Value)
        //    {

        //        //Console.WriteLine("fdsf : " + t + " :否");
        //        yesnoList2[t] = false;
        //    }
        //    else
        //    {
        //        //Console.WriteLine("fdsf : " + t + " :是");
        //    }
        //    ////Console.WriteLine("classid " + ClassId);
        //    //yesnoList[ClassId] = false;
        //    //ans2 = e.Value;
        //    //if (ans == true)
        //    //{
        //    //    ans3 = "2";
        //    //    //Console.WriteLine("ans3" + ans);
        //    //}


        //    //}

        //}

        // 紀錄GPS，船到後台
        private async void post_gps()
        {
            try
            {
                Console.WriteLine("postGPS~~~");
                Console.WriteLine("lat~~ " + position.Latitude.ToString());
                Console.WriteLine("lot~~~ " + position.Longitude.ToString());
                web.post_gps(MainPage.token, position.Latitude.ToString(), position.Longitude.ToString());
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("postgpserror~~ " + ex.ToString());
            }
        }
        public void formclose()
        {
            Form.IsEnabled = false;
            Form.IsVisible = false;
        }
        public void fadeformin()
        {
            // 簽到成功訊息消失
            formin_1.IsVisible = false;
            formin_1.IsEnabled = false;
            formin_2.IsVisible = false;
            formin_2.IsEnabled = false;
        }

        public void fadeformout()
        {
            // 簽退成功訊息消失
            formout.IsVisible = false;
            formout.IsEnabled = false;
            // 跳出回饋單
            // 等待30秒
            // if 回庫單沒填(變數=null)
            // 回饋單自動消失
            // 並+到listview
            // else
            // +到listview
        }

        bool OnTimerTick()
        {
            Task.Run(() =>
            {
                try
                {
                    // Run code here
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        // UI interaction goes here
                        Console.WriteLine("TIMER~~~");
                        await getLocation();

                        post_gps();
                        //checknowifi();
                    });
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("ONERROR");
                    //Console.WriteLine(ex.ToString());
                    //DisplayAlert(param.SYSYTEM_MESSAGE, param.LOCATION_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                }
            });
            return true;
        }
        bool OnTimerTick2()
        {
            Task.Run(() =>
            {
                try
                {
                    // Run code here
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        // UI interaction goes here
                        await getLocation2();
                        post_gps();
                    });
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("ONERROR");
                    //Console.WriteLine(ex.ToString());
                    //DisplayAlert(param.SYSYTEM_MESSAGE, param.LOCATION_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                }
            });
            return true;
        }

        //private double Distance(double lat1, double lon1, double lat2, double lon2, char unit)
        //{
        //    if ((lat1 == lat2) && (lon1 == lon2))
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        double theta = lon1 - lon2;
        //        double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
        //        dist = Math.Acos(dist);
        //        dist = rad2deg(dist);
        //        dist = dist * 60 * 1.1515;
        //        if (unit == 'K')
        //        {
        //            dist = dist * 1.609344;
        //        }
        //        else if (unit == 'N')
        //        {
        //            dist = dist * 0.8684;
        //        }
        //        return (dist);
        //    }
        //}

        //private double deg2rad(double deg)
        //{
        //    return (deg * Math.PI / 180.0);
        //}

        //private double rad2deg(double rad)
        //{
        //    return (rad / Math.PI * 180.0);
        //}

        private void MapUiSetting()
        {
            MyMap.MyLocationEnabled = true;

            //Enables or disables the zoom controls.
            MyMap.UiSettings.ZoomControlsEnabled = true;

            //Sets the preference for whether zoom gestures should be enabled or disabled.
            MyMap.UiSettings.ZoomGesturesEnabled = true;

            //Sets the preference for whether scroll gestures should be enabled or disabled.
            MyMap.UiSettings.ScrollGesturesEnabled = true;

            //Gets whether tilt gestures are enabled/disabled.
            MyMap.UiSettings.TiltGesturesEnabled = true;

            //Gets whether rotate gestures are enabled/disabled.
            MyMap.UiSettings.RotateGesturesEnabled = true;

            MyMap.UiSettings.MyLocationButtonEnabled = true;
            if(MainPage.AUTH == "6")
            {
                if (MainPage.userList.daily_shipment_nums > 0)
                {
                    DeliverMap.MyLocationEnabled = true;

                    //Enables or disables the zoom controls.
                    DeliverMap.UiSettings.ZoomControlsEnabled = true;

                    //Sets the preference for whether zoom gestures should be enabled or disabled.
                    DeliverMap.UiSettings.ZoomGesturesEnabled = true;

                    //Sets the preference for whether scroll gestures should be enabled or disabled.
                    DeliverMap.UiSettings.ScrollGesturesEnabled = true;

                    //Gets whether tilt gestures are enabled/disabled.
                    DeliverMap.UiSettings.TiltGesturesEnabled = true;

                    //Gets whether rotate gestures are enabled/disabled.
                    DeliverMap.UiSettings.RotateGesturesEnabled = true;

                    DeliverMap.UiSettings.MyLocationButtonEnabled = true;
                }
            }
            
        }
        // for社工身分且協助送餐的送餐地圖
        private void PinMarker(string resource, Xamarin.Forms.GoogleMaps.Position position, string label, string gps)
        {
            try
            {
                //Console.WriteLine("PIN");
                
                var pin = new Pin()
                {
                    Icon = ResourceToBitmap(resource),
                    Position = position,
                    Label = label,

                };
                // //Console.WriteLine("ICON" + );
                //Console.WriteLine("DDD");
                pin.Clicked += async (sender, e) =>
                {
                    //await Navigation.PushAsync(new CompanyDetailView(cmplist));
                    try
                    {
                        //Console.WriteLine("CLICK");
                        //Console.WriteLine("PINGPD" + gps);
                        string uri = "https://www.google.com.tw/maps/place/" + gps;
                        //Console.WriteLine("URI" + uri);
                        if (await Launcher.CanOpenAsync(uri))
                        {
                            await Launcher.OpenAsync(uri);
                        }
                        else
                        {
                            await DisplayAlert(param.SYSYTEM_MESSAGE, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.ToString());
                    }
                };
                //Console.WriteLine("PINADD");
                DeliverMap.Pins.Add(pin);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }
        // for社工身分的地圖
        private void PinMarker2(string resource, Xamarin.Forms.GoogleMaps.Position position, string label, string gps, string gender, string bday, string phone, string cellphone)
        {
            try
            {
                //Console.WriteLine("PIN");

                var pin = new Pin()
                {
                    Icon = ResourceToBitmap(resource),
                    Position = position,
                    Label = label,
                    //Address = bday
                };
                // //Console.WriteLine("ICON" + );
                //Console.WriteLine("pingender~~~" + gender);
                //Console.WriteLine("pinbday~~~" + bday);
                //Console.WriteLine("pinphone~~~" + phone);
                //Console.WriteLine("DDD");
                if(gender == "F")
                {
                    gendertxt = "男";
                }
                else
                {
                    gendertxt = "女";
                }
                pin.Clicked += async (sender, e) =>
                {
                    //await Navigation.PushAsync(new CompanyDetailView(cmplist));
                    try
                    {
                        //Console.WriteLine("CLICK");
                        //Console.WriteLine("PINGPD" + gps);
                        string uri = "https://www.google.com.tw/maps/place/" + gps;
                        //Console.WriteLine("URI" + uri);
                        if (await Launcher.CanOpenAsync(uri))
                        {
                            // await DisplayAlert(param.RESERVE_INFO_NAME, gender, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                            btnGPS = gps;
                            InfoWindow.IsVisible = true;
                            closebtn.IsVisible = true;
                            navigatebtn.IsVisible = true;
                            clnName.Text = label;
                            clnGender.Text = "性別: " + gendertxt;
                            clnBday.Text = "生日: " + bday;
                            clnPhone.Text = "電話: " +phone;
                            clnCellphone.Text = "手機: " + cellphone;
                        }

                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.ToString());
                    }
                };
                //Console.WriteLine("PINADD");
                MyMap.Pins.Add(pin);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }

        // for外送員的地圖
        private void PinMarker3(string resource, Xamarin.Forms.GoogleMaps.Position position, string label, string gps)
        {
            try
            {
                //Console.WriteLine("PIN");

                var pin = new Pin()
                {
                    Icon = ResourceToBitmap(resource),
                    Position = position,
                    Label = label,

                };
                // //Console.WriteLine("ICON" + );
                //Console.WriteLine("DDD");
                pin.Clicked += async (sender, e) =>
                {
                    //await Navigation.PushAsync(new CompanyDetailView(cmplist));
                    try
                    {
                        //Console.WriteLine("CLICK");
                        //Console.WriteLine("PINGPD" + gps);
                        string uri = "https://www.google.com.tw/maps/place/" + gps;
                        //Console.WriteLine("URI" + uri);
                        if (await Launcher.CanOpenAsync(uri))
                        {
                            await Launcher.OpenAsync(uri);
                        }
                        else
                        {
                            await DisplayAlert(param.SYSYTEM_MESSAGE, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.ToString());
                    }
                };
                //Console.WriteLine("PINADD");
                MyMap.Pins.Add(pin);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }

        private void closebtn_Clicked(object sender, EventArgs e)
        {
            InfoWindow.IsVisible = false;
        }
        private  async void navigatebtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                //var button = (Button)sender;
                //var classId = button.ClassId;
                ////Console.WriteLine("text~~~" + button.Text);
                ////Console.WriteLine("classId~~~" + classId);
                //for (int i = 0; i < allclientList.Count; i++) // 社工map
                //{
                //    if(classId == allclientList[i].ct01 + allclientList[i].ct02)
                //    {
                //        gps = allclientList[i].ct16 + "," + allclientList[i].ct17;
                //    }
                //}

                //Console.WriteLine("CLICK");
                //Console.WriteLine("PINGPD~~gps~~" + btnGPS);
                string uri = "https://www.google.com.tw/maps/place/" + btnGPS;
                //Console.WriteLine("URI" + uri);
                if (await Launcher.CanOpenAsync(uri))
                {
                    await Launcher.OpenAsync(uri);
                }
                else
                {
                    await DisplayAlert(param.SYSYTEM_MESSAGE, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }
        //private void PinMarker2(string resource, Xamarin.Forms.GoogleMaps.Position position, string label, string gps, string gender, string bday, string phone)
        //{
        //    try
        //    {
        //        //Console.WriteLine("PIN222~~");
        //        var pin = new Pin()
        //        {
        //            Icon = ResourceToBitmap(resource),
        //            Position = position,
        //            Label = label,
        //            Address = gender
        //        };
        //        // //Console.WriteLine("ICON" + );
        //        //Console.WriteLine("DDD");
        //        pin.Clicked += async (sender, e) =>
        //        {
        //            //await Navigation.PushAsync(new CompanyDetailView(cmplist));
        //            try
        //            {
        //                ////Console.WriteLine("CLICK");
        //                ////Console.WriteLine("PINGPD" + gps);
        //                //string uri = "https://www.google.com.tw/maps/place/" + gps;
        //                ////Console.WriteLine("URI" + uri);
        //                //if (await Launcher.CanOpenAsync(uri))
        //                //{
        //                //    await Launcher.OpenAsync(uri);
        //                //}
        //                //else
        //                //{
        //                //    await DisplayAlert(param.SYSYTEM_MESSAGE, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
        //                //}
        //                InfoWindow.IsVisible = true;
        //                clnName.Text = label;
        //                clnGender.Text = gender;
        //                clnBday.Text = bday;
        //                clnPhone.Text = phone;
        //            }
        //            catch (Exception ex)
        //            {
        //                //Console.WriteLine(ex.ToString());
        //            }
        //        };
        //        //Console.WriteLine("PINADD");
        //        DeliverMap.Pins.Add(pin);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Console.WriteLine(ex.ToString());
        //    }
        //}


        //private async Task get_dailyShipment()
        //{
        //    try
        //    {
        //        //Console.WriteLine("GDIN");
        //        totalList = await web.Get_Daily_Shipment(MainPage.token);
        //        //Console.WriteLine("TOTAL" + totalList.daily_shipments);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        await DisplayAlert("shipment_error", "message :\n" + ex.Message + "\n" +
        //            "stackTrace :\n" + ex.StackTrace, "ok");
        //    }
        //}
        public void Messager2()
        {
            try
            {
                MessagingCenter.Send(this, "CLOSE_INFORM", true);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }

        public void Messager3()
        {
            try
            {
                MessagingCenter.Send(this, "CLOSE_OUTFORM", true);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }

        private void Messager4()
        {
            try
            {
                MessagingCenter.Send(this, "SET_TMP_FORM", true);

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }

        private void Messager()
        {
            try
            {
                
                MessagingCenter.Subscribe<HomeView, bool>(this, "SET_MAP", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {

                        //Console.WriteLine("MAPPPPPPP");
                     
                        buttonhelp.IsVisible = false;
                        buttonhelp.IsEnabled = false;
                        buttondeliver.IsEnabled = false;
                        buttondeliver.IsVisible = false;
                        setView();
                    }
                });
                MessagingCenter.Subscribe<HomeView2, bool>(this, "SET_MAP", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        //Console.WriteLine("MAPPPPPPP");

                        buttonhelp.IsVisible = false;
                        buttonhelp.IsEnabled = false;
                        buttondeliver.IsEnabled = false;
                        buttondeliver.IsVisible = false;
                        setView();
                    }
                });
                MessagingCenter.Subscribe<HomeViewHelperAndDiliver, bool>(this, "SET_MAP", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        //Console.WriteLine("MAPPPPPPP");

                        buttonhelp.IsVisible = false;
                        buttonhelp.IsEnabled = false;
                        buttondeliver.IsEnabled = false;
                        buttondeliver.IsVisible = false;
                        setView();
                    }
                });
                
                MessagingCenter.Subscribe<MapView, bool>(this, "CLOSE_INFORM", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        fadeformin();
                    }
                });
                MessagingCenter.Subscribe<MapView, bool>(this, "CLOSE_OUTFORM", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        fadeformout();
                        formclose();
                    }
                });
                
                MessagingCenter.Subscribe<MemberView, bool>(this, "LOGOUT", (sender, arg) =>
                {
                    if(arg)
                    {
                        clientList = null;
                        //cList.Clear();

                        cList2.Clear();
                        
                        totalList = null;
                        allclientList.Clear();
                        MyMap.Pins.Clear();
                        DeliverMap.Pins.Clear();
                        punchList.Clear();
                        punch_in.Clear();
                        punch_out.Clear();
                        isform.Clear();
                        Navigation.PushAsync(new MapView());
                    }
                });
                
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }

        public void yesckeckbox(object sender, CheckedChangedEventArgs e)
        {
            bool yes = e.Value;

        }

        protected override void OnAppearing()
        {
            //setView();
            base.OnAppearing();
            //clientList = null;
            //cList = new List<ClientInfo>();
            //cList2 = new List<ClientInfo>();
            //totalList = new TotalList();
            //shipList = new daily_shipment();
            //allclientList = new List<AllClientInfo>();
        }

        //lock the previous page
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}
using Deliver.Models;
using Deliver.Models.DataInfo;
using Deliver.Services;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
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
        public static TotalList totalList = new TotalList();

        //public static daily_shipment shipList = new daily_shipment();
        public static List<AllClientInfo> allclientList = new List<AllClientInfo>(); // for auth = 6 社工
        public static List<questionnaire> questionnaireslist = new List<questionnaire>();

        WebService web = new WebService();
        ParamInfo param = new ParamInfo();
        private double lat = 0;
        private double lot = 0;
        private string home;
        private string s_num;
        private string gps;
        private string gps2;
        private string bday;
        private string phone;
        private string cellphone;
        private string gender;
        private int mapcount = 0;
        string ct_s_num = "";
        string sec_s_num = "";
        string mlo_s_num = "";
        private string reh_s_num = "";
        private string bn_s_num = "";
        private string name;
        private List<checkInfo> checkList2 = new List<checkInfo>();
        private string Clname;
        private int which;
        private int FINAL;
        //public static bool isform = false;
        //static string[] punchList;
        Dictionary<string, bool> punchList = new Dictionary<string, bool>(); // 判斷簽到+簽退都成功的
        Dictionary<string, bool> punch_in = new Dictionary<string, bool>(); // 判斷簽到成功的
       // Dictionary<string, bool> tmp_punch_in = new Dictionary<string, bool>(); // 
        //Dictionary<string, bool> tmp_punch_out = new Dictionary<string, bool>(); // 
        Dictionary<string, bool> punch_out = new Dictionary<string, bool>(); // 判斷簽退成功的
        Dictionary<string, bool> isform = new Dictionary<string, bool>(); // 判斷跳出問卷的
        Dictionary<string, bool> gomap = new Dictionary<string, bool>(); // 判斷導航的
        Dictionary<string, bool> punchyesorno = new Dictionary<string, bool>(); // 判斷是否進入判斷打卡(無論打卡成功與否)
        Queue<PunchInfo> PunchInfoQueue = new Queue<PunchInfo>();
        public static TempDatabase AccDatabase; // 紀錄問卷的
        //public static PunchDatabase PunchDatabase; // 記錄無網路環境打卡的
        //public static PunchDatabase2 PunchDatabase2; // 紀錄案主家打卡進度的(setnum)
        //public static PunchDataBaseTmp PunchTmp;  // 紀錄無網路環境下，後來自動簽到成功的
        //public static PunchDataBaseTmp2 PunchTmp2; // 紀錄無網路環境下，後來自動簽退成功的
        //public static Wifi_Punchout_Database Wifi_Punchout_DB; // 紀錄有網路環境下，簽退成功紀錄
        //public static Wifi_Punchin_Database Wifi_Punchin_DB; // 紀錄有網路環境下，簽到成功紀錄
        //public static PunchYesOrNo PunchYN; // 紀錄是否進入判斷打卡(無論打卡成功與否)
       // public static string entrytxt;
        //public static int number;
        private bool isSet = false;
        //public static int TmpID;
        //public static string TmpNum;
        //Dictionary<string, int> Temp = new Dictionary<string, int>();
        //public static int id;
        //public static bool ispress = false;
        //public static string homenum;
        //public static string homename;
        private double px;
        private double py;
        private string pa;
        private string pb;
        private double dx;
        private double dy;
        private double d;
        private int setnum;
        public static double NowLon;
        public static double NowLat;
        private string inorout; // 判斷SQLite為簽到還是簽退
        private string gendertxt; // for社工地圖?!
        public static List<string> name_list_in = new List<string>(); // 紀錄處理無網路簽到成功
        public static List<string> WIFI_name_list_in = new List<string>(); // 紀錄處理有網路簽到成功
        //public static List<TmpPunchList> name_list_in2 = new List<TmpPunchList>();
        public static List<string> name_list_out = new List<string>(); // 記錄處理無網路簽退成功
        public static List<string> WIFI_name_list_out = new List<string>(); // 記錄處理有網路簽退成功

        //public static List<TmpPunchList> name_list_out2 = new List<TmpPunchList>();
        //public static List<int> trylist2;
        //public static bool TmpPunch;
        private string btnGPS;
        private string num;
        //public static List<string> total_reserve_name = new List<string>();
        //public static bool DeliverOver = false; // for判斷是否顯示送餐完畢
        private Label label_name = new Label();
        private Label label_wqh = new Label();
        private Label label_qh = new Label();
        private StackLayout stack = new StackLayout();
        private Label label_que_name = new Label();
        private StackLayout stack_ques = new StackLayout();
        private checkInfo check3 = new checkInfo();
        private CheckBox check_box = new CheckBox();
        private Label label_check = new Label();
        private StackLayout stack_check = new StackLayout();
        private StackLayout final_stack = new StackLayout();
        private Frame frame = new Frame();
        private CameraPosition cameraPosition;

        private IEnumerable<Punch> punchdatatable;
        bool no_wifi_punch_in;
        bool no_wifi_punch_out;
        string d2;
        string time;
        bool wifi_punch_in;
        bool wifi_punch_out;
        bool successIn;
        bool successOut;
        //public static string NOWHOME; // for拍照上傳帶案主家編號
        private static string MQTTREH;  // for mqtt gps info帶reh_s_num
        private string dys09; // 早上 1 , 晚上 2
        private int count = 0;
        private string googleMapUrl; // 存要導到google map整條路線導航的url
        public static double nowlat;
        public static double nowlon;
        private List<string> Urllist = new List<string>();
        private List<string> GPSlist = new List<string>();
        public static List<string> Urlname = new List<string>();

        public MapView()
        {
            InitializeComponent();
            AccDatabase = new TempDatabase();
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
            if(MainPage.AUTH == "14") // 外送員(有打卡功能)
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTick);
                Device.StartTimer(TimeSpan.FromSeconds(5), OnTimerTick2);
                Device.StartTimer(TimeSpan.FromSeconds(3), OnTimerTick_for_PunchInfo);
                //Device.StartTimer(TimeSpan.FromSeconds(10), OnTimerTick_for_movemap);
              
                //Console.WriteLine("shipment~~~");
            }
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

       

        private Button supplyBtn(string supply)
        {
            try
            {
                var button = new Button
                {
                    Text = supply,
                    BackgroundColor = Color.FromHex("#FD5523"),
                    TextColor = Color.FromHex("#FFFFFF"),
                    FontAttributes = FontAttributes.Bold,
                    HeightRequest = 52,
                    WidthRequest = 52,
                    CornerRadius = 50,
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button))
                };

                button.Clicked += async (sender, args) =>
                {
                    string btnnumber = ((Button)sender).Text;
                    Console.WriteLine("btnnumber~~~ " + btnnumber);
                    //location = CrossGeolocator.Current;
                    //Console.WriteLine("location~~ " + location);
                    //location.DesiredAccuracy = location_DesiredAccuracy;
                    //Console.WriteLine("location.DesiredAccuracy~~ " + location.DesiredAccuracy);
                    //position = await location.GetPositionAsync(TimeSpan.FromSeconds(1));
                    //Console.WriteLine("position~~ " + position);
                    //var nowlon = position.Longitude;
                    //var nowlat = position.Latitude;
                    //var nowlon = "120.14222";
                    //var nowlat = "23.25555";
                    //Console.WriteLine("nowlat" + nowlat);
                    //Console.WriteLine("nowlon" + nowlon);
                    //all_navigate_button_Clicked(btnnumber, nowlat, nowlon);
                    //Console.WriteLine("MainPage.Finallist QQQQ~~ " + MainPage.Finallist[Int32.Parse(btnnumber)]);
                    //DisplayAlert("msgbtn", GPSlist[0], "ok");
                    if (GPSlist != null)
                    {
                        //string uri = "https://www.google.com.tw/maps/dir/" + MainPage.googleMapUrl;
                        string uri = "https://www.google.com.tw/maps/dir/" + NowLat.ToString() + ',' + NowLon.ToString() + '/' + GPSlist[Int32.Parse(btnnumber)];
                        Console.WriteLine("URI" + uri);
                        Console.WriteLine(await Launcher.CanOpenAsync(uri));
                        if (await Launcher.CanOpenAsync(uri))
                        {
                            await Launcher.OpenAsync(uri);

                        }
                        else
                        {
                            await DisplayAlert(param.SYSYTEM_MESSAGE, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                        }
                    }

                };

                return button;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        private async Task<List<string>> getUrl(TotalList totalList)
        {

            //if (MainPage.AUTH == "14")
            //{

            //    //Console.WriteLine("外送員~~~~");
            //    MyMap.IsVisible = true;
            //    MyMap.IsEnabled = true;
            //    //Console.WriteLine("AUTH " + MainPage.AUTH);
            //    //Console.WriteLine("timemap~~~ " + MainPage._time);
            //    if (MainPage._time == "早上") // 早上跟下午用不同api
            //    {
            //        dys09 = "1";
            //        totalList = await web.Get_Daily_Shipment(MainPage.token);
            //        if (totalList != null)
            //        {
            //            MQTTREH = totalList.daily_shipments[0].reh_s_num;

            //        }
            //        else
            //        {
            //            totalList = await web.Get_Daily_Shipment(MainPage.token);
            //            MQTTREH = totalList.daily_shipments[0].reh_s_num;
            //        }

            //    }
            //    else
            //    {
            //        dys09 = "2";
            //        totalList = await web.Get_Daily_Shipment_night(MainPage.token);
            //        if (totalList != null)
            //        {
            //            MQTTREH = totalList.daily_shipments[0].reh_s_num;
            //        }
            //        else
            //        {
            //            totalList = await web.Get_Daily_Shipment_night(MainPage.token);
            //            MQTTREH = totalList.daily_shipments[0].reh_s_num;
            //        }
            //    }
            //}
            try
            {
                Urllist.Clear();
                Urlname.Clear();
               
                //Console.WriteLine("QQQQcount~~~ ");
                //Console.WriteLine(totalList.daily_shipments.Count);
                //DisplayAlert("msgTotal", totalList.daily_shipments.Count().ToString(), "ok");
                for (int i = 0; i < totalList.daily_shipments.Count; i++)
                {

                    if (!Urlname.Contains(totalList.daily_shipments[i].ct_name))
                    {
                        Urlname.Add(totalList.daily_shipments[i].ct_name);
                        //Console.WriteLine("~~~~~");
                        //Console.WriteLine(totalList.daily_shipments[i].ct_name);
                        //Console.WriteLine("EE" + totalList.daily_shipments[i].ct16);

                        if (totalList.daily_shipments[i].ct16.Equals("0") == false && totalList.daily_shipments[i].ct17.Equals("0") == false)
                        {
                            //Console.WriteLine("AA" + totalList.daily_shipments[i].ct16);

                            if (i == 0)
                            {
                                // 過濾掉志工經緯度為0(google map會找不到點)
                                //Console.WriteLine("AAA");
                                //Console.WriteLine(totalList.daily_shipments[i].ct_name);
                                //Console.WriteLine(i);
                                ////googleMapUrl = nowlat.ToString() + ',' + nowlon.ToString() + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                                //Console.WriteLine(String.IsNullOrEmpty(totalList.daily_shipments[i].dys21));
                                //Console.WriteLine(totalList.daily_shipments[i].dys21);
                                if (String.IsNullOrEmpty(totalList.daily_shipments[i].dys21) == true)
                                {
                                    googleMapUrl = totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                                }

                            }
                            else
                            {
                               // Console.WriteLine("AAB");
                                //Console.WriteLine(i);
                               // Console.WriteLine(totalList.daily_shipments[i].ct_name);
                               // Console.WriteLine(String.IsNullOrEmpty(totalList.daily_shipments[i].dys21));
                               // Console.WriteLine(totalList.daily_shipments[i].dys21);
                                if (String.IsNullOrEmpty(totalList.daily_shipments[i].dys21) == true)
                                {
                                    googleMapUrl = googleMapUrl + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                                }

                                // Console.WriteLine(googleMapUrl);
                            }
                            //googleMapUrl = googleMapUrl + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                        }
                        else
                        {
                            //Console.WriteLine("BB");
                            if (i == 0)
                            {
                                //Console.WriteLine("BBB");
                                //Console.WriteLine(i);
                                // 過濾掉志工經緯度為0(google map會找不到點)
                                totalList.daily_shipments[i].ct16 = "";
                                totalList.daily_shipments[i].ct17 = "";
                                googleMapUrl = totalList.daily_shipments[i].ct16 + totalList.daily_shipments[i].ct17;
                                //googleMapUrl = nowlat.ToString() + ',' + nowlon.ToString() + '/' + totalList.daily_shipments[i].ct16 + totalList.daily_shipments[i].ct17;
                                // Console.WriteLine(googleMapUrl);
                                //googleMapUrl = totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            }
                            else
                            {
                                //Console.WriteLine("XXXX");
                                //Console.WriteLine(i);
                               // Console.WriteLine(String.IsNullOrEmpty(totalList.daily_shipments[i].dys21));
                              //  Console.WriteLine(totalList.daily_shipments[i].dys21);
                                if (String.IsNullOrEmpty(totalList.daily_shipments[i].dys21) == true)
                                {
                                    googleMapUrl = googleMapUrl + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                                }


                            }


                        }
                        //Console.WriteLine("i~~ ");
                        //Console.WriteLine(i);
                        //Console.WriteLine(Urlname.Count);
                        //Console.WriteLine(Urlname.Count % 23);
                        //Console.WriteLine(totalList.daily_shipments.Count - 1);
                        //Console.WriteLine(i == totalList.daily_shipments.Count - 1);
                        if (Urlname.Count % 23 == 0 && Urlname.Count != 0)
                        {
                            //Console.WriteLine("WWWWWW" + i);
                            //Console.WriteLine(googleMapUrl);
                            Urllist.Add(googleMapUrl);
                            googleMapUrl = "";
                        }
                        else
                        {
                            //Console.Write("EEEEE" + i);
                            if (Urlname.Count % 23 != 0 && i == totalList.daily_shipments.Count - 1)
                            {
                                //Console.Write("RRRR" + i);
                                //Console.WriteLine(googleMapUrl);
                                Urllist.Add(googleMapUrl);
                            }
                        }
                    }
                    else
                    {
                        if (Urlname.Count % 23 != 0 && i == totalList.daily_shipments.Count - 1)
                        {
                            //Console.Write("RRRR" + i);
                            //Console.WriteLine(googleMapUrl);
                            Urllist.Add(googleMapUrl);
                        }
                    }
                    //Console.WriteLine("i~~ ");
                    //Console.WriteLine(i);
                    //Console.WriteLine(i % 23);

                    //if (i % 23 == 0)
                    //{
                    //    Console.WriteLine("inAAA~~~ ");
                    //    Urllist[i / 23] = googleMapUrl;
                    //    Console.WriteLine("WEEEEEEE");
                    //    Console.WriteLine(Urllist[i / 23]);
                    //    googleMapUrl = "";
                    //}
                    //else
                    //{
                    //    continue;
                    //}

                }
                //for (int i = 0; i < Urlname.Count; i++)
                //{
                //    Console.WriteLine("countAAA" + Urlname.Count);
                //    Console.WriteLine("AAAA" + Urlname[i]);
                //    Console.WriteLine("i~~~~" + i);

                //}
                //for (int i = 0; i < Urllist.Count; i++)
                //{
                //    Console.WriteLine("countTTTT" + Urllist.Count);
                //    Console.WriteLine("TTTT" + Urllist[i]);
                //}
                return Urllist;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        private async void setView()
        {
            try
            {

                //if (totalList.daily_shipments == null)
                //{

                //location = CrossGeolocator.Current;
                //location.DesiredAccuracy = location_DesiredAccuracy;
                //position = await location.GetPositionAsync();
                //var nowlon = position.Longitude;
                //var nowlat = position.Latitude;
              
                questionnaireslist = await web.Get_Questionaire(MainPage.token); // 拿問卷
                    
                    
                   
                    //Console.WriteLine("TOKEN" +MainPage.token);
                   
                    if (MainPage.AUTH == "14")
                    {

                        //Console.WriteLine("外送員~~~~");
                        MyMap.IsVisible = true;
                        MyMap.IsEnabled = true;
                        //Console.WriteLine("AUTH " + MainPage.AUTH);
                        //Console.WriteLine("timemap~~~ " + MainPage._time);
                        if(MainPage._time == "早上") // 早上跟下午用不同api
                        {
                            dys09 = "1";
                            totalList = await web.Get_Daily_Shipment(MainPage.token);
                            if (totalList != null) {
                                MQTTREH = totalList.daily_shipments[0].reh_s_num;
                               
                            } else {
                                totalList = await web.Get_Daily_Shipment(MainPage.token);
                                MQTTREH = totalList.daily_shipments[0].reh_s_num;
                            }
                       
                        }
                        else
                        {
                            dys09 = "2";
                            totalList = await web.Get_Daily_Shipment_night(MainPage.token);
                            if (totalList != null)
                            {
                                MQTTREH = totalList.daily_shipments[0].reh_s_num;
                            }
                            else
                            {
                                totalList = await web.Get_Daily_Shipment_night(MainPage.token);
                                MQTTREH = totalList.daily_shipments[0].reh_s_num;
                            }
                        }
                    qrStack.Children.Clear();
                    if(totalList != null)
                    {
                        GPSlist = await getUrl(totalList);
                    } else
                    {
                        if (MainPage._time == "早上") // 早上跟下午用不同api
                        {
                            dys09 = "1";
                            totalList = await web.Get_Daily_Shipment(MainPage.token);
                            if (totalList != null)
                            {
                                MQTTREH = totalList.daily_shipments[0].reh_s_num;

                            }
                            else
                            {
                                totalList = await web.Get_Daily_Shipment(MainPage.token);
                                MQTTREH = totalList.daily_shipments[0].reh_s_num;
                            }

                        }
                        else
                        {
                            dys09 = "2";
                            totalList = await web.Get_Daily_Shipment_night(MainPage.token);
                            if (totalList != null)
                            {
                                MQTTREH = totalList.daily_shipments[0].reh_s_num;
                            }
                            else
                            {
                                totalList = await web.Get_Daily_Shipment_night(MainPage.token);
                                MQTTREH = totalList.daily_shipments[0].reh_s_num;
                            }
                        }
                        GPSlist = await getUrl(totalList);
                    }
                    
                    //DisplayAlert("msg", GPSlist[0], "ok");
                    //DisplayAlert("msg22", GPSlist.Count().ToString(), "ok");
                    var btnumber = Urlname.Count/23;
                    Console.WriteLine("btnumber");
                    Console.WriteLine(btnumber);
                    //Console.WriteLine(MainPage.Urlname.Count);
                    //Console.WriteLine(btnumber Mod 23);
                    if(Urlname.Count % 23 == 0)
                    {
                        Console.WriteLine("qwe~~ ");
                        for (int i = 0; i < btnumber; i++)
                        {
                            qrStack.Children.Add(supplyBtn(i.ToString()));
                        }
                    } else
                    {
                        Console.WriteLine("asd~~~ ");
                        for (int i = 0; i < btnumber + 1; i++)
                        {
                            qrStack.Children.Add(supplyBtn(i.ToString()));
                        }
                    }

                    for (int i = 0; i < totalList.daily_shipments.Count; i++)
                    {

                        //Console.WriteLine(totalList.daily_shipments[i].ct_name);
                        Console.WriteLine(totalList.daily_shipments[i].ct16);
                        Console.WriteLine(totalList.daily_shipments[i].ct17);
                    }

                    //Console.WriteLine("nnnn~~~ " + totalList.daily_shipments.Count());


                    //clientList = await web.Get_Client(MainPage.token);
                    //Console.WriteLine("DATA2~" + clientList.Count());


                    /*
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
                    */
                    //Console.WriteLine("total_need_to_serve~~ " + total_reserve_name.Count());
                    //cList2.Reverse();
                    //for (int s = 0; s < cList2.Count(); s++)
                    //{
                    //Console.WriteLine("PPPP" + cList2[s].ct_name);
                    //}
                    //Console.WriteLine("SSSS" + cList2.Count);
                    //totalList = await web.Get_Daily_Shipment(MainPage.token);


                    //Console.WriteLine("DATA3~ " + clientList.Count());
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


                        //Console.WriteLine("loginway~~" + MainPage.Loginway);
                        // 輸入帳號登入
                        //DisplayAlert("way", MainPage.Loginway, "ok");
                            if (MainPage.Loginway == "Enter")
                            {
                            //DisplayAlert("msg", "bb" + totalList.daily_shipments.Count(), "ok");
                            AccDatabase.DeleteAll_Punch(); // 記錄無網路環境打卡的
                                AccDatabase.DeleteAll_Punch2(); // 紀錄案主家打卡進度的(setnum)
                                AccDatabase.DeleteAll_PunchTmp(); // 紀錄無網路環境下，後來自動簽到成功的
                                AccDatabase.DeleteAll_PunchTmp2(); // 紀錄無網路環境下，後來自動簽退成功的
                                
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
                        Console.WriteLine("url_A");
                        //SetIcon(setnum); // 地圖上設案主標點(只存在下一家要送餐的)
                        for (int i = 0; i < totalList.daily_shipments.Count; i++)
                                {
                            //location.DesiredAccuracy = location_DesiredAccuracy;
                            //position = await location.GetPositionAsync();
                            //var nowlon = position.Longitude;
                            //var nowlat = position.Latitude;
                            //Console.WriteLine(totalList.daily_shipments[i].ct16);
                            //Console.WriteLine(totalList.daily_shipments[i].ct16.Equals('0'));
                            //if (i == 0)
                            //{
                            //    // 過濾掉志工經緯度為0(google map會找不到點)
                            //    if (!totalList.daily_shipments[i].ct16.Equals("0"))
                            //    {
                            //        Console.WriteLine("AA");
                            //        googleMapUrl = nowlat.ToString() + ',' + nowlon.ToString() + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //    }
                            //    else
                            //    {
                            //        Console.WriteLine("AB");
                            //        googleMapUrl = nowlat.ToString() + ',' + nowlon.ToString() + '/';
                            //    }

                            //    //googleMapUrl = totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //}
                            //else
                            //{
                            //    if (!totalList.daily_shipments[i].ct16.Equals("0"))
                            //    {
                            //        googleMapUrl = googleMapUrl + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //    }


                            //}

                            //if (i == 0)
                            //{
                            //    // 過濾掉志工經緯度為0(google map會找不到點)
                            //    Console.WriteLine("AAA");
                            //    Console.WriteLine(i);
                            //    googleMapUrl = nowlat.ToString() + ',' + nowlon.ToString() + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //    Console.WriteLine(googleMapUrl);
                            //    //googleMapUrl = totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //}
                            //else
                            //{
                            //    Console.WriteLine("AAB");
                            //    Console.WriteLine(i);
                            //    googleMapUrl = googleMapUrl + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //    Console.WriteLine(googleMapUrl);
                            //}


                            punchList[totalList.daily_shipments[i].ct_name] = false; // 判斷簽到+簽退都成功的
                                    punch_in[totalList.daily_shipments[i].ct_name] = false; // 判斷簽到成功的
                                    punch_out[totalList.daily_shipments[i].ct_name] = false; // 判斷簽退成功的
                                    isform[totalList.daily_shipments[i].ct_name] = false; // 判斷跳出問卷的
                                    gomap[totalList.daily_shipments[i].ct_name] = false; // 判斷導航的
                                    punchyesorno[totalList.daily_shipments[i].ct_name] = false; // 判斷是否進入判斷打卡(無論打卡成功與否)
                            double lat;
                            double lot;
                            Double.TryParse(totalList.daily_shipments[i].ct16, out lat);
                            Double.TryParse(totalList.daily_shipments[i].ct17, out lot);
                           // double lat = Convert.ToDouble(totalList.daily_shipments[i].ct16);
                                    //Console.WriteLine("LAT" + lat);
                                   // double lot = Convert.ToDouble(totalList.daily_shipments[i].ct17);
                                    //Console.WriteLine("LOT" + lot);
                                    home = totalList.daily_shipments[i].ct_name + " 的家";
                                    s_num = totalList.daily_shipments[i].ct_s_num;
                                //Console.WriteLine("@@@@ " + s_num);
                                    ////Console.WriteLine("HOME" + home);
                                    gps = lat + "," + lot;
                                    //gender = allclientList[i].ct04; // 性別
                                    //bday = allclientList[i].ct05; // 生日
                                    //phone = allclientList[i].ct06_homephone; // 家裡電話
                                    //cellphone = allclientList[i].ct06_telephone; // 手機

                                    // 全部ICON都在map上
                                    //PinMarker(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);
                                    PinMarker3(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps, s_num);
                                    
                                }
                                /*
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
                                */
                            }
                            else
                            {
                            //DisplayAlert("loginway", "autologin" + totalList.daily_shipments.Count, "ok");
                                // 自動登入
                                //Console.WriteLine("Auto~~~");
                                if (MainPage.dateDatabase.GetAccountAsync2().Count() != 0) // 如果紀錄登入日期的SQLite裡面有資料，先比對
                                {
                                    Console.WriteLine("old~~MMap~~~ " + wifiuploadrecord.oldday);
                                    Console.WriteLine("new~~~MMap~~~ " + MainPage._login_time);
                                    // 判斷上次登入日期跟這次登入日期
                                    // 若不同則刪除SQLite裡面的資料
                                    if (MainPage._login_time != wifiuploadrecord.oldday)
                                    {

                                        Console.WriteLine("newdayrecieve~~Mapview~~22~");
                                        AccDatabase.DeleteAll_TempAccount();  // 紀錄問卷的
                                        AccDatabase.DeleteAll_Punch(); // 記錄無網路環境打卡的
                                        //AccDatabase.DeleteAll_Punch2(); // 紀錄案主家打卡進度的(setnum)
                                        //if(AccDatabase.GetAccountAsync2_Punch2().Count() == 0)
                                        //{
                                        //    //Console.WriteLine("new_day~~no_setnum ");
                                        //}
                                        AccDatabase.DeleteAll_PunchTmp(); // 紀錄無網路環境下，後來自動簽到成功的
                                        AccDatabase.DeleteAll_PunchTmp2(); // 紀錄無網路環境下，後來自動簽退成功的
                                        //PunchYN.DeleteAll(); // 紀錄是否進入判斷打卡(無論打卡成功與否)
                                        AccDatabase.DeleteAll_Wifi_Punchin(); // 有網路簽到記錄
                                        AccDatabase.DeleteAll_Wifi_Punchout(); // 有網路簽退紀錄
                                        name_list_in.Clear(); // 紀錄處理無網路簽到成功
                                        name_list_out.Clear(); // 紀錄處理無網路簽退成功
                                        WIFI_name_list_in.Clear(); // 紀錄處理有網路簽到成功
                                        WIFI_name_list_out.Clear(); // 紀錄處理有網路簽退成功
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
                        //DisplayAlert("msg", "aa" + totalList.daily_shipments.Count, "ok");
                        Console.WriteLine("url_B");
                        for (int i = 0; i < totalList.daily_shipments.Count; i++)
                                {

                            //location.DesiredAccuracy = location_DesiredAccuracy;
                            //position = await location.GetPositionAsync();
                            //var nowlon = position.Longitude;
                            //var nowlat = position.Latitude;
                            //Console.WriteLine(totalList.daily_shipments[i].ct16.GetType());
                            //Console.WriteLine(totalList.daily_shipments[i].ct16.Equals("0"));
                            //Console.WriteLine(totalList.daily_shipments[i].ct16);
                            //if (totalList.daily_shipments[i].ct16.Equals("0") == false && totalList.daily_shipments[i].ct17.Equals("0") == false)
                            //{
                            //    Console.WriteLine("AA");
                            //    if (i == 0)
                            //    {
                            //        // 過濾掉志工經緯度為0(google map會找不到點)
                            //        Console.WriteLine("AAA");
                            //        Console.WriteLine(i);
                            //        googleMapUrl = nowlat.ToString() + ',' + nowlon.ToString() + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //        Console.WriteLine(googleMapUrl);
                            //        //googleMapUrl = totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //    }
                            //    else
                            //    {
                            //        Console.WriteLine("AAB");
                            //        Console.WriteLine(i);
                            //        googleMapUrl = googleMapUrl + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //        Console.WriteLine(googleMapUrl);
                            //    }
                            //} else
                            //{
                            //    Console.WriteLine("BB");
                            //    if (i == 0)
                            //    {
                            //        Console.WriteLine("BBB");
                            //        Console.WriteLine(i);
                            //        // 過濾掉志工經緯度為0(google map會找不到點)
                            //        googleMapUrl = nowlat.ToString() + ',' + nowlon.ToString() + '/';
                            //        Console.WriteLine(googleMapUrl);
                            //        //googleMapUrl = totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //    }else
                            //    {
                            //        Console.WriteLine("XXXX");
                            //        Console.WriteLine(i);
                            //    }
                               
                            //}

                            //if (i == 0)
                            //{
                            //    // 過濾掉志工經緯度為0(google map會找不到點)
                            //    Console.WriteLine("AAA");
                            //    Console.WriteLine(i);
                            //    googleMapUrl = nowlat.ToString() + ',' + nowlon.ToString() + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //    Console.WriteLine(googleMapUrl);
                            //    //googleMapUrl = totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //}
                            //else
                            //{
                            //    Console.WriteLine("AAB");
                            //    Console.WriteLine(i);
                            //    googleMapUrl = googleMapUrl + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            //    Console.WriteLine(googleMapUrl);
                            //}



                            punchList[totalList.daily_shipments[i].ct_name] = false;  // 判斷簽到+簽退都成功的
                                    punch_in[totalList.daily_shipments[i].ct_name] = false;  // 判斷簽到成功的
                                    punch_out[totalList.daily_shipments[i].ct_name] = false;  // 判斷簽退成功的
                                    isform[totalList.daily_shipments[i].ct_name] = false; // 判斷跳出問卷的
                                    gomap[totalList.daily_shipments[i].ct_name] = false; // 判斷導航的
                                    punchyesorno[totalList.daily_shipments[i].ct_name] = false; // 判斷是否進入判斷打卡(無論打卡成功與否)
                                    if (AccDatabase.GetAccountAsync2_Punch().Count() > 0) // 無網路環境下打卡的database裡面有資料
                                    {
                                        //Console.WriteLine("WWWW~~~~");
                                        //Console.WriteLine("pp~~" + PunchDatabase.GetAccountAsync2().Count());
                                        for (int b = 0; b < AccDatabase.GetAccountAsync2_Punch().Count(); b++)
                                        {
                                            var c = AccDatabase.GetAccountAsync_Punch(b);


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
                      
                                    if (AccDatabase.GetAccountAsync2_Wifi_Punchin().Count() > 0) // 有網路環境下打卡的database裡面有資料
                                    {
                                        //Console.WriteLine("WWWW~~~~");
                                        //Console.WriteLine("pp~~" + PunchDatabase.GetAccountAsync2().Count());
                                        for (int b = 0; b < AccDatabase.GetAccountAsync2_Wifi_Punchin().Count(); b++)
                                        {
                                            var c = AccDatabase.GetAccountAsync_Wifi_Punchin(b);


                                            foreach (var TempNameList in c)
                                            {
                                                //Console.WriteLine("tmpname1111~~~" + TempNameList.name);
                                                if (!name_list_in.Contains(TempNameList.name))
                                                {
                                                    // 把判斷無網路打卡的紀錄寫回去
                                                    name_list_in.Add(TempNameList.name);
                                                }
                                                if(!WIFI_name_list_in.Contains(TempNameList.name))
                                                {
                                                    WIFI_name_list_in.Add(TempNameList.name);
                                                }
                                                if (TempNameList.name == totalList.daily_shipments[i].ct_name)
                                                {
                                                    //punchList[totalList.daily_shipments[i].ct_name] = true;
                                                    punch_in[totalList.daily_shipments[i].ct_name] = true;
                                                    //punch_out[totalList.daily_shipments[i].ct_name] = true;
                                                }
                                            }
                                        }
                                    }
                                    //Console.WriteLine("in_length");
                                    //Console.WriteLine(name_list_in.Count());
                                    //Console.WriteLine("wifi_punch_out");
                                    //Console.WriteLine(AccDatabase.GetAccountAsync2_Wifi_Punchout().Count());
                                    if (AccDatabase.GetAccountAsync2_Wifi_Punchout().Count() > 0) // 有網路環境下打卡的database裡面有資料
                                    {
                                        //Console.WriteLine("WWWW~~~~");
                                        //Console.WriteLine("pp~~" + PunchDatabase.GetAccountAsync2().Count());
                                        for (int b = 0; b < AccDatabase.GetAccountAsync2_Wifi_Punchout().Count(); b++)
                                        {
                                            var c = AccDatabase.GetAccountAsync_Wifi_Punchout(b);


                                            foreach (var TempNameList in c)
                                            {
                                                //Console.WriteLine("tmpname2222~~~" + TempNameList.name);
                                            if (!name_list_out.Contains(TempNameList.name))
                                            {
                                                name_list_out.Add(TempNameList.name);
                                            }
                                            if (!WIFI_name_list_out.Contains(TempNameList.name))
                                            {
                                                WIFI_name_list_out.Add(TempNameList.name);
                                            }
                                            if (TempNameList.name == totalList.daily_shipments[i].ct_name)
                                                {
                                                    //punchList[totalList.daily_shipments[i].ct_name] = true;
                                                    //punch_in[totalList.daily_shipments[i].ct_name] = true;
                                                    punch_out[totalList.daily_shipments[i].ct_name] = true;
                                                }
                                            }
                                        }
                                    }
                                //Console.WriteLine("out_length");
                                //Console.WriteLine(name_list_out.Count());
                                    if (punch_out[totalList.daily_shipments[i].ct_name] == true && punch_in[totalList.daily_shipments[i].ct_name] == true)
                                    {
                                        punchList[totalList.daily_shipments[i].ct_name] = true;
                                    }
                            //double lat = Convert.ToDouble(totalList.daily_shipments[i].ct16);
                            double lat;
                            Double.TryParse(totalList.daily_shipments[i].ct16, out lat);
                                    Console.WriteLine("LAT" + totalList.daily_shipments[i].ct16);
                            Console.WriteLine(lat);
                            double lot;
                            //double lot = Convert.ToDouble(totalList.daily_shipments[i].ct17);
                            Double.TryParse(totalList.daily_shipments[i].ct17, out lot);
                                    ////Console.WriteLine("LOT" + lot);
                                    home = totalList.daily_shipments[i].ct_name + " 的家";
                                    s_num = totalList.daily_shipments[i].ct_s_num;
                                    //Console.WriteLine("@@@@ " + s_num);
                                    ////Console.WriteLine("HOME" + home);
                                    gps = lat + "," + lot;
                                    //gender = allclientList[i].ct04; // 性別
                                    //bday = allclientList[i].ct05; // 生日
                                    //phone = allclientList[i].ct06_homephone; // 家裡電話
                                    //cellphone = allclientList[i].ct06_telephone; // 手機

                                    // 全部ICON都在map上
                                    //PinMarker(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps);
                                    PinMarker3(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps, s_num);
                                }
                        Console.WriteLine("googleMapUrl");
                        Console.WriteLine(googleMapUrl);
                                /*
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
                                */
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
                    
                    


                //}
            }
            catch (Exception ex)
            {
                //Console.WriteLine("MAPA");
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
            //Console.WriteLine("NAMEEEE~~" + totalList.daily_shipments[setnum].ct_name);

            MyMap.Pins.Clear(); // 要加下一個點之前先把之前的點清掉
            PinMarker3(param.PNG_MAP_HOME_ICON, new Xamarin.Forms.GoogleMaps.Position(lat, lot), home, gps, s_num);

        }
        public async void SetIcon3(int setnum) 
        {
            //Console.WriteLine("AUTH " + MainPage.AUTH);
            //Console.WriteLine("countBB " + totalList.daily_shipments.Count);

            //Console.WriteLine("count" + totalList.daily_shipments.Count);
            //.WriteLine("SETNUM~~~~" + totalList.daily_shipments[setnum].ct16);
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
                //Console.WriteLine("LOCATION~~~~");
                //Console.WriteLine("INTERNET~~~~" + CrossConnectivity.Current.IsConnected);
                //Console.WriteLine("setnum3333~~~~" + setnum);
                //Console.WriteLine("tmpcount~~" + PunchTmp.GetAccountAsync2().Count());
                //Console.WriteLine("tmpcount2~~" + PunchTmp2.GetAccountAsync2().Count());

                // 判斷有無網路打卡紀錄，有則更新至memberview的listview
                if (AccDatabase.GetAccountAsync_PunchTmp().Count() > 0) // 無網路簽到記錄
                {
                    MessagingCenter.Send(this, "Setlist", true);
                    //Console.WriteLine("sendsetlist222~~~");
                }
                if (AccDatabase.GetAccountAsync_PunchTmp2().Count() > 0) // 無網路簽退紀錄
                {
                    MessagingCenter.Send(this, "Setlist2", true);
                    //Console.WriteLine("sendsetlist333~~~");
                }
                if (AccDatabase.GetAccountAsync3_Wifi_Punchin().Count() > 0) // 有網路簽到記錄
                {
                    MessagingCenter.Send(this, "wifi_Setlist_in", true);
                    //Console.WriteLine("sendsetlist222~~~");
                }
                if (AccDatabase.GetAccountAsync3_Wifi_Punchout().Count() > 0) // 有網路簽退紀錄
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
                //Console.WriteLine("location~~ " + location);
                try
                {
                    if (location != null)
                    {
                        //Console.WriteLine("location_in~~~ ");
                        try
                        {
                            d = 0;
                            mapcount = mapcount + 1;
                            location.DesiredAccuracy = location_DesiredAccuracy;
                            position = await location.GetPositionAsync(TimeSpan.FromSeconds(1));
                            NowLon = position.Longitude;
                            NowLat = position.Latitude;

                            //Console.WriteLine("nowlat" + position.Latitude);
                            //Console.WriteLine("nowlot" + position.Longitude);
                            if (mapcount == 0 || mapcount % 20 == 0)
                            {
                                //Console.WriteLine("mapcountin~~~" + mapcount);
                                cameraPosition = new CameraPosition(new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude), map_Zoom);
                                await MyMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition)); // 地圖上抓取目前位置
                                await DeliverMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
                                mapcount = 0;
                            }



                            // for無網路環境(不會及時跳出打卡成功訊息)
                            // 偵測到網路
                            // 先判斷SQLite有無資料
                            // if有資料判斷將其自動打卡
                            // 全都打卡完之後將SQLite delete all
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                if (AccDatabase.GetAccountAsync2_Punch().Count() > 0) // 記錄無網路環境打卡的database裡面有資料
                                {
                                    //Console.WriteLine("RRRRRR~~~~");
                                    //Console.WriteLine("pp~~" + AccDatabase.GetAccountAsync2_Punch().Count());
                                    for (int b = 0; b < AccDatabase.GetAccountAsync2_Punch().Count(); b++)
                                    {
                                        punchdatatable = AccDatabase.GetAccountAsync_Punch(b);


                                        foreach (var TempAnsList in punchdatatable)
                                        {

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
                                                        //Console.WriteLine("nowifi_in~~~");
                                                        //Console.WriteLine(TempAnsList.name);
                                                        //Console.WriteLine(TempAnsList.token);
                                                        //Console.WriteLine(TempAnsList.ct_s_num);
                                                        //Console.WriteLine(TempAnsList.sec_s_num);
                                                        //Console.WriteLine(TempAnsList.mlo_s_num);
                                                        //Console.WriteLine(TempAnsList.reh_s_num);
                                                        //Console.WriteLine(TempAnsList.latitude);
                                                        //Console.WriteLine(TempAnsList.longitude);
                                                        //Console.WriteLine(TempAnsList.time);
                                                        PunchIn punin = new PunchIn
                                                        {
                                                            token = TempAnsList.token,
                                                            ct_s_num = TempAnsList.ct_s_num,
                                                            sec_s_num = TempAnsList.sec_s_num,
                                                            mlo_s_num = TempAnsList.mlo_s_num,
                                                            reh_s_num = TempAnsList.reh_s_num,
                                                            latitude = TempAnsList.latitude.ToString(),
                                                            longitude = TempAnsList.longitude.ToString(),
                                                            time = TempAnsList.time,
                                                            phl50 = "2"
                                                        };
                                                        // post打卡訊息到mqtt
                                                        await Connected_punch(NowLat.ToString(), NowLon.ToString(), MQTTREH, "1", "2", TempAnsList.ct_s_num, TempAnsList.sec_s_num, TempAnsList.mlo_s_num, TempAnsList.time, "2", "1", "1", "1", dys09, "APP");
                                                        //no_wifi_punch_in = await web.Save_Punch_In(TempAnsList.token, TempAnsList.ct_s_num, TempAnsList.sec_s_num, TempAnsList.mlo_s_num, TempAnsList.reh_s_num, TempAnsList.latitude, TempAnsList.longitude, TempAnsList.time, "2");
                                                        // --------post 打卡訊息 to 後台--------------------
                                                        //no_wifi_punch_in = await web.Save_Punch_In(punin);
                                                        //----------------------------------------------

                                                        //Console.WriteLine("web_resin~~~ " + web_res2);
                                                        //if (no_wifi_punch_in == true)
                                                        //{
                                                            // 打卡成功
                                                            Console.WriteLine("nowifi_in_true~~~ ");
                                                            Console.WriteLine(TempAnsList.name);
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
                                                            AccDatabase.DeleteItem_Punch(TempAnsList.ID); // 把那筆刪掉
                                                                                                          //formin.IsVisible = true;
                                                                                                          //formin.IsEnabled = true;
                                                                                                          //await Task.Delay(10000); // 等待30秒
                                                                                                          //Messager2();
                                                            AccDatabase.DeleteItem_PunchTmp(TempAnsList.ID); // 把那筆刪掉

                                                            MessagingCenter.Send(this, "Setlist", true); // 更新主頁面的吳網路打卡紀錄
                                                                                                         //Console.WriteLine("deletein~~~" + TempAnsList.name);
                                                                                                         //Console.WriteLine("incount111~~~" + PunchDatabase.GetAccountAsync2().Count());
                                                            if (!WIFI_name_list_in.Contains(TempAnsList.name))
                                                            {
                                                                AccDatabase.SaveAccountAsync_Wifi_Punchin(new Wifi_Punchin // 存進有網路簽到成功的SQLite
                                                                {
                                                                    name = TempAnsList.name, // 案主姓名
                                                                    time = TempAnsList.timeforpost  // 簽到時間
                                                                });
                                                                WIFI_name_list_in.Add(TempAnsList.name);
                                                            }
                                                            //if (name_list_in.Count() == WIFI_name_list_in.Count())
                                                            //{
                                                            //    AccDatabase.DeleteAll_Punch2();
                                                            //    AccDatabase.DeleteAll_PunchTmp();
                                                            //}
                                                            //MessagingCenter.Send(this, "Setlist", true);
                                                        //}
                                                        //else
                                                        //{
                                                            //await DisplayAlert("FAIL", "打卡失敗in" + setName, "OK");
                                                            //Console.WriteLine("ASQLite簽到失敗");
                                                        //}
                                                    }
                                                    //else
                                                    //{
                                                    //    // 已經處理過的話就直接刪除SQLite中這筆紀錄
                                                    //    AccDatabase.DeleteItem_Punch2(TempAnsList.ID);
                                                    //    AccDatabase.DeleteItem_PunchTmp(TempAnsList.ID);
                                                    //    if(name_list_in.Count() == WIFI_name_list_in.Count())
                                                    //    {
                                                    //        AccDatabase.DeleteAll_Punch2();
                                                    //        AccDatabase.DeleteAll_PunchTmp();
                                                    //    }
                                                    //    MessagingCenter.Send(this, "Setlist", true); 
                                                    //}

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

                                                
                                                if (TempAnsList.name != null)
                                                {
                                                    if (!name_list_out.Contains(TempAnsList.name)) // 還沒處理過這筆案主的簽退
                                                    {
                                                        // 自動簽退
                                                        //Console.WriteLine("count~");
                                                        //Console.WriteLine(name_list_out.Count());
                                                        
                                                        //Console.WriteLine("nowifi_out~~~ ");
                                                        //Console.WriteLine(TempAnsList.name);
                                                        //Console.WriteLine(TempAnsList.token);
                                                        //Console.WriteLine(TempAnsList.ct_s_num);
                                                        //Console.WriteLine(TempAnsList.sec_s_num);
                                                        //Console.WriteLine(TempAnsList.mlo_s_num);
                                                        //Console.WriteLine(TempAnsList.reh_s_num);
                                                        //Console.WriteLine(TempAnsList.latitude);
                                                        //Console.WriteLine(TempAnsList.longitude);
                                                        //Console.WriteLine(TempAnsList.time);
                                                        PunchIn punout = new PunchIn
                                                        {
                                                            token = TempAnsList.token,
                                                            ct_s_num = TempAnsList.ct_s_num,
                                                            sec_s_num = TempAnsList.sec_s_num,
                                                            mlo_s_num = TempAnsList.mlo_s_num,
                                                            reh_s_num = TempAnsList.reh_s_num,
                                                            latitude = TempAnsList.latitude.ToString(),
                                                            longitude = TempAnsList.longitude.ToString(),
                                                            time = TempAnsList.time,
                                                            phl50 = "2"
                                                        };
                                                        // post打卡訊息到mqtt
                                                        await Connected_punch(NowLat.ToString(), NowLon.ToString(), MQTTREH, "2", "2", TempAnsList.ct_s_num, TempAnsList.sec_s_num, TempAnsList.mlo_s_num, TempAnsList.time, "2", "2", "1", "1", dys09, "APP");
                                                        //no_wifi_punch_out = await web.Save_Punch_Out(TempAnsList.token, TempAnsList.ct_s_num, TempAnsList.sec_s_num, TempAnsList.reh_s_num, TempAnsList.mlo_s_num, TempAnsList.latitude, TempAnsList.longitude, TempAnsList.time, "2");
                                                        // --------------post打卡訊息到後台-------------------
                                                        //no_wifi_punch_out = await web.Save_Punch_Out(punout);
                                                        //----------------------------------------------------

                                                        //Console.WriteLine("no_wifi_punch_out~~~ " + no_wifi_punch_out);
                                                        //if (no_wifi_punch_out == true)
                                                        //{
                                                            //Console.WriteLine("no_wifi_out_true~~~");
                                                            //Console.WriteLine(TempAnsList.name);
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
                                                            //Console.WriteLine("RRRR");
                                                            //Console.WriteLine(AccDatabase.GetAccountAsync2_Punch().Count());
                                                            AccDatabase.DeleteItem_Punch(TempAnsList.ID); // 把那筆刪掉
                                                            //Console.WriteLine("TTTT");
                                                            //Console.WriteLine(AccDatabase.GetAccountAsync2_Punch().Count());
                                                            //formin.IsVisible = true;
                                                            //formin.IsEnabled = true;
                                                            //await Task.Delay(10000); // 等待30秒
                                                            //Messager2();
                                                            //Console.WriteLine("id~~ ");
                                                            //Console.WriteLine(TempAnsList.ID);
                                                            AccDatabase.DeleteItem_PunchTmp2(TempAnsList.ID);

                                                            MessagingCenter.Send(this, "Setlist2", true); // 更新主頁面的無網路簽退紀錄
                                                                                                          //Console.WriteLine("deleteout~~~" + TempAnsList.name);
                                                                                                          //Console.WriteLine("outcount111~~~" + PunchDatabase.GetAccountAsync2().Count());
                                                            if (!WIFI_name_list_out.Contains(TempAnsList.name))
                                                            {
                                                                AccDatabase.SaveAccountAsync_Wifi_Punchout(new Wifi_Punchout// 把簽退成功紀錄到無網路簽退的SQLite
                                                                {
                                                                    name = TempAnsList.name, // 姓名
                                                                    time = TempAnsList.timeforpost // 時間
                                                                });
                                                                WIFI_name_list_out.Add(TempAnsList.name);
                                                            }
                                                            //if (name_list_out.Count() == WIFI_name_list_out.Count() && name_list_in.Count() != 0)
                                                            //{
                                                            //    AccDatabase.DeleteAll_Punch2();
                                                            //    AccDatabase.DeleteAll_PunchTmp2();
                                                            //}
                                                            //Console.WriteLine("XXXX");
                                                            //Console.WriteLine(name_list_out.Count());
                                                            //Console.WriteLine(WIFI_name_list_out.Count());
                                                            //MessagingCenter.Send(this, "Setlist2", true);
                                                        //}
                                                        //else
                                                        //{
                                                            //await DisplayAlert("FAIL", "打卡失敗in" + setName, "OK");
                                                            //Console.WriteLine("ASQLite簽退失敗");
                                                        //}
                                                    }
                                                    //else
                                                    //{
                                                    //    // 已經處理過這筆簽退，直接刪除這筆紀錄
                                                    //    AccDatabase.DeleteItem_Punch2(TempAnsList.ID);
                                                    //    AccDatabase.DeleteItem_PunchTmp2(TempAnsList.ID);
                                                    //    if (name_list_out.Count() == WIFI_name_list_out.Count() && name_list_in.Count() != 0)
                                                    //    {
                                                    //        AccDatabase.DeleteAll_Punch2();
                                                    //        AccDatabase.DeleteAll_PunchTmp2();
                                                    //    }
                                                    //    Console.WriteLine("QQQQ");
                                                    //    Console.WriteLine(name_list_out.Count());
                                                    //    Console.WriteLine(WIFI_name_list_out.Count());
                                                    //    MessagingCenter.Send(this, "Setlist2", true); // 更新主頁面的無網路簽退紀錄
                                                    //}
                                                }

                                            }

                                        }
                                    }
                                    //Console.WriteLine("GGG");
                                    //Console.WriteLine(AccDatabase.GetAccountAsync2_Wifi_Punchin().Count());
                                    //Console.WriteLine(AccDatabase.GetAccountAsync2_Wifi_Punchout().Count());
                                    //if(AccDatabase.GetAccountAsync2_Wifi_Punchin().Count() == WIFI_name_list_in.Count())
                                    //{
                                    //    AccDatabase.DeleteAll_PunchTmp();
                                    //    successIn = true;
                                    //}
                                    //if (AccDatabase.GetAccountAsync2_Wifi_Punchout().Count() == WIFI_name_list_out.Count())
                                    //{
                                    //    AccDatabase.DeleteAll_PunchTmp2();
                                    //    successOut = true;
                                    //}
                                    //if(successIn == true && successOut == true)
                                    //{
                                    //    AccDatabase.DeleteAll_Punch();
                                    //    successIn = false;
                                    //    successOut = false;
                                    //}




                                    //Console.WriteLine("number~~ " + PunchDatabase.GetAccountAsync2().Count());
                                    //if (AccDatabase.GetAccountAsync2_Punch2().Count() == 0) // 判斷是否還有未處理的無網路打卡
                                    //{
                                    //    //Console.WriteLine("punchtmpSUCESS");
                                    //    // 全部刪除，且更新主頁面上的紀錄
                                    //    AccDatabase.DeleteAll_PunchTmp();
                                    //    AccDatabase.DeleteAll_PunchTmp2();
                                    //    MessagingCenter.Send(this, "Setlist", true);
                                    //    //Console.WriteLine("sendsetlist~~~");
                                    //    MessagingCenter.Send(this, "Setlist2", true);
                                    //    //Console.WriteLine("sendsetlist22~~~");

                                    //}


                                    //---------------------------------------------------------------------
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
 
                            for (int i = 0; i < totalList.daily_shipments.Count(); i++)
                            {
                              
                                //if (homename == cList2[i].ct_name)
                                //{

                                which = 0;

                                //Console.WriteLine("who1" + cList2[i].ct_name);
                                ////Console.WriteLine("punch1" + punchList[cList[i].ct_name]);
                                //Console.WriteLine("whoami~~~" + setnum);
                                // 算目前使用者位置跟案主家的距離
                                /*

                                    */
                                //px = double.Parse(totalList.daily_shipments[setnum].ct16);
                                Console.WriteLine("ct16~~~ ");
                                // Console.WriteLine(totalList.daily_shipments[i].ct16);
                                // Console.WriteLine(totalList.daily_shipments[i].ct17);
                                //px = double.Parse(totalList.daily_shipments[i].ct16);


                                //px = Convert.ToDouble(totalList.daily_shipments[i].ct16);
                                Double.TryParse(totalList.daily_shipments[i].ct16, out px);
                                //pa = totalList.daily_shipments[i].ct16;
                                //py = Convert.ToDouble(totalList.daily_shipments[i].ct17);
                                Double.TryParse(totalList.daily_shipments[i].ct17, out py);
                                //py = double.Parse(totalList.daily_shipments[i].ct17);
                                //pb = totalList.daily_shipments[i].ct17);
                                dx = position.Latitude - px > 0 ? position.Latitude - px : px - position.Latitude;
                                dy = position.Longitude - py > 0 ? position.Longitude - py : py - position.Longitude;


                                d = Math.Sqrt(dx * 110000 * dx * 110000 + dy * 100000 * dy * 100000);
                                //Console.WriteLine("d2" + d);
                                d2 = d.ToString();
                                //Console.WriteLine("@@@@@   " + d2);
                                distance.Text = d2;
                                Latitude.Text = position.Latitude.ToString();
                                Longitude.Text = position.Longitude.ToString();
                                //NOWHOME = "0";
                                //NOWREH = totalList.daily_shipments[i].reh_s_num;
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
                                if (punchList[totalList.daily_shipments[i].ct_name] == false) // 先判斷有沒有打卡(簽到+簽退)過
                                {
                                    //Console.WriteLine("name~~~ " + cList2[i].ct_name);
                                    //Console.WriteLine("mlo~~~ " + cList2[i].mlo_s_num); 
                                    //Console.WriteLine("who2>>>>>" + totalList.daily_shipments[setnum].ct_name);
                                    //Console.WriteLine("punch2>>>>" + punchList[totalList.daily_shipments[setnum].ct_name]);
                                    //Console.WriteLine("ddddistance~~" + d);

                                    // GPS 簽到
                                    if (d < 30 && punch_in[totalList.daily_shipments[i].ct_name] == false) // 符合簽到距離且尚未簽到過
                                    {
                                        punchyesorno[totalList.daily_shipments[i].ct_name] = true;
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
                                        //NOWHOME = totalList.daily_shipments[i].ct_s_num;
                                        //NOWREH = totalList.daily_shipments[i].reh_s_num;
                                        setname.Text = "成功簽到" + totalList.daily_shipments[i].ct_name + "的家";
                                        //setname2.Text = totalList.daily_shipments[i].ct_name;
                                        setname3.Text = totalList.daily_shipments[i].ct_name;
                                        dys05_type.Text = totalList.daily_shipments[i].dys05_type;
                                        //Console.WriteLine("dys05~~~ " + dys05_type.Text);
                                        sec06.Text = totalList.daily_shipments[i].sec06;
                                        ct06_telephone.Text = totalList.daily_shipments[i].ct06_telephone;
                                        dys03.Text = totalList.daily_shipments[i].dys03;
                                        dys02.Text = totalList.daily_shipments[i].dys02;
                                        //-------------------Queue--------------------------

                                        AddPunchInfoToQueue("in", totalList.daily_shipments[i].ct_name, totalList.daily_shipments[i].dys05_type, totalList.daily_shipments[i].ct06_telephone, totalList.daily_shipments[i].sec06, totalList.daily_shipments[i].dys03, totalList.daily_shipments[i].dys02, i);

                                        //----------------------------------
                                        //Console.WriteLine("name1~~" + setname.Text);
                                        //Clname = totalList.daily_shipments[i].ct_name;



                                        //ct_s_num = totalList.daily_shipments[i].ct_s_num;

                                        //sec_s_num = totalList.daily_shipments[i].sec_s_num;

                                        //mlo_s_num = totalList.daily_shipments[i].mlo_s_num;  // 訂單s_num(
                                        //reh_s_num = totalList.daily_shipments[i].reh_s_num;
                                        //bn_s_num = cList2[i].bn_s_num; //  打卡鄰近的beancon_s_num(beacon id)
                                        bn_s_num = "0";

                                        if (CrossConnectivity.Current.IsConnected) // 有連到網路
                                        {
                                            // 自動簽到
                                            DateTime myDate = DateTime.Now;
                                            time = myDate.ToString("yyyy-MM-dd HH:mm:ss");
                                            //Console.WriteLine("time~~~ " + time);

                                            //---------跳出訊息先註解掉-------
                                            /*
                                           formin_1.IsVisible = true; // 跳出簽到案主家成功訊息
                                           //formin_1.IsEnabled = true;
                                           formin_2.IsVisible = true; // 跳出案主家相關資訊
                                           //formin_2.IsEnabled = true;
                                           //await Task.Delay(10000); // 等待30秒
                                           await Task.Delay(TimeSpan.FromSeconds(5));
                                           //Messager2(); // 訊息消失(自動關閉)
                                           formin_1.IsVisible = false;
                                           formin_2.IsVisible = false;
                                            */
                                            //--------------------------------
                                            //Console.WriteLine("time~~~ " + time);
                                            punch_in[totalList.daily_shipments[i].ct_name] = true; // 簽到成功
                                            PunchIn punin = new PunchIn
                                            {
                                                token = MainPage.token,
                                                ct_s_num = totalList.daily_shipments[i].ct_s_num,
                                                sec_s_num = totalList.daily_shipments[i].sec_s_num,
                                                mlo_s_num = totalList.daily_shipments[i].mlo_s_num,
                                                reh_s_num = totalList.daily_shipments[i].reh_s_num,
                                                latitude = position.Latitude.ToString(),
                                                longitude = position.Longitude.ToString(),
                                                time = time,
                                                phl50 = "1"
                                            };
                                            //Console.WriteLine("time~~~ " + time);
                                            // post打卡訊息到mqtt
                                            await Connected_punch(NowLat.ToString(), NowLon.ToString(), MQTTREH, "1", "1", totalList.daily_shipments[i].ct_s_num, totalList.daily_shipments[i].sec_s_num, totalList.daily_shipments[i].mlo_s_num, time, "1", "1", "1","1", dys09, "APP");
                                            //wifi_punch_in = await web.Save_Punch_In(MainPage.token, totalList.daily_shipments[i].ct_s_num, totalList.daily_shipments[i].sec_s_num, totalList.daily_shipments[i].mlo_s_num, totalList.daily_shipments[i].reh_s_num, position.Latitude, position.Longitude, time, "1");
                                            // --------post 打卡訊息 to 後台--------------------
                                            //wifi_punch_in = await web.Save_Punch_In(punin);
                                            //-------------------------------------------

                                            //Console.WriteLine("web_res" + web_res);
                                            //if (wifi_punch_in == true)
                                            //{
                                                // 打卡成功
                                                //Console.WriteLine("name~~~~" + totalList.daily_shipments[setnum].ct_name + punch_in[totalList.daily_shipments[setnum].ct_name]);

                                                if (!WIFI_name_list_in.Contains(totalList.daily_shipments[i].ct_name))
                                                {
                                                    AccDatabase.SaveAccountAsync_Wifi_Punchin(new Wifi_Punchin // 存進有網路簽到成功的SQLite
                                                    {
                                                        name = totalList.daily_shipments[i].ct_name, // 案主姓名
                                                        time = DateTime.Now.ToShortTimeString() // 簽到時間
                                                    });
                                                    WIFI_name_list_in.Add(totalList.daily_shipments[i].ct_name);
                                                }

                                                //Console.WriteLine("punchin~~~gps" + punch_in[totalList.daily_shipments[setnum].ct_name] + "name " + totalList.daily_shipments[setnum].ct_name);
                                                //Console.WriteLine("true");
                                                //Console.WriteLine("BEE~~ " + BeaconScan.letpunchin);


                                                //punchinmsg = "簽到成功" + setName + "的家";
                                                ////Console.WriteLine("punchinmsg" + punchinmsg);

                                                //Thread.Sleep(5000); // 等待五秒之後
                                                //fadeformin(); // 簽到成功訊息自動消失
                                            //}
                                            //else
                                            //{
                                                //await DisplayAlert("FAIL", "打卡失敗in" + setName, "OK");
                                                //Console.WriteLine("簽到失敗");
                                            //}
                                        }
                                        else // 無網路環境下，先將要打卡資料存進SQLite
                                        {

                                            inorout = "in"; // 簽到
                                                            ////Console.WriteLine("");
                                                            // 將簽到資訊存進SQLite
                                                            //bool web_res = await web.Save_Punch_In(MainPage.token, ct_s_num, sec_s_num, mlo_s_num, position.Latitude, position.Longitude);
                                            DateTime myDate = DateTime.Now;
                                            time = myDate.ToString("yyyy-MM-dd HH:mm:ss");
                                            //Console.WriteLine("time~~~ " + time);
                                            //Console.WriteLine("nowifi_puch_in~~~");
                                            //Console.WriteLine(AccDatabase.GetAccountAsync_PunchTmp().Count());
                                            //Console.WriteLine(MainPage.token);
                                            //Console.WriteLine(totalList.daily_shipments[i].ct_name);
                                            //Console.WriteLine(inorout);
                                            //Console.WriteLine(totalList.daily_shipments[i].ct_s_num);
                                            //Console.WriteLine(totalList.daily_shipments[i].sec_s_num);
                                            //Console.WriteLine(totalList.daily_shipments[i].reh_s_num);
                                            //Console.WriteLine(position.Latitude);
                                            //Console.WriteLine(position.Longitude);
                                            //Console.WriteLine(time);
                                            //Console.WriteLine(DateTime.Now.ToShortTimeString());
                                            // 存要上傳到後台的資料
                                            punch_in[totalList.daily_shipments[i].ct_name] = true; // 簽到成功
                                            getPunch(MainPage.token, totalList.daily_shipments[i].ct_name, inorout, totalList.daily_shipments[i].ct_s_num, totalList.daily_shipments[i].sec_s_num, totalList.daily_shipments[i].reh_s_num, totalList.daily_shipments[i].mlo_s_num, position.Latitude, position.Longitude, time, DateTime.Now.ToShortTimeString());
                                            //Console.WriteLine(AccDatabase.GetAccountAsync_PunchTmp().Count());
                                            //Console.WriteLine("no_wifi_in~~~~");
                                            
                                                                                                   // 存要顯示在記錄頁面的資料
                                            AccDatabase.SaveAccountAsync_PunchTmp(new PunchTmp // 存進無網路簽到成功的SQLite
                                            {
                                                name = totalList.daily_shipments[i].ct_name, // 案主姓名
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
                                    if (d > 30 && punch_in[totalList.daily_shipments[i].ct_name] == true && punch_out[totalList.daily_shipments[i].ct_name] == false)
                                    {

                                        if (CrossConnectivity.Current.IsConnected) // 有連到網路
                                        {
                                            DateTime myDate = DateTime.Now;
                                            time = myDate.ToString("yyyy-MM-dd HH:mm:ss");

                                            //Console.WriteLine("time~~~ " + time);

                                            //------跳出訊息先註解掉---------
                                            /*
                                           setname.Text = "成功簽退" + totalList.daily_shipments[i].ct_name + "的家";
                                           formin_1.IsVisible = true; // 跳出簽退成功訊息
                                           formin_1.IsEnabled = true;
                                           Form.IsVisible = true; // 跳出問卷
                                           Form.IsEnabled = true;
                                           //await Task.Delay(10000); // 等待30秒
                                           await Task.Delay(TimeSpan.FromSeconds(5));
                                            //Messager2(); // 簽退成功訊息消失(自動關閉)
                                            // 自動簽退
                                            formin_1.IsVisible = false; // 跳出簽退成功訊息
                                            formin_1.IsEnabled = false;
                                            Form.IsVisible = false; // 跳出問卷
                                            Form.IsEnabled = false;
                                            */
                                            //-----------------------------------
                                            //--------------Queue--------------------

                                            AddPunchInfoToQueue("out", totalList.daily_shipments[i].ct_name, totalList.daily_shipments[i].dys05_type, totalList.daily_shipments[i].ct06_telephone, totalList.daily_shipments[i].sec06, totalList.daily_shipments[i].dys03, totalList.daily_shipments[i].dys02, i);

                                            //----------------------------------------
                                            //------------------------跳出問卷先拿掉----------------------------
                                            /*
                                            if (isform[totalList.daily_shipments[i].ct_name] == false)
                                            {
                                                try
                                                {
                                                    if (questionnaireslist != null)
                                                    {
                                                        if (questionnaireslist.Count != 0)
                                                        {
                                                            setQues(i);
                                                            isform[totalList.daily_shipments[i].ct_name] = true; // 紀錄是否跳出問卷
                                                        }

                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DisplayAlert("系統訊息", "Error : deliver_mapview_questionnairelist_null", "ok");
                                                }
                                            }
                                            */
                                            //----------------------------------------------------------------------------------
                                            //Clname = totalList.daily_shipments[i].ct_name;
                                            //ct_s_num = totalList.daily_shipments[i].ct_s_num;

                                            //sec_s_num = totalList.daily_shipments[i].sec_s_num;

                                            //mlo_s_num = totalList.daily_shipments[i].mlo_s_num;  // 訂單s_num(
                                            //reh_s_num = totalList.daily_shipments[i].reh_s_num;
                                            punch_out[totalList.daily_shipments[i].ct_name] = true;  // 簽退成功
                                            punchList[totalList.daily_shipments[i].ct_name] = true; // 打卡完成設為true(簽到+簽退成功)
                                            PunchIn punout = new PunchIn
                                            {
                                                token = MainPage.token,
                                                ct_s_num = totalList.daily_shipments[i].ct_s_num,
                                                sec_s_num = totalList.daily_shipments[i].sec_s_num,
                                                mlo_s_num = totalList.daily_shipments[i].mlo_s_num,
                                                reh_s_num = totalList.daily_shipments[i].reh_s_num,
                                                latitude = position.Latitude.ToString(),
                                                longitude = position.Longitude.ToString(),
                                                time = time,
                                                phl50 = "1"
                                            };
                                            // post打卡訊息到mqtt
                                            await Connected_punch(NowLat.ToString(), NowLon.ToString(), MQTTREH, "2", "1", totalList.daily_shipments[i].ct_s_num, totalList.daily_shipments[i].sec_s_num, totalList.daily_shipments[i].mlo_s_num, time, "1", "2", "1", "1", dys09, "APP");
                                            //wifi_punch_out = await web.Save_Punch_Out(MainPage.token, totalList.daily_shipments[i].ct_s_num, totalList.daily_shipments[i].sec_s_num, totalList.daily_shipments[i].reh_s_num, totalList.daily_shipments[i].mlo_s_num, position.Latitude, position.Longitude, time, "1");
                                            // --------post 打卡訊息 to 後台--------------------
                                            //wifi_punch_out = await web.Save_Punch_Out(punout);
                                            //------------------------------------------
                                            //Console.WriteLine("web_res2" + web_res2);
                                            //if (wifi_punch_out == true)
                                            //{

                                                if (!WIFI_name_list_out.Contains(totalList.daily_shipments[i].ct_name))
                                                {
                                                    AccDatabase.SaveAccountAsync_Wifi_Punchout(new Wifi_Punchout// 把簽退成功紀錄到無網路簽退的SQLite
                                                    {
                                                        name = totalList.daily_shipments[i].ct_name, // 姓名
                                                        time = DateTime.Now.ToShortTimeString() // 時間
                                                    });
                                                    WIFI_name_list_out.Add(totalList.daily_shipments[i].ct_name);
                                                }

                                                //Console.WriteLine("punchList~~~" + punchList[totalList.daily_shipments[setnum].ct_name] + "name " + totalList.daily_shipments[setnum].ct_name);

                                                //trylist2.Add(setnum);
                                                /*
                                                PunchSavesetnumToSQLite(setnum); // 把送餐進度存進SQLite
                                                //Console.WriteLine("setnumadd111~~~" + setnum + "count " + trylist2.Count());
                                                num = num + 1;

                                                // for該案主同時有兩張單的狀況(只需要打卡一次)
                                                // 判斷是否打過卡，有的話就跳過
                                                foreach (var a in punchList)
                                                {
                                                    if (a.Key == totalList.daily_shipments[i].ct_name)
                                                    {
                                                        if (a.Value == true)
                                                        {
                                                            //Console.WriteLine("key~~~" + a.Key);
                                                            PunchSavesetnumToSQLite(setnum);
                                                            //setnum = setnum + 1;

                                                        }

                                                    }
                                                }
                                                */

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

                                            //}
                                            //else
                                            //{
                                                //await DisplayAlert("FAIL", "打卡失敗in" + setName, "OK");
                                                //Console.WriteLine("簽退失敗");
                                            //}
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
                                            time = myDate.ToString("yyyy-MM-dd HH:mm:ss");
                                            //Console.WriteLine("time~~~ " + time);
                                            //Console.WriteLine("nowifi_puch_out~~~");
                                            //Console.WriteLine(AccDatabase.GetAccountAsync_PunchTmp().Count());
                                            //Console.WriteLine(MainPage.token);
                                            //Console.WriteLine(totalList.daily_shipments[i].ct_name);
                                            //Console.WriteLine(inorout);
                                            //Console.WriteLine(totalList.daily_shipments[i].ct_s_num);
                                            //Console.WriteLine(totalList.daily_shipments[i].sec_s_num);
                                            //Console.WriteLine(totalList.daily_shipments[i].reh_s_num);
                                            //Console.WriteLine(position.Latitude);
                                            //Console.WriteLine(position.Longitude);
                                            //Console.WriteLine(time);
                                            //Console.WriteLine(DateTime.Now.ToShortTimeString());
                                            //DisplayAlert("msgone", totalList.daily_shipments[i].ct_name + AccDatabase.GetAccountAsync2_Punch().Count(), "ok");
                                            punch_out[totalList.daily_shipments[i].ct_name] = true;  // 謙退成功
                                            punchList[totalList.daily_shipments[i].ct_name] = true; // 打卡完成設為true
                                            getPunch(MainPage.token, totalList.daily_shipments[i].ct_name, inorout, totalList.daily_shipments[i].ct_s_num, totalList.daily_shipments[i].sec_s_num, totalList.daily_shipments[i].reh_s_num, totalList.daily_shipments[i].mlo_s_num, position.Latitude, position.Longitude, time, DateTime.Now.ToShortTimeString());
                                            //DisplayAlert("msgtwo", totalList.daily_shipments[i].ct_name + AccDatabase.GetAccountAsync2_Punch().Count(), "ok");
                                            //Console.WriteLine(AccDatabase.GetAccountAsync2_Punch().Count());
                                            //Console.WriteLine("no_wifi_out~~~~");
                                            
                                            AccDatabase.SaveAccountAsync_PunchTmp2(new PunchTmp2 // 把簽退成功紀錄到無網路簽退的SQLite
                                            {
                                                name = totalList.daily_shipments[i].ct_name, // 姓名
                                                time = DateTime.Now.ToShortTimeString() // 時間
                                            });
                                            //PunchSavesetnumToSQLite(setnum); // 紀錄送餐進度

                                            // for該案主同時有兩張單的狀況(只需要打卡一次)
                                            // 判斷是否打過卡，有的話就跳過
                                            //foreach (var a in punchList)
                                            //{
                                            //    if (a.Key == totalList.daily_shipments[i].ct_name)
                                            //    {
                                            //        if (a.Value == true)
                                            //        {
                                            //            //Console.WriteLine("key~~~" + a.Key);
                                            //            PunchSavesetnumToSQLite(setnum);
                                            //            //setnum = setnum + 1;

                                            //        }

                                            //    }
                                            //}

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
                                    //-------------<<<<<beacon punchout dont delete------>>>>>>>
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
                            Console.WriteLine("EX222~~ " + ex.ToString());
                            //DisplayAlert("msg", ex.ToString(), "ok");
                            //DisplayAlert("error", "AAA" + ex.ToString(), "OK");
                            //Console.WriteLine("GET");
                            //Console.WriteLine("ERRORLA~~~" + ex.ToString());
                            //if (!isAlert)
                            //{
                            //    isAlert = true;
                            //    //await DisplayAlert(param.SYSYTEM_MESSAGE, param.LOCATION_ERROR_MESSAGE, param.DIALOG_AGREE_MESSAGE);
                            //}
                        }
                    }
                } catch(Exception ex)
                {
                    Console.WriteLine("EX~~ " + ex.ToString());
                    //DisplayAlert("error", "location null" + ex.ToString(), "ok");
                }
               
                
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("EX333~~ " + ex.ToString());
                //DisplayAlert("error", "BBB" + ex.ToString(), "ok");
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

            label_name = new Label
            {
                Text = questionList.ClientName,
                TextColor = Color.DarkBlue,
                FontSize = 20
            };

            label_wqh = new Label
            {
                Text = questionList.wqh_s_num
            };
            label_qh = new Label
            {
                Text = questionList.qh_s_num
            };

            stack = new StackLayout
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
                        label_que_name = new Label // 問題題號+題目
                        {
                            Text = i.qb_order + " " + i.qb01,
                            FontSize = 20,
                            TextColor = Color.Black
                        };

                        stack_ques = new StackLayout
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
                                check3 = new checkInfo
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
                                                //Console.WriteLine("YYYYYY_jj~~ " + temp_j);
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


                            check_box = new CheckBox // 產生checkbox
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
                                        check3 = new checkInfo
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

                            label_check = new Label // 選項
                            {
                                Text = j,
                                TextColor = Color.Black,
                                FontSize = 20
                            };



                            stack_check = new StackLayout // checkbox跟選項
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


                        final_stack = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { label_que_name, stack_ques }
                        };

                        //var lastest_stack = new StackLayout
                        //{
                        //    Orientation = StackOrientation.Vertical,
                        //    Children = { final_stack, more_form }
                        //};

                        frame = new Frame // frame包上面那個stacklayout
                        {
                            Padding = new Thickness(15, 5, 10, 5),

                            BackgroundColor = Color.FromHex("eddcd2"),
                            CornerRadius = 10,
                            HasShadow = false,
                            Content = final_stack
                        };

                        quesStack.Children.Add(frame);
                        //quesStack.Children.Add(more_form);
                        //Messager4();

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

        public void AddPunchInfoToQueue(string _inorout, string _name, string _dys05_type, string _ct06_telephone, string _sec06, string _dys03, string _dys02, int _num)
        {
            PunchInfoQueue.Enqueue(new PunchInfo
            {
                inorout = _inorout,
                name = _name,
                dys_05_type = _dys05_type,
                ct06_telephone = _ct06_telephone,
                sec06 = _sec06,
                dys03 = _dys03,
                dys02 = _dys02,
                num = _num
            });
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
        // ---------------nowifiPunchSaveToSQLite---------------------
        public void getPunch(string _token, string _name, string _inorout, string _ct_s_num, string _sec_s_num, string _reh_s_num, string _mlo_s_num, double _lat, double _lot, string _time, string _timeforpost)
        {
            Punch pun = new Punch
            {
                token = _token,
                name = _name,
                inorout = _inorout,
                ct_s_num = _ct_s_num,
                sec_s_num = _sec_s_num,
                mlo_s_num = _mlo_s_num,
                reh_s_num = _reh_s_num,
                latitude = _lat,
                longitude = _lot,
                time = _time,
                timeforpost = _timeforpost
            };
            PunchSaveToSQLite(pun);


        }

        public void PunchSaveToSQLite(Punch pun)
        {
            // MainPage.token, ct_s_num, sec_s_num, mlo_s_num, bn_s_num, position.Latitude, position.Longitude
            Console.WriteLine("punchsave~~~");
            AccDatabase.SaveAccountAsync_Punch(new Punch
            {
                token = pun.token,
                name = pun.name,
                inorout = pun.inorout,
                ct_s_num = pun.ct_s_num,
                sec_s_num = pun.sec_s_num,
                mlo_s_num = pun.mlo_s_num,
                reh_s_num = pun.reh_s_num,
                latitude = pun.latitude,
                longitude = pun.longitude,
                time = pun.time,
                timeforpost = pun.timeforpost
            });
        }
        // ---------------nowifiPunchSaveToSQLite---------------------


        //public void PunchSavesetnumToSQLite(int _setnum)
        //{
        //    //Console.WriteLine("setnumadd~~~");
        //    AccDatabase.SaveAccountAsync_Punch2(new Punch2
        //    {
        //        setnum = _setnum
        //    });
        //}

        public void PunchSavepunchnameToSQLite(string _punchname)
        {
            AccDatabase.SaveAccountAsync_Punch(new Punch
            {
                punchname = _punchname
            });
        }

        //public void PunchSavePunchYesOrNoToSQLite(string _punchname)
        //{
        //    PunchYN.SaveAccountAsync(new PunchYorN
        //    {
        //        name = _punchname
        //    });
        //}

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
                //Console.WriteLine("postGPS~~~");
                //Console.WriteLine("lat~~ " + position.Latitude.ToString());
                //Console.WriteLine("lot~~~ " + position.Longitude.ToString());
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
            //formout.IsVisible = false;
            //formout.IsEnabled = false;
            formin_1.IsVisible = false;
            formin_1.IsEnabled = false;
        }

        bool OnTimerTick_for_PunchInfo()
        {
            try
            {
                //Task.Run(() =>
                //{
                    try
                    {
                        // Run code here
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (PunchInfoQueue.Count() != 0)
                            {
                                var punchinfo = PunchInfoQueue.Dequeue();
                                if (punchinfo.inorout == "in")
                                {
                                    setname.Text = "成功簽到" + punchinfo.name + "的家";
                                }
                                else
                                {
                                    setname.Text = "成功簽退" + punchinfo.name + "的家";
                                }

                                setname3.Text = punchinfo.name;
                                dys05_type.Text = punchinfo.dys_05_type;
                                //Console.WriteLine("dys05~~~ " + dys05_type.Text);
                                sec06.Text = punchinfo.sec06;
                                ct06_telephone.Text = punchinfo.ct06_telephone;
                                dys03.Text = punchinfo.dys03;
                                dys02.Text = punchinfo.dys02;
                                if (punchinfo.inorout == "in")
                                {
                                    formin_1.IsVisible = true; // 跳出簽到案主家成功訊息
                                    formin_1.IsEnabled = true;
                                    formin_2.IsVisible = true; // 跳出案主家相關資訊
                                    formin_2.IsEnabled = true;
                                    Form.IsVisible = true;
                                    Form.IsEnabled = true;
                                    if (isform[totalList.daily_shipments[punchinfo.num].ct_name] == false)
                                    {
                                        try
                                        {
                                            if (questionnaireslist != null)
                                            {
                                                if (questionnaireslist.Count != 0)
                                                {
                                                    setQues(punchinfo.num);
                                                    isform[totalList.daily_shipments[punchinfo.num].ct_name] = true; // 紀錄是否跳出問卷
                                                }

                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DisplayAlert("系統訊息", "Error : deliver_mapview_questionnairelist_null", "ok");
                                        }
                                    }
                                }
                                else
                                {
                                    formin_1.IsVisible = true; // 跳出簽到案主家成功訊息
                                    formin_1.IsEnabled = true;
                                    formin_2.IsVisible = false; // 跳出案主家相關資訊
                                    formin_2.IsEnabled = false;
                                    Form.IsVisible = false;
                                    Form.IsEnabled = false;
                                }
                            }
                            else
                            {
                                formin_1.IsVisible = false; // 跳出簽到案主家成功訊息
                                formin_1.IsEnabled = false;
                                formin_2.IsVisible = false; // 跳出案主家相關資訊
                                formin_2.IsEnabled = false;

                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("ONERROR");
                        //Console.WriteLine(ex.ToString());
                        //DisplayAlert(param.SYSYTEM_MESSAGE, param.LOCATION_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                    }
                //});
                return true;
            } catch(Exception ex) {
                DisplayAlert("msg", ex.ToString(), "ok");
                return false;
            }
            
        }

        bool OnTimerTick()
        {
            // running something on another thread 
            //Task.Run(() =>
            //{
                try
                {
                    // execute the UI action on the UI thread
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        // Await 運算子可以套用至標示為 async 之方法內的工作。 它會使方法在該時間點停止執行，並等候工作完成。
                        // 在等候工作時，不會封鎖使用者介面執行緒
                        // 當工作完成時，方法會在程式碼中的相同位置繼續執行
                        //Console.WriteLine("TIMER~~~");
                        await getLocation();
                        //post_gps();
                        
                        //-----------------MQTT-----------------------
                        //var lat = position.Latitude.ToString();
                        //var lon = position.Longitude.ToString();
                        //Console.WriteLine("TF~~ " + MainPage.mqttClient.IsConnected);
                        //Console.WriteLine("lat~~ " + lat);
                        //Console.WriteLine("lon~~~" + lon);
                        /*
                        if(CrossConnectivity.Current.IsConnected)
                        {
                            //Console.WriteLine("mqttin~~~");
                            if (MainPage.mqttClient.IsConnected == true)
                            { // 有連線的話就傳送資料
                                Connected(lat, lon, MainPage.NAME, MQTTREH);
                            }
                            else
                            { // 沒有連線的話就重新連線再傳送資料 
                                MainPage.Start();
                                Connected(lat, lon, MainPage.NAME, MQTTREH);
                            }
                        }
                        */
                       
                        //--------------------------------------------
                        //post_gps();
                        //checknowifi();
                    });
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("ONERROR");
                    //Console.WriteLine(ex.ToString());
                    //DisplayAlert(param.SYSYTEM_MESSAGE, param.LOCATION_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                }
            //});
            return true;
        }

        // push gps to mqtt server
        private static async Task Connected(string lat, string lon, string name, string reh)
        {
            try
            {
                //List<TopicFilter> listTopic = new List<TopicFilter>();
                //if (listTopic.Count() <= 0)
                //{
                //    var topicFilterBulder = new TopicFilterBuilder().WithTopic("sensor/TestView/room1").Build();
                //    //listTopic.Add(topicFilterBulder);
                //    Console.WriteLine("Connected >>Subscribe + Topic");
                //}

                var message = new MqttApplicationMessageBuilder()
                 .WithTopic("sensor/Test/room1")
                 //.WithTopic("sensor/Test/room3")
                 .WithPayload(lat + "," + lon+ "," + name + "," + reh + "," + MainPage.token)
                 .WithExactlyOnceQoS()
                 .Build();

                await MainPage.mqttClient.PublishAsync(message);
                //Console.WriteLine("Connected >>publish msg Success");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        // push punch record to mqtt server
        private static async Task Connected_punch(string lat, string lon,string reh, string inorout, string wifi , string ctsnum, string secsnum, string mlosnum, string phl01, string phl50, string phl02, string phl05, string phl99, string dys09, string source)
        {
            try
            {
                //List<TopicFilter> listTopic = new List<TopicFilter>();
                //if (listTopic.Count() <= 0)
                //{
                //    var topicFilterBulder = new TopicFilterBuilder().WithTopic("sensor/TestView/room1").Build();
                //    //listTopic.Add(topicFilterBulder);
                //    Console.WriteLine("Connected >>Subscribe + Topic");
                //}

                var message = new MqttApplicationMessageBuilder()
                 .WithTopic("sensor/Test/room2")
                 //.WithTopic("sensor/Test/room4")
                 .WithPayload(lat + "," + lon + "," + reh + "," + MainPage.token + "," + inorout + "," + wifi + "," + ctsnum + "," + secsnum + "," + mlosnum + "," + phl01 + "," + phl50 + "," + phl02 + "," + phl05 + "," + phl99 + "," + dys09 + "," + source)
                 .WithExactlyOnceQoS()
                 .Build();

                await MainPage.mqttClient.PublishAsync(message);
                //Console.WriteLine("Connected >>publish msg Success");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        bool OnTimerTick2()
        {
            //Task.Run(() =>
            //{
                try
                {
                    // Run code here
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        // UI interaction goes here
                        await getLocation2();
                        //web.post_gps(MainPage.token, position.Latitude.ToString(), position.Longitude.ToString());
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            //Console.WriteLine("mqttin~~~");
                            if (MainPage.mqttClient.IsConnected == true)
                            { // 有連線的話就傳送資料
                                Console.WriteLine("AAAAAAAAAAAAAAA");
                                //Console.WriteLine("reh~~~ ");
                                Console.WriteLine(MQTTREH);
                                await Connected(position.Latitude.ToString(), position.Longitude.ToString(), MainPage.NAME, MQTTREH);
                            }
                            else
                            { // 沒有連線的話就重新連線再傳送資料 
                                Console.WriteLine("BBBBBBBBBBBBBBB");
                                MainPage.Start();
                                await Connected(position.Latitude.ToString(), position.Longitude.ToString(), MainPage.NAME, MQTTREH);
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("ONERROR");
                    //Console.WriteLine(ex.ToString());
                    //DisplayAlert(param.SYSYTEM_MESSAGE, param.LOCATION_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                }
            //});
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
            if(MainPage.AUTH == "13")
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
        private void PinMarker3(string resource, Xamarin.Forms.GoogleMaps.Position position, string label, string gps, string s_num)
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
                        //string uri = "https://www.google.com.tw/maps/place/" + gps;
                        //Console.WriteLine("URI" + uri);
                        //if (await Launcher.CanOpenAsync(uri))
                        //{
                        //    //await Launcher.OpenAsync(uri);

                        //}
                        //else
                        //{
                        //    await DisplayAlert(param.SYSYTEM_MESSAGE, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                        //}
                        Console.WriteLine("who~~ " + sender.ToString());
                        Console.WriteLine("who2~~~ " + e.ToString());
                        num = s_num;
                        btnGPS = gps;
                        UpdateWindow.IsVisible = true;
                        navigatebtn_deliver.IsVisible = true; // 導航按鈕
                        update_closebtn.IsVisible = true; // 關閉按鈕
                        updatebtn.IsVisible = true; // 校正按鈕
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
        private void update_closebtn_Clicked(object sender, EventArgs e)
        {
            UpdateWindow.IsVisible = false;
        }
        private  async void navigatebtn_Clicked(object sender, EventArgs e)
        {
            try
            {
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
        private async void navigatebtn_deliver_Clicked(object sender, EventArgs e)
        {
            try
            {
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

        private async void all_navigate_Clicked(object sender, EventArgs e)
        {
            NavigateWindow.IsVisible = true;
        }



        private async void all_navigate_button_Clicked(string num, double nowlat, double nowlon)
        {
            try
            {
                //location = CrossGeolocator.Current;
                //location.DesiredAccuracy = location_DesiredAccuracy;
                //position = await location.GetPositionAsync();
                //var nowlon = position.Longitude;
                //var nowlat = position.Latitude;
                //googleMapUrl = getUrl(nowlat.ToString(), nowlon.ToString());
                //Console.WriteLine("QQQQQQQ");
                //Console.WriteLine(googleMapUrl);
                Console.WriteLine("all_navigate_button_Clicked");
                Console.WriteLine("MainPage.Finallist QQQQ~~ " + MainPage.Finallist);
                //location = CrossGeolocator.Current;
                //location.DesiredAccuracy = location_DesiredAccuracy;
                //position = await location.GetPositionAsync();
                //var nowlon = position.Longitude;
                //var nowlat = position.Latitude;
                Console.WriteLine("MainPage.Finallist~~ " + MainPage.Finallist);
                if (MainPage.Finallist != null)
                {
                    //string uri = "https://www.google.com.tw/maps/dir/" + MainPage.googleMapUrl;
                    string uri = "https://www.google.com.tw/maps/dir/" + nowlat.ToString() + ','+ nowlon.ToString() +'/'+ MainPage.Finallist[Int32.Parse(num)];
                    Console.WriteLine("URI" + uri);
                    if (await Launcher.CanOpenAsync(uri))
                    {
                        await Launcher.OpenAsync(uri);
                        
                    }
                    else
                    {
                        await DisplayAlert(param.SYSYTEM_MESSAGE, param.BROWSER_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                    }
                } else
                {
                    //googleMapUrl = getUrl(nowlat.ToString(), nowlon.ToString());
                }
              
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }

        private void infoCancelTapped(object sender, EventArgs e)
        {

            NavigateWindow.IsVisible = false;
        }

        private async void updatebtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool response = await web.update_gps(MainPage.token, num, position.Latitude.ToString(), position.Longitude.ToString());
                if(response == true)
                {
                    DisplayAlert("系統訊息","校正成功","ok");
                }
                //Console.WriteLine("snum~~ " + num);
                //Console.WriteLine("lat~~~ " + position.Latitude.ToString());
                //Console.WriteLine("lon~~~" + position.Longitude.ToString());
            }
            catch(Exception ex)
            {

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
                Console.WriteLine("Msg~~~~~ ");
               

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
                        //getUrl();


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
                        //getUrl();


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
                        //formclose();
                    }
                });
                
                MessagingCenter.Subscribe<MemberView, bool>(this, "LOGOUT", (sender, arg) =>
                {
                    if(arg)
                    {
                        //clientList = null;
                        //cList.Clear();

                        //cList2.Clear();
                        
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
                Console.WriteLine("YYSSS :" + ex.ToString());
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
using Deliver.Models;
using Deliver.Models.DataInfo;
using Deliver.Services;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using PULI.Model;
using PULI.Models.DataInfo;
using PULI.Services.SQLite;
using PULI.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace PULI
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        //public IMqttClient mqttClient;
        //public IMqttClientOptions options;
        IGeolocator location;
        Plugin.Geolocator.Abstractions.Position position;
        int location_DesiredAccuracy = 20, map_Zoom = 14;
        public static MqttClient mqttClient = null;
        private static IMqttClientOptions options = null;
        private static bool runState = false;
        private static bool running = false;
        // public static LoginInfo loginList = null;
        public static string token = "";
        //public static List<ClientInfo> clientList = new List<ClientInfo>(); 
        //public static IEnumerable<ClientInfo> clientList = null; // for auth = 4 送餐員 
                                                                 //public static IEnumerable<TotalList> totalList = null;
                                                                 // public static AllClientInfo allclientList = new AllClientInfo();
        public static TotalList totalList = new TotalList();
        public static List<AllClientInfo> allclientList = new List<AllClientInfo>(); // for auth = 6 社工
        public static LoginInfo userList = new LoginInfo();

        //4=送餐員,6社工
        public static string AUTH;
        public static string NAME;
        //public static string user_name;
        public static Database fooDoggyDatabase;
        public static Date dateDatabase;
        WebService web = new WebService();
        ParamInfo param = new ParamInfo();
        private string[] loginData = new string[4];
        private string Account;
        private string Password;
        private string LoginTime;
        private string logacc;
        private string logpwd;
        public static string Loginway;
        public static string oldday2;
        public static string _login_time;
        public static string function;
        private string _identity;
        public static string _time = "早上";
        private string time;
        public static bool checkdate = false;
        private string _resIdentity = "送餐員";
        private string[] identityArray = new string[] { "社工", "送餐員" };
        private string[] timeArray = new string[] { "早上", "下午"};
        public static string googleMapUrl; // 存要導到google map整條路線導航的url
        private List<string> Urlname = new List<string>();
        private List<string> Urllist = new List<string>();
        public static List<string> Finallist = new List<string>();

        public MainPage()
        {
            InitializeComponent();
            fooDoggyDatabase = new Database();
            dateDatabase = new Date();
            AUTH = "";
            token = "";
            Loginway = "";
            identityStack.Children.Add(pickerStack());
            //fooDoggyDatabase.DeleteAll();
            //Content = ViewService.LoadingLogin();
            checkDatabase();

        }

        private async void checkDatabase()
        {
            //var accountList = await App.Database.GetAccountAsync();
            // 判斷有沒有登入過
            // 如果有資料
            Console.WriteLine("LALALA~~~~" + fooDoggyDatabase.GetAccountAsync().Count());
            if (fooDoggyDatabase.GetAccountAsync().Count() > 0)
            {
                //Console.WriteLine("LOGIN~~~");
                welcome.IsVisible = true;
                
                Account accountList = fooDoggyDatabase.GetAccountAsync().FirstOrDefault();
                loginData[0] = accountList.account;
                
                //logacc = loginData[0]; // 紀錄登入帳號
                //Console.WriteLine("0000~~~" + loginData[0]);
                Account = loginData[0];
                deliver_name.Text = "你好" + Account;
                //Console.WriteLine("0000!!!!" + Account);
                loginData[1] = accountList.password;
                //logpwd = loginData[1]; // 紀錄登入密碼
               // Console.WriteLine("1111~~~" + loginData[1]);
                loginData[2] = accountList.identity;
                loginData[3] = accountList.time;
                Password = loginData[1];
                LoginTime = accountList.login_time;
                //Console.WriteLine("login_time@@~~~" + LoginTime);
                //Console.WriteLine("1111!!!!" + Password);
                DateTime time = DateTime.Now;
                string date = time.ToString("MMdd");
                //Console.WriteLine("time~~~" + time.ToString("t"));
                //Console.WriteLine("timeshort~~~~" + time.ToShortTimeString());
                //Console.WriteLine("time2~~~" + time.ToString("hh tt"));
                //Console.WriteLine("date~~~~" + time.ToString("MMdd"));
                //Console.WriteLine("date_database_count~~" + dateDatabase.GetAccountAsync2().Count());
                
                login(loginData[0], loginData[1], loginData[2], loginData[3]);
            }
            //else//沒有的話進去登入頁面
            //{
            //    Console.WriteLine("2222222222222222");
            //    account.Text = "";
            //    pwd.Text = "";
            //    token = "";
            //    Login.IsVisible = true;
            //    Login.IsEnabled = true;
             
            //}
        }

        private async void login(String acc, String pwd, String iden, String time)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected) // 有網路
                {
                    Loginway = "Auto";
                    MessagingCenter.Send(this, "Auto", true);
                    Console.WriteLine("send~~~");
                    //var time = string.Format("{hh:mm:ss tt}", DateTime.Now);
                    //Console.WriteLine("time~~~" + time);
                    
                    //string format = "MMM ddd d HH:mm yyyy";
                    //Console.WriteLine(time.ToString(format));
                    Login.IsVisible = false;
                    Login.IsEnabled = false;
                    AutoLogin.IsVisible = true;
                    AutoLogin.IsEnabled = true;
                    userList = await web.Login(acc, pwd, iden);
                    //DisplayAlert("userlist", userList.acc_auth + userList.acc_name + userList.acc_password + userList.acc_token + userList.acc_user, "ok");
                    if(userList != null)
                    {

                        _login_time = userList.login_time;
                        Console.WriteLine("login_time~~~" + _login_time);
                        AUTH = userList.acc_auth;
                        //Console.WriteLine("auth~~~" + userList.acc_auth);
                        NAME = userList.acc_name;
                        //Console.WriteLine("name~~~" + userList.acc_name);
                        //Console.WriteLine("NAME~~~" + userList.acc_name);
                        token += userList.acc_token;
                        //Console.WriteLine("OOOOO " + token);
                        //Console.WriteLine("auto_time~~ " + time);
                        if (token != null)
                        {
                            if (time == "早上")
                            {
                                _time = "早上";
                                totalList = await web.Get_Daily_Shipment(token);
                                ActivityView.totalList = await web.Get_Daily_Shipment(token);
                                ActivityView.stopList = await web.Get_Stop(token);
                                ActivityView.restoreList = await web.Get_Restore(token);
                            }
                            else
                            {
                                _time = "晚上";
                                totalList = await web.Get_Daily_Shipment_night(token);
                                ActivityView.totalList = await web.Get_Daily_Shipment_night(token);
                                ActivityView.stopList = await web.Get_Stop(token);
                                ActivityView.restoreList = await web.Get_Restore(token);
                            }
                         
                            //googleMapUrl = getUrl(nowlat.ToString(), nowlon.ToString());
                            Finallist = getUrl();
                            Console.WriteLine("MAinurl");
                            //Console.WriteLine(googleMapUrl);
                            Console.WriteLine(Finallist);
                            Console.WriteLine(Finallist[0]);

                            if (string.IsNullOrEmpty(NAME))
                            {
                                login(acc, pwd, iden, time);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(AUTH))
                                {
                                    login(acc, pwd, iden, time);
                                }
                                else
                                {
                                    //BeaconScan scan = new BeaconScan();
                                    //if (BeaconScan.BleStatus == 0)
                                    //{
                                    //    //await DisplayAlert("提示", "藍芽未開啟", "ok");
                                    //    AutoLogin.IsVisible = true;
                                    //    searchLabel.Text = param.CONNECT_BLUETOOTH_ERROR_MESSAGE;
                                    //}
                                    //Console.WriteLine("auth" + AUTH + "auth");
                                    //Console.WriteLine("AUTH~~~" + AUTH);
                                    //DisplayAlert("提示", "[弗傳慈心基金會] 會收集位置資料，以便在應用程式關閉或未使用時，也可支援紀錄外送員gps位置以判斷打卡。", "ok");

                                    if (AUTH == "13")
                                    {
                                        allclientList = await web.Get_All_Client(token);
                                        //Console.WriteLine("num~~~~" + userList.daily_shipment_nums);
                                        //Console.WriteLine("ALL~~~" + allclientList.Count());
                                    }
                                    //if (dateDatabase.GetAccountAsync2().Count() != 0) // 裡面有資料，先比對
                                    //{
                                        
                                    //    string oldday = dateDatabase.GetAccountAsync2().Last().date;
                                    //    oldday2 = fooDoggyDatabase.GetAccountAsync().Last().login_time;
                                    //    Console.WriteLine("olddayA~~" + oldday);
                                    //    Console.WriteLine("oldday2A~~~" + oldday2);
                                    //    //Console.WriteLine("LoginTime~~~" + LoginTime);
                                    //    // Console.WriteLine("date~~~" + date);
                                    //    string now = DateTime.Now.ToString("yyyy-MM-dd");
                                    //    Console.WriteLine("now~~~~" + now);
                                    //    if (now != oldday2)
                                    //    {
                                    //        function = "Auto_A2";
                                    //        try
                                    //        {
                                    //            MessagingCenter.Send(this, "NewDayDelete", true);
                                    //            //MapView.AccDatabase.DeleteAll_TempAccount();
                                    //            //MapView.AccDatabase.DeleteAll_Punch();
                                    //            //MapView.AccDatabase.DeleteAll_Punch2();
                                    //            //MapView.AccDatabase.DeleteAll_PunchTmp();
                                    //            //MapView.AccDatabase.DeleteAll_PunchTmp2();
                                    //            ////MapView.PunchYN.DeleteAll();
                                    //            //MapView.name_list_in.Clear();
                                    //            //MapView.name_list_out.Clear();
                                    //            //MapView.WIFI_name_list_in.Clear();
                                    //            //MapView.WIFI_name_list_out.Clear();
                                    //            //MapView.AccDatabase.DeleteAll_Wifi_Punchin();
                                    //            //MapView.AccDatabase.DeleteAll_Wifi_Punchout();
                                    //            Console.WriteLine("newdaysend~~~");
                                    //        }
                                    //        catch (Exception ex)
                                    //        {
                                    //            Console.WriteLine("Error_send~~" + ex.ToString());
                                    //        }


                                    //        checkdate = true;
                                    //        //Console.WriteLine("howmany~" + MapView.PunchDatabase2.GetAccountAsync2().Count());
                                    //        dateDatabase.DeleteAll(); // 讓裡面永遠只保持最新的一筆
                                    //        dateDatabase.SaveAccountAsync(new CheckDate
                                    //        {
                                    //            date = now
                                    //        });
                                    //        fooDoggyDatabase.SaveAccountAsync(new Account
                                    //        {
                                    //            account = acc,
                                    //            password = pwd,
                                    //            identity = iden,
                                    //            login_time = now
                                    //        });
                                    //    }
                                    //    if (_login_time != oldday2)
                                    //    {
                                    //        function = "Auto_A1";
                                    //        //Console.WriteLine("mainpage~~~1~~~");
                                    //        //Console.WriteLine("date_renew_save~~~");
                                    //        try
                                    //        {
                                    //            MessagingCenter.Send(this, "NewDayDelete", true);
                                    //            //MapView.AccDatabase.DeleteAll_TempAccount();
                                    //            //MapView.AccDatabase.DeleteAll_Punch();
                                    //            //MapView.AccDatabase.DeleteAll_Punch2();
                                    //            //MapView.AccDatabase.DeleteAll_PunchTmp();
                                    //            //MapView.AccDatabase.DeleteAll_PunchTmp2();
                                    //            ////MapView.PunchYN.DeleteAll();
                                    //            //MapView.name_list_in.Clear();
                                    //            //MapView.name_list_out.Clear();
                                    //            //MapView.WIFI_name_list_in.Clear();
                                    //            //MapView.WIFI_name_list_out.Clear();
                                    //            //MapView.AccDatabase.DeleteAll_Wifi_Punchin();
                                    //            //MapView.AccDatabase.DeleteAll_Wifi_Punchout();
                                    //            Console.WriteLine("newdaysend~~~");
                                    //        }
                                    //        catch (Exception ex)
                                    //        {
                                    //            Console.WriteLine("Error_send~~" + ex.ToString());
                                    //        }


                                    //        checkdate = true;
                                    //        //Console.WriteLine("howmany~" + MapView.PunchDatabase2.GetAccountAsync2().Count());
                                    //        dateDatabase.DeleteAll(); // 讓裡面永遠只保持最新的一筆
                                    //        dateDatabase.SaveAccountAsync(new CheckDate
                                    //        {
                                    //            date = _login_time
                                    //        });
                                    //        fooDoggyDatabase.SaveAccountAsync(new Account
                                    //        {
                                    //            account = acc,
                                    //            password = pwd,
                                    //            identity = iden,
                                    //            login_time = _login_time
                                    //        });

                                    //    }
                                       
                                    //}
                                    //else // 裡面還沒有資料
                                    //{
                                    //    dateDatabase.SaveAccountAsync(
                                    //    new CheckDate
                                    //    {
                                    //        date = _login_time
                                    //    });
                                    //    Console.WriteLine("date_nodata_save~~");
                                    //}
                                    //~~~~~~test2~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                                    //if (dateDatabase.GetAccountAsync2().Count() != 0) // 裡面有資料，先比對
                                    //{
                                    //    string oldday = dateDatabase.GetAccountAsync2().Last().date;
                                    //    oldday2 = fooDoggyDatabase.GetAccountAsync().Last().login_time;
                                    //    string now = DateTime.Now.ToString("yyyy-MM-dd");
                                    //    Console.WriteLine("now~~~~" + now);
                                    //    Console.WriteLine("olddayB~~~main~~~" + oldday);
                                    //    Console.WriteLine("oldday2B~~~main~~~" + oldday2);
                                    //    //Console.WriteLine("_login_time~~main~~" + _login_time);
                                    //    ////Console.WriteLine("LoginTime~~~" + LoginTime);
                                    //    // Console.WriteLine("date~~~" + date);
                                    //    if (now.Equals(oldday2) == false)
                                    //    {
                                    //        function = "Auto_B2";
                                    //        try
                                    //        {
                                    //            MessagingCenter.Send(this, "NewDayDelete", true);
                                    //            //MapView.AccDatabase.DeleteAll_TempAccount();
                                    //            //MapView.AccDatabase.DeleteAll_Punch();
                                    //            //MapView.AccDatabase.DeleteAll_Punch2();
                                    //            //MapView.AccDatabase.DeleteAll_PunchTmp();
                                    //            //MapView.AccDatabase.DeleteAll_PunchTmp2();
                                    //            ////MapView.PunchYN.DeleteAll();
                                    //            //MapView.name_list_in.Clear();
                                    //            //MapView.name_list_out.Clear();
                                    //            //MapView.WIFI_name_list_in.Clear();
                                    //            //MapView.WIFI_name_list_out.Clear();
                                    //            //MapView.AccDatabase.DeleteAll_Wifi_Punchin();
                                    //            //MapView.AccDatabase.DeleteAll_Wifi_Punchout();
                                    //            Console.WriteLine("newdaysend~~~");
                                    //        }
                                    //        catch (Exception ex)
                                    //        {
                                    //            Console.WriteLine("Error_send~~" + ex.ToString());
                                    //        }


                                    //        checkdate = true;
                                    //        //Console.WriteLine("howmany~" + MapView.PunchDatabase2.GetAccountAsync2().Count());
                                    //        dateDatabase.DeleteAll(); // 讓裡面永遠只保持最新的一筆
                                    //        dateDatabase.SaveAccountAsync(new CheckDate
                                    //        {
                                    //            date = now
                                    //        });

                                    //        fooDoggyDatabase.SaveAccountAsync(new Account
                                    //        {
                                    //            account = acc,
                                    //            password = pwd,
                                    //            identity = iden,
                                    //            login_time = now
                                    //        });
                                    //    }
                                    //    if (_login_time.Equals(oldday2) == false)
                                    //    {
                                    //        //Console.WriteLine("test~~~~2~~~");
                                    //        //Console.WriteLine("date_renew_save~~~");
                                    //        function = "Auto_B1";
                                    //        try
                                    //        {
                                    //            MessagingCenter.Send(this, "NewDayDelete", true);
                                    //            //MapView.AccDatabase.DeleteAll_TempAccount();
                                    //            //MapView.AccDatabase.DeleteAll_Punch();
                                    //            //MapView.AccDatabase.DeleteAll_Punch2();
                                    //            //MapView.AccDatabase.DeleteAll_PunchTmp();
                                    //            //MapView.AccDatabase.DeleteAll_PunchTmp2();
                                    //            ////MapView.PunchYN.DeleteAll();
                                    //            //MapView.name_list_in.Clear();
                                    //            //MapView.name_list_out.Clear();
                                    //            //MapView.WIFI_name_list_in.Clear();
                                    //            //MapView.WIFI_name_list_out.Clear();
                                    //            //MapView.AccDatabase.DeleteAll_Wifi_Punchin();
                                    //            //MapView.AccDatabase.DeleteAll_Wifi_Punchout();
                                    //            Console.WriteLine("newdaysend~~~");
                                    //        }
                                    //        catch (Exception ex)
                                    //        {
                                    //            Console.WriteLine("Error_send~~" + ex.ToString());
                                    //        }


                                    //        checkdate = true;
                                    //        //Console.WriteLine("howmany~" + MapView.PunchDatabase2.GetAccountAsync2().Count());
                                    //        dateDatabase.DeleteAll(); // 讓裡面永遠只保持最新的一筆
                                    //        dateDatabase.SaveAccountAsync(new CheckDate
                                    //        {
                                    //            date = _login_time
                                    //        });
                                    //        fooDoggyDatabase.SaveAccountAsync(new Account
                                    //        {
                                    //            account = acc,
                                    //            password = pwd,
                                    //            identity = iden,
                                    //            login_time = _login_time
                                    //        });
                                    //    }
                                      
                                    //}
                                    //else // 裡面還沒有資料
                                    //{
                                    //    dateDatabase.SaveAccountAsync(
                                    //    new CheckDate
                                    //    {
                                    //        date = _login_time
                                    //    });
                                    //    Console.WriteLine("date_nodata_save~~");
                                    //}

                                    Console.WriteLine("ACC" + token);
                                    //dateDatabase.DeleteAll(); // 讓裡面永遠只保持最新的一筆
                                    //dateDatabase.SaveAccountAsync(new CheckDate
                                    //{
                                    //    date = _login_time
                                    //});
                                    //InitMqttClient();
                                    //ConnectMqttServer();

                                   
                                    //await App.Database.SaveAccountAsync(acc);
                                    //Console.WriteLine("LOGIN2");
                                    //Console.WriteLine("TOKEN2" + token);

                                    // Console.WriteLine("AUTH~~~" + AUTH);
                                    //Console.WriteLine("CHANGE" + totalList.abnormals.Count);
                                    //Console.WriteLine("SHIP~~" + totalList.daily_shipments.Count);
                                    //Console.WriteLine("ABNORMAL~~" + totalList.abnormals.Count);
                                    if (AUTH == "14") // 純外送員 & 社工幫忙送餐
                                    {
                                        //------------------MQTT-------------------
                                        Start();
                                        //--------------------------------------------
                                        Console.WriteLine("4~~~~");
                                        await Navigation.PushModalAsync(new HomeView2());

                                    }
                                    //else if (AUTH == "6" && userList.daily_shipment_nums > 0) // 社工mix外送員
                                    //{
                                    //    Console.WriteLine("6mix~~~~~");
                                    //    await Navigation.PushModalAsync(new HomeViewHelperAndDiliver());

                                    //}
                                    else // 純社工
                                    {
                                        Console.WriteLine("6only~~~~");
                                        await Navigation.PushModalAsync(new HomeView());

                                    }
                                }
                            }
                        }
                        else
                        {
                            login(acc, pwd, iden, time); // token沒拿到，再登入一次，打一次api要token
                        }
                    } else
                    {
                        login(acc, pwd, iden, time); // token沒拿到，再登入一次，打一次api要token
                    }
                    
                    

                    
                    //if (BeaconScan.BleStatus != 0) // 有開藍芽
                    //{
                        
                    //}
                    //else
                    //{
                    //    searchLabel.Text = param.CONNECT_BLUETOOTH_ERROR_MESSAGE; // 沒開藍芽
                    //}
                }
                else // 無網路
                {
                    //searchLabel.Text = param.CONNECT_SERVER_ERROR_MESSAGE;
                    DisplayAlert("系統訊息", "自動登入無網路請稍後再試", "OK");
                    Console.WriteLine("QAQAQA~~~~~");
                }
            }
            catch (Exception ex)
            {
                //await DisplayAlert("錯誤訊息", ex.ToString(), "重試");

                //AutoLogin.IsVisible = true;
                //searchLabel.Text = param.CONNECT_SERVER_ERROR_MESSAGE;
                DisplayAlert("系統訊息", "自動登入登入錯誤請稍後再試" + ex.ToString(), "OK");
                Console.WriteLine("WEWEWEW~~~~");
                Console.WriteLine("EXCEPTION~~~" + ex.ToString());
                //if (!CrossConnectivity.Current.IsConnected)
                //{
                //    AutoLogin.IsVisible = true;
                //    searchLabel.Text = param.CONNECT_SERVER_ERROR_MESSAGE;

                //}
                //else
                //{
                //    AutoLogin.IsVisible = true;
                //    searchLabel.Text = param.CONNECT_BLUETOOTH_ERROR_MESSAGE;
                //}
            }
        }



        private async void login_Clicked(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Internet~~~" + CrossConnectivity.Current.IsConnected);
                if (CrossConnectivity.Current.IsConnected) // 有網路
                {
                    
                    Loginway = "Enter";
                    Console.WriteLine("AAA " + account.Text);
                    Console.WriteLine("BBB " + pwd.Text);
                    Console.WriteLine("internet11~~~" + CrossConnectivity.Current.IsConnected);
                    if(_resIdentity == "送餐員")
                    {
                        _identity = "dp";
                        Console.WriteLine("_residentity11~~~~" + _identity);
                    }
                    else
                    {
                        _identity = "sw";
                        Console.WriteLine("_residentity22~~~~" + _identity);
                    }
                    userList = await web.Login(account.Text, pwd.Text, _identity);
                    //AutoLogin.IsVisible = true;
                   
                    Console.WriteLine("usrstate~~~" + userList.state);
                    Console.WriteLine("internet222~~~" + CrossConnectivity.Current.IsConnected);
                    if (userList.state == "false")
                    {
                        //AutoLogin.IsVisible = true;
                        DisplayAlert("提示", "帳號或密碼錯誤", "ok");
                        //await DisplayAlert("提示", "帳號或密碼錯誤", "ok");
                        //if (CrossConnectivity.Current.IsConnected == false)
                        //{
                        //    await DisplayAlert("提示", "未開啟網路或目前無網路訊號", "ok");
                        //}
                        ////else if(BeaconScan.BleStatus == 0)
                        ////{
                        ////    await DisplayAlert("提示", "藍芽未開啟", "ok");
                        ////}
                        //else
                        //{

                        //}

                    }
                    else
                    {

                        token += userList.acc_token;
                        if(token != null)
                        {
                            fooDoggyDatabase.DeleteAll();
                            Login.IsVisible = false;
                            Login.IsEnabled = false;
                            Console.WriteLine("OOOOOAAAAtoken~~ " + token);
                            AUTH = userList.acc_auth;
                            NAME = userList.acc_name;
                            //Content = ViewService.LoadingLogin();
                            loadingView.IsVisible = true;
                            _login_time = userList.login_time;
                            Console.WriteLine("login_time~~~" + _login_time);
                            
                            // -------------beacon-------------------
                            //BeaconScan scan = new BeaconScan();
                            // -------------beacon-------------------
                            DateTime myDate = DateTime.Now;
                            string time = myDate.ToString("yyyy-MM-dd HH:mm:ss");
                            Console.WriteLine("time~~ " + time);
                            //if (BeaconScan.BleStatus == 0)
                            //{
                            //    //await DisplayAlert("提示", "藍芽未開啟", "ok");
                            //    AutoLogin.IsVisible = true;
                            //    searchLabel.Text = param.CONNECT_BLUETOOTH_ERROR_MESSAGE;
                            //}
                            Console.WriteLine("auth" + AUTH + "auth");
                            Console.WriteLine("name~~~" + userList.acc_name);


                            if (AUTH == "13")
                            {
                                allclientList = await web.Get_All_Client(token);
                                //Console.WriteLine("num~~~~" + userList.daily_shipment_nums);
                                //Console.WriteLine("ALL~~~" + allclientList.Count());
                            }
                            //get_client();
                            //get_dailyShipment();
                            //Account acc = new Account()
                            //{
                            //    account = account.Text,
                            //    password = pwd.Text,
                            //};
                            // Console.WriteLine("ACC" + token);

                            fooDoggyDatabase.SaveAccountAsync(new Account
                            {
                                account = account.Text,
                                password = pwd.Text,
                                identity = _identity,
                                login_time = _login_time,
                                time = _time
                            });
                            Console.WriteLine("LALALA2222~~~~" + fooDoggyDatabase.GetAccountAsync().Count());
                            //await App.Database.SaveAccountAsync(acc);
                            Console.WriteLine("LOGIN2");
                            Console.WriteLine("TOKEN2" + token);
                            if (MainPage._time == "早上")
                            {
                                totalList = await web.Get_Daily_Shipment(MainPage.token);
                                ActivityView.totalList = await web.Get_Daily_Shipment(token);
                                ActivityView.stopList = await web.Get_Stop(token);
                                ActivityView.restoreList = await web.Get_Restore(token);
                                if (totalList.daily_shipments != null)
                                {
                                    Console.WriteLine("count~~~ " + totalList.daily_shipments.Count());
                                }
                                else
                                {
                                    Console.WriteLine("daily_null~~~ ");
                                }
                            }
                            else
                            {
                                totalList = await web.Get_Daily_Shipment_night(MainPage.token);
                                ActivityView.totalList = await web.Get_Daily_Shipment_night(token);
                                ActivityView.stopList = await web.Get_Stop(token);
                                ActivityView.restoreList = await web.Get_Restore(token);
                            }

                            //Console.WriteLine("CHANGE" + totalList.abnormals.Count);
                            //Console.WriteLine("SHIP" + totalList.daily_shipments.Count);
                            //DateTime time = DateTime.Now;
                            //string date = time.ToString("MMdd");
                            //Console.WriteLine("time~~~" + time.ToString("t"));
                            //Console.WriteLine("timeshort~~~~" + time.ToShortTimeString());
                            //Console.WriteLine("time2~~~" + time.ToString("hh tt"));
                            //Console.WriteLine("date~~~~" + time.ToString("MMdd"));
                            //Console.WriteLine("date_database_count~~" + dateDatabase.GetAccountAsync2().Count());
                            if (dateDatabase.GetAccountAsync2().Count() != 0) // 裡面有資料，先比對
                            {
                                string oldday = dateDatabase.GetAccountAsync2().Last().date;
                                Console.WriteLine("oldday~~" + oldday);
                                string now = DateTime.Now.ToString("yyyy-MM-dd");
                                Console.WriteLine("now~~~~" + now);
                                // Console.WriteLine("date~~~" + date);
                                if (_login_time != oldday)
                                {
                                    function = "Click_A1";
                                    Console.WriteLine("date_renew_save~~~");
                                    MessagingCenter.Send(this, "Deletesetnum", true);
                                    Console.WriteLine("send~~~");
                                    //Console.WriteLine("howmany~" + MapView.PunchDatabase2.GetAccountAsync2().Count());
                                    dateDatabase.DeleteAll(); // 讓裡面永遠只保持最新的一筆
                                    dateDatabase.SaveAccountAsync(new CheckDate
                                    {
                                        date = _login_time
                                    });

                                }
                                if(now != oldday)
                                {
                                    function = "Click_A2";
                                    MessagingCenter.Send(this, "Deletesetnum", true);
                                    Console.WriteLine("send~~~");
                                    //Console.WriteLine("howmany~" + MapView.PunchDatabase2.GetAccountAsync2().Count());
                                    dateDatabase.DeleteAll(); // 讓裡面永遠只保持最新的一筆
                                    dateDatabase.SaveAccountAsync(new CheckDate
                                    {
                                        date = now
                                    });
                                }
                            }
                            else // 裡面還沒有資料
                            {
                                string now = DateTime.Now.ToString("yyyy-MM-dd");
                                dateDatabase.SaveAccountAsync(
                                new CheckDate
                                {
                                    date = now
                                });
                                Console.WriteLine("date_nodata_save~~");
                                string oldday = dateDatabase.GetAccountAsync2().Last().date;
                                Console.WriteLine("login_clicked~~~ " + oldday);
                            }

                            //await Navigation.PushModalAsync(new HomeView());
                            Console.WriteLine("AAAUTH~~~ " + AUTH);
                            if (AUTH == "14") // 純外送員 & 社工幫忙外送
                            {
                                //--------------------MQTT--------------------------
                                Start();
                                //-------------------------------------------------
                                loadingView.IsVisible = false;
                               
                                
                                await Navigation.PushModalAsync(new HomeView2());
                                Console.WriteLine("deliver~~ ");
                            }
                            //else if (AUTH == "6" && userList.daily_shipment_nums > 0) // 社工mix外送員
                            //{
                            //    await Navigation.PushModalAsync(new HomeViewHelperAndDiliver());
                            //}
                            else // 純社工
                            {
                                loadingView.IsVisible = false;
                                await Navigation.PushModalAsync(new HomeView());
                                Console.WriteLine("helper~~~ ");
                            }
                        }
                        else
                        {
                            DisplayAlert("系統訊息", "token取得失敗，請再次點擊登入按鈕嘗試登入", "OK");
                        }
                        //await Navigation.PushAsync(new MapView());
                    }
                    //if (BeaconScan.BleStatus != 0) // 有開藍芽
                    //{
                        
                    //}
                    //else
                    //{
                    //    searchLabel.Text = param.CONNECT_BLUETOOTH_ERROR_MESSAGE; // 沒開藍芽
                    //}

                }
                else // 無網路
                {
                    Console.WriteLine("nointernet~~~");
                    //AutoLogin.IsVisible = true;
                    //searchLabel.Text = param.CONNECT_SERVER_ERROR_MESSAGE;
                    DisplayAlert("系統訊息", "輸入帳密登入無網路請稍後再試", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("系統訊息", "輸入帳密登入錯誤請稍後再試" + ex.ToString(), "OK");


                Console.WriteLine("login_error", ex.ToString());
            }

        }

        //private string getUrl(string nowlat, string nowlon)
        private List<string>  getUrl()
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
                Console.WriteLine("QQQQcount~~~ ");
                Console.WriteLine(totalList.daily_shipments.Count);
                for (int i = 0; i < totalList.daily_shipments.Count; i++)
                {

                    if (!Urlname.Contains(totalList.daily_shipments[i].ct_name))
                    {
                        Urlname.Add(totalList.daily_shipments[i].ct_name);
                        Console.WriteLine("~~~~~");
                        Console.WriteLine(totalList.daily_shipments[i].ct_name);

                        if (totalList.daily_shipments[i].ct16.Equals("0") == false && totalList.daily_shipments[i].ct17.Equals("0") == false)
                        {
                            // Console.WriteLine("AA");
                            if (i == 0)
                            {
                                // 過濾掉志工經緯度為0(google map會找不到點)
                                Console.WriteLine("AAA");
                                Console.WriteLine(i);
                                //googleMapUrl = nowlat.ToString() + ',' + nowlon.ToString() + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                                Console.WriteLine(googleMapUrl);
                                googleMapUrl = totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            }
                            else
                            {
                                Console.WriteLine("AAB");
                                Console.WriteLine(i);
                                googleMapUrl = googleMapUrl + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                                Console.WriteLine(googleMapUrl);
                            }
                            //googleMapUrl = googleMapUrl + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                        }
                        else
                        {
                            //Console.WriteLine("BB");
                            if (i == 0)
                            {
                                Console.WriteLine("BBB");
                                Console.WriteLine(i);
                                // 過濾掉志工經緯度為0(google map會找不到點)
                                totalList.daily_shipments[i].ct16 = "";
                                totalList.daily_shipments[i].ct17 = "";
                                googleMapUrl = totalList.daily_shipments[i].ct16 + totalList.daily_shipments[i].ct17 ;
                                //googleMapUrl = nowlat.ToString() + ',' + nowlon.ToString() + '/' + totalList.daily_shipments[i].ct16 + totalList.daily_shipments[i].ct17;
                                Console.WriteLine(googleMapUrl);
                                //googleMapUrl = totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/' + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';
                            }
                            else
                            {
                                Console.WriteLine("XXXX");
                                Console.WriteLine(i);
                                googleMapUrl = googleMapUrl + totalList.daily_shipments[i].ct16 + ',' + totalList.daily_shipments[i].ct17 + '/';

                            }


                        }
                    }
                    Console.WriteLine("i~~ ");
                    Console.WriteLine(i);
                    Console.WriteLine(i % 23);
                    if (i % 23 == 0 && i != 0)
                    {
                        Console.WriteLine("WWWWWW" + i);
                        Urllist.Add(googleMapUrl);
                        googleMapUrl = "";
                    }
                    else
                    {
                        Console.Write("EEEEE" + i);
                        if (i % 23 != 0 && i == totalList.daily_shipments.Count-1)
                        {
                            Console.Write("RRRR" + i);
                            Urllist.Add(googleMapUrl);
                        }
                    }
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
                //for(int i = 0; i < Urllist.Count; i++)
                //{
                //    Console.WriteLine("TTTT" + Urllist[i]);
                //}
                return Urllist;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        private StackLayout pickerStack()
        {
            try
            {
                Label label = new Label
                {
                   FontSize = 20,
                    Text = "身分 : "
                };

                Picker picker = new Picker
                {
                    BackgroundColor = Color.White,
                    Title = "請選擇身分",
                    TextColor = Color.FromHex("#326292"),
                    TitleColor = Color.FromHex("#326292")
                };
                picker.SelectedIndexChanged += usrIdentity_SelectedIndexChanged; // 選了一個職之後會觸發一個事件

                List<string> identityList = new List<string>();
                foreach (var i in identityArray)
                {
                    identityList.Add(i);
                }
                picker.ItemsSource = identityList;

                Frame frame = new Frame // frame包上面那個stacklayout
                {
                    BorderColor = Color.Olive,
                    Padding = new Thickness(10, 5, 10, 5),
                    Margin = new Thickness(0, 0, 0, 0),
                    //BackgroundColor = Color.FromHex("eddcd2"),
                    CornerRadius = 20,
                    HasShadow = false,
                    Content = picker
                };

                StackLayout stack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = { label, frame }
                };
                Label time_label = new Label
                {
                    FontSize = 20,
                    Text = "時段 : "
                };

                Picker time_picker = new Picker
                {
                    BackgroundColor = Color.White,
                    Title = "請選擇時段",
                    TextColor = Color.FromHex("#326292"),
                    TitleColor = Color.FromHex("#326292")
                };
                time_picker.SelectedIndexChanged += usrTime_SelectedIndexChanged; // 選了一個職之後會觸發一個事件

                List<string> timeList = new List<string>();
                foreach (var i in timeArray)
                {
                    timeList.Add(i);
                }
                time_picker.ItemsSource = timeList;

                Frame time_frame = new Frame // frame包上面那個stacklayout
                {
                    BorderColor = Color.Olive,
                    Padding = new Thickness(10, 5, 10, 5),
                    Margin = new Thickness(0, 0, 0, 0),
                    //BackgroundColor = Color.FromHex("eddcd2"),
                    CornerRadius = 20,
                    HasShadow = false,
                    Content = time_picker
                };

                var notice_lable = new Label
                {
                    FontSize = 16,
                    Text = "(社工身分選擇早上或下午皆可以)",
                    TextColor = Color.Red
                };

                StackLayout time_stack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = { time_label, time_frame , notice_lable}
                };

                StackLayout final_stack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = { stack, time_stack }
                };




                return final_stack;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        private void usrIdentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Picker picker = (Picker)sender;
                int selectedIndex = picker.SelectedIndex;

                if (selectedIndex != -1)
                {
                    _resIdentity = (string)picker.ItemsSource[selectedIndex];
                    Console.WriteLine("selected~~~ " + selectedIndex);
                    Console.WriteLine("identity~~~" + _resIdentity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void usrTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Picker picker = (Picker)sender;
                int selectedIndex = picker.SelectedIndex;

                if (selectedIndex != -1)
                {
                    _time = (string)picker.ItemsSource[selectedIndex];
                    Console.WriteLine("time~~~" + _time);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static void Start()
        {
            try
            {
                //runState = true;
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Work));
                //Thread thread = new ThreadStart(InitMqttClient);　　　　//原帖中是這樣寫的 Thread thread = new Thread(new ThreadStart( Work));
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("啟動客戶端出現問題:" + ex.ToString());
            }
        }
        private async static void Work()
        {
            running = true;
            Console.WriteLine("Work >>Begin");
            try
            {
                var factory = new MqttFactory();　　　　　　　　//聲明一個MQTT客戶端的標準步驟 的第一步
                mqttClient = factory.CreateMqttClient() as MqttClient;  //factory.CreateMqttClient()實際是一個介面類型（IMqttClient）,這裡是把他的類型變了一下
                options = new MqttClientOptionsBuilder()　　　　//實例化一個MqttClientOptionsBulider
                    //.WithTcpServer("192.168.50.163", 1883)
                    .WithTcpServer("61.218.250.30",4220)
                    .Build();
                        
                await mqttClient.ConnectAsync(options);      //連接伺服器
                //if (!mqttClient.IsConnected)
                //{
                //    Console.WriteLine("isconnect? " + mqttClient.IsConnected);
                //    //Console.WriteLine("Not connected, connecting from CheckMqttConnection");
                //    try
                //    {

                //        mqttClient.ConnectAsync(options);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e);
                //    }
                //}
                //Console.WriteLine("MQTTconnected");
                await mqttClient.SubscribeAsync(new TopicFilterBuilder()
                  //.WithTopic("sensor/Test/room1")
                  .WithTopic("sensor/Test/room3")
                  //.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                  .Build());
                await mqttClient.SubscribeAsync(new TopicFilterBuilder()
                  //.WithTopic("sensor/Test/room2")
                  .WithTopic("sensor/Test/room4")
                  //.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                  .Build());
                Console.WriteLine("Connected >>Subscribe Success");
                //mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(new Func<MqttClientConnectedEventArgs, Task>(Connected));
                //mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(new Func<MqttClientDisconnectedEventArgs, Task>(Disconnected));
                //mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(new Action<MqttApplicationMessageReceivedEventArgs>(MqttApplicationMessageReceived));
                //while (runState)
                //{

                  //  Thread.Sleep(100);

                //}
            }
            catch (Exception exp)
            {

               // Console.WriteLine("MQTT~~ " + exp);
            }
            Console.WriteLine("Work >>End");

            running = false;

            runState = false;
        }
        
        private static async Task Disconnected(MqttClientDisconnectedEventArgs e)
        {
            try
            {
                Console.WriteLine("Disconnected >>Disconnected Server");
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await mqttClient.ConnectAsync(options);
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Disconnected >>Exception " + exp.Message);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
        private static void MqttApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                string text = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                string Topic = e.ApplicationMessage.Topic; string QoS = e.ApplicationMessage.QualityOfServiceLevel.ToString();
                string Retained = e.ApplicationMessage.Retain.ToString();
                Console.WriteLine("MessageReceived >>Topic:" + Topic + "; QoS: " + QoS + "; Retained: " + Retained + ";");
                Console.WriteLine("MessageReceived >>Msg: " + text);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
        //public async void InitMqttClient()
        //  {
        //      // Create a new MQTT client.
        //      var factory = new MqttFactory();
        //      mqttClient = factory.CreateMqttClient();



        //      // Create TCP based options using the builder.
        //      options = new MqttClientOptionsBuilder()
        //          .WithClientId("Client4")
        //          .WithTcpServer("192.168.50.163", 1883) // Use TCP connection, Port is opptinal
        //                                               //.WithWebSocketServer("broker.hivemq.com:8000/mqtt") // Use WebSocket connection.
        //                                               //.WithCredentials("bud", "%spencer%")
        //                                               //.WithTls()
        //                                               //.WithTls(new MqttClientOptionsBuilderTlsParameters
        //                                               //{
        //                                               //    UseTls = true,
        //                                               //    CertificateValidationCallback = (X509Certificate x, X509Chain y, SslPolicyErrors z, IMqttClientOptions o) =>
        //                                               //    {
        //                                               //        // TODO: Check conditions of certificate by using above parameters.
        //                                               //        return true;
        //                                               //    }
        //                                               //})
        //          .WithCleanSession()
        //          .Build();
        //    await mqttClient.ConnectAsync(options, CancellationToken.None); // Since 3.0.5 with CancellationToken
        //    var message = new MqttApplicationMessageBuilder()
        //        .WithTopic("hello/world")
        //        .WithPayload("hey")
        //        .WithAtLeastOnceQoS()
        //        .Build();
        //    await mqttClient.PublishAsync(message, CancellationToken.None);

        //}



        public async void ConnectMqttServer()
        {
              
 
        }

        
        


        protected override void OnAppearing()
        {
            //setView();
            base.OnAppearing();
            token = "";
            AUTH = "";
            userList = null;
            totalList = null;
            allclientList = null;
        }

    }
}

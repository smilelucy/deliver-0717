using Deliver.Models;
using Deliver.Models.DataInfo;
using Deliver.Services;
using JSJ.Services;
using Newtonsoft.Json;
using Plugin.Connectivity;
using PULI.Models.DataCell;
using PULI.Models.DataInfo;
using PULI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PULI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MemberView : ContentPage
    {
        // public static LoginInfo loginList = null;
        public static string token = "";
        //public static List<ClientInfo> clientList = new List<ClientInfo>(); 
        public static IEnumerable<ClientInfo> clientList = null; // for auth = 4 送餐員 
                                                                 //public static IEnumerable<TotalList> totalList = null;
                                                                 // public static AllClientInfo allclientList = new AllClientInfo();
        public static TotalList totalList = new TotalList();
        public static List<AllClientInfo> allclientList = new List<AllClientInfo>(); // for auth = 6 社工
        public static LoginInfo userList = new LoginInfo();

        //4=送餐員,6社工
        public static string AUTH;
        //public static string user_name;
        Database fooDoggyDatabase;
        public static TempDatabase AccDatabase;
        WebService web = new WebService();
        ParamInfo param = new ParamInfo();
        static string[] loginData = new string[2];
        public string Account;
        public string Password;
        public static string logacc;
        public static string logpwd;
        public static List<PunchTmp> TmpPunchList2 = new List<PunchTmp>();
        public static List<PunchTmp2> TmpPunchList3 = new List<PunchTmp2>();
        //ParamInfo param = new ParamInfo();
        //public static string auth;
        //public static string usrname;

        public MemberView()
        {
            InitializeComponent();
            fooDoggyDatabase = new Database();
            
            //fooDoggyDatabase.DeleteAll();
            setView();
            Messager();
            if(MainPage.AUTH == "13")
            {
                if (MainPage.userList.daily_shipment_nums == 0)
                {
                    FormInfo.IsVisible = false;
                }
            }
            listview.ItemTemplate = new DataTemplate(typeof(RecordCell));
            listview2.ItemTemplate = new DataTemplate(typeof(RecordCell));
            //if(MapView.PunchTmp.GetAccountAsync2().Count > 0)
            //{
            //    setlist();
            //}
            //checkDatabase(); 自動登入
        }

        public async void setView()
        {
           // Console.WriteLine("membername~~~" + MainPage.userList.acc_name);
            if(MainPage.AUTH == "14")
            {
                auth.Text = "送餐員";
                usrname.Text = MainPage.userList.acc_name;
                newdate.Text = MainPage._login_time;
                olddate.Text = wifiuploadrecord.oldday;
                function.Text = MainPage.function;
                if (!CrossConnectivity.Current.IsConnected)
                {
                    activity_btn.IsEnabled = false;
                }
            }
            else
            {
               
                if (string.IsNullOrEmpty(MainPage.NAME))
                {
                    setView();
                }
                else
                {
                    auth.Text = "社工";
                    usrname.Text = MainPage.NAME;
                    //addnew_btn.IsEnabled = true;
                    //addnew_btn.IsVisible = true;
                }

            }
            
            
           
        }
      

        

        public async void logout(object sender, EventArgs e)
        {
            
            bool resultEnd = await DisplayAlert(param.SYSYTEM_MESSAGE, "登出需要關閉APP，確定要登出嗎?", param.DIALOG_AGREE_MESSAGE, param.DIALOG_DISAGREE_MESSAGE);
            if (resultEnd)
            {
                if(MainPage.AUTH == "14") // 送餐員
                {
                    fooDoggyDatabase.DeleteAll();
                   // MapView.AccDatabase.DeleteAll_TempAccount();
                    MapView.AccDatabase.DeleteAll_Punch();
                    MapView.AccDatabase.DeleteAll_Punch2();
                    //PunchDataBaseTmp ppunchTmp = new PunchDataBaseTmp();
                    //ppunchTmp.DeleteAll();
                    MapView.AccDatabase.DeleteAll_PunchTmp();
                    MapView.AccDatabase.DeleteAll_PunchTmp2();
                    //MapView.PunchYN.DeleteAll();
                    MapView.AccDatabase.DeleteAll_Wifi_Punchout();
                    MapView.AccDatabase.DeleteAll_Wifi_Punchin();
                    MapView.name_list_in.Clear();
                    MapView.name_list_out.Clear();
                    MapView.WIFI_name_list_in.Clear();
                    MapView.WIFI_name_list_out.Clear();
                    MainPage.dateDatabase.DeleteAll();
                    TestView.ChooseDB.DeleteAll();
                    TestView.ResetDB.DeleteAll();
                    TestView.EntryDB.DeleteAll();
                    TestView.EntrytxtDB.DeleteAll();
                    TestView.ResetLabelDB.DeleteAll();
                    //TmpPunchList2.Clear();
                    MessagingCenter.Send(this, "LOGOUT", true);
                }
                else // 社工
                {
                    fooDoggyDatabase.DeleteAll();
                    MapView.AccDatabase.DeleteAll_TempAccount();
                    MapView.AccDatabase.DeleteAll_Punch();
                    MapView.AccDatabase.DeleteAll_Punch2();
                    //PunchDataBaseTmp ppunchTmp = new PunchDataBaseTmp();
                    //ppunchTmp.DeleteAll();
                    MapView.AccDatabase.DeleteAll_PunchTmp();
                    MapView.AccDatabase.DeleteAll_PunchTmp2();
                   // MapView.PunchYN.DeleteAll();
                    MapView.name_list_in.Clear();
                    MapView.name_list_out.Clear();
                    MainPage.dateDatabase.DeleteAll();
                    MessagingCenter.Send(this, "LOGOUT", true);
                }
                
                //TestView.TmpCheckList.Clear(); 
                //AccDatabase.DeleteAll();
                //var accountList = await App.Database.GetAccountAsync();
                Console.WriteLine("LOGOUT~~~~");

                //Console.WriteLine("TEST" + test);
                if (fooDoggyDatabase.GetAccountAsync().Count() == 0)
                {
                    Console.WriteLine("fooSUCESS");
                }
                else
                {
                    Console.WriteLine("fooFAIL");
                    //fooDoggyDatabase.DeleteAll();
                    Console.WriteLine("failQAQ~~~" + fooDoggyDatabase.GetAccountAsync().Count());
                }
                if (MapView.AccDatabase.GetAccountAsync2().Count() == 0)
                {
                    Console.WriteLine("accSUCESS");
                }
                else
                {
                    Console.WriteLine("accFAIL");
                    //fooDoggyDatabase.DeleteAll();
                    Console.WriteLine("failQAQ~~~" + fooDoggyDatabase.GetAccountAsync().Count());
                }
                if (MapView.AccDatabase.GetAccountAsync2_Punch().Count() == 0)
                {
                    Console.WriteLine("punchSUCESS");
                }
                else
                {
                    Console.WriteLine("punchFAIL");
                    //fooDoggyDatabase.DeleteAll();
                    Console.WriteLine("failQAQ~~~" + fooDoggyDatabase.GetAccountAsync().Count());
                }
                if (MapView.AccDatabase.GetAccountAsync2_Punch2().Count() == 0)
                {
                    Console.WriteLine("punch2SUCESS");
                }
                else
                {
                    Console.WriteLine("punch2FAIL");
                    //fooDoggyDatabase.DeleteAll();
                    Console.WriteLine("failQAQ~~~" + fooDoggyDatabase.GetAccountAsync().Count());
                }
                //if (ppunchTmp.GetAccountAsync2().Count() == 0)
                //{
                //    Console.WriteLine("punchtmpSUCESS");
                //}
                //else
                //{
                //    Console.WriteLine("punchtmpFAIL");
                //    //fooDoggyDatabase.DeleteAll();
                //    Console.WriteLine("failQAQ~~~" + ppunchTmp.GetAccountAsync2().Count());
                //}

                //if (MapView.PunchTmp2.GetAccountAsync2().Count() == 0)
                //{
                //    Console.WriteLine("punchtmp2SUCESS");
                //}
                //else
                //{
                //    Console.WriteLine("punchtmp2FAIL");
                //    //fooDoggyDatabase.DeleteAll();
                //    Console.WriteLine("failQAQ~~~" + fooDoggyDatabase.GetAccountAsync().Count());
                //}

                //if (MapView.PunchYN.GetAccountAsync2().Count() == 0)
                //{
                //    Console.WriteLine("punchYNSUCESS");
                //}
                //else
                //{
                //    Console.WriteLine("punchYNFAIL");
                //    //fooDoggyDatabase.DeleteAll();
                //    Console.WriteLine("failQAQ~~~" + fooDoggyDatabase.GetAccountAsync().Count());
                //}
                //Console.WriteLine("ACCLIST" + accountList[0].account);
                //Console.WriteLine("ACCLIST" + accountList[0].password);

                //UserInfo.IsVisible = false;
                //UserInfo.IsEnabled = false;
                //UsrInfobtn.IsVisible = false;
                //UsrInfobtn.IsEnabled = false;
                //FormInfo.IsVisible = false;
                //FormInfo.IsEnabled = false;
                //userList = null;

             //   await DependencyService.Get<IQRCode>().ScanAsync();
                MessagingCenter.Send(this, "OUT", true);
                DependencyService.Get<IAppExit>().Exit();// 把APP關掉再重開
            }
            
            
            //DependencyService.Get<IAppExit>().Exit(); // 把APP關掉再重開


            //await Navigation.PopAsync();
            //await Navigation.PushAsync(new MainPage());
            //MainPage = new MainPage();
            //checkDatabase();
        }

        //public async void login(string acc, string pwd)
        //{
        //    try
        //    {

        //        MultipartFormDataContent formData = new MultipartFormDataContent();
        //        formData.Add(new StringContent(acc), "acc_user");
        //        formData.Add(new StringContent(pwd), "acc_password");

        //        HttpClient _client = new HttpClient();
        //        var uri = new Uri(string.Format("http://59.120.147.32:8080/lt_care/api/account/login"));

        //        var response = await _client.PostAsync(uri, formData);


        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            var list = JsonConvert.DeserializeObject<LoginInfo>(content);
        //            if (list.state == "false")
        //            {
        //                await DisplayAlert("提示", "帳號或密碼錯誤", "ok");
        //            }
        //            else
        //            {
        //                Login.IsVisible = false;
        //                Login.IsEnabled = false;
        //                UserInfo.IsVisible = true;
        //                UserInfo.IsEnabled = true;
        //                UsrInfobtn.IsVisible = true;
        //                UsrInfobtn.IsEnabled = true;
        //                FormInfo.IsVisible = true;
        //                FormInfo.IsEnabled = true;

        //                token += list.acc_token;
        //                AUTH = list.acc_auth;
        //                auth.Text = list.acc_auth;
        //                usrname.Text = list.acc_name;
        //                Console.WriteLine("auth" + AUTH);
                        
        //                if (auth.Text == "4")
        //                {
        //                    auth.Text = "送餐員";

        //                }
        //                else
        //                {
        //                    auth.Text = "社工";
        //                    Console.WriteLine("auth" + auth);
        //                }
        //                //get_client();
        //                //get_dailyShipment();

        //                totalList = await web.Get_Daily_Shipment(token);
        //                //Console.WriteLine("CHANGE" + totalList.abnormals.Count);
        //                //Console.WriteLine("CHANGEWHO" + totalList.abnormals[0].ClientName);
        //                //Console.WriteLine("CHANGEDIFF" + totalList.abnormals[0].different);
        //                //Console.WriteLine("SHIP" + totalList.daily_shipments.Count);
        //                Messager();
        //                //await Navigation.PushAsync(new MapView());
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Console.WriteLine("RRR" + acc);
        //        Console.WriteLine("SSS" + pwd);
        //        await DisplayAlert("login_error", "message" + ex.Message + "\n" + "stackTrace" + ex.StackTrace, "ok");
        //    }
        //}

        private async void setlist()
        {
            //listview = null;
            //TmpPunchList2.Clear();
            listview.ItemsSource = null;
            //TmpPunchList2.Clear();
            //listview.Items.Clear();
            //listview.ItemTemplate = new DataTemplate(typeof(RecordCell)); // 把模式設為activitycell
            listview.SelectedItem = null; // 
            TmpPunchList2.Clear();
            TmpPunchList2 = MapView.AccDatabase.GetAccountAsync2_PunchTmp();
            listview.ItemsSource = TmpPunchList2; // itemtemplate的資料來源
                                         //listview.ItemsSource = MapView.name_list_in2; // itemtemplate的資料來源
        }
        private async void setlist2()
        {
            //listview2 = null;
            //TmpPunchList3.Clear();
            listview2.ItemsSource = null;
            //TmpPunchList3.Clear();
            //listview2.ItemTemplate = new DataTemplate(typeof(RecordCell)); // 把模式設為activitycell
            listview2.SelectedItem = null; // 
            TmpPunchList3.Clear();
            TmpPunchList3 = MapView.AccDatabase.GetAccountAsync2_PunchTmp2();
            listview2.ItemsSource = TmpPunchList3; // itemtemplate的資料來源
                                                  //listview.ItemsSource = MapView.name_list_in2; // itemtemplate的資料來源
        }

        private async void total_btn_clicked(object sender, EventArgs e)
        {
            //Messager2();
            await Navigation.PushAsync(new ActivityView());
        }

        private async void upload_btn_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UploadView());
        }

        private async void activity_btn_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ActivityView());

        }
        //private void Messager2()
        //{
        //    MessagingCenter.Send(this, "SET_FORM_TOTAL", true);
        //}


        private void Messager()
        {
            try
            {
                MessagingCenter.Subscribe<MapView, bool>(this, "Setlist", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        try
                        {
                            Console.WriteLine("setlist~~~");

                            setlist();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("setlistviewerror~~~");
                            DisplayAlert(param.SYSYTEM_MESSAGE, ex.ToString(), param.DIALOG_AGREE_MESSAGE);
                            Console.WriteLine(ex.ToString());
                        }
                        //totalList = new TotalList();

                    }
                });
                MessagingCenter.Subscribe<MapView, bool>(this, "Setlist2", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        //totalList = new TotalList();
                        try
                        {
                            Console.WriteLine("setlist2~~~");

                            setlist2();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("setlistview2error~~~");
                            DisplayAlert(param.SYSYTEM_MESSAGE, ex.ToString(), param.DIALOG_AGREE_MESSAGE);
                            Console.WriteLine(ex.ToString());
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        protected override void OnAppearing()
        {
            //setView();
            base.OnAppearing();
            //token = "";
            //AUTH = "";
            //user_name = "";
            //clint_Datas = new List<clint_data>();
            totalList = new TotalList();
            allclientList = new List<AllClientInfo>();
        }

        //lock the previous page
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
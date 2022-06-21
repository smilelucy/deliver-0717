using PULI.Models.DataCell;
using PULI.Models.DataInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PULI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class wifiuploadrecord : ContentPage
    {
        ParamInfo param = new ParamInfo();
        private List<Wifi_Punchin> Wifi_Punchin_List = new List<Wifi_Punchin>();
        private List<Wifi_Punchout> Wifi_Punchout_List = new List<Wifi_Punchout>();
        private string wifi_page_function;
        public static string oldday2;
        public static string oldday;


        public wifiuploadrecord()
        {
            InitializeComponent();
            checkSQLite();
        }

        private void checkSQLite()
        {
            try {
                if (MainPage.dateDatabase.GetAccountAsync2().Count() != 0) // 裡面有資料，先比對
                {
                    oldday = MainPage.dateDatabase.GetAccountAsync2().Last().date;
                    oldday2 = MainPage.fooDoggyDatabase.GetAccountAsync().Last().login_time;
                    string now = DateTime.Now.ToString("yyyy-MM-dd");
                    Console.WriteLine("nowWIFIupload~~~~" + now);
                    Console.WriteLine("oldday~~~wifiupload~~~" + oldday);
                    Console.WriteLine("oldday2~~~wifiupload~~~" + oldday2);
                    //Console.WriteLine("_login_time~~main~~" + _login_time);
                    ////Console.WriteLine("LoginTime~~~" + LoginTime);
                    // Console.WriteLine("date~~~" + date);

                    if (now.Equals(oldday) == false)
                    {
                        wifi_page_function = "Wifi_Auto_B2";
                        try
                        {
                            //MapView.AccDatabase.DeleteAll_TempAccount();
                            MapView.AccDatabase.DeleteAll_Punch();
                            MapView.AccDatabase.DeleteAll_Punch2();
                            MapView.AccDatabase.DeleteAll_PunchTmp();
                            MapView.AccDatabase.DeleteAll_PunchTmp2();
                            MapView.AccDatabase.DeleteAll_Wifi_Punchin();
                            MapView.AccDatabase.DeleteAll_Wifi_Punchout();
                            //MapView.PunchYN.DeleteAll();
                            MapView.name_list_in.Clear();
                            MapView.name_list_out.Clear();
                            MapView.WIFI_name_list_in.Clear();
                            MapView.WIFI_name_list_out.Clear();
                            Console.WriteLine("wifi_newdaysend~~~");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error_send~~" + ex.ToString());
                        }


                        //checkdate = true;
                        //Console.WriteLine("howmany~" + MapView.PunchDatabase2.GetAccountAsync2().Count());
                        MainPage.dateDatabase.DeleteAll(); // 讓裡面永遠只保持最新的一筆
                        MainPage.dateDatabase.SaveAccountAsync(new CheckDate
                        {
                            date = now
                        });
                    }
                    if (MainPage._login_time.Equals(oldday) == false)
                    {
                        //Console.WriteLine("test~~~~2~~~");
                        //Console.WriteLine("date_renew_save~~~");
                        wifi_page_function = "Wifi_Auto_B1";
                        try
                        {
                            // MapView.AccDatabase.DeleteAll_TempAccount();
                            MapView.AccDatabase.DeleteAll_Punch();
                            MapView.AccDatabase.DeleteAll_Punch2();
                            MapView.AccDatabase.DeleteAll_PunchTmp();
                            MapView.AccDatabase.DeleteAll_PunchTmp2();
                            //MapView.PunchYN.DeleteAll();
                            MapView.name_list_in.Clear();
                            MapView.name_list_out.Clear();
                            MapView.WIFI_name_list_in.Clear();
                            MapView.WIFI_name_list_out.Clear();
                            MapView.AccDatabase.DeleteAll_Wifi_Punchin();
                            MapView.AccDatabase.DeleteAll_Wifi_Punchout();
                            Console.WriteLine("newdaysend~~~");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error_send~~" + ex.ToString());
                        }


                        //checkdate = true;
                        //Console.WriteLine("howmany~" + MapView.PunchDatabase2.GetAccountAsync2().Count());
                        MainPage.dateDatabase.DeleteAll(); // 讓裡面永遠只保持最新的一筆
                        MainPage.dateDatabase.SaveAccountAsync(new CheckDate
                        {
                            date = MainPage._login_time
                        });

                    }
                    Console.WriteLine("wifiupload_wifi_page_function~~~" + wifi_page_function);
                }
                else // 裡面還沒有資料
                {
                    MainPage.dateDatabase.SaveAccountAsync(
                    new CheckDate
                    {
                        date = MainPage._login_time
                    });
                    Console.WriteLine("date_nodata_save~~");
                }
            } catch (Exception ex)
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

        private async void wifi_punchin_setlist()
        {
            //listview = null;
            //TmpPunchList2.Clear();
            wifi_punchin_listview.ItemsSource = null;
            //TmpPunchList2.Clear();
            //listview.Items.Clear();
            //listview.ItemTemplate = new DataTemplate(typeof(RecordCell)); // 把模式設為activitycell
            wifi_punchin_listview.SelectedItem = null; // 
            Wifi_Punchin_List.Clear();
            Wifi_Punchin_List = MapView.AccDatabase.GetAccountAsync2_Wifi_Punchin();
            //Console.WriteLine("wifi_punchin_tmpnum~~~" + Wifi_Punchin_List.Count());
            wifi_punchin_listview.ItemsSource = Wifi_Punchin_List; // itemtemplate的資料來源
                                                  //listview.ItemsSource = MapView.name_list_in2; // itemtemplate的資料來源
        }
        private async void wifi_punchout_setlist2()
        {
            //listview2 = null;
            //TmpPunchList3.Clear();
            wifi_punchout_listview.ItemsSource = null;
            //TmpPunchList3.Clear();
            //listview2.ItemTemplate = new DataTemplate(typeof(RecordCell)); // 把模式設為activitycell
            wifi_punchout_listview.SelectedItem = null; // 
            Wifi_Punchout_List.Clear();
            Wifi_Punchout_List = MapView.AccDatabase.GetAccountAsync2_Wifi_Punchout();
            //Console.WriteLine("wifi_punchout_tmpnum2~~~" + Wifi_Punchout_List.Count());
            wifi_punchout_listview.ItemsSource = Wifi_Punchout_List; // itemtemplate的資料來源
                                                   //listview.ItemsSource = MapView.name_list_in2; // itemtemplate的資料來源
        }

        private void Messager()
        {
            try
            {
                MessagingCenter.Subscribe<MapView, bool>(this, "wifi_Setlist_in", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        try
                        {
                            //Console.WriteLine("setlist~~~");

                            wifi_punchin_setlist();
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine("wifi_setlistviewerror~~~");
                            DisplayAlert(param.SYSYTEM_MESSAGE, ex.ToString(), param.DIALOG_AGREE_MESSAGE);
                            //Console.WriteLine(ex.ToString());
                        }
                        //totalList = new TotalList();

                    }
                });
                MessagingCenter.Subscribe<MapView, bool>(this, "wifi_Setlist_out", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        //totalList = new TotalList();
                        try
                        {
                            //Console.WriteLine("setlist2~~~");

                            wifi_punchout_setlist2();
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine("wifi_setlistview2error~~~");
                            DisplayAlert(param.SYSYTEM_MESSAGE, ex.ToString(), param.DIALOG_AGREE_MESSAGE);
                            //Console.WriteLine(ex.ToString());
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
            Messager();
            wifi_punchin_listview.ItemTemplate = new DataTemplate(typeof(RecordCell));
            wifi_punchout_listview.ItemTemplate = new DataTemplate(typeof(RecordCell));
            base.OnAppearing();
        }
    }
}
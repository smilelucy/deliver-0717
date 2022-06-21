using PULI.Services;
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
    public partial class HomeView2 : BottomTabPage
    {
        public HomeView2()
        {
            InitializeComponent();
            Messager();
        }
        private void Messager()
        {
            MessagingCenter.Send(this, "BEACON_SCAN", true);
            Console.WriteLine("BEACONSCAN");
            Console.WriteLine("AUTH~~~" + MainPage.AUTH);
            MessagingCenter.Subscribe<MainPage, bool>(this, "NewDayDelete", (sender, arg) =>
            {
                if (arg)
                {
                    Console.WriteLine("newdayrecieve~homeview2~");
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
                }

            });
            if (MainPage.AUTH == "14") 
            {
                // run 地圖
                try
                {
                    if(MainPage.totalList.daily_shipments != null)
                    {
                        MessagingCenter.Send(this, "SET_MAP", true);
                        Console.WriteLine("SETMAP_4");
                    }
                }
                catch(Exception ex)
                {
                    DisplayAlert("系統訊息", "Error : deliver_homeview2_totalList_daily_shipment_SET_MAP", "ok");
                }

                // run問卷
                try
                {
                    if (MainPage.totalList.daily_shipments != null)
                    {
                        MessagingCenter.Send(this, "SET_FORM", true);
                        Console.WriteLine("SETFORM");
                    }

                }
                catch (Exception ex)
                {
                    DisplayAlert("系統訊息", "Error : deliver_homeview2_totalList_daily_shipment_SET_FORM", "ok");
                    Console.WriteLine("exception1~~ " + ex.ToString());
                    //Console.WriteLine("count~~ " + MainPage.totalList.daily_shipments.Count());
                }

                // run總表
                try
                {
                    if (MainPage.totalList.daily_shipments != null)
                    {
                        MessagingCenter.Send(this, "SET_SHIPMENT_FORM", true);
                        Console.WriteLine("SET_SHIPMENT_FORM");
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("系統訊息", "Error : deliver_homeview2_totalList_daily_shipment_SET_SHIPMENT_FORM", "ok");
                    Console.WriteLine("exception2~~ " + ex.ToString());
                }

            }
            //else
            //{
            //    // run 社工地圖
            //    try
            //    {
            //        if(MainPage.allclientList != null)
            //        {
            //            MessagingCenter.Send(this, "SET_MAP", true); 
            //            Console.WriteLine("SETMAP_6");
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        DisplayAlert("系統訊息", "Error : helper_homeview2_totalList_daily_shipment_SET_MAP", "ok");
            //    }

            //    MessagingCenter.Send(this, "SET_AddCln_FORM", true);
            //    //MessagingCenter.Send(this, "SET_AddCln_FORM", true);
            //    Console.WriteLine("AddCln");
            //}


           
            

           
            MessagingCenter.Subscribe<MainPage, bool>(this, "Deletesetnum", (sender, arg) =>
            {
                // do something when the msg "UPDATE_BONUS" is recieved
                if (arg)
                {
                    Console.WriteLine("Deletesetnum~~homeview2~~~");
                    MapView.AccDatabase.DeleteAll_Punch2();
                }
            });
            MessagingCenter.Subscribe<MemberView, bool>(this, "OUT", (sender, arg) =>
            {
                if (arg)
                {
                    Navigation.PopModalAsync();

                }
            });

        }
    }
}
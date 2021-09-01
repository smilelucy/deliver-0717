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
    public partial class HomeView : BottomTabPage
    {
        public HomeView()
        {
            InitializeComponent();
            Messager();
            
        }
        private void Messager()
        {
            MessagingCenter.Send(this, "BEACON_SCAN", true);
            Console.WriteLine("BEACONSCAN");
           

            // run 社工地圖
            if(MainPage.AUTH == "13")
            {
                try
                {
                    if (MainPage.allclientList != null)
                    {
                        MessagingCenter.Send(this, "SET_MAP", true);
                        Console.WriteLine("SETMAP_6");
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("系統訊息", "Error : helper_homeview_allclientList_SET_MAP", "ok");
                }
            }
            


            //if (MainPage.AUTH == "4")
            //{
            //    if (MainPage.totalList.daily_shipments.Count != 0)
            //    {
            //        MessagingCenter.Send(this, "SET_SHIPMENT_FORM", true);
            //        Console.WriteLine("SETSHIPMENT");
            //    }
            //    else
            //    {
            //        //DisplayAlert("系統訊息", "後臺尚未產生資料或資料接收不齊全", "ok");
            //        Console.WriteLine("no shipment_homeview~~");
            //        Console.WriteLine("homecount~~ " + MainPage.totalList.daily_shipments.Count);
            //    }
            //}
            //else
            //{
            //    if(MainPage.userList.daily_shipment_nums > 0)
            //    {
            //        MessagingCenter.Send(this, "SET_SHIPMENT_FORM", true); // for社工總表
            //        Console.WriteLine("SETSHIPMENT_6");
            //    }
            //}

           
            if(MainPage.AUTH == "13")
            {
                MessagingCenter.Send(this, "SET_AddCln_FORM", true);
                //MessagingCenter.Send(this, "SET_AddCln_FORM", true);
                Console.WriteLine("AddCln");
            }
            MessagingCenter.Subscribe<MainPage, bool>(this, "Deletesetnum", (sender, arg) =>
            {
                // do something when the msg "UPDATE_BONUS" is recieved
                if (arg)
                {
                    Console.WriteLine("Deletesetnum~~hinmeview~~~");
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
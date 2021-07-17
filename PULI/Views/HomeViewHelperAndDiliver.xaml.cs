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
    public partial class HomeViewHelperAndDiliver : BottomTabPage
    {
        public HomeViewHelperAndDiliver()
        {
            InitializeComponent();
            Messager();
        }
        private void Messager()
        {
            MessagingCenter.Send(this, "BEACON_SCAN", true);
            Console.WriteLine("BEACONSCAN");
            if (MainPage.AUTH == "4")
            {
                if (MainPage.totalList.daily_shipments.Count != 0)
                {
                    MessagingCenter.Send(this, "SET_MAP", true); // 傳送"UPDATE_BONUS"的指令給訂閱者(Subscribe)
                    Console.WriteLine("SETMAP_4");
                }
            }
            else
            {
                if (MainPage.allclientList.Count() != 0)
                {
                    MessagingCenter.Send(this, "SET_MAP", true); // 傳送"UPDATE_BONUS"的指令給訂閱者(Subscribe)
                    Console.WriteLine("SETMAP_6");
                }
            }
            if (MainPage.AUTH == "4")
            {
                if (MainPage.totalList.daily_shipments.Count != 0)
                {
                    MessagingCenter.Send(this, "SET_FORM", true);
                    Console.WriteLine("SETFORM");
                }
            }
            else
            {
                if (MainPage.userList.daily_shipment_nums > 0)
                {
                    MessagingCenter.Send(this, "SET_FORM", true);
                    Console.WriteLine("SETFORM"); // for外送員的回饋單
                }
            }

            if (MainPage.AUTH == "4")
            {
                if (MainPage.totalList.daily_shipments.Count != 0)
                {
                    MessagingCenter.Send(this, "SET_SHIPMENT_FORM", true);
                    Console.WriteLine("SETSHIPMENT");
                }
            }
            else
            {
                if (MainPage.userList.daily_shipment_nums > 0)
                {
                    MessagingCenter.Send(this, "SET_SHIPMENT_FORM", true); // for社工總表
                    Console.WriteLine("SETSHIPMENT_6");
                }
            }

            if (MainPage.AUTH == "4")
            {
                if (MainPage.totalList.abnormals.Count != 0)
                {
                    MessagingCenter.Send(this, "SET_CHANGE_FORM", true);
                    Console.WriteLine("CHANGE");
                }
            }
            else
            {
                if (MainPage.userList.daily_shipment_nums > 0)
                {
                    MessagingCenter.Send(this, "SET_CHANGE_FORM", true);
                    Console.WriteLine("CHANGE_6"); // for社工的異動表
                }
            }
            if (MainPage.AUTH == "6")
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
                    Console.WriteLine("Deletesetnum~~6mix~~~~");
                    MapView.PunchDatabase2.DeleteAll();
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
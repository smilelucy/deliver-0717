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
        public wifiuploadrecord()
        {
            InitializeComponent();
            
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
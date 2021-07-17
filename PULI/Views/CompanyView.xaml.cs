using Deliver.Services;

using PULI.Models;
using PULI.Models.DataInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PULI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompanyView : ContentPage
    {
        public static List<questionnaire> questionnaireslist = new List<questionnaire>();
        public static List<qb> qbList = new List<qb>();
        //IEnumerable<upload_Q> whichup = null;
        //public static List<questionnaire> yesnoquestionlist = new List<questionnaire>();

        public ObservableCollection<questionnaire> yesnoquestionlist { get; set; }
        public ObservableCollection<questionnaire> selectedList;
        public static List<upload_Q> uplList = new List<upload_Q>();

        //public static IEnumerable<upload_Q> uplListAll = null;
        public static List<upload_Q> uplList2 = new List<upload_Q>();
        WebService web = new WebService();
        public static bool ans;
        public static bool ans2 = false;
        public static bool ans3 = false;
        public static string finalans1;
        public static string finalans2;
        List<Item> anslist = null;
        public static string _wqh_s_num; // 問卷編號
        public static string _qh_s_num;  // 
        public static string _qb_s_num; // 第1題還是第2題
        public static string _wqb01; // 問題答案
        Dictionary<string, bool> yesnoList = new Dictionary<string, bool>();
        public static int count = 0;
        public static string name;


        //public ObservableCollection<Item> veggies { get; set; }

        //public List<Item> selectedItems; // define `selectedItems` as the list of selected items.

        public CompanyView()
        {
            InitializeComponent();
            Messager();
            //_questionnaireslist = questionnaireslist;
        }

        private async void get_questionnaire()
        {
            questionnaireslist = await web.Get_Questionaire(MainPage.token);
            Console.WriteLine("dfgdf" + MainPage.token);
            qbList = await web.Get_Qb(MainPage.token);
            Console.WriteLine("GETQUES" + questionnaireslist[0]);
            questionnaireView.ItemsSource = questionnaireslist;
            questionnaireView.HasUnevenRows = true;

         
            //selectedItems = new List<Item>(); // init the `selectedItems`

            //veggies = new ObservableCollection<Item>();
            //for (int i = 0; i < questionnaireslist.Count; i++)
            //{
            //    veggies.Add(new Item { 
            //        Name = questionnaireslist[i].ClientName, 
            //        Questionnum = questionnaireslist[i].qbs[1].qb_order, 
            //        Question = questionnaireslist[i].qbs[1].qb01, 
            //        isChecked = false});

            //}
            //listview.ItemsSource = veggies;
        }
        //MapModel previousModel;
        //private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    if (previousModel != null)
        //    {
        //        previousModel.IsSelected = false;
        //    }
        //    MapModel currentModel = ((CheckBox)sender).BindingContext as MapModel;
        //    previousModel = currentModel;
        //    Console.WriteLine("VVVV" + previousModel);

        //}
        //private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if (previousModel != null)
        //    {
        //        previousModel.IsSelected = false;
        //    }
        //    MapModel currentModel = e.SelectedItem as MapModel;
        //    currentModel.IsSelected = true;
        //    previousModel = currentModel;
        //}

        //private void CheckBox_CheckChanged(object sender, EventArgs e)
        //{
        //    var checkbox = (Plugin.InputKit.Shared.Controls.CheckBox)sender;
        //    var ob = checkbox.BindingContext as Item;

        //    if (ob != null)
        //    {
        //        System.Diagnostics.Debug.WriteLine("isChecked = " + ob.isChecked + "<--->  Name = " + ob.Name + "<--->  Questionnum = " + ob.Questionnum + "<--->  Question = " + ob.Question);
        //        if (ob.isChecked) // true
        //        {
        //            // then Add
        //            selectedItems.Add(ob);

        //            Console.WriteLine("OBBB" + selectedItems);
        //            Console.WriteLine("MMMMM" + );
        //        }
        //        else
        //        {
        //            // remove the item
        //        }
        //    }
        //}
        private async void post_questionClicked(object sender, EventArgs e)
        {
            int num = 0;
            Console.WriteLine("TOKEN" + MainPage.token);
            for (int i = 0; i < questionnaireslist.Count; i++)
            {
                //upload_Q r1 = new upload_Q();
                //_wqh_s_num = 
                //var _ansList = questionnaireslist.Where(info => info.ClientName == name);
                //Console.WriteLine("GGGG" + _ansList);
                //Console.WriteLine("LIIIII" + yesnoList[questionnaireslist[i].ClientName] + questionnaireslist[i].ClientName);
                //Console.WriteLine("LIIIII" + yesnoList[0]);

                // 目前設定問卷每題一定要填答案才可以送出
                Console.WriteLine("YYYY" + i + questionnaireslist[i].qbs.Count);
                Console.WriteLine("XXXX" + i + questionnaireslist[i].qbs[0].qb_order);
                Console.WriteLine("VVVV" + i + questionnaireslist[i].qbs[1].qb_order);
                Console.WriteLine("howmany" + questionnaireslist[i].qbs.Count);
                for (int j = 1; j < questionnaireslist[i].qbs.Count; j++)
                {
                    //IEnumerable<upload_Q> r1 = null;

                    Console.WriteLine("TESTTTT   + check" + j);
                    string count = Convert.ToString(j);
                    Console.WriteLine("count" + count);
                    string whichques = questionnaireslist[i].ClientName + questionnaireslist[i].wqh_s_num + count;
                    Console.WriteLine("whichques" + whichques);
                    if (yesnoList[whichques] == true)
                    {
                        finalans1 = "1";
                    }
                    else
                    {
                        finalans1 = "2";
                    }
                    Console.WriteLine("finalans1: " + finalans1);
                    Console.WriteLine("count" + count);
                    Console.WriteLine("wqh_s_num" + questionnaireslist[i].wqh_s_num);
                    Console.WriteLine("qh_s_num" + questionnaireslist[i].qbs[j-1].qb01);
                    //upload_Q whichup = "r" + num;
                    //ObservableCollection<upload_Q> r1 = new ObservableCollection<upload_Q>();
                    //r1.Add(new upload_Q
                    //{
                    //    wqh_s_num = questionnaireslist[i].wqh_s_num,  // 工作問卷編號(彈出來的問卷) 編號幾就會傳到編號己的後台欄位
                    //    qh_s_num = questionnaireslist[i].qbs[j].qb01,  // 是否看到案主
                    //    qb_s_num = count,  // 哪一題(1 or 2)
                    //    wqb01 = finalans1 // 問題答案(複選)
                    //                      //wqb01 = "2" // 問題答案(複選)
                    //});
                    upload_Q r1 = new upload_Q()
                    {
                        wqh_s_num = questionnaireslist[i].wqh_s_num,  // 工作問卷編號(彈出來的問卷) 編號幾就會傳到編號己的後台欄位
                        qh_s_num = questionnaireslist[i].qbs[j-1].qb01,  // 是否看到案主
                        qb_s_num = count,  // 哪一題(1 or 2)
                        wqb01 = finalans1 // 問題答案(複選)
                                          //wqb01 = "2" // 問題答案(複選)
                    };

                    Console.WriteLine("ADD");
                    uplList.Add(r1);
                    
                    //uplList.Add(r2);
                    //num = num + 1;
                }
                // string check1 = questionnaireslist[i].ClientName + questionnaireslist[i].wqh_s_num + 1;
                // string check2 = questionnaireslist[i].ClientName + questionnaireslist[i].wqh_s_num + 2;


                // if (yesnoList[check1] == true)
                // {
                //     finalans1 = "1";
                // }
                // else
                // {
                //     finalans1 = "2";
                // }
                // if (yesnoList[check2] == true)
                // {
                //     finalans2 = "1";
                // }
                // else
                // {
                //     finalans2 = "2";
                // }

                //// Console.WriteLine("finalans" + questionnaireslist[i].ClientName + finalans);

                // Console.WriteLine("finalans1: " + finalans1);
                // Console.WriteLine("finalans2: " + finalans2);
                // upload_Q r1 = new upload_Q()
                // {
                //     wqh_s_num = questionnaireslist[i].wqh_s_num,  // 工作問卷編號(彈出來的問卷) 編號幾就會傳到編號己的後台欄位
                //     qh_s_num = questionnaireslist[i].qbs[1].qb01,  // 是否看到案主
                //     qb_s_num = "2",  // 哪一題(1 or 2)
                //     wqb01 = finalans2 // 問題答案(複選)
                //     //wqb01 = "2" // 問題答案(複選)
                // };
                // upload_Q r2 = new upload_Q()
                // {
                //     wqh_s_num = questionnaireslist[i].wqh_s_num,  // 工作問卷編號(彈出來的問卷) 編號幾就會傳到編號己的後台欄位
                //     qh_s_num = questionnaireslist[i].qbs[0].qb01,  // 是否有狀況
                //     qb_s_num = "1",  // 哪一題(1 or 2)
                //     wqb01 = finalans1 // 問題答案(複選)
                //     //wqb01 = "2" // 問題答案(複選)
                // };

                // uplList.Add(r1);
                // uplList.Add(r2);
            }
            //int num = MapView.cList.Count;


            work_data resault = new work_data()
            {
                //upload_Qs = uplList,
                // upload_Qs2 = uplList2
            };
            //bool web_res3 = await web.Save_Questionaire(MemberView.token, resault);
            //if (web_res3 == true)
            //{
            //    //yesnoList.Clear();
            //    await DisplayAlert("上傳結果", "上傳成功！", "ok");
            //}
            //else
            //{
            //    await DisplayAlert("提示", "上傳失敗！", "ok");
            //}
        }

        


        private void Messager()
        {
            try
            {
                MessagingCenter.Subscribe<HomeView, bool>(this, "SET_FORM", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        Console.WriteLine("MESS");
                        get_questionnaire();
                    }
                });
                MessagingCenter.Subscribe<HomeView2, bool>(this, "SET_FORM", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        Console.WriteLine("MESS");
                        get_questionnaire();
                    }
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public void yescheck(object sender, CheckedChangedEventArgs e)
        {
            //ans2 = true;
            var t = ((CheckBox)sender).ClassId;
            //ClassId = questionnaireslist[0].wqh_s_num;
            if (e.Value)
            {
                name = t;
                Console.WriteLine("fdsf : " + t + " :是");
                yesnoList[t] = true;
                
                //Console.WriteLine("PPP" + yesnoList["張三1971"]);
                //Console.WriteLine("PPP" + yesnoList["張三1972"]);
            } else
            {
                Console.WriteLine("fdsf : " + t + " :否");
                //yesnoList[t] = ;
            }
            //Console.WriteLine("classid " + ClassId);
            
            //var _ansList = questionnaireslist.Where(info => info.wqh_s_num == ClassId);

        }

        public void yesonecheck(object sender, CheckedChangedEventArgs e)
        {
            //ans2 = true;
            var t = ((CheckBox)sender).ClassId;
            //ClassId = questionnaireslist[0].wqh_s_num;
            if (e.Value)
            {

                Console.WriteLine("fdsf : " + t + " :是");
                yesnoList[t] = true;
            }
            else
            {
                Console.WriteLine("fdsf : " + t + " :否");
                //yesnoList[t] = null;
            }
            //Console.WriteLine("classid " + ClassId);
            
            //var _ansList = questionnaireslist.Where(info => info.wqh_s_num == ClassId);

        }

        public void nocheck(object sender, CheckedChangedEventArgs e)
        {
            var t = ((CheckBox)sender).ClassId;
            //ClassId = questionnaireslist[0].wqh_s_num;
            if (e.Value)
            {

                Console.WriteLine("fdsf : " + t + " :否");
                yesnoList[t] = false;
            }
            else
            {
                Console.WriteLine("fdsf : " + t + " :是");
            }
            //Console.WriteLine("classid " + ClassId);
            
            //ans2 = e.Value;
            //if (ans == true)
            //{
            //    ans3 = "2";
            //    Console.WriteLine("ans3" + ans);
            //}


            //}

        }
        public void noonecheck(object sender, CheckedChangedEventArgs e)
        {
            var t = ((CheckBox)sender).ClassId;
            //ClassId = questionnaireslist[0].wqh_s_num;
            if (e.Value)
            {

                Console.WriteLine("fdsf : " + t + " :否");
                yesnoList[t] = false;
            }
            else
            {
                Console.WriteLine("fdsf : " + t + " :是");
            }
            //Console.WriteLine("classid " + ClassId);
            //yesnoList[ClassId] = false;
            //ans2 = e.Value;
            //if (ans == true)
            //{
            //    ans3 = "2";
            //    Console.WriteLine("ans3" + ans);
            //}


            //}

        }

    }
}
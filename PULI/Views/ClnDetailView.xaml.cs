using Deliver.Models.DataInfo;
using Deliver.Services;
using PULI.Models.DataInfo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PULI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClnDetailView : ContentPage
    {
        ParamInfo param = new ParamInfo();
        WebService web = new WebService();

        AllClientInfo _clnList = null;
        
        private string _s_num = "";
        private string s_num = "";
        private string ct01 = "";
        private string ct02 = "";
        private string ct03 = "";
        private string ct04 = "";
        private string ct05 = "";
        private string ct06_homephone = "";
        private string ct06_telephone = "";
        private string bday = "";
       
    
        public ClnDetailView(AllClientInfo clnList)
        {
            InitializeComponent();
            Messager();
            RenewForm.IsVisible = true;
            RenewForm.IsEnabled = true;
            _clnList = clnList;
            _s_num = _clnList.s_num;
            _usrNamea.Text = _clnList.ct01;
            _usrNameb.Text = _clnList.ct02;
            _usrCard.Text = _clnList.ct03;
            if(_clnList.ct04 == "M")
            {
                _usrGender.Text = "男";
            }
            else
            {
                _usrGender.Text = "女";
            }
            
            bday = _clnList.ct05;
            
            //bday2 = string.Format("{0:yyyy-MM-dd}", bday);
            //Console.WriteLine("bday2~~~" + bday2); 
            try
            {
                DateTime NewDate = DateTime.ParseExact(bday, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                //DateTime NewDate2 = DateTime.ParseExact(NewDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                Console.WriteLine("DAY~~~" + NewDate);
                DP.Date = NewDate;

            }
            catch
            {
                Console.WriteLine("ERROR~~~");
            }
            Console.WriteLine("bday~~~~" + bday);
            
            _usrPhone.Text = _clnList.ct06_homephone; // 家電
            _usrCellPhone.Text = _clnList.ct06_telephone; // 手機
            //_usrLat = _clnList.ct16_actual;
            //_usrLon = _clnList.ct17_actual;
            //setView(_clnList);
        }

        //private void setView(AllClientInfo clnList)
        //{
        //    _usrNamea = clnList.ct01;
        //    _usrNameb = clnList.ct02;
        //    _usrCard = clnList.ct03;
        //    _usrGender = clnList.ct04;
        //    _usrBirthday = clnList.ct05;
        //    _usrPhone = clnList.ct06;
        //    _usrLat = clnList.ct16_actual;
        //    _usrLon = clnList.ct17_actual;

        //    res_infoStack.Children.Add(entryStack(param.RESERVE_INFO_NAMEA, Keyboard.Text, _usrNamea));
        //    res_infoStack.Children.Add(entryStack(param.RESERVE_INFO_NAMEB, Keyboard.Text, _usrNameb));
        //    res_infoStack.Children.Add(entryStack(param.RESERVE_INFO_CARD, Keyboard.Text, _usrCard));
        //    res_infoStack.Children.Add(entryStack(param.RESERVE_INFO_GENDER, Keyboard.Text, _usrGender));
        //    res_infoStack.Children.Add(entryStack(param.RESERVE_INFO_BIRTHDAY, Keyboard.Text, _usrBirthday));
        //    res_infoStack.Children.Add(entryStack(param.RESERVE_INFO_TELEPHONE, Keyboard.Text, _usrPhone));
        //}

        //private StackLayout entryStack(string name, Keyboard keyboard, string value)
        //{
        //    Label label = new Label
        //    {
        //        Margin = new Thickness(10, 0, 10, 0),
        //        TextColor = Color.Black,
        //        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
        //        Text = name
        //    };

        //    RoundEntry entry = new RoundEntry
        //    {
        //        Placeholder = "點我輸入",
        //        ReturnType = ReturnType.Send,
        //        HeightRequest = 50,
        //        Keyboard = keyboard,
        //        VerticalOptions = LayoutOptions.Center,
        //        ClassId = name,
        //        PlaceholderColor = Color.FromHex("#326292"),
        //        TextColor = Color.FromHex("#326292"),
        //        Text = value

        //    };

        //    entry.TextChanged += Entry_TextChanged;

        //    StackLayout stack = new StackLayout
        //    {
        //        Orientation = StackOrientation.Vertical,
        //        Children = { label, entry }
        //    };

        //    return stack;
        //}

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((Entry)sender).ClassId == param.RESERVE_INFO_NAMEA)
            {
                _usrNamea.Text = e.NewTextValue; // 姓
            }
            else if (((Entry)sender).ClassId == param.RESERVE_INFO_NAMEB)
            {
                _usrNameb.Text = e.NewTextValue; // 名
            }
            else if (((Entry)sender).ClassId == param.RESERVE_INFO_CARD)
            {
                _usrCard.Text = e.NewTextValue; // 身分證
            }
            else if (((Entry)sender).ClassId == param.RESERVE_INFO_GENDER)
            {
                _usrGender.Text = e.NewTextValue; // 性別
            }
            //else if (((Entry)sender).ClassId == param.RESERVE_INFO_BIRTHDAY)
            //{
            //    _usrBirthday.Text = e.NewTextValue; // 生日
            //}
            else if (((Entry)sender).ClassId == param.RESERVE_INFO_TELEPHONE)
            {
                _usrPhone.Text = e.NewTextValue; // 手機
            }
            //else if (((Entry)sender).ClassId == param.RESERVE_INFO_LON)
            //{
            //    _usrLon.Text = e.NewTextValue; // 經度
            //}
            //else if (((Entry)sender).ClassId == param.RESERVE_INFO_LAT)
            //{
            //    _usrLat.Text = e.NewTextValue; // 緯度
            //}
            //else if (((Entry)sender).ClassId == param.RESERVE_INFO_AGE)
            //{
            //    _usrAge.Text = e.NewTextValue;
            //}
        }

        private void DP_DateSelected(object sender, DateChangedEventArgs e)
        {

            //lblResult.Text += $"原資料：{e.OldDate.ToString("yyyy-MM-dd")}" + Environment.NewLine;
            bday = e.NewDate.ToString("yyyyMMdd");
            Console.WriteLine("bdayLA~~~~" + bday);
        }

        private bool CheckFill(string ct01, string ct02, string ct03, string ct04, string ct05, string ct06_home, string ct06_cellphone)
        {
            
            Console.WriteLine("ct01~~~" + ct01);
            Console.WriteLine("ct02~~~" + ct02);
            Console.WriteLine("ct03~~~" + ct03);
            Console.WriteLine("ct04~~~" + ct04);
            //ct04 = _usrGender.Text;
            Console.WriteLine("ct05~~~" + ct05);
            Console.WriteLine("ct06_home~~~" + ct06_home);
            if (String.IsNullOrEmpty(ct01))
            {
                return false;
                //await DisplayAlert("提示", "姓 欄位未填", "OK");
            }
            if (String.IsNullOrEmpty(ct02))
            {
                return false;
                //await DisplayAlert("提示", "名 欄位未填", "OK");
            }
            if (String.IsNullOrEmpty(ct03))
            {
                return false;
                //await DisplayAlert("提示", "身分證 欄位未填", "OK");
            }
            if (String.IsNullOrEmpty(ct04))
            {
                return false;
                //await DisplayAlert("提示", "性別 欄位未填", "OK");
            }
            if (String.IsNullOrEmpty(ct05))
            {
                return false;
                //await DisplayAlert("提示", "生日 欄位未填", "OK");
            }
            if (String.IsNullOrEmpty(ct06_home)) // 家電
            {
                return false;
                //await DisplayAlert("提示", "電話 欄位未填", "OK");
            }
            if (String.IsNullOrEmpty(ct06_cellphone)) // 手機
            {
                return false;
                //await DisplayAlert("提示", "電話 欄位未填", "OK");
            }
            return true;
        }

        private async void resCommand_Clicked(object sender, EventArgs e)
        {
            s_num = _s_num;
            Console.WriteLine("s_num~~~" + s_num);
            ct01 = _usrNamea.Text;
            Console.WriteLine("ct01~~~" + ct01);
            ct02 = _usrNameb.Text;
            Console.WriteLine("ct02~~~" + ct02);
            ct03 = _usrCard.Text;
            Console.WriteLine("ct03~~~" + ct03);
            if (_usrGender.Text == "男")
            {
                ct04 = "M";
            }
            else
            {
                ct04 = "F";
            }
            Console.WriteLine("ct04~~~" + ct04);
            //ct04 = _usrGender.Text;
            ct05 = bday;
            Console.WriteLine("ct05~~~" + ct05);
            ct06_homephone = _usrPhone.Text;
            Console.WriteLine("ct06_home~~~" + ct06_homephone);
            ct06_telephone = _usrCellPhone.Text;
            Console.WriteLine("ct06_cellphone~~~" + ct06_telephone);
            bool CheckAns = CheckFill(ct01, ct02, ct03, ct04, ct05,ct06_homephone, ct06_telephone);
            Console.WriteLine("checkans~~" + CheckAns);
            if(CheckAns == true)
            {
                bool web_res = await web.Save_ReNew_Client_Info(MainPage.token, _s_num, ct01, ct02, ct03, ct04, ct05, ct06_homephone, ct06_telephone);
                Console.WriteLine("webres~~~" + web_res);
                if (web_res == true)
                {
                    await DisplayAlert(param.SYSYTEM_MESSAGE, "修改成功", param.DIALOG_AGREE_MESSAGE);
                    MessagingCenter.Send(this, "Resetlist", true);
                    Console.WriteLine("SEND~~~~");
                    await Navigation.PopAsync();
                }
                else
                {
                    if (String.IsNullOrEmpty(ct01))
                    {
                        await DisplayAlert("提示", "姓 欄位未填", "OK");
                    }
                    if (String.IsNullOrEmpty(ct02))
                    {
                        await DisplayAlert("提示", "名 欄位未填", "OK");
                    }
                    if (String.IsNullOrEmpty(ct03))
                    {
                        await DisplayAlert("提示", "身分證 欄位未填", "OK");
                    }
                    if (String.IsNullOrEmpty(ct04))
                    {
                        await DisplayAlert("提示", "性別 欄位未填", "OK");
                    }
                    if (String.IsNullOrEmpty(ct05))
                    {
                        await DisplayAlert("提示", "生日 欄位未填", "OK");
                    }
                    if (String.IsNullOrEmpty(ct06_homephone))
                    {
                        await DisplayAlert("提示", "家電 欄位未填", "OK");
                    }
                    if (String.IsNullOrEmpty(ct06_telephone))
                    {
                        await DisplayAlert("提示", "手機 欄位未填", "OK");
                    }
                    //await DisplayAlert(param.SYSYTEM_MESSAGE, "ERROR", param.DIALOG_AGREE_MESSAGE);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(ct01))
                {
                    await DisplayAlert("提示", "姓 欄位未填", "OK");
                }
                if (String.IsNullOrEmpty(ct02))
                {
                    await DisplayAlert("提示", "名 欄位未填", "OK");
                }
                if (String.IsNullOrEmpty(ct03))
                {
                    await DisplayAlert("提示", "身分證 欄位未填", "OK");
                }
                if (String.IsNullOrEmpty(ct04))
                {
                    await DisplayAlert("提示", "性別 欄位未填", "OK");
                }
                if (String.IsNullOrEmpty(ct05))
                {
                    await DisplayAlert("提示", "生日 欄位未填", "OK");
                }
                if (String.IsNullOrEmpty(ct06_homephone))
                {
                    await DisplayAlert("提示", "家電 欄位未填", "OK");
                }
                if (String.IsNullOrEmpty(ct06_telephone))
                {
                    await DisplayAlert("提示", "手機 欄位未填", "OK");
                }
            }

        }

        private void Messager()
        {
            MessagingCenter.Subscribe<MemberView, bool>(this, "OUT", (sender, arg) =>
            {
                if (arg)
                {
                    Navigation.PushAsync(new ClnDetailView(_clnList));
                }
            });
        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
        }
    }

}
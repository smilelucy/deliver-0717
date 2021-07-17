using Deliver.Models.DataInfo;
using Deliver.Services;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using PULI.Model;
using PULI.Models.DataCell;
using PULI.Models.DataInfo;
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
    public partial class AddNewClnView : ContentPage
    {

        ParamInfo param = new ParamInfo();
        WebService web = new WebService();
        public static List<AllClientInfo> allclientList2 = new List<AllClientInfo>(); // for auth = 6 社工
        public static List<AllClientInfo2> allclientList3 = new List<AllClientInfo2>();
        IGeolocator location;
        Plugin.Geolocator.Abstractions.Position position;
        int location_DesiredAccuracy = 20;
        bool is_user_name = true, is_user_phone = true;
        string[] genderArray = new string[] { "男", "女" };
        int Age;
        //string _usrName = "", _usrPhone = "", _usrAge = "", _usrGender = "";
        bool isPrivacy = false;
        public static double _nowLon;
        public static double _nowLat;
        public static string _usrBirthday;
        string ct01 = "";
        string ct02 = "";
        string ct03 = "";
        string ct04 = "";
        string ct05 = "";
        string ct06_homephone = "";
        string ct06_telephone = "";
        double ct16_actual;
        double ct17_actual;
        public static string gender;
        public static StreamContent img_sc;
        public AddNewClnView()
        {
            InitializeComponent();
            allclientList2 = null;
            allclientList3 = null;
            Messager();
            Device.StartTimer(TimeSpan.FromSeconds(5), OnTimerTick);
            if(MainPage.AUTH == "6")
            {
                Console.WriteLine("addnew~~~");
                AllForm.IsVisible = true;
                setallcln();
                AddForm.IsVisible = true;
                AddForm.IsEnabled = true;
                RenewForm.IsVisible = false;
                RenewForm.IsEnabled = false;
                buttonadd.IsVisible = true;
                buttonadd.IsEnabled = true;
                buttonrenew.IsEnabled = true;
                buttonrenew.IsVisible = true;
            }
            else
            {
                AllForm.IsVisible = false;
            }
            //_usrLat.Text = _nowLat;
            //privacyLabel.Text = param.PRIVACY_MESSAGE;
            //Messager();
        }

        private async void setallcln()
        {
            allclientList2 = await web.Get_All_Client(MainPage.token);
            allclientList3 = await web.Get_All_Client2(MainPage.token);
            listview.ItemTemplate = new DataTemplate(typeof(RenewCell));

            listview.ItemsSource = allclientList2;
        }

        private async void buttonadd_Clicked(object sender, EventArgs e)
        {
            AddForm.IsVisible = true;
            AddForm.IsEnabled = true;
            RenewForm.IsVisible = false;
            RenewForm.IsEnabled = false;
        }

        // 典籍已預約資訊button
        private async void buttonrenew_Clicked(object sender, EventArgs e)
        {
            AddForm.IsVisible = false;
            AddForm.IsEnabled = false;
            RenewForm.IsVisible = true;
            RenewForm.IsEnabled = true;
        }

        private void Button_OnPressed(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackgroundColor = Color.FromHex("f1ab86");
                btn.TextColor = Color.White;
                buttonrenew.BackgroundColor = Color.White;
                buttonrenew.TextColor = Color.FromHex("f1ab86");
            }

        }
        private void Button2_OnPressed(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackgroundColor = Color.FromHex("f1ab86");
                btn.TextColor = Color.White;
                buttonadd.BackgroundColor = Color.White;
                buttonadd.TextColor = Color.FromHex("f1ab86");
            }
        }

        private async void listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem != null)
                {
                    int item = e.SelectedItemIndex;
                    Console.WriteLine("ITEM~~~~" + item);
                    MessagingCenter.Send(this, "Renew", true);
                    await Navigation.PushAsync(new ClnDetailView(allclientList2.ElementAt(item)));
                    ((ListView)sender).SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //public void setView()
        //{


        //    //privacyLabel.Text = param.PRIVACY_MESSAGE;

        //    //_usrName = userInfo.user_name;
        //    //_usrPhone = userInfo.user_telephone;
        //    //_usrAge = userInfo.user_age;
        //    //_usrGender = userInfo.user_gender;

        //    //if (!string.IsNullOrEmpty(userInfo.user_name))
        //    //{
        //    //    is_user_name = false;
        //    //}

        //    //if (!string.IsNullOrEmpty(userInfo.user_telephone))
        //    //{
        //    //    is_user_phone = false;
        //    //}

        //    usr_infoStack.Children.Add(entryStack(param.RESERVE_INFO_NAME, Keyboard.Text));
        //    usr_infoStack.Children.Add(entryStack(param.RESERVE_INFO_TELEPHONE, Keyboard.Telephone));
        //    usr_infoStack.Children.Add(entryStack(param.RESERVE_INFO_AGE, Keyboard.Numeric));
        //    usr_infoStack.Children.Add(pickerStack(param.RESERVE_INFO_GENDER));
        //}

        private StackLayout entryStack(string name, Keyboard keyboard)
        {
            try
            {
                Label label = new Label
                {
                    Margin = new Thickness(10, 0, 10, 0),
                    TextColor = Color.Black,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    Text = name
                };

                RoundEntry entry = new RoundEntry
                {
                    Placeholder = "點我輸入",
                    ReturnType = ReturnType.Send,
                    HeightRequest = 50,
                    Keyboard = keyboard,
                    VerticalOptions = LayoutOptions.Center,
                    ClassId = name,
                    PlaceholderColor = Color.FromHex("#326292"),
                    TextColor = Color.FromHex("#326292"),

                };

                entry.TextChanged += Entry_TextChanged;

                StackLayout stack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = { label, entry }
                };

                return stack;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ENTRY~~~" + ex.ToString());
                return null;
            }
           
        }

        //private StackLayout pickerStack(string name)
        //{
        //    try
        //    {
        //        Label label = new Label
        //        {
        //            Margin = new Thickness(10, 0, 10, 0),
        //            TextColor = Color.Black,
        //            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
        //            Text = name
        //        };

        //        Picker picker = new Picker
        //        {
        //            BackgroundColor = Color.White,
        //            Title = "請選擇",
        //            TitleColor = Color.FromHex("#326292")
        //        };
        //        picker.SelectedIndexChanged += usrGender_SelectedIndexChanged; // 選了一個職之後會觸發一個事件

        //        List<string> genderList = new List<string>();
        //        foreach (var i in genderArray)
        //        {
        //            genderList.Add(i);
        //        }
        //        picker.ItemsSource = genderList;

        //        StackLayout stack = new StackLayout
        //        {
        //            Orientation = StackOrientation.Vertical,
        //            Children = { label, picker }
        //        };

        //        return stack;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return null;
        //    }
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

        private async Task getLocation()
        {
            try
            {
                location = CrossGeolocator.Current;
                if (location != null)
                {
                    try
                    {
                        location.DesiredAccuracy = location_DesiredAccuracy;
                        position = await location.GetPositionAsync(TimeSpan.FromSeconds(1));
                        _nowLon = position.Longitude;

                        //_usrLon.Text = _nowLon;
                        Console.WriteLine("Lon11~~~" + _nowLon);
                        //_usrLon.Text = _nowLon;
                        _nowLat = position.Latitude;
                        //_usrLat.Text = _nowLat;
                        Console.WriteLine("Lat11~~~" + _nowLat);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert(param.SYSYTEM_MESSAGE, param.LOCATION_ERROR_MESSAGE, param.DIALOG_AGREE_MESSAGE);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR!!!!! ",ex.ToString());
            }
        }

        bool OnTimerTick()
        {
            Task.Run(() =>
            {
                try
                {
                    // Run code here
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        // UI interaction goes here
                        await getLocation();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    DisplayAlert(param.SYSYTEM_MESSAGE, param.LOCATION_ERROR_MESSAGE, param.DIALOG_MESSAGE);
                }
            });
            return true;
        }

        private void DP_DateSelected(object sender, DateChangedEventArgs e)
        {
            
            //lblResult.Text += $"原資料：{e.OldDate.ToString("yyyy-MM-dd")}" + Environment.NewLine;
            _usrBirthday = e.NewDate.ToString("yyyyMMdd");
        }

        private void privacy_check_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            isPrivacy = (e.Value) ? true : false;
        }

       
        private async void btnCam_Clicked(object sender, EventArgs e)
        {
            try
            {
                //img.IsVisible = true;
                await CrossMedia.Current.Initialize();
                var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                {
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    Directory = "Xamarin",
                    SaveToAlbum = true
                });
                if (photo != null)
                {
                    img.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                    //BinaryReader br = new BinaryReader(photo.GetStream());
                    img_sc = new StreamContent(photo.GetStream());
                    Console.WriteLine("path~~" + photo.AlbumPath);
                    //Console.WriteLine($"File size: {img_sc.} bytes");
                    //bmpPic = BytesToBitmap(photo.GetStream())
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Errorcamera :", ex.Message.ToString(), "ok");
            }
        }

        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }



        private async void post_Clicked(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(note.Text))
            //    await DisplayAlert("提示", "您尚有東西未填寫", "ok");
            //else
            //{
            //    try
            //    {
            //        HttpClient client = new HttpClient();
            //        client.DefaultRequestHeaders.Add("AUTHORIZATION", "Token " + MainPage.token);
            //        MultipartFormDataContent formData = new MultipartFormDataContent();
            //        //img_sc.Headers.Add("Content-Type", "image/jpeg");

            //        if (!string.IsNullOrEmpty(note.Text))
            //            formData.Add(new StringContent(note.Text), "WorkLogNote");
            //        //WorkLogPicture
            //        formData.Add(img_sc, "WorkLogPicture", "WorkLogPicture");
            //        var request = new HttpRequestMessage()
            //        {
            //            RequestUri = new Uri("http://59.120.147.32:8080/lt_care/api/account/save_worklog"),
            //            Method = HttpMethod.Post,
            //            Content = formData
            //        };
            //        request.Headers.Add("Connection", "closed");

            //        var response = await client.SendAsync(request);
            //        Console.WriteLine("WHY ~" + response.ToString());
            //        if (response.IsSuccessStatusCode)
            //        {
            //            var content = await response.Content.ReadAsStringAsync();
            //            if (content == "ok")
            //            {
            //                Console.WriteLine("xxxxxxxxxxxxx : " + content);
            //                await Navigation.PopAsync();
            //                await DisplayAlert("上傳結果", "上傳成功！", "ok");
            //            }
            //            else
            //            {
            //                Console.WriteLine("================================ : " + content);
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("WHY2~ ");
            //            Console.WriteLine("WHY ~2" + response.ToString());
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        await DisplayAlert("ErrorMA~~~", ex.Message.ToString(), "ok");
            //        Console.WriteLine("uploaderror");
            //    }
            //}
        }

        private async void Command_Clicked(object sender, EventArgs e)
        {

            //AllClientInfo insertList = new AllClientInfo
            //{
            //    ct01 = _usrNamea.Text,
            //    ct02 = _usrNameb.Text,
            //    ct03 = _usrCard.Text,
            //    ct04 = _usrGender.Text,
            //    ct05 = _usrBirthday.Text,
            //    ct06 = _usrPhone.Text,
            //    ct16_actual = _nowLon,
            //    ct17_actual = _nowLat
            //};
            Content = ViewService.Loading();
            ct01 = _usrNamea.Text;
            ct02 = _usrNameb.Text;
            ct03 = _usrCard.Text;
            if(_usrGender.Text == "男")
            {
                gender = "M";
            }
            else
            {
                gender = "F";
            }
            ct04 = gender;

            ct05 = _usrBirthday;
            Console.WriteLine("BBBday~~" + _usrBirthday);
            ct06_homephone = _usrPhone.Text;
            ct06_telephone = _usrCellPhone.Text;
            ct16_actual = _nowLon;
            ct17_actual = _nowLat;
            Console.WriteLine("Lon~~" + _nowLon);
            Console.WriteLine("Lat~~" + _nowLat);
            bool web_res = await web.Save_New_Client_Info(MainPage.token, ct01, ct02, ct03, ct04, ct05, ct06_homephone, ct06_telephone, ct16_actual, ct17_actual, img_sc);
            if(web_res == true)
            {
                Content = AddNewLayout;
                await DisplayAlert("Alert","新增案主成功", "OK");
               

                setallcln();
                _usrNamea.Text = "";
                _usrNameb.Text = "";
                _usrCard.Text = "";
                _usrGender.Text = "";
                _usrPhone.Text = "";
                Navigation.PushAsync(new AddNewClnView());
                //await Navigation.PopAsync();
                //photo = null;
                //  CrossMedia.Current.Initialize();
                //img_sc = null;
                //img.IsVisible = false;
                // await CrossMedia.Current.Initialize();
            }
            else
            {
                if(String.IsNullOrEmpty(ct01))
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

                //await DisplayAlert("Alert", "Error", "OK");
            }
        }
        //private void usrGender_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Picker picker = (Picker)sender;
        //    int selectedIndex = picker.SelectedIndex;
        //    if (selectedIndex != -1)
        //    {
        //        _usrGender = (string)picker.ItemsSource[selectedIndex];
        //    }
        //}

        private void Messager()
        {
            MessagingCenter.Subscribe<ClnDetailView, bool>(this, "Resetlist", (sender, arg) =>
            {
                // do something when the msg "UPDATE_BONUS" is recieved
                if (arg)
                {
                    //setView();
                    setallcln();
                    Console.WriteLine("RESEIVE~~~");
                }
            });
            MessagingCenter.Subscribe<MemberView, bool>(this, "OUT", (sender, arg) =>
            {
                if (arg)
                {
                    Navigation.PushAsync(new AddNewClnView());
                }
            });
        }
    }
}
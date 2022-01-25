using Deliver.Services;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PULI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PULI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadView : ContentPage
    {
        WebService web = new WebService();
        private static bool istakephoto = false;
        private static bool ispickphoto = false;
        private string _selecttext = "";
        List<string> selectvalueList = new List<string>();
        private static string NOWHOME; // for拍照上傳帶案主家編號
        private static string NOWREH;  // for拍照上傳帶reh_s_num
        public UploadView()
        {
            InitializeComponent();
            identityStack.Children.Add(pickerStack());
            if(MapView.totalList.daily_shipments.Count != 0)
            {
                foreach (var i in MapView.totalList.daily_shipments)
                {
                    selectvalueList.Add(i.ct_s_num);
                }
                NOWREH = MapView.totalList.daily_shipments[0].reh_s_num;
            }
            

        }

        StreamContent img_sc;
        MediaFile _mediafile;
        StreamContent img_from_gallery;
        //Bitmap bmpPic;
        private async void btnCam_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                //var IMAGE_NAME = "aaa";
                //string saveDirectory = @"/storage/Pictures";
                istakephoto = true;
                ispickphoto = false;
                var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                {
                    //DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    //DefaultCamera = CameraDevice.Front,
                    Directory = "弗傳慈心基金會",
                    CompressionQuality = 40,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    SaveToAlbum = true,
                    
                    //Name = IMAGE_NAME + ".png"
                    Name = $"{DateTime.Now}.png",
                    
                });
                Console.WriteLine("picname~~~ " + DateTime.UtcNow);
                if (photo != null)
                {
                    img.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                    //BinaryReader br = new BinaryReader(photo.GetStream());
                    img_sc = new StreamContent(photo.GetStream());
                    Console.WriteLine("path~~" + photo.AlbumPath);
                    Console.WriteLine("path2~~~ " + photo.Path);
                    //Console.WriteLine($"File size: {img_sc.} bytes");
                    //bmpPic = BytesToBitmap(photo.GetStream())
                }
                
                //bool checkuse = await DisplayAlert("系統訊息", "進入路徑storage/Pictures", "Yes", "No");
                //if (checkuse == true)
                //{
                //    if (!Directory.Exists(saveDirectory))
                //    {
                //        Directory.CreateDirectory(saveDirectory);
                //    }
                //    string fileName = Path.GetFileName(DateTime.UtcNow + ".png");
                //    string fileSavePath = Path.Combine(saveDirectory, fileName);
                //    Console.WriteLine("newpath~~ " + fileSavePath);
                //    File.Copy(DateTime.UtcNow + ".png", fileSavePath, true);
                //}

                //var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());
                //if (selectedImageFile != null)
                //{
                //    se
                //}

            }
            catch (Exception ex)
            {
                await DisplayAlert("Errorcamera :", ex.Message.ToString(), "ok");
            }
        }
        
        private async void PickPhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            img_sc = null;
            ispickphoto = true;
            istakephoto = false;
            _mediafile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
            }) ;
            if(_mediafile == null)
            {
                return;
            }

            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                return _mediafile.GetStream();
            });
            img_from_gallery = new StreamContent(_mediafile.GetStream());
        }

        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        
        private StackLayout pickerStack()
        {
            try
            {
                Label label = new Label
                {
                    FontSize = 25,
                    Text = "案主 : "
                };

                Picker picker = new Picker
                {
                    BackgroundColor = Color.White,
                    Title = "請選擇案主",
                    TextColor = Color.FromHex("#326292"),
                    TitleColor = Color.FromHex("#326292")
                };
                picker.SelectedIndexChanged += usrIdentity_SelectedIndexChanged; // 選了一個職之後會觸發一個事件

                List<string> identityList = new List<string>();
                foreach (var i in MapView.totalList.daily_shipments)
                {
                    identityList.Add(i.ct_name);
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
                




                return stack;

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
                    _selecttext = (string)picker.ItemsSource[selectedIndex];
                    NOWHOME = selectvalueList[selectedIndex];
                    //Console.WriteLine("selected~~~ " + selectedIndex);
                    //Console.WriteLine("text~~~" + _selecttext);
                    //Console.WriteLine("value~~~" + NOWHOME);
                    //Console.WriteLine("reh~~~" + NOWREH);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        
        private async void post_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(note.Text))
                await DisplayAlert("提示", "您尚有東西未填寫", "ok");
            else
            {
                try
                {
                    if(!CrossConnectivity.Current.IsConnected)
                    {
                        DisplayAlert("提示", "目前無網路無法上傳，照片已存入相簿，可在有網路的時候透過照片選擇上傳", "ok");
                    }
                    else
                    {
                        Content = ViewService.Loading();

                        //bool post = web.Post_work(MainPage.token, note.Text, img_sc);
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("AUTHORIZATION", "Token " + MainPage.token);
                        MultipartFormDataContent formData = new MultipartFormDataContent();
                        //img_sc.Headers.Add("Content-Type", "image/jpeg");

                        if (!string.IsNullOrEmpty(note.Text))
                            formData.Add(new StringContent(note.Text), "WorkLogNote");
                        formData.Add(new StringContent(NOWHOME), "ct_s_num");
                        formData.Add(new StringContent(NOWREH), "reh_s_num");
                        //Console.WriteLine("picupload" + MapView.NOWHOME);
                        //WorkLogPicture
                        if (istakephoto == true && ispickphoto == false)
                        {
                            formData.Add(img_sc, "WorkLogPicture", "WorkLogPicture");
                            Console.WriteLine("Ain~~~ ");
                        }
                        else if (istakephoto == false && ispickphoto == true)
                        {
                            formData.Add(img_from_gallery, "WorkLogPicture", "WorkLogPicture");
                            Console.WriteLine("Bin~~~ ");
                        }
                        //else
                        //{
                        //    if (ispickphoto == true && istakephoto == true)
                        //    {
                        //        formData.Add(img_sc, "WorkLogPicture", "WorkLogPicture");
                        //        formData.Add(img_from_gallery, "WorkLogPicture", "WorkLogPicture");
                                
                        //    }
                        //}

                        var request = new HttpRequestMessage()
                        {
                            //RequestUri = new Uri("http://59.120.147.32:8080/lt_care/api/account/save_worklog"),
                            RequestUri = new Uri("https://s1.fcts.org.tw/api/account/save_worklog"),
                            Method = HttpMethod.Post,
                            Content = formData
                        };
                        request.Headers.Add("Connection", "closed");

                        var response = await client.SendAsync(request);
                        Console.WriteLine("WHY ~  " + response.ToString());
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            if (content == "ok")
                            {
                                Console.WriteLine("xxxxxxxxxxxxx : " + content);
                                await Navigation.PopAsync();
                                //Content = uploadlayout;
                                await DisplayAlert("上傳結果", "上傳成功！", "ok");
                                istakephoto = false;
                                ispickphoto = false;

                            }
                            else
                            {
                                Console.WriteLine("================================ : " + content);
                            }
                        }
                        else
                        {
                            Console.WriteLine("WHY2~ ");
                            Console.WriteLine("WHY ~2" + response.ToString());
                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    await DisplayAlert("ErrorMA~~~", ex.Message.ToString(), "ok");
                    Console.WriteLine("uploaderror~~~ " + ex.Message.ToString());
                }
            }
        }
        private async void post_from_gallery_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(note.Text))
                await DisplayAlert("提示", "您尚有東西未填寫", "ok");
            else
            {
                try
                {
                    Content = ViewService.Loading();
                    //bool post = web.Post_work(MainPage.token, note.Text, img_sc);
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("AUTHORIZATION", "Token " + MainPage.token);
                    MultipartFormDataContent formData = new MultipartFormDataContent();
                    //img_sc.Headers.Add("Content-Type", "image/jpeg");

                    if (!string.IsNullOrEmpty(note.Text))
                        formData.Add(new StringContent(note.Text), "WorkLogNote");
                    formData.Add(new StringContent(NOWHOME), "ct_s_num");
                    formData.Add(new StringContent(NOWREH), "reh_s_num");
                    //WorkLogPicture
                    formData.Add(img_from_gallery, "WorkLogPicture", "WorkLogPicture");
                    var request = new HttpRequestMessage()
                    {
                        //RequestUri = new Uri("http://59.120.147.32:8080/lt_care/api/account/save_worklog"),
                        RequestUri = new Uri("https://s1.fcts.org.tw/api/account/save_worklog"),
                        Method = HttpMethod.Post,
                        Content = formData
                    };
                    request.Headers.Add("Connection", "closed");

                    var response = await client.SendAsync(request);
                    Console.WriteLine("WHY ~  " + response.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (content == "ok")
                        {
                            Console.WriteLine("xxxxxxxxxxxxx : " + content);
                            await Navigation.PopAsync();
                            //Content = uploadlayout;
                            await DisplayAlert("上傳結果", "上傳成功！", "ok");

                        }
                        else
                        {
                            Console.WriteLine("================================ : " + content);
                        }
                    }
                    else
                    {
                        Console.WriteLine("WHY2~ ");
                        Console.WriteLine("WHY ~2" + response.ToString());
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("ErrorMA~~~", ex.Message.ToString(), "ok");
                    Console.WriteLine("uploaderror");
                }
            }
        }
    }
}
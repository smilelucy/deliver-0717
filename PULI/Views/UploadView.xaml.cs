using Deliver.Services;
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

        public UploadView()
        {
            InitializeComponent();
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
            ispickphoto = true;
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



        private async void post_Clicked(object sender, EventArgs e)
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
                    //WorkLogPicture
                    if(istakephoto == true && ispickphoto == false)
                    {
                        formData.Add(img_sc, "WorkLogPicture", "WorkLogPicture");
                    }
                    else if(istakephoto == false && ispickphoto == true)
                    {
                        formData.Add(img_from_gallery, "WorkLogPicture", "WorkLogPicture");
                    }
                    else
                    {
                        if(ispickphoto == true && istakephoto == true)
                        {
                            formData.Add(img_sc, "WorkLogPicture", "WorkLogPicture");
                            formData.Add(img_from_gallery, "WorkLogPicture", "WorkLogPicture");
                        }
                    }
                    
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri("http://59.120.147.32:8080/lt_care/api/account/save_worklog"),
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
                    //WorkLogPicture
                    formData.Add(img_from_gallery, "WorkLogPicture", "WorkLogPicture");
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri("http://59.120.147.32:8080/lt_care/api/account/save_worklog"),
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
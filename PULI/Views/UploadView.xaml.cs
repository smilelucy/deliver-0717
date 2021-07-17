using Deliver.Services;
using Plugin.Media;
using PULI.Model;
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
    public partial class UploadView : ContentPage
    {
        WebService web = new WebService();

        public UploadView()
        {
            InitializeComponent();
        }

        StreamContent img_sc;
        //Bitmap bmpPic;
        private async void btnCam_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                {
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    Directory = "Xamarin",
                    CompressionQuality = 40,
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
                    formData.Add(img_sc, "WorkLogPicture", "WorkLogPicture");
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
using Deliver.Services;
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
    public partial class ActivityView : ContentPage
    {
        private TotalList totalList = new TotalList();
        private stopname stopList = new stopname();
        private List<string> dys08_list = new List<string>();
        private List<string> ct_name_list = new List<string>();
        private List<string> dys02_list = new List<string>();
        private List<string> dys03_list = new List<string>();
        private List<string> dys04_list = new List<string>();
        private List<string> dys05_type_list = new List<string>();
        private List<string> ct06_telephone_list = new List<string>();
        private List<string> dys13_list = new List<string>();
        private List<string> ct_address_list = new List<string>();
        private List<string> ct_mp06_list = new List<string>();
        private List<string> ct_mp04_list = new List<string>(); // 代餐是否送達
        private restorename restoreList = new restorename();
        WebService web = new WebService();
        

        public ActivityView()
        {
            InitializeComponent();
            //_totalList = totalList;
            Messager();
            //setView();
            //_totalList = totalList;
            //total_table.ItemsSource = MapView.totalList.daily_shipments;
        }
        public async void setView() // 總表
        {
            Console.WriteLine("SETVIEW");
            //Console.WriteLine("timeactivity~~~ " + MainPage._time);
            if (MainPage._time == "早上")
            {
                totalList = await web.Get_Daily_Shipment(MainPage.token);
            }
            else
            {
                totalList = await web.Get_Daily_Shipment_night(MainPage.token);
            }
            stopList = await web.Get_Stop(MainPage.token);
            restoreList = await web.Get_Restore(MainPage.token);
            //Console.WriteLine("04~~~~" + totalList.daily_shipments[0].dys04);
            //Console.WriteLine("05~~~~" + totalList.daily_shipments[0].dys05);
            //Console.WriteLine("QQQQ~~" + totalList.daily_shipments.Count());
            try
            {
                if (totalList.daily_shipments.Count() != 0)
                {
                    //listview.ItemTemplate = new DataTemplate(typeof(ShipCell)); // 把模式設為ActivityCell
                    //listview.SelectedItem = null; // 
                    //listview.ItemsSource = totalList.daily_shipments; // ItemTemplate的資料來源
                   // Console.WriteLine("count~~ " + totalList.daily_shipments.Count());
                    for (int i = 0; i < totalList.daily_shipments.Count; i++)
                    {

                        dys08_list.Add(totalList.daily_shipments[i].dys08);
                        ct_name_list.Add(totalList.daily_shipments[i].ct_name);
                        dys02_list.Add(totalList.daily_shipments[i].dys02);
                        dys03_list.Add(totalList.daily_shipments[i].dys03);
                        dys04_list.Add(totalList.daily_shipments[i].dys04);
                        dys05_type_list.Add(totalList.daily_shipments[i].dys05_type);
                        dys13_list.Add(totalList.daily_shipments[i].dys13);
                        ct06_telephone_list.Add(totalList.daily_shipments[i].ct06_telephone);
                        ct_address_list.Add(totalList.daily_shipments[i].ct_address);
                        ct_mp06_list.Add(totalList.daily_shipments[i].ct_mp06);
                        if(totalList.daily_shipments[i].ct_mp04 == "Y")
                        {
                            ct_mp04_list.Add("已送");
                        }
                        else
                        {
                            ct_mp04_list.Add("未送");
                        }
                    }
                    //Console.WriteLine("restore~~~ " + restoreList.restore);
                   // Console.WriteLine("restore0~~~ " + restoreList.restore[0]);
                    //Console.WriteLine("total~~~ " + totalList.daily_shipments.Count());
                    //Console.WriteLine("ct_name~~ " + ct_name_list[0]);
                    //for(int j = 0; j < stopList.stop.Count(); j++)
                    //{
                    //    stop_list.Add(stopList.stop);
                    //}

                    //num_list.Sort();

                    //for (int i = 0; i < totalList.daily_shipments.Count; i++)
                    //{
                    //    if(totalList.daily_shipments[i].s_num == num_list[0])
                    //    {
                    //        cansee = true;
                    //    }
                    //    else
                    //    {
                    //        cansee = false;
                    //    }
                    //}

                    Grid grid = new Grid
                    {

                        RowSpacing = 0,
                        ColumnSpacing = 0,
                        BackgroundColor = Color.FromHex("CFD4DD"),
                        WidthRequest = 1700,
                        
                        //HeightRequest = 2000,
                        //Margin = new Thickness(-20),
                        ColumnDefinitions =
                        {
                            
                            new ColumnDefinition{ 
                                Width = new GridLength(2.5, GridUnitType.Star)
                            },
                            new ColumnDefinition{
                                Width = new GridLength(2.5, GridUnitType.Star)
                            },
                            new ColumnDefinition{
                                Width = new GridLength(2.5, GridUnitType.Star)
                            },
                            new ColumnDefinition{
                                Width = new GridLength(2.5, GridUnitType.Star)
                            },
                            new ColumnDefinition{
                                Width = new GridLength(2.5, GridUnitType.Star)
                            },
                            new ColumnDefinition{
                                Width = new GridLength(2.5, GridUnitType.Star)
                            },
                            new ColumnDefinition{
                                Width = new GridLength(2.5, GridUnitType.Star)
                            },
                            new ColumnDefinition{
                                Width = new GridLength(2.5, GridUnitType.Star)
                            },
                            new ColumnDefinition{
                                Width = new GridLength(2.5, GridUnitType.Star)
                            },
                            new ColumnDefinition{
                                Width = new GridLength(11, GridUnitType.Star)
                            },
                        },
                        
                    };
                    Grid grid_other = new Grid
                    {
                        RowSpacing = 0,
                        ColumnSpacing = 0,
                        BackgroundColor = Color.FromHex("CFD4DD"),
                        WidthRequest = 700,
                        HeightRequest = 150,
                        ColumnDefinitions =
                        {

                            new ColumnDefinition {
                                 Width = new GridLength(2.5, GridUnitType.Star)
                            },
                            new ColumnDefinition {
                                Width = new GridLength(10, GridUnitType.Star)
                            },

                        },
                        //RowDefinitions =
                        //{
                        //    new RowDefinition {
                        //        Height = GridLength.Auto
                        //    },
                        //    new RowDefinition {
                        //        Height = GridLength.Auto
                        //    },
                        //}
                    };
                    //var total = new Button
                    //{
                    //    Text = "總表",
                    //    CornerRadius = 60,
                    //    BackgroundColor = Color.FromHex("f1ab86"),
                    //    TextColor = Color.White,
                    //    FontSize = 20

                    //};
                    //var change = new Button
                    //{
                    //    Text = "異動表",
                    //    CornerRadius = 60,
                    //    BackgroundColor = Color.White,
                    //    TextColor = Color.FromHex("f1ab86"),
                    //    FontSize = 20

                    //};
                    //var button_stack = new StackLayout
                    //{
                    //    Orientation = StackOrientation.Horizontal,
                    //    HorizontalOptions = LayoutOptions.FillAndExpand,
                    //    Children = { total, change }
                    //};
                    //grid.Children.Add(new StackLayout
                    //{
                    //    Children = {button_stack}
                    //});
                    grid.Children.Add(new Label
                    {
                        Text = "順序",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 120,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 0,0);
                    grid.Children.Add(new Label
                    {
                        Text = "姓名",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 120,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 1,0);
                    grid.Children.Add(new Label
                    {
                        Text = "餐別",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 120,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 2, 0);
                    grid.Children.Add(new Label
                    {
                        Text = "餐種",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 120,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 3, 0);
                    grid.Children.Add(new Label
                    {
                        Text = "特殊內容",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 120,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 4, 0);
                    grid.Children.Add(new Label
                    {
                        Text = "代餐種類",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 120,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 5, 0);
                    grid.Children.Add(new Label
                    {
                        Text = "代餐是否送達",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 130,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 2, 2)

                    }, 6, 0);
                    grid.Children.Add(new Label
                    {
                        Text = "是否自費",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 120,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 7, 0);
                    grid.Children.Add(new Label
                    {
                        Text = "案主電話",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 120,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 8, 0);
                   
                    grid.Children.Add(new Label
                    {
                        Text = "聯絡地址",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 900,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(2, 2, 0, 2)

                    }, 9, 0);

                    for (int i = 0; i < totalList.daily_shipments.Count(); i++)
                        {

                        int j = 0;
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        grid.Children.Add(new Label
                        {
                            Text = dys08_list[i],
                            TextColor = Color.Black,
                            FontSize = 18,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            BackgroundColor = Color.White,
                            WidthRequest = 120,
                            Padding = new Thickness(5,16,5,16),
                            Margin = new Thickness(4,2,0,2)

                        }, j, i+1) ;
                        
                        j = j + 1;
                        //Console.WriteLine("ADD~~Q1");
                            grid.Children.Add(new Label
                            {
                                Text = ct_name_list[i],
                                FontSize = 18,
                                TextColor = Color.Black,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Padding = new Thickness(5, 16, 5, 16),
                                WidthRequest = 120,
                                BackgroundColor = Color.White,
                                Margin = new Thickness(2, 2, 0, 2)
                            }, j, i+1);
                        j = j + 1;
                        //Console.WriteLine("ADD~~Q2");
                        grid.Children.Add(new Label
                        {
                            Text = dys02_list[i],
                            FontSize = 18,
                            TextColor = Color.Black,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            Padding = new Thickness(5, 16, 5, 16),
                            WidthRequest = 120,
                            BackgroundColor = Color.White,
                            Margin = new Thickness(2, 2, 0, 2)
                        }, j, i+1);
                        j = j + 1;
                        grid.Children.Add(new Label
                        {
                            Text = dys03_list[i],
                            FontSize = 18,
                            TextColor = Color.Black,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            Padding = new Thickness(5, 16, 5, 16),
                            WidthRequest = 120,
                            BackgroundColor = Color.White,
                            Margin = new Thickness(2, 2, 0, 2)
                        }, j, i+1);
                        j = j + 1;
                        grid.Children.Add(new Label
                        {
                            Text = dys04_list[i],
                            FontSize = 18,
                            TextColor = Color.Black,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            Padding = new Thickness(5, 16, 5, 16),
                            WidthRequest = 120,
                            BackgroundColor = Color.White,
                            Margin = new Thickness(2, 2, 0, 2)
                        }, j, i+1);
                        j = j + 1;
                        if(ct_mp06_list[i] == "Y")
                        {
                            //Console.WriteLine("dys061~~ " + ct_mp06_list[i]);
                            grid.Children.Add(new Label
                            {
                                Text = dys05_type_list[i],
                                FontSize = 18,
                                TextColor = Color.Red,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Padding = new Thickness(5, 16, 5, 16),
                                WidthRequest = 120,
                                BackgroundColor = Color.White,
                                Margin = new Thickness(2, 2, 0, 2)
                            }, j, i + 1);
                        }
                        else
                        {
                            //Console.WriteLine("dys062~~ " + ct_mp06_list[i]);
                            grid.Children.Add(new Label
                            {
                                Text = dys05_type_list[i],
                                FontSize = 18,
                                TextColor = Color.Black,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Padding = new Thickness(5, 16, 5, 16),
                                WidthRequest = 120,
                                BackgroundColor = Color.White,
                                Margin = new Thickness(2, 2, 0, 2)
                            }, j, i + 1);
                        }
                        j = j + 1;
                        if(ct_mp04_list[i] == "已送")
                        {
                            //Console.WriteLine("ct01~~~ " + ct_mp04_list[i]);
                            grid.Children.Add(new Label
                            {
                                Text = ct_mp04_list[i],
                                FontSize = 18,
                                TextColor = Color.Black,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                WidthRequest = 130,
                                Padding = new Thickness(5, 16, 5, 16),
                                BackgroundColor = Color.White,
                                Margin = new Thickness(2, 2, 3, 2)
                            }, j, i + 1);
                        }
                        else
                        {
                           // Console.WriteLine("ct02~~~ " + ct_mp04_list[i]);
                            grid.Children.Add(new Label
                            {
                                Text = ct_mp04_list[i],
                                FontSize = 18,
                                TextColor = Color.Black,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                WidthRequest = 130,
                                Padding = new Thickness(5, 16, 5, 16),
                                BackgroundColor = Color.FromHex("ffd7ba"),
                                Margin = new Thickness(2, 2, 3, 2)
                            }, j, i + 1);
                        }
                        j = j + 1;
                        grid.Children.Add(new Label
                        {
                            Text = dys13_list[i],
                            FontSize = 18,
                            TextColor = Color.Black,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            Padding = new Thickness(5, 16, 5, 16),
                            WidthRequest = 120,
                            BackgroundColor = Color.White,
                            Margin = new Thickness(2, 2, 0, 2)
                        }, j, i+1);
                        j = j + 1;
                        grid.Children.Add(new Label
                        {
                            Text = ct06_telephone_list[i],
                            FontSize = 18,
                            TextColor = Color.Black,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            WidthRequest = 120,
                            Padding = new Thickness(5, 16, 5, 16),
                            BackgroundColor = Color.White,
                            Margin = new Thickness(2, 2, 2, 2)
                        }, j, i+1);
                        j = j + 1;
                        
                        grid.Children.Add(new Label
                        {
                            Text = ct_address_list[i],
                            FontSize = 18,
                            WidthRequest = 900,
                            //HeightRequest = 80,
                            TextColor = Color.Black,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            Padding = new Thickness(5, 16, 5, 16),
                            BackgroundColor = Color.White,
                            Margin = new Thickness(2, 2, 0, 2)
                        }, j, i+1);
                        //Console.WriteLine("ctadress~~~ " + ct_address_list[i]);  
                    }

                    grid_other.Children.Add(new Label
                    {
                        Text = "停餐名單",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        BackgroundColor = Color.FromHex("8296B0"), //b7b7a4
                        
                        WidthRequest = 120,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 0, 0);
                    grid_other.Children.Add(new Label
                    {
                        Text = "復餐名單",
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        BackgroundColor = Color.FromHex("8296B0"),
                        WidthRequest = 120,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 0, 1);

                    grid_other.Children.Add(new Label
                    {
                        Text = stopList.stop,
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        BackgroundColor = Color.White,
                        WidthRequest = 700,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 1,0);
                    grid_other.Children.Add(new Label
                    {
                        Text = restoreList.restore,
                        TextColor = Color.Black,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        BackgroundColor = Color.White,
                        WidthRequest = 700,
                        Padding = new Thickness(5, 16, 5, 16),
                        Margin = new Thickness(0, 2, 0, 2)

                    }, 1, 1);
                    // Row 0
                    //grid.Children.Add(new BoxView
                    //{
                    //    Color = Color.AliceBlue
                    //});
                    //grid.Children.Add(new Label
                    //{
                    //    Text = "Upper left",
                    //    HorizontalOptions = LayoutOptions.Start,
                    //    VerticalOptions = LayoutOptions.Start
                    //});

                    //grid.Children.Add(new BoxView
                    //{
                    //    Color = Color.LightSkyBlue
                    //}, 1, 0);
                    //grid.Children.Add(new Label
                    //{
                    //    Text = "Upper center",
                    //    HorizontalOptions = LayoutOptions.Center,
                    //    VerticalOptions = LayoutOptions.Start
                    //}, 1, 0);

                    //grid.Children.Add(new BoxView
                    //{
                    //    Color = Color.CadetBlue
                    //}, 2, 0);
                    //grid.Children.Add(new Label
                    //{
                    //    Text = "Upper right",
                    //    HorizontalOptions = LayoutOptions.End,
                    //    VerticalOptions = LayoutOptions.Start
                    //}, 2, 0);
                    //grid.Children.Add(new BoxView
                    //{
                    //    Color = Color.Red
                    //}, 3, 0);

                    //// Row 1
                    //grid.Children.Add(new BoxView
                    //{
                    //    Color = Color.CornflowerBlue
                    //}, 0, 1);
                    //grid.Children.Add(new Label
                    //{
                    //    Text = "Center left",
                    //    HorizontalOptions = LayoutOptions.Start,
                    //    VerticalOptions = LayoutOptions.Center
                    //}, 0, 1);

                    //grid.Children.Add(new BoxView
                    //{
                    //    Color = Color.DodgerBlue
                    //}, 1, 1);
                    //grid.Children.Add(new Label
                    //{
                    //    Text = "Center center",
                    //    HorizontalOptions = LayoutOptions.Center,
                    //    VerticalOptions = LayoutOptions.Center
                    //}, 1, 1);

                    //grid.Children.Add(new BoxView
                    //{
                    //    Color = Color.DarkSlateBlue
                    //}, 2, 1);
                    //grid.Children.Add(new Label
                    //{
                    //    Text = "Center right",
                    //    HorizontalOptions = LayoutOptions.End,
                    //    VerticalOptions = LayoutOptions.Center
                    //}, 2, 1);

                    //// Row 2
                    //grid.Children.Add(new BoxView
                    //{
                    //    Color = Color.SteelBlue
                    //}, 0, 2);
                    //grid.Children.Add(new Label
                    //{
                    //    Text = "Lower left",
                    //    HorizontalOptions = LayoutOptions.Start,
                    //    VerticalOptions = LayoutOptions.End
                    //}, 0, 2);

                    //grid.Children.Add(new BoxView
                    //{
                    //    Color = Color.LightBlue
                    //}, 1, 2);
                    //grid.Children.Add(new Label
                    //{
                    //    Text = "Lower center",
                    //    HorizontalOptions = LayoutOptions.Center,
                    //    VerticalOptions = LayoutOptions.End
                    //}, 1, 2);

                    //grid.Children.Add(new BoxView
                    //{
                    //    Color = Color.BlueViolet
                    //}, 2, 2);
                    //grid.Children.Add(new Label
                    //{
                    //    Text = "Lower right",
                    //    HorizontalOptions = LayoutOptions.End,
                    //    VerticalOptions = LayoutOptions.End
                    //}, 2, 2);

                    //ScrollView scrollView = new ScrollView {
                    //    Content = grid ,
                    //    Orientation = ScrollOrientation.Both,
                    //    VerticalOptions = LayoutOptions.FillAndExpand,
                    //    HorizontalOptions = LayoutOptions.FillAndExpand
                    //};
                    //Content = scrollView;

                    
                    TotalStack.Children.Add(grid);
                    TotalStack_other.Children.Add(grid_other);


                    resComStack.IsVisible = true;
                    resComStack.IsEnabled = true;
                    resSucStack.IsEnabled = false;
                    resSucStack.IsVisible = false;
                    buttonres.IsVisible = true;
                    //buttoncheck.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("系統訊息", "後臺尚未產生資料或資料接收不齊全", "ok");
                    Console.WriteLine("no shipment~~");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("whyerror~~~" + ex.ToString());
            }
           
            
            //totalList = await web.Get_Daily_Shipment(MemberView.token);

        }

        //public StackLayout StopView(stopname stopList)
        //{
        //    var stop_Label = new Label
        //    {
        //        Text = "停餐名單 :",
        //        VerticalTextAlignment = TextAlignment.Start,
        //        TextColor = Color.Black,
        //        FontSize = 20
        //    };

        //    for (int i = 0; i < stopList.stop.Count(); i++)
        //    {
                
        //        var name_content = new Label // 問題題號+題目
        //        {
        //            Text = stopList.stop[i].ToString(),
        //            FontSize = 20,
        //            TextColor = Color.Black
        //        };
        //    }

        //    var finalLayout = new StackLayout
        //    {
        //        Orientation = StackOrientation.Horizontal,
        //        Padding = new Thickness(5, 5, 10, 5),
        //        Children = { stop_Label, name_content }
        //    };
        //    quesStack.Children.Add(button_stack);
        //    return null;
        //}
        //public async void setView2() // 異動表
        //{
        //    Console.WriteLine("SETVIEW2");
        //    Console.WriteLine("timeactivity~~~ " + MainPage._time);
        //    if (MainPage._time == "早上")
        //    {
        //        totalList = await web.Get_Daily_Shipment(MainPage.token);
        //    }
        //    else
        //    {
        //        totalList = await web.Get_Daily_Shipment_night(MainPage.token);
        //    }
        //    //Console.WriteLine("tttt" + totalList.abnormals[0].different);
        //    //Console.WriteLine("iiii" + totalList.abnormals[0].ClientName);
        //    listview2.ItemTemplate = new DataTemplate(typeof(AbnormalCell)); // 把模式設為activitycell
        //    listview2.SelectedItem = null; // 
        //    listview2.ItemsSource = totalList.abnormals; // itemtemplate的資料來源
        //    //resSucStack.IsEnabled = true;
        //    //resSucStack.IsVisible = true;
        //    //resSucStack.IsVisible = true;
        //    //resSucStack.IsEnabled = true;
        //}

        private void Messager()
        {
            try
            {
                MessagingCenter.Subscribe<HomeView, bool>(this, "SET_SHIPMENT_FORM", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        //totalList = new TotalList();
                        totalList = null;
                        setView();
                    }
                });
                MessagingCenter.Subscribe<HomeView2, bool>(this, "SET_SHIPMENT_FORM", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        //totalList = new TotalList();
                        totalList = null;
                        setView();
                        Console.WriteLine("setshipform1~~~~");
                    }
                });
                MessagingCenter.Subscribe<HomeViewHelperAndDiliver, bool>(this, "SET_SHIPMENT_FORM", (sender, arg) =>
                {
                    // do something when the msg "UPDATE_BONUS" is recieved
                    if (arg)
                    {
                        //totalList = new TotalList();
                        totalList = null;
                        setView();
                        Console.WriteLine("setshipform2~~~~");
                    }
                });
                //MessagingCenter.Subscribe<HomeView, bool>(this, "SET_CHANGE_FORM", (sender, arg) =>
                //{
                //    // do something when the msg "UPDATE_BONUS" is recieved
                //    if (arg)
                //    {
                //        //totalList = new TotalList();
                //        totalList = null;
                //        setView2();

                //    }
                //});
                //MessagingCenter.Subscribe<HomeView2, bool>(this, "SET_CHANGE_FORM", (sender, arg) =>
                //{
                //    // do something when the msg "UPDATE_BONUS" is recieved
                //    if (arg)
                //    {
                //        //totalList = new TotalList();
                //        totalList = null;
                //        setView2();
                //    }
                //});
                // HomeViewHelperAndDiliver
                //MessagingCenter.Subscribe<HomeViewHelperAndDiliver, bool>(this, "SET_CHANGE_FORM", (sender, arg) =>
                //{
                //    // do something when the msg "UPDATE_BONUS" is recieved
                //    if (arg)
                //    {
                //        //totalList = new TotalList();
                //        totalList = null;
                //        setView2();
                //    }
                //});
                MessagingCenter.Subscribe<MemberView, bool>(this, "OUT", (sender, arg) =>
                {
                    if (arg)
                    {
                        Navigation.PushAsync(new ActivityView());
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void buttonres_Clicked(object sender, EventArgs e)
        {
            //res_infoStack.IsVisible = true;
            resComStack.IsVisible = true;
            resComStack.IsEnabled = true;
            resSucStack.IsVisible = false; // 顯示已預約資訊
            resSucStack.IsEnabled = false;
            resSucStack2.IsVisible = false;
            resSucStack2.IsEnabled = false;
        }

        // 典籍異動表
        private async void buttoncheck_Clicked(object sender, EventArgs e)
        {
            //res_infoStack.IsVisible = false;
            if(totalList.abnormals == null)
            {
                resComStack.IsVisible = false;
                resComStack.IsEnabled = false;
                resSucStack2.IsVisible = true; // 站無異動資訊
                resSucStack2.IsEnabled = true;
            }
            else
            {
                if (totalList.abnormals.Count() > 0)
                {
                    resComStack.IsVisible = false;
                    resComStack.IsEnabled = false;
                    resSucStack.IsVisible = true; // 顯示已預約資訊
                    resSucStack.IsEnabled = true;
                }
                else
                {
                    resComStack.IsVisible = false;
                    resComStack.IsEnabled = false;
                    resSucStack2.IsVisible = true; // 站無異動資訊
                    resSucStack2.IsEnabled = true;
                }
            }
            
        }

        //private void Button_OnPressed(object sender, EventArgs e)
        //{
        //    if (sender is Button btn)
        //    {
        //        btn.BackgroundColor = Color.White;
        //        btn.TextColor = Color.FromHex("5ABFC9");
        //        buttoncheck.BackgroundColor = Color.FromHex("5ABFC9");
        //        buttoncheck.TextColor = Color.White;
        //    }

        //}
        private void Button2_OnPressed(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackgroundColor =Color.White ;
                btn.TextColor = Color.FromHex("5ABFC9");
                buttonres.BackgroundColor = Color.FromHex("5ABFC9");
                buttonres.TextColor = Color.White;
            }
        }


        protected override void OnAppearing()
        {
            //setView();
            base.OnAppearing();
            //totalList = new TotalList();
            
            
            //allclientList = new List<AllClientInfo>();
        }

        //lock the previous page
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //res_infoStack.IsVisible = true;
            resComStack.IsVisible = true;
            resComStack.IsEnabled = true;
            resSucStack.IsVisible = false; // 顯示已預約資訊
            resSucStack.IsEnabled = false;
            resSucStack2.IsVisible = false;
            resSucStack2.IsEnabled = false;
            buttonres.BackgroundColor = Color.White;
            buttonres.TextColor = Color.FromHex("#5ABFC9");
            //buttoncheck.BackgroundColor = Color.FromHex("#5ABFC9");
            //buttoncheck.TextColor = Color.White;
        }
    }
}
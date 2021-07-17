using PULI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataCell
{
    public class ShipCell : ViewCell
    {
        public ShipCell()
        {
            
            var s_num_Label = new Label
            {
                Text = "序號 :",
              
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                //IsVisible = Binding{ ActivityView.cansee},
                WidthRequest = 200,
                TextColor = Color.Black,
                FontSize = 20
            };
            //s_num_Label.IsVisible = "{Binding }";



            var name_Label = new Label
            {
                Text = "姓名 :",
                WidthRequest = 200,
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };

            var dys02_Label = new Label
            {
                Text = "餐別 :",
                WidthRequest = 200,
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };

            var dys03_Label = new Label
            {
                Text = "餐種 :",
                WidthRequest = 200,
                VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };

            var dys04_Label = new Label
            {
                Text = "特殊內容 :",
                WidthRequest = 200,
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };

            var dys05_type_Label = new Label
            {
                Text = "代餐種類 :",
                WidthRequest = 200,
                //VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };

            var dys13_Label = new Label
            {
                Text = "自費 :",
                WidthRequest = 200,
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };

            var ct06_telephone_Label = new Label
            {
                Text = "電話 :",
                WidthRequest = 200,
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };

            var ct_address_Label = new Label
            {
                Text = "地址 :",
                WidthRequest = 200,
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };

            //var s_num_Layout = new StackLayout
            //{
            //    Orientation = StackOrientation.Horizontal,
            //    Margin = new Thickness(10, 5, 10, 5),
            //    Children = { s_num_Label, nameLabel, }
            //};

            var Label_layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(20, 5, 2e0, 5),
                Children = { s_num_Label, name_Label, dys02_Label, dys03_Label, dys04_Label, dys05_type_Label, dys13_Label, ct06_telephone_Label, ct_address_Label }
            };

            var s_num_Content = new Label
            {
                //VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            s_num_Content.SetBinding(Label.TextProperty, "s_num"); // 配送單序號

            

            

            var nameContent = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            nameContent.SetBinding(Label.TextProperty, "ct_name"); // 姓名

            //var name_layout = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Margin = new Thickness(0, 5, 10, 5),
            //    Children = { nameLabel, nameContent }
            //};

            

            var dys02_Content = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            dys02_Content.SetBinding(Label.TextProperty, "dys02"); // 餐別

            //var dys02_layout = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Margin = new Thickness(0, 5, 10, 5),
            //    Children = { dys02_Label, dys02_Content }
            //};

           

            var dys03_Content = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            dys03_Content.SetBinding(Label.TextProperty, "dys03"); // 餐種(一班餐不用顯示)

            //var dys03_layout = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Margin = new Thickness(0, 5, 10, 5),
            //    Children = { dys03_Label, dys03_Content }
            //};

            

            var dys04_Content = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            dys04_Content.SetBinding(Label.TextProperty, "dys04"); // 特殊內容

            //var dys04_layout = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Margin = new Thickness(0, 5, 10, 5),
            //    Children = { dys04_Label, dys04_Content }
            //};


           

            var dys05_type_Content = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            dys05_type_Content.SetBinding(Label.TextProperty, "dys05_type"); // 代餐種類

            //var dys05_layout = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
                
            //    Margin = new Thickness(0, 5, 10, 5),
            //    Children = { dys05_type_Label, dys05_type_Content }
            //};

            

            var dys13_Content = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            dys13_Content.SetBinding(Label.TextProperty, "dys13"); // 自費

            //var dys13_layout = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Margin = new Thickness(0, 5, 10, 5),
            //    Children = { dys13_Label, dys13_Content }
            //};

            

            var ct06_telephone_Content = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            ct06_telephone_Content.SetBinding(Label.TextProperty, "ct06_telephone"); // 代餐種類

            //var ct06_telephone_layout = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Margin = new Thickness(0, 5, 10, 5),
            //    Children = { ct06_telephone_Label, ct06_telephone_Content }
            //};

           

            var ct_address_Content = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            ct_address_Content.SetBinding(Label.TextProperty, "ct_address"); // 地址

            //var ct_address_layout = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Margin = new Thickness(0, 5, 10, 5),
            //    Children = { ct_address_Label, ct_address_Content }
            //};

            //var orderContent = new Label
            //{
            //    VerticalTextAlignment = TextAlignment.Start,
            //    TextColor = Color.Black,
            //    FontSize = 20
            //};
            //orderContent.SetBinding(Label.TextProperty, "different");// 餐點名稱



            var contentLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5, 10, 5),
                Children = { s_num_Content, nameContent, dys02_Content, dys03_Content, dys04_Content, dys05_type_Content, dys13_Content, ct06_telephone_Content, ct_address_Content }
            };

            var finalLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(5, 5, 10, 5),
                Children = {Label_layout, contentLayout}
            };

            View = finalLayout;
        }
    }
}
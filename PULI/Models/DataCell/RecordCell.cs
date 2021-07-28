using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataCell
{
    public class RecordCell : ViewCell
    {
        public RecordCell()
        {
            var nameContent = new Label
            {
                VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };

            nameContent.SetBinding(Label.TextProperty, "name");


            var nameLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 0, 5, 0),
                Children = { nameContent }
            };

           // var titleContent = new Label
           // {
           //     VerticalTextAlignment = TextAlignment.Start,
           //     TextColor = Color.Black,
           //     FontSize = 20
           // };
           //// titleContent.SetBinding(Label.TextProperty, );

           // var titleLayout = new StackLayout
           // {
           //     Orientation = StackOrientation.Horizontal,
           //     Padding = new Thickness(5, 0, 5, 0),
           //     Children = { titleContent }
           // };

            //var boxView = new BoxView
            //{
            //    Color = Color.Black,
            //    HeightRequest = 1,
            //    HorizontalOptions = LayoutOptions.Fill,
            //    WidthRequest = Application.Current.MainPage.Width - 130,
            //    Margin = new Thickness(10, 0, 10, 0)
            //};


            var timeContent = new Label
            {
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                FontSize = 15
            };
            timeContent.SetBinding(Label.TextProperty, "time");

            var timeLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 0, 5, 0),
                Children = { timeContent }
            };


            var textlayout = new StackLayout
            {
                Padding = new Thickness(0, 10, 0, 10),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = { nameLayout, timeLayout }
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(10, 10, 0, 10),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                //BackgroundColor = Color.FromHex("fae1dd"),
                Children = { textlayout }
            };

            View = layout;
        }
    }
}
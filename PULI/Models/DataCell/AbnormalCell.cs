using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataCell
{
    public class AbnormalCell : ViewCell
    {
        public AbnormalCell()
        {
            


            var nameContent = new Label
            {
                VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            nameContent.SetBinding(Label.TextProperty, "ClientName");

            //var orderContent = new Label
            //{
            //    VerticalTextAlignment = TextAlignment.Start,
            //    TextColor = Color.Black,
            //    FontSize = 20
            //};
            //orderContent.SetBinding(Label.TextProperty, "dys03");// 餐點名稱

            var AContent = new Label
            {
                VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            AContent.SetBinding(Label.TextProperty, "different");// 餐點指示

            //var nameLayout = new StackLayout
            //{
            //    Orientation = StackOrientation.Horizontal,
            //    Padding = new Thickness(5, 0, 10, 0),
            //    Children = { orderContent, AContent }
            //};

            var finalLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(5, 5, 10, 5),
                Children = { nameContent, AContent }
            };

            View = finalLayout;
        }
    }
}
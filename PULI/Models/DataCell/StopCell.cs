using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataCell
{
    public class StopCell : ViewCell
    {
        public StopCell ()
        {
            var stop_Label = new Label
            {
                Text = "停餐名單 :",
                VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };

            var stop_Content = new Label
            {
                VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black,
                FontSize = 20
            };
            stop_Content.SetBinding(Label.TextProperty, "stop"); // 

            var finalLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5, 10, 5),
                Children = { stop_Label, stop_Content }
            };

            View = finalLayout;
        }
    }
}
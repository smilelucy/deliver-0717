using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models.DataCell
{
    public class QuestionCell : ViewCell
    {
        public QuestionCell()
        {
            var nameContent = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };
            nameContent.SetBinding(Label.TextProperty, "ClientName");

            var qb_orderContent = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };
            qb_orderContent.SetBinding(Label.TextProperty, "qb_order");

            var wqh_s_numContent = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };
            wqh_s_numContent.SetBinding(Label.TextProperty, "wqh_s_num");

            var qh_s_numContent = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };
            qh_s_numContent.SetBinding(Label.TextProperty, "qh_s_num");

            var qh01Content = new Label
            {

            };
            qh01Content.SetBinding(Label.TextProperty, "qh01");

            var InfoLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5, 10, 5),
                Children = { nameContent, qb_orderContent, wqh_s_numContent, qh_s_numContent, qh01Content }
            };

             
        }
    }
}
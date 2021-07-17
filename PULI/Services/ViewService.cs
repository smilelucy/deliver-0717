using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PULI.Model
{
    public class ViewService
    {
        public static Grid Loading()
        {
            Label searchingLabel;
            ActivityIndicator spinner;
            StackLayout searchingLayout;
            Image image;
            Label space;
            string version = "1.4";

            Grid grid;

            //image = new Image
            //{
            //    Source = "icon.png",
            //    HeightRequest = 150,
            //    WidthRequest = 150,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //};

            searchingLabel = new Label
            {
                Text = "上傳中...請稍候!",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 30,
                TextColor = Color.Black
            };

            spinner = new ActivityIndicator
            {
                IsRunning = true,
                Color = Color.Black,
                Scale = 1,
            };

            space = new Label
            {
                Text = "   ",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            Label version_text = new Label
            {
                Text = "ver_" + version,
                VerticalTextAlignment = TextAlignment.End,
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                TextColor = Color.Black,
                FontSize = 15
            };


            searchingLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { spinner, space, searchingLabel, version_text }
            };

            StackLayout versionLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End,
                Children = { version_text }
            };

            StackLayout stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { searchingLayout, versionLayout }
            };

            //Image backImg = new Image
            //{
            //    Source = "loading.png",
            //    Aspect = Aspect.AspectFill
            //};

            //StackLayout backStack = new StackLayout
            //{
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    Children = { backImg }
            //};

            //grid = new Grid
            //{
            //    Children = { backImg, stack }
            //};

            grid = new Grid
            {
                Children = { stack }
            };

            return grid;
        }
        public static Grid LoadingLogin()
        {
            Label searchingLabel;
            ActivityIndicator spinner;
            StackLayout searchingLayout;
            Image image;
            Label space;
            string version = "1.4";

            Grid grid;

            //image = new Image
            //{
            //    Source = "icon.png",
            //    HeightRequest = 150,
            //    WidthRequest = 150,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //};

            searchingLabel = new Label
            {
                Text = "載入中...請稍候!",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 30,
                TextColor = Color.Black
            };

            spinner = new ActivityIndicator
            {
                IsRunning = true,
                Color = Color.Black,
                Scale = 1,
            };

            space = new Label
            {
                Text = "   ",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            Label version_text = new Label
            {
                Text = "ver_" + version,
                VerticalTextAlignment = TextAlignment.End,
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                TextColor = Color.Black,
                FontSize = 15
            };


            searchingLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { spinner, space, searchingLabel, version_text }
            };

            StackLayout versionLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End,
                Children = { version_text }
            };

            StackLayout stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { searchingLayout, versionLayout }
            };

            //Image backImg = new Image
            //{
            //    Source = "loading.png",
            //    Aspect = Aspect.AspectFill
            //};

            //StackLayout backStack = new StackLayout
            //{
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    Children = { backImg }
            //};

            //grid = new Grid
            //{
            //    Children = { backImg, stack }
            //};

            grid = new Grid
            {
                Children = { stack }
            };

            return grid;
        }
    }
}

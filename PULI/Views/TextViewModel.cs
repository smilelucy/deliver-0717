using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Views
{
    public class TextViewModel : INotifyPropertyChanged
    {
        public static bool isEntry;
        public TextViewModel()
        {
           
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
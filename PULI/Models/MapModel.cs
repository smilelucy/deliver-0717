using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PULI.Models
{
    public class MapModel : INotifyPropertyChanged
    {
        bool isSelected;
        public bool IsSelected
        {
            set
            {
                isSelected = value;
                onPropertyChanged();
            }
            get => isSelected;
        }

        // ... Other properties

        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
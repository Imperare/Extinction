﻿using System.ComponentModel;

namespace Extinction.Helpers
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(params string[] propertyName)
        {
            foreach (var property in propertyName)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

    }
}

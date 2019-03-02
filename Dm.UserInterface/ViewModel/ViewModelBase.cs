using System;
using System.ComponentModel;

namespace Dm.UserInterface.ViewModel
{
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChange(string propertyName)
        {
            try
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (NullReferenceException) { }
        }
    }
}

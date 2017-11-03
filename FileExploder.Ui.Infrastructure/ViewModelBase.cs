using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileExploder.Ui.Infrastructure
{
    /// <summary>
    /// Base class for ViewModels, implementing <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The event raised when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
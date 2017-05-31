using System.Windows.Data;
using System;
using System.Globalization;
using System.Windows;

namespace FileExploder.Ui.Infrastructure.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolvalue = (value as Boolean?);

            if (boolvalue != null)
            {
                return boolvalue == true ? Visibility.Visible : Visibility.Collapsed;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
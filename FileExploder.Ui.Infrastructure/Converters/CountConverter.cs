using System.Collections.Generic;
using System.Linq;

using System;
using System.Globalization;
using System.Windows.Data;

namespace FileExploder.Ui.Infrastructure.Converters
{
    public class CountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = value as IEnumerable<Object>;

            if (list != null)
            {
                return ($"({list.Count()} files)");
            }

            return Binding.DoNothing;
               
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
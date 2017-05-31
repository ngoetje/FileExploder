using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FileExploder.Ui.Infrastructure.Converters
{
    public class LengthToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LengthConverterTargetEnum target;
            if (parameter != null) {
                if (Enum.TryParse<LengthConverterTargetEnum>(parameter.ToString(), out target))
                {
                    long length = long.MaxValue;
                    if (long.TryParse(value.ToString(), out length))
                    {
                        var formattedLength = (length / Math.Pow(1024, (int)target)).ToString("F2", CultureInfo.InvariantCulture);
                        return ($" {formattedLength} {target.ToString("F")}");
                    }
                }
            }


            return Binding.DoNothing;
                
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public enum LengthConverterTargetEnum
    {
        
        KB = 1,
        MB = 2,
        GB = 3
    }
}

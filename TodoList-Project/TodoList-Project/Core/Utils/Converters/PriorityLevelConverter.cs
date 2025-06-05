using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TodoList_Project.Core.Utils.Converters
{
    public class PriorityLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double rating)
            {
                return rating switch
                {
                    <= 1 => "Low",
                    <= 2 => "Low",
                    <= 3 => "Medium",
                    <= 4 => "High",
                    _ => "High",
                };
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Không cần dùng đến
        }
    }

}

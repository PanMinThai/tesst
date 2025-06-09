using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TodoList_Project.Core.Utils.Converters
{
    public class EqualsCurrentPageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values.Length == 2 && values[0] is int currentPage && values[1] is int pageNumber)
                {
                    return currentPage == pageNumber;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Ghi log để debug
                System.Diagnostics.Debug.WriteLine($"Error in EqualsCurrentPageConverter: {ex.Message}");
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

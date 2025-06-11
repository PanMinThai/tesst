using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TodoList_Project.Core.Utils.Converters
{
    public class IndexBrushMap
    {
        public int Index { get; set; }
        public Brush Brush { get; set; }
    }

    public class IndexToBrushConverter : IMultiValueConverter
    {
        public List<IndexBrushMap> Mapping { get; } = new List<IndexBrushMap>();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int index)
            {
                var map = Mapping.FirstOrDefault(m => m.Index == index % Mapping.Count);
                return map?.Brush ?? Brushes.Gray;
            }
            return Brushes.Gray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

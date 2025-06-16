using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TodoList_Project.Features.Categories.Views
{
    /// <summary>
    /// Interaction logic for CategoryCardControl.xaml
    /// </summary>
    public partial class CategoryCardControl : UserControl
    {
        public CategoryCardControl()
        {
            InitializeComponent();
        }
        private void ProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ProgressBar progressBar && progressBar.Template != null)
            {
                var indicator = progressBar.Template.FindName("PART_Indicator", progressBar) as FrameworkElement;
                if (indicator != null)
                {
                    double progressWidth = progressBar.ActualWidth * (progressBar.Value / progressBar.Maximum);
                    indicator.Width = progressWidth;
                }
            }
        }

    }
}

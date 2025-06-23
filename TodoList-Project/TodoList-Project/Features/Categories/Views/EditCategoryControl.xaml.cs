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
    /// Interaction logic for EditCategoryControl.xaml
    /// </summary>
    public partial class EditCategoryControl : UserControl
    {
        public EditCategoryControl()
        {
            InitializeComponent();
        }
        //public static readonly RoutedEvent CloseRequestedEvent =
        //EventManager.RegisterRoutedEvent(
        //    "CloseRequested",
        //    RoutingStrategy.Bubble,
        //    typeof(RoutedEventHandler),
        //    typeof(EditCategoryControl));

        //public event RoutedEventHandler CloseRequested
        //{
        //    add { AddHandler(CloseRequestedEvent, value); }
        //    remove { RemoveHandler(CloseRequestedEvent, value); }
        //}

        //private void CloseButton_Click(object sender, RoutedEventArgs e)
        //{
        //    RaiseEvent(new RoutedEventArgs(CloseRequestedEvent));
        //}

        //private void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    RaiseEvent(new RoutedEventArgs(CloseRequestedEvent));
        //}

        //private void SaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Handle save logic here
        //    RaiseEvent(new RoutedEventArgs(CloseRequestedEvent));
        //}
        //private void ColorButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button button && button.Tag is string colorHex)
        //    {
        //        // Xử lý khi click vào button màu
        //        // Ví dụ:
        //        // SelectedColor = colorHex;
        //    }
        //}

        //private void CustomColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (sender is TextBox textBox)
        //    {
        //        // Xử lý khi text thay đổi
        //        // Ví dụ:
        //        // ValidateColor(textBox.Text);
        //    }
        //}
    }
}

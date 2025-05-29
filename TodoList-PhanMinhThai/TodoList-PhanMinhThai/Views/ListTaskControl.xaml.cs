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
using TodoList_PhanMinhThai.ViewModels;

namespace TodoList_PhanMinhThai.Views
{
    /// <summary>
    /// Interaction logic for ListTaskControl.xaml
    /// </summary>
    public partial class ListTaskControl : UserControl
    {
        public ListTaskControl()
        {
            InitializeComponent();
        }
        private void YesterdayBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ListTaskViewModel vm)
                vm.FilterYesterdayCommand.Execute(null);
        }

        private void TodayBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ListTaskViewModel vm)
                vm.FilterTodayCommand.Execute(null);
        }

        private void ThisWeekBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ListTaskViewModel vm)
                vm.FilterThisWeekCommand.Execute(null);
        }

    }
}

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exercise2_ThaiPM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)// chuot trai co giu hay ko
                DragMove();// keo window
        }

        private void btnMinisize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(txtUser.Text + "\t"+txtPassword.Password);
        }
        private void ForgotPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Vui lòng nạp vip");
        }
        private void Reset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUser.Clear();
            txtPassword.Clear();
        }
    }
}
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
using Beauty_Salon.Views;

namespace Beauty_Salon.Controls
{
    /// <summary>
    /// Логика взаимодействия для GuestProfile.xaml
    /// </summary>
    public partial class GuestProfile : UserControl
    {
        public GuestProfile()
        {
            InitializeComponent();
            SetCurrentDate(Enter_Date);
            SetCurrentTime(Enter_Time);
        }

        public void SetCurrentDate(TextBlock date)
        {
            if (date != null)
            {
                date.Text = DateTime.Now.ToString("dd.MM.yyyy"); 
            }
        }

        public void SetCurrentTime(TextBlock time)
        {
            if (time != null)
            {
                time.Text = DateTime.Now.ToString("HH:mm:ss"); 
            }
        }
        public void Auth_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            SessionManager.Instance.OpenWindow(mainWindow);

            mainWindow.StartContent.Visibility = Visibility.Collapsed;
            mainWindow.StartButton.Visibility = Visibility.Collapsed;
            mainWindow.GuestBtn.Visibility = Visibility.Collapsed;

            mainWindow.StartFrame.Visibility = Visibility.Visible;
            mainWindow.StartFrame.Navigate(new Views.LoginPage());


        }

    }
}

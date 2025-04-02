using System.Windows;
using System.Windows.Controls;
using Beauty_Salon.DAL.Repositories.TablesRepositories;
using Beauty_Salon.DAL;
using Beauty_Salon.Views;
using System.Windows.Input;

namespace Beauty_Salon.Controls
{
    /// <summary>
    /// Логика взаимодействия для CommonHeader.xaml
    /// </summary>
    public partial class CommonHeader : UserControl
    {
        public string Image => SessionManager.Instance.Image;
        public CommonHeader()
        {
            InitializeComponent();
            DataContext = this;
            CheckedProfileImage();
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow();
            SessionManager.Instance.OpenWindow(menu);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            SessionManager.Instance.OpenWindow(settings);
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.Instance.UserName != "Guest")
            {
                ReservationWindow reserv = new ReservationWindow();

                SessionManager.Instance.OpenWindow(reserv);
            }
            else
                MessageBox.Show("Записываться могут только авторизированные пользователи.", "Вы не авторизированны", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.Instance.UserName != "Guest")
            {
                HistoryWindow history = new HistoryWindow();
                //SessionManager.Instance.OpenWindow(history);
                history.Show();
            }
            else
                MessageBox.Show("Смотреть истории могут только авторизированные пользователи.", "Вы не авторизированны", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.Instance.UserName != "Guest")
            {
               FavoriteWindow favorite = new FavoriteWindow();
                //SessionManager.Instance.OpenWindow(favorite);
                favorite.Show();
            }
            else
                MessageBox.Show("Смотреть избранное могут только авторизированные пользователи.", "Вы не авторизированны", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void OpenProfile_Click(object sender, MouseButtonEventArgs e)
        {
            ProfileWindow profile = new ProfileWindow();

            SessionManager.Instance.OpenWindow(profile); 
        }

        private void CheckedProfileImage()
        {
            if(SessionManager.Instance.UserName == "Guest")
            {
                SessionManager.Instance.Image = "D:\\BSTU3_1\\CourseProject\\Beauty_Salon\\Beauty_Salon\\Views\\pic\\user_icon.png";
            }

        }


    }
}

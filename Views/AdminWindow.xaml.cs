using Beauty_Salon.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Beauty_Salon.Views
{
    public partial class AdminWindow : Window
    {

        public AdminWindow()
        {
            InitializeComponent();
            DataContext = new AdminViewModel();
        }

        // Обработчик события при изменении вкладки
        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedTab = tabControl.SelectedItem as System.Windows.Controls.TabItem;

            if (selectedTab == null)
                return;

            if (selectedTab.Header.ToString() == "Users")
            {
                var viewModel = (AdminViewModel)this.DataContext;
                viewModel.UsersViewModel.LoadUsersCommand.Execute(null);
            }
            else if (selectedTab.Header.ToString() == "Roles")
            {
                var viewModel = (AdminViewModel)this.DataContext;
                viewModel.RolesViewModel.LoadRolesCommand.Execute(null);
            }
            else if (selectedTab.Header.ToString() == "Statuses")
            {
                var viewModel = (AdminViewModel)this.DataContext;
                viewModel.StatusesViewModel.LoadStatusesCommand.Execute(null);
            }
            else if (selectedTab.Header.ToString() == "Services")
            {
                var viewModel = (AdminViewModel)this.DataContext;
                viewModel.ServicesViewModel.LoadServicesCommand.Execute(null);
            }
            else if (selectedTab.Header.ToString() == "Reservations")
            {
                var viewModel = (AdminViewModel)this.DataContext;
                viewModel.ReservationsViewModel.LoadReservationsCommand.Execute(null);
            }
            else if (selectedTab.Header.ToString() == "Favorites")
            {
                var viewModel = (AdminViewModel)this.DataContext;
                viewModel.FavoritesViewModel.LoadFavoriteCommand.Execute(null);
            }
            else if (selectedTab.Header.ToString() == "Grades")
            {
                var viewModel = (AdminViewModel)this.DataContext;
                viewModel.GradesViewModel.LoadGradesCommand.Execute(null);
            }
            else
            {
                var viewModel = (AdminViewModel)this.DataContext;
                viewModel.SpecialistsViewModel.LoadSpecialistsCommand.Execute(null);
            }
        }

        private void Back_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            SessionManager.Instance.OpenWindow(main);
        }
    }
}

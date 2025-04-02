using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using Beauty_Salon.Views;

namespace Beauty_Salon
{
    public partial class MainWindow : Window
    {
        private bool _isStartFrameActive = false;
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            SessionManager.Instance.SetInitialWindow(this);
            
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _isStartFrameActive = true;

            StartContent.Visibility = Visibility.Collapsed;
            StartButton.Visibility = Visibility.Collapsed;
            GuestBtn.Visibility = Visibility.Collapsed;

            StartFrame.Visibility = Visibility.Visible;
            StartFrame.Navigate(new Views.LoginPage());
        }

        private void Guest_ButtonClick(object sender, RoutedEventArgs e)
        {
            MenuWindow set = new MenuWindow();
            SessionManager.Instance.OpenWindow(set);

            SessionManager.Instance.UserName = "Guest";
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickedElement = e.OriginalSource as DependencyObject;

            if (_isStartFrameActive && clickedElement != null && IsClickInsideInteractiveArea(clickedElement))
            {
                return;
            }

            ResetToInitialState();
        }

        private bool IsClickInsideInteractiveArea(DependencyObject clickedElement)
        {
            while (clickedElement != null)
            {
                if (clickedElement == StartFrame)
                {
                    return true;
                }

                if (clickedElement is Button || clickedElement is TextBox || clickedElement is ListView)
                {
                    return true;
                }

                clickedElement = VisualTreeHelper.GetParent(clickedElement);
            }
            return false;
        }

        private void ResetToInitialState()
        {
            _isStartFrameActive = false;

            StartFrame.Visibility = Visibility.Collapsed;

            StartContent.Visibility = Visibility.Visible;
            StartButton.Visibility = Visibility.Visible;
            GuestBtn.Visibility = Visibility.Visible;
        }
    }
}

using System.Windows;
using Beauty_Salon.ViewModels;

namespace Beauty_Salon.Views
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(MenuWindow), new PropertyMetadata(string.Empty));

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }
        public MenuWindow()
        {
            InitializeComponent();
            DataContext = new MenuViewModel();
        }

    }
}

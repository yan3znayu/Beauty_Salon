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
using System.Windows.Shapes;
using Beauty_Salon.ViewModels;

namespace Beauty_Salon.Views
{
    /// <summary>
    /// Логика взаимодействия для FavoriteWindow.xaml
    /// </summary>
    public partial class FavoriteWindow : Window
    {
        public FavoriteWindow()
        {
            InitializeComponent();
            DataContext = new CategoryViewModel();
        }

        private void CloseWindow_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}

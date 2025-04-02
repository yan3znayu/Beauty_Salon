using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Beauty_Salon.ViewModels;

namespace Beauty_Salon.Views
{
    /// <summary>
    /// Логика взаимодействия для HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            InitializeComponent();
            var viewModel = new ReservationsViewModel();
            DataContext = viewModel;

        }
    }

}

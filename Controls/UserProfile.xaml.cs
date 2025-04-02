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

namespace Beauty_Salon.Controls
{
    /// <summary>
    /// Логика взаимодействия для UserProfile.xaml
    /// </summary>
    public partial class UserProfile : UserControl
    {
        public UserProfile()
        {
            InitializeComponent();
        }

        private void ResetInfo_Click(object sender, EventArgs e)
        {
            Login_Text.IsReadOnly = true;
            FIO_Text.IsReadOnly = true;
            Email_Text.IsReadOnly = true;
            Phone_Text.IsReadOnly = true;
            Descript_Text.IsReadOnly = true;
        }
    }
}

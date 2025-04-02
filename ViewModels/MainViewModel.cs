using System.Windows.Input;

namespace Beauty_Salon.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public string Guest => SessionManager.Instance.UserName;

        public MainViewModel() 
        {
            SessionManager.Instance.UserName = "Guest";
        }
    }
}

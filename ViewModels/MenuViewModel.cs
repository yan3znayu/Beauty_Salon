using Beauty_Salon.DAL.Repositories.TablesRepositories;
using Beauty_Salon.DAL;
using Beauty_Salon.Models;
using Beauty_Salon.Views;
using System.Windows;
using Beauty_Salon.ViewModels;

namespace Beauty_Salon.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public string UserName => SessionManager.Instance.UserName;

        public CategoryViewModel CategoryViewModel { get; set; }
        public SpecialistsViewModel SpecialistsViewModel { get; set; }


        public MenuViewModel()
        {
            SpecialistsViewModel = new SpecialistsViewModel();
            CategoryViewModel = new CategoryViewModel();

        }
       
    }
}

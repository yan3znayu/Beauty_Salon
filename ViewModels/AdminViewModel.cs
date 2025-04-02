namespace Beauty_Salon.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        
        public UsersViewModel UsersViewModel { get; set; }
        public RolesViewModel RolesViewModel { get; set; }
        public StatusesViewModel StatusesViewModel { get; set; }
        public GradesViewModel GradesViewModel { get; set; }
        public ReservationsViewModel ReservationsViewModel { get; set; }
        public ServicesViewModel ServicesViewModel { get; set; }
        public SpecialistsViewModel SpecialistsViewModel { get; set; }
        public FavoritesViewModel FavoritesViewModel { get; set; }


        public AdminViewModel()
        {
            UsersViewModel = new UsersViewModel();
            RolesViewModel = new RolesViewModel();
            StatusesViewModel = new StatusesViewModel();
            GradesViewModel = new GradesViewModel();
            ReservationsViewModel = new ReservationsViewModel();
            ServicesViewModel = new ServicesViewModel();
            SpecialistsViewModel = new SpecialistsViewModel();
            FavoritesViewModel = new FavoritesViewModel();
        }
    }
}

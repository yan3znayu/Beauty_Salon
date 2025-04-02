using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Beauty_Salon.Commands;
using Beauty_Salon.DAL;
using Beauty_Salon.DAL.Repositories.TablesRepositories;
using Beauty_Salon.Models;
using Beauty_Salon.Views;

namespace Beauty_Salon.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        private const int PageSize = 4; // Услуги на странице
        private const int FavoritePageSize = 2; // Избранные услуги на странице

        private int currentPage = 1;
        private int favoriteCurrentPage = 1;

        private string _favoriteEmptyText;
        private bool _isFavoriteVisible;

        private List<Services> allServices = new List<Services>();
        private List<Favorites> allFavorites = new List<Favorites>();
        private List<Services> filteredServices = new List<Services>();

        private readonly ServicesRepository _servicesRepository;
        private readonly FavoritesRepository _favoritesRepository;

        private ObservableCollection<Services> _paginatedServices;
        private ObservableCollection<Services> _favoriteServices;
        private ObservableCollection<Services> _paginatedFavoriteServices;
        private Services _currentService;

        private ObservableCollection<string> _serviceTypes;
        private string _selectedServiceType;

        private bool _isFavorite;
        private string _favoriteText;

        public CategoryViewModel()
        {
            var dbContext = new DatabaseContext();
            _favoritesRepository = new FavoritesRepository(dbContext);
            _servicesRepository = new ServicesRepository(dbContext);

            FavoriteServices = new ObservableCollection<Services>();
            PaginatedServices = new ObservableCollection<Services>();
            PaginatedFavoriteServices = new ObservableCollection<Services>();
            ServiceTypes = new ObservableCollection<string>();
            CurrentService = new Services();

            Task.Run(async () =>
            {
                await LoadServicesAsync();
                await LoadServiceAsync();
                await LoadFavoritesAsync();
                await LoadServiceTypesAsync();
                LoadFavoriteStatus();
            });

            ServiceCardOpenCommand = new RelayCommand(ServiceCardOpen);
            ToggleFavoriteCommand = new RelayCommand(async param => await ToggleFavoriteAsync(), param => CanAddToFavorite());
            PreviousPageCommand = new RelayCommand(GoToPreviousPage);
            NextPageCommand = new RelayCommand(GoToNextPage);
            PreviousFavoritePageCommand = new RelayCommand(GoToPreviousFavoritePage, CanGoToPreviousFavoritePage);
            NextFavoritePageCommand = new RelayCommand(GoToNextFavoritePage, CanGoToNextFavoritePage);

            _isFavorite = false;
            FavoriteText = "Добавить в избранное";
            FavoriteEmptyText = "Избранное пусто!"; 
            IsFavoriteVisible = false; 
            SelectedServiceType = "Все категории";
        }

        public ObservableCollection<string> ServiceTypes
        {
            get => _serviceTypes;
            set
            {
                _serviceTypes = value;
                OnPropertyChanged(nameof(ServiceTypes));
            }
        }

        public string SelectedServiceType
        {
            get => _selectedServiceType;
            set
            {
                _selectedServiceType = value;
                OnPropertyChanged(nameof(SelectedServiceType));
                ApplyCategoryFilter();
            }
        }


        public ObservableCollection<Services> PaginatedServices
        {
            get => _paginatedServices;
            set
            {
                _paginatedServices = value;
                OnPropertyChanged(nameof(PaginatedServices));
            }
        }

        public ObservableCollection<Services> FavoriteServices
        {
            get => _favoriteServices;
            set
            {
                _favoriteServices = value;
                OnPropertyChanged(nameof(FavoriteServices));
            }
        }

        public ObservableCollection<Services> PaginatedFavoriteServices
        {
            get => _paginatedFavoriteServices;
            set
            {
                _paginatedFavoriteServices = value;
                OnPropertyChanged(nameof(PaginatedFavoriteServices));
            }
        }

        public string FavoriteEmptyText
        {
            get => _favoriteEmptyText;
            set
            {
                _favoriteEmptyText = value;
                OnPropertyChanged(nameof(FavoriteEmptyText));
            }
        }

        public bool IsFavoriteVisible
        {
            get => _isFavoriteVisible;
            set
            {
                _isFavoriteVisible = value;
                OnPropertyChanged(nameof(IsFavoriteVisible));
            }
        }

        public Services CurrentService
        {
            get => _currentService;
            set
            {
                _currentService = value;
                OnPropertyChanged(nameof(CurrentService));
                UpdatePaginatedServices();
            }
        }

        public string FavoriteText
        {
            get => _favoriteText;
            set
            {
                _favoriteText = value;
                OnPropertyChanged(nameof(FavoriteText));
            }
        }

        public int CurrentPage
        {
            get => currentPage;
            set
            {
                if (currentPage != value)
                {
                    currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                    UpdatePage();
                }
            }
        }

        public int FavoriteCurrentPage
        {
            get => favoriteCurrentPage;
            set
            {
                if (favoriteCurrentPage != value)
                {
                    favoriteCurrentPage = value;
                    OnPropertyChanged(nameof(FavoriteCurrentPage));
                    UpdateFavoritePage();
                }
            }
        }

        public int TotalPages => (int)Math.Ceiling((double)filteredServices.Count / PageSize);
        public int FavoriteTotalPages => (int)Math.Ceiling((double)(FavoriteServices?.Count ?? 0) / FavoritePageSize);

        public ICommand ServiceCardOpenCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousFavoritePageCommand { get; }
        public ICommand NextFavoritePageCommand { get; }

        private async void LoadFavoriteStatus()
        {
            var userId = SessionManager.Instance.UserID;
            var serviceId = SessionManager.Instance.ServiceID;

            var favorite = await _favoritesRepository.GetFavoriteAsync(userId, serviceId);

            _isFavorite = favorite != null;

            UpdateFavoriteUI();
        }

        private async Task LoadServiceTypesAsync()
        {
            var types = await Task.Run(() => allServices
                .Select(s => s.Type)
                .Distinct()
                .OrderBy(t => t)
                .ToList());

            Application.Current.Dispatcher.Invoke(() =>
            {
                ServiceTypes = new ObservableCollection<string>(types);
                ServiceTypes.Insert(0, "Все категории"); 
                SelectedServiceType = "Все категории"; 
            });

        }

        private bool CanAddToFavorite()
        {
            return SessionManager.Instance.UserName != "Guest";
        }

        private async Task ToggleFavoriteAsync()
        {
            var userId = SessionManager.Instance.UserID;
            var serviceId = SessionManager.Instance.ServiceID;

            var existingFavorite = await _favoritesRepository.GetFavoriteAsync(userId, serviceId);

            if (existingFavorite != null)
            {
                await _favoritesRepository.DeleteAsync(existingFavorite.Favorite_ID);
                _isFavorite = false;
                
            }
            else
            {
                var favorite = new Favorites
                {
                    User_ID = userId,
                    Service_ID = serviceId
                };
                await _favoritesRepository.AddAsync(favorite);
                _isFavorite = true;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateFavoriteUI();
                Task.Run(async () => await LoadFavoritesAsync());
            });
        }

        private void UpdateFavoriteUI()
        {
            FavoriteText = _isFavorite ? "Уже в избранном" : "Добавить в избранное";
        }

        private void ServiceCardOpen(object parameter)
        {
            var serviceSelect = parameter as Services;
            if (serviceSelect != null)
            {
                SessionManager.Instance.ServiceID = serviceSelect.Service_ID;
                ServiceCardWindow serviceCardWindow = new ServiceCardWindow();
                serviceCardWindow.Show();
                _ = LoadServiceAsync();
            }
        }

        private async Task LoadServicesAsync()
        {
            var services = await _servicesRepository.GetAllAsync();
            allServices = services.ToList();
            filteredServices = new List<Services>(allServices); 
            UpdatePaginatedServices();
            //UpdatePage();
        }

        private async Task LoadServiceAsync()
        {
            var ServiceId = SessionManager.Instance.ServiceID;
            CurrentService = await _servicesRepository.GetByIdAsync(ServiceId);
        }

        private void ApplyCategoryFilter()
        {
            CurrentPage = 1; 

            if (SelectedServiceType == "Все категории")
            {
                filteredServices = new List<Services>(allServices);
            }
            else
            {
                filteredServices = allServices
                    .Where(s => s.Type == SelectedServiceType)
                    .ToList();
            }

            UpdatePaginatedServices();
            OnPropertyChanged(nameof(TotalPages));
        }

        private void UpdatePaginatedServices()
        {
            var servicesToDisplay = filteredServices
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PaginatedServices = new ObservableCollection<Services>(servicesToDisplay);

            OnPropertyChanged(nameof(CanGoToNextPage));
            OnPropertyChanged(nameof(CanGoToPreviousPage));
        }

        private async Task LoadFavoritesAsync()
        {
            var userId = SessionManager.Instance.UserID;

            var favorites = await _favoritesRepository.FindAsync(f => f.User_ID == userId);

            var favoriteServiceIds = favorites.Select(f => f.Service_ID).ToList();
            var favoriteServices = allServices
                .Where(s => favoriteServiceIds.Contains(s.Service_ID))
                .ToList();

            Application.Current.Dispatcher.Invoke(() =>
            {
                FavoriteServices = new ObservableCollection<Services>(favoriteServices);

                if (FavoriteServices.Count > 0)
                {
                    FavoriteEmptyText = "";
                    IsFavoriteVisible = false; 
                    FavoriteCurrentPage = 1;  
                }
                else
                {
                    FavoriteEmptyText = "Избранное пусто!";
                    IsFavoriteVisible = true; 
                    FavoriteCurrentPage = 0;  
                }

                UpdateFavoritePage();
            });
        }


        private void UpdatePage()
        {
            var servicesToDisplay = allServices
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PaginatedServices = new ObservableCollection<Services>(servicesToDisplay);
        }

        private void UpdateFavoritePage()
        {
            if (FavoriteServices == null || FavoriteServices.Count == 0)
            {
                PaginatedFavoriteServices = new ObservableCollection<Services>();
                return;
            }

            var servicesToDisplay = FavoriteServices
                .Skip((favoriteCurrentPage - 1) * FavoritePageSize)
                .Take(FavoritePageSize)
                .ToList();

            PaginatedFavoriteServices = new ObservableCollection<Services>(servicesToDisplay);
        }


        private void GoToPreviousPage(object parameter)
        {
            if (CanGoToPreviousPage)
            {
                CurrentPage--;
            }
        }
        public bool CanGoToPreviousPage => CurrentPage > 1;
        public bool CanGoToNextPage => CurrentPage < TotalPages;

        private void GoToNextPage(object parameter)
        {
            if (CanGoToNextPage)
            {
                CurrentPage++;
            }
        }

        private void GoToPreviousFavoritePage(object parameter)
        {
            if (FavoriteCurrentPage > 1)
            {
                FavoriteCurrentPage--;
            }
        }

        private bool CanGoToPreviousFavoritePage(object parameter) => FavoriteCurrentPage > 1;

        private void GoToNextFavoritePage(object parameter)
        {
            if (FavoriteCurrentPage < FavoriteTotalPages)
            {
                FavoriteCurrentPage++;
            }
        }

        private bool CanGoToNextFavoritePage(object parameter) => FavoriteCurrentPage < FavoriteTotalPages;
    }
}

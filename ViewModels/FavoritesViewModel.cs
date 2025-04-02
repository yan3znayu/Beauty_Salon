using Beauty_Salon.Models;
using Beauty_Salon.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Beauty_Salon.DAL;
using Beauty_Salon.DAL.Repositories.TablesRepositories;
using Microsoft.Win32;
using System.Windows;

namespace Beauty_Salon.ViewModels
{
    public class FavoritesViewModel: BaseViewModel
    {
        private readonly FavoritesRepository _favoriteRepository;
        private ObservableCollection<Favorites> _favorites;
        private Favorites _selectedFavorites;

        public FavoritesViewModel()
        {
            _favoriteRepository = new FavoritesRepository(new DatabaseContext());
            LoadFavoriteCommand = new RelayCommand(async (param) => await LoadFavoriteAsync());
            DeleteFavoriteCommand = new RelayCommand(async (param) => await DeleteFavoriteAsync());
            SelectedFavorite = new Favorites();
        }

        public ObservableCollection<Favorites> Favorites
        {
            get => _favorites;
            set
            {
                _favorites = value;
                OnPropertyChanged(nameof(Favorites));
            }
        }

        public Favorites SelectedFavorite
        {
            get => _selectedFavorites;
            set
            {
                _selectedFavorites = value;
                OnPropertyChanged(nameof(SelectedFavorite));
            }
        }

        public ICommand LoadFavoriteCommand { get; set; }
        public ICommand DeleteFavoriteCommand { get; set; }

        private async Task LoadFavoriteAsync()
        {
            var favorites = await _favoriteRepository.GetAllAsync();
            Favorites = new ObservableCollection<Favorites>(favorites);

        }

        private async Task DeleteFavoriteAsync()
        {
            if (SelectedFavorite != null && SelectedFavorite.Favorite_ID > 0)
            {
                await _favoriteRepository.DeleteAsync(SelectedFavorite.Favorite_ID);
                await LoadFavoriteAsync();
                SelectedFavorite = new Favorites();
            }
        }
    }
}

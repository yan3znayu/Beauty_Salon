using Beauty_Salon.Models;
using Beauty_Salon.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Beauty_Salon.DAL;
using Beauty_Salon.DAL.Repositories.TablesRepositories;

namespace Beauty_Salon.ViewModels
{
    public class StatusesViewModel : BaseViewModel
    {
        private readonly StatusesRepository _statusesRepository;
        private ObservableCollection<Statuses> _statuses;
        private Statuses _selectedStatus;

        public StatusesViewModel()
        {
            _statusesRepository = new StatusesRepository(new DatabaseContext());
            LoadStatusesCommand = new RelayCommand(async (param) => await LoadStatusesAsync());
            AddStatusCommand = new RelayCommand(async (param) => await AddStatusAsync());
            SaveStatusCommand = new RelayCommand(async (param) => await SaveStatusAsync());
            DeleteStatusCommand = new RelayCommand(async (param) => await DeleteStatusAsync());
            SelectedStatus = new Statuses();
        }

        public ObservableCollection<Statuses> Statuses
        {
            get => _statuses;
            set
            {
                _statuses = value;
                OnPropertyChanged(nameof(Statuses));
            }
        }

        public Statuses SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        public ICommand LoadStatusesCommand { get; set; }
        public ICommand AddStatusCommand { get; set; }
        public ICommand SaveStatusCommand { get; set; }
        public ICommand DeleteStatusCommand { get; set; }

        private async Task LoadStatusesAsync()
        {
            var statuses = await _statusesRepository.GetAllAsync();
            Statuses = new ObservableCollection<Statuses>(statuses);
        }

        private async Task AddStatusAsync()
        {
            if (!string.IsNullOrEmpty(SelectedStatus.Status_Name))
            {
                await _statusesRepository.AddAsync(SelectedStatus);
                await LoadStatusesAsync();
                SelectedStatus = new Statuses();
            }
        }

        private async Task SaveStatusAsync()
        {
            if (SelectedStatus != null && SelectedStatus.Status_ID > 0)
            {
                await _statusesRepository.UpdateAsync(SelectedStatus);
                await LoadStatusesAsync();
                SelectedStatus = new Statuses();
            }
        }

        private async Task DeleteStatusAsync()
        {
            if (SelectedStatus != null && SelectedStatus.Status_ID > 0)
            {
                await _statusesRepository.DeleteAsync(SelectedStatus.Status_ID);
                await LoadStatusesAsync();
                SelectedStatus = new Statuses();
            }
        }
    }
}

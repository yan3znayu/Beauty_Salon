using Beauty_Salon.Commands;
using Beauty_Salon.DAL.Repositories.TablesRepositories;
using Beauty_Salon.DAL;
using Beauty_Salon.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Beauty_Salon.ViewModels
{
    public class ReservationsViewModel : BaseViewModel
    {
        public int UserID => SessionManager.Instance.UserID;


        private bool _isHistoryVisible;
        private ReservationsRepository _reservationsRepository;
        private int _currentPage = 0; 
        private int _totalPages = 0;
        private ObservableCollection<Reservations> _reservations;
        private Reservations _selectedReservation;
        private Reservations _deletedReservation;
        private Specialists _specialist;
        private Services _services;

        private string _historyText;

        public ReservationsViewModel()
        {
            _reservationsRepository = new ReservationsRepository(new DatabaseContext());
            LoadReservationsCommand = new RelayCommand(async (param) => await LoadReservationsAsync());
            AddReservationCommand = new RelayCommand(async (param) => await AddReservationAsync());
            SaveReservationCommand = new RelayCommand(async (param) => await SaveReservationAsync());
            DeleteReservationCommand = new RelayCommand(async (param) => await DeleteReservationAsync());
            DeleteReservationAdminCommand = new RelayCommand(async (param) => await DeleteReservationAdminAsync());
            SelectedReservation = null;
            DeletedReservation = new Reservations();

            if (SessionManager.Instance.UserName != null)
                _ = LoadHistorysAsync();
            else
                _ = LoadReservationsAsync();

            HistoryText = "Ваша история пуста!"; 
        }

        public bool IsHistoryVisible
        {
            get => _isHistoryVisible;
            set
            {
                _isHistoryVisible = value;
                OnPropertyChanged(nameof(IsHistoryVisible));
            }
        }

        public int CurrentPage
        {
            get => _currentPage + 1; 
            set
            {
                _currentPage = value - 1; 
                OnPropertyChanged(nameof(CurrentPage));
                LoadPage();
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }

        public ObservableCollection<Reservations> Reservations
        {
            get => _reservations;
            set
            {
                _reservations = value;
                OnPropertyChanged(nameof(Reservations));
                UpdateHistoryUI(); 
            }
        }

        public Reservations SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
                LoadServiceAndSpecialist();
            }
        }

        public Reservations DeletedReservation
        {
            get => _deletedReservation;
            set
            {
                _deletedReservation = value;
                OnPropertyChanged(nameof(DeletedReservation));
            }
        }

        public Specialists Specialist
        {
            get => _specialist;
            set
            {
                _specialist = value;
                OnPropertyChanged(nameof(Specialist));
            }
        }

        public Services Service
        {
            get => _services;
            set
            {
                _services = value;
                OnPropertyChanged(nameof(Service));
            }
        }

        public string HistoryText
        {
            get => _historyText;
            set
            {
                _historyText = value;
                OnPropertyChanged(nameof(HistoryText));
            }
        }

        public ICommand LoadReservationsCommand { get; set; }
        public ICommand AddReservationCommand { get; set; }
        public ICommand SaveReservationCommand { get; set; }
        public ICommand DeleteReservationCommand { get; set; }
        public ICommand DeleteReservationAdminCommand {  get; set; }

        private async void LoadPage()
        {
            if (Reservations != null && Reservations.Count > 0 && _currentPage >= 0 && _currentPage < Reservations.Count)
            {
                SelectedReservation = Reservations[_currentPage];
            }
            else
            {
                SelectedReservation = null; 
            }
        }

        private async Task LoadServiceAndSpecialist()
        {
            if (SelectedReservation == null) return;

            using (var context = new DatabaseContext())
            {
                var service = context.Services.FirstOrDefault(s => s.Service_ID == SelectedReservation.Service_ID);
                string serviceName = service != null ? service.Service_Name : "Service not found";

                var specialist = context.Specialists.FirstOrDefault(sp => sp.Specialists_ID == SelectedReservation.Specialist_ID);
                string specialistName = specialist != null ? specialist.Specialist_Name : "Specialist not found";

                Specialist = new Specialists { Specialist_Name = specialistName };
                Service = new Services { Service_Name = serviceName };
            }
        }

        public ICommand PreviousCommand => new RelayCommand(param => PreviousPage(), param => CanPreviousPage());
        public ICommand NextCommand => new RelayCommand(param => NextPage(), param => CanNextPage());

        private void PreviousPage()
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                LoadPage();
                OnPropertyChanged(nameof(CurrentPage)); 
            }
        }

        private bool CanPreviousPage()
        {
            return _currentPage > 0;
        }

        private void NextPage()
        {
            if (_currentPage < TotalPages - 1)
            {
                _currentPage++;
                LoadPage();
                OnPropertyChanged(nameof(CurrentPage)); 
            }
        }

        private bool CanNextPage()
        {
            return _currentPage < TotalPages - 1;
        }

        private async Task AddReservationAsync()
        {
            if (SelectedReservation.User_ID > 0 &&
                SelectedReservation.Specialist_ID > 0 &&
                SelectedReservation.Service_ID > 0 &&
                SelectedReservation.Appointment_Date != default &&
                SelectedReservation.Appointment_Time != default)
            {
                await _reservationsRepository.AddAsync(SelectedReservation);
                await LoadReservationsAsync();
                SelectedReservation = new Reservations();
            }
        }

        private void UpdateHistoryUI()
        {
            if (Reservations == null || Reservations.Count == 0 || SessionManager.Instance.History)
            {
                HistoryText = "Ваша история пуста!"; 
                IsHistoryVisible = false; 
            }
            else
            {
                HistoryText = ""; 
                IsHistoryVisible = true;
            }
        }

    private async Task SaveReservationAsync()
        {
            if (SelectedReservation != null && SelectedReservation.Reservation_ID > 0)
            {
                await _reservationsRepository.UpdateAsync(SelectedReservation);
                await LoadReservationsAsync();
                SelectedReservation = null;
            }
        }

        private async Task DeleteReservationAsync()
        {
            if (SelectedReservation != null && SelectedReservation.Reservation_ID > 0)
            {
                int deletedIndex = Reservations.IndexOf(SelectedReservation);

                await _reservationsRepository.DeleteAsync(SelectedReservation.Reservation_ID);

                Reservations.Remove(SelectedReservation);

                UpdateHistoryUI();

                if (Reservations.Count > 0)
                {
                    if (deletedIndex >= Reservations.Count)
                        deletedIndex = Reservations.Count - 1;

                    SelectedReservation = Reservations[deletedIndex];
                }
                else
                {
                    SelectedReservation = null;
                    _currentPage = 0;
                    TotalPages = 0;
                }

                CurrentPage = Reservations.Count > 0 ? deletedIndex + 1 : 0;
                TotalPages = Reservations.Count;

                OnPropertyChanged(nameof(SelectedReservation));
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(TotalPages));
            }
        }

        private async Task DeleteReservationAdminAsync()
        {
            if (DeletedReservation != null && DeletedReservation.Reservation_ID > 0)
            {
                await _reservationsRepository.DeleteAsync(DeletedReservation.Reservation_ID);
                await LoadReservationsAsync();
                DeletedReservation = new Reservations();
            }
        }



        public async Task LoadReservationsAsync()
        {
            var reservation = await _reservationsRepository.GetAllAsync();

            var sortedReservations = reservation
                .OrderBy(r => r.Appointment_Date)
                .ThenBy(r => r.Appointment_Time)
                .ToList();

            Reservations = new ObservableCollection<Reservations>(sortedReservations);

            TotalPages = Reservations.Count > 0 ? Reservations.Count : 0;
            CurrentPage = Reservations.Count > 0 ? 1 : 0;

            UpdateHistoryUI();
        }


        public async Task LoadHistorysAsync()
        {
            var reservation = await _reservationsRepository.FindAsync(r => r.User_ID == UserID);

            var sortedReservations = reservation
                .OrderBy(r => r.Appointment_Date)
                .ThenBy(r => r.Appointment_Time)
                .ToList();

            Reservations = new ObservableCollection<Reservations>(sortedReservations);

            if (Reservations.Count > 0)
            {
                TotalPages = Reservations.Count;
                CurrentPage = 1;
            }
            else
            {
                TotalPages = 0;
                CurrentPage = 0;
            }

            UpdateHistoryUI();
        }


    }
}

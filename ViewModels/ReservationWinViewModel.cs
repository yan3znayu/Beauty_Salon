using Beauty_Salon.Models;
using Beauty_Salon.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Beauty_Salon.DAL;
using Beauty_Salon.DAL.Repositories.TablesRepositories;
using System.Linq;

namespace Beauty_Salon.ViewModels
{
    public class ReservationWinViewModel : BaseViewModel
    {
        private readonly ReservationsRepository _reservationRepository;

        public CategoryViewModel CategoryViewModel { get; set; }
        public SpecialistsViewModel SpecialistsViewModel { get; set; }

        private Reservations _selectedReservation;
        private string _selectedTimeSlot;
        private string _selectedTimePeriod;

        private ObservableCollection<string> _timeSlots;

        public ICommand SelectTimeSlotCommand { get; }
        public ICommand SelectServiceCommand { get; }
        public ICommand SelectSpecialistCommand { get; }
        public ICommand SaveReservationCommand { get; }

        public ReservationWinViewModel()
        {
            var dbContext = new DatabaseContext();
            _reservationRepository = new ReservationsRepository(dbContext);

            SpecialistsViewModel = new SpecialistsViewModel();
            CategoryViewModel = new CategoryViewModel();

            SelectedReservation = new Reservations
            {
                Appointment_Date = DateTime.UtcNow,
                Service_ID = 0,
                Specialist_ID = 0,
                Reserv_Date = DateTime.Today,
                User_ID = SessionManager.Instance.UserID
            };

            SelectedTimePeriod = "Утро";
            UpdateTimeSlots();

            SelectTimeSlotCommand = new RelayCommand(param => SelectTimeSlot(param.ToString()));
            SelectServiceCommand = new RelayCommand(param => SelectService(param as Services));
            SelectSpecialistCommand = new RelayCommand(param => SelectSpecialist(param as Specialists));
            SaveReservationCommand = new RelayCommand(async _ => await AddReservationAsync(), CanAddReservation);
        }

        public ObservableCollection<string> TimeSlots
        {
            get => _timeSlots;
            set { _timeSlots = value; OnPropertyChanged(nameof(TimeSlots)); }
        }

        public Reservations SelectedReservation
        {
            get => _selectedReservation;
            set { _selectedReservation = value; OnPropertyChanged(nameof(SelectedReservation)); }
        }

        public string SelectedTimePeriod
        {
            get => _selectedTimePeriod;
            set
            {
                if (_selectedTimePeriod != value)
                {
                    _selectedTimePeriod = value;
                    OnPropertyChanged(nameof(SelectedTimePeriod));
                    UpdateTimeSlots();
                }
            }
        }

        public string SelectedTimeSlot
        {
            get => _selectedTimeSlot;
            set
            {
                if (_selectedTimeSlot != value)
                {
                    _selectedTimeSlot = value;
                    OnPropertyChanged(nameof(SelectedTimeSlot));
                    UpdateTimeSlots();
                }
            }
        }

        private void UpdateTimeSlots()
        {
            if (SelectedTimePeriod == "Утро")
            {
                TimeSlots = new ObservableCollection<string> { "8:00", "9:00", "10:00", "11:00", "12:00", "13:00" };
            }
            else if (SelectedTimePeriod == "День")
            {
                TimeSlots = new ObservableCollection<string> { "14:00", "15:00", "16:00", "17:00", "18:00", "19:00" };
            }
        }

        private void SelectTimeSlot(string timeSlot)
        {
            SelectedTimeSlot = timeSlot;
            SelectedReservation.Appointment_Time = TimeSpan.Parse(timeSlot);
        }

        private void SelectService(Services service)
        {
            if (service != null)
            {
                SelectedReservation.Service_ID = service.Service_ID;
            }
        }

        private void SelectSpecialist(Specialists specialist)
        {
            if (specialist != null)
            {
                SelectedReservation.Specialist_ID = specialist.Specialists_ID;
            }
        }

        private bool CanAddReservation(object parameter)
        {
            return SelectedReservation != null &&
                   !string.IsNullOrEmpty(SelectedTimeSlot) &&
                   SelectedReservation.Service_ID > 0 &&
                   SelectedReservation.Specialist_ID > 0 &&
                   SelectedReservation.Appointment_Date >= DateTime.Today;
        }

        private async Task AddReservationAsync()
        {
            try
            {
                var appointmentDateUtc = DateTime.SpecifyKind(SelectedReservation.Appointment_Date.Date + SelectedReservation.Appointment_Time, DateTimeKind.Utc);

                var existingReservation = (await _reservationRepository.FindAsync(r =>
                    r.Appointment_Date == appointmentDateUtc &&
                    r.Appointment_Time == SelectedReservation.Appointment_Time &&
                    r.User_ID == SelectedReservation.User_ID)).FirstOrDefault();

                if (existingReservation != null)
                {
                    var serviceName = CategoryViewModel?.PaginatedServices.FirstOrDefault(s => s.Service_ID == existingReservation.Service_ID)?.Service_Name ?? "Услуга неизвестна";
                    var specialistName = SpecialistsViewModel?.Specialists.FirstOrDefault(s => s.Specialists_ID == existingReservation.Specialist_ID)?.Specialist_Name ?? "Мастер неизвестен";

                    MessageBox.Show(
                        $"У вас уже есть запись на выбранную дату {existingReservation.Appointment_Date:dd.MM.yyyy} и время {existingReservation.Appointment_Time:hh\\:mm} " +
                        $"на услугу \"{serviceName}\" к мастеру \"{specialistName}\". Пожалуйста, выберите другое время или отмените прошлую запись.",
                        "Альцгеймер?",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );

                    return;
                }

                if (SelectedReservation.Appointment_Date.Date == DateTime.Now.Date)
                {
                    var selectedTime = DateTime.Now.Date + SelectedReservation.Appointment_Time;
                    if (selectedTime < DateTime.Now)
                    {
                        MessageBox.Show(
                            $"Невозможно записаться на выбранное время {SelectedReservation.Appointment_Time:hh\\:mm} на сегодняшнее число. Мы в прошлое пока не умеем :(",
                            "Прыжок в прошлое",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
                    }
                    if ((selectedTime - DateTime.Now).TotalMinutes < 60)
                    {
                        MessageBox.Show(
                            $"Нельзя записаться позднее, чем за час до указанного времени. Пожалуйста, выберите другое время.",
                            "Раньше думать надо было",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
                    }
                }

                SelectedReservation.Appointment_Date = appointmentDateUtc;
                SelectedReservation.Reserv_Date = DateTime.UtcNow;

                await _reservationRepository.AddAsync(SelectedReservation);

                var addedServiceName = CategoryViewModel?.PaginatedServices.FirstOrDefault(s => s.Service_ID == SelectedReservation.Service_ID)?.Service_Name ?? "Услуга неизвестна";
                var addedSpecialistName = SpecialistsViewModel?.Specialists.FirstOrDefault(s => s.Specialists_ID == SelectedReservation.Specialist_ID)?.Specialist_Name ?? "Мастер неизвестен";

                MessageBox.Show(
                    $"Запись успешно добавлена на {SelectedReservation.Appointment_Date:dd.MM.yyyy} в {SelectedReservation.Appointment_Time:hh\\:mm} " +
                    $"на услугу \"{addedServiceName}\" к мастеру \"{addedSpecialistName}\".",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                ResetState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ResetState()
        {
            SelectedReservation = new Reservations
            {
                Appointment_Date = DateTime.UtcNow,
                Service_ID = 0,
                Specialist_ID = 0,
                Reserv_Date = DateTime.Today,
                User_ID = SessionManager.Instance.UserID
            };
            SelectedTimeSlot = null;
            SelectedTimePeriod = "Утро";
            UpdateTimeSlots();
        }
    }
}

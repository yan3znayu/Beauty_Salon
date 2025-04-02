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
    public class ServicesViewModel : BaseViewModel
    {
        private readonly ServicesRepository _servicesRepository;
        private ObservableCollection<Services> _services;
        private Services _selectedService;

        public ServicesViewModel()
        {
            _servicesRepository = new ServicesRepository(new DatabaseContext());
            LoadServicesCommand = new RelayCommand(async (param) => await LoadServicesAsync());
            AddServiceCommand = new RelayCommand(async (param) => await AddServiceAsync());
            SaveServiceCommand = new RelayCommand(async (param) => await SaveServiceAsync());
            DeleteServiceCommand = new RelayCommand(async (param) => await DeleteServiceAsync());
            ChooseImageCommand = new RelayCommand(param => ChooseImage());
            SelectedService = new Services();
            EmptyPhoto();
        }

        public ObservableCollection<Services> Services
        {
            get => _services;
            set
            {
                _services = value;
                OnPropertyChanged(nameof(Services));
            }
        }

        public Services SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
            }
        }

        public ICommand LoadServicesCommand { get; set; }
        public ICommand AddServiceCommand { get; set; }
        public ICommand SaveServiceCommand { get; set; }
        public ICommand DeleteServiceCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }

        private async Task LoadServicesAsync()
        {
            var services = await _servicesRepository.GetAllAsync();
            Services = new ObservableCollection<Services>(services);
            
        }

        private async Task AddServiceAsync()
        {
            if (!string.IsNullOrEmpty(SelectedService.Service_Name) && SelectedService.Price > 0)
            {
                await _servicesRepository.AddAsync(SelectedService);
                await LoadServicesAsync();
                SelectedService = new Services();
            }
        }

        private async Task SaveServiceAsync()
        {
            if (SelectedService != null && SelectedService.Service_ID > 0)
            {
                await _servicesRepository.UpdateAsync(SelectedService);
                await LoadServicesAsync();

                SelectedService = new Services();
                if (string.IsNullOrEmpty(SelectedService.ImagePath))
                {
                    EmptyPhoto();
                }
            }
        }

        private async Task DeleteServiceAsync()
        {
            if (SelectedService != null && SelectedService.Service_ID > 0)
            {
                await _servicesRepository.DeleteAsync(SelectedService.Service_ID);
                await LoadServicesAsync();
                SelectedService = new Services();
            }
        }

        private void ChooseImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedService.ImagePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedService));
            }
        }

        private void EmptyPhoto()
        {
            SelectedService.ImagePath = "D:/BSTU3_1/CourseProject/Beauty_Salon/Beauty_Salon/Views/pic/no-pic.png";
            OnPropertyChanged(nameof(SelectedService));
        }
    }
}

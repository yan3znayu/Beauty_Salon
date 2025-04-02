using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Beauty_Salon.Commands;
using Beauty_Salon.DAL;
using Beauty_Salon.DAL.Repositories.TablesRepositories;
using Beauty_Salon.Models;
using Beauty_Salon.Views;
using Microsoft.Win32;

namespace Beauty_Salon.ViewModels
{
    public class SpecialistsViewModel : BaseViewModel
    {
 
        private const int PageSize = 4;
        private int currentPage = 1;

        private List<Specialists> allSpecialists = new List<Specialists>();
        private ObservableCollection<Specialists> _specialists;
        private Specialists _selectedSpecialist;
        private readonly SpecialistsRepository _specialistsRepository;
        private readonly GradesRepository _gradesRepository;

        public SpecialistsViewModel()
        {
            _specialistsRepository = new SpecialistsRepository(new DatabaseContext());
            _gradesRepository = new GradesRepository(new DatabaseContext());
            LoadSpecialistsCommand = new RelayCommand(async (param) => await LoadSpecialistsAsync());
            AddSpecialistCommand = new RelayCommand(async (param) => await AddSpecialistAsync());
            SaveSpecialistCommand = new RelayCommand(async (param) => await SaveSpecialistAsync());
            DeleteSpecialistCommand = new RelayCommand(async (param) => await DeleteSpecialistAsync());
            ChooseImageCommand = new RelayCommand(param => ChooseImage());
            SpecialistsCardOpenCommand = new RelayCommand(SpecialistsCardOpen);
            SelectedSpecialist = new Specialists();

            Task.Run(async () =>
            {
                await LoadSpecialistsAsync();
                await LoadSpecialistAsync();
            });

            EmptyPhoto();
        }

        public ObservableCollection<Specialists> PaginatedSpecialists { get; set; }

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

        public ObservableCollection<Specialists> Specialists
        {
            get => _specialists;
            set
            {
                _specialists = value;
                OnPropertyChanged(nameof(Specialists));
            }
        }

        public Specialists SelectedSpecialist
        {
            get => _selectedSpecialist;
            set
            {
                _selectedSpecialist = value;
                OnPropertyChanged(nameof(SelectedSpecialist));
            }
        }

        public ICommand LoadSpecialistsCommand { get; set; }
        public ICommand AddSpecialistCommand { get; set; }
        public ICommand SaveSpecialistCommand { get; set; }
        public ICommand DeleteSpecialistCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }
        public ICommand SpecialistsCardOpenCommand { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)allSpecialists.Count / PageSize);

        public async Task LoadSpecialistsAsync()
        {
            var specialists = await _specialistsRepository.GetAllAsync();
            Specialists = new ObservableCollection<Specialists>(specialists);
            var grades = await _gradesRepository.GetAllAsync();
            var gradesGroupedBySpecialist = grades.GroupBy(g => g.Specialists_ID);

            foreach (var specialist in specialists)
            {
                var specialistGrades = gradesGroupedBySpecialist
                    .FirstOrDefault(g => g.Key == specialist.Specialists_ID)
                    ?.Select(g => g.Grade)
                    .ToList();

                if (specialistGrades != null && specialistGrades.Any())
                {
                    specialist.Average_Grade = (float)specialistGrades.Average();
                }
                else
                {
                    specialist.Average_Grade = 0;
                }
            }


            allSpecialists = specialists.ToList();
            UpdatePage();

        }

        private void UpdatePage()
        {
            var specialistsToDisplay = allSpecialists
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PaginatedSpecialists = new ObservableCollection<Specialists>(specialistsToDisplay);
            OnPropertyChanged(nameof(PaginatedSpecialists));
        }



        private void SpecialistsCardOpen(object parameter)
        {
            var selectedSpecialist = parameter as Specialists;
            if (selectedSpecialist != null)
            {
                SessionManager.Instance.SpecialistID = selectedSpecialist.Specialists_ID;
                SpecialistsCardWindowl specialistCardWindow = new SpecialistsCardWindowl();
                specialistCardWindow.Show();
                _ = LoadSpecialistAsync();
            }
        }

        public async Task LoadSpecialistAsync()
        {
            var SpecialistId = SessionManager.Instance.SpecialistID;
            SelectedSpecialist = await _specialistsRepository.GetByIdAsync(SpecialistId);

        }

        public ICommand PreviousPageCommand => new RelayCommand(GoToPreviousPage, CanGoToPreviousPage);

        private void GoToPreviousPage(object parameter)
        {
            if (currentPage > 1)
            {
                CurrentPage--;
            }
        }

        private bool CanGoToPreviousPage(object parameter) => currentPage > 1;

        public ICommand NextPageCommand => new RelayCommand(GoToNextPage, CanGoToNextPage);

        private void GoToNextPage(object parameter)
        {
            if (currentPage < TotalPages)
            {
                CurrentPage++;
            }
        }

        private bool CanGoToNextPage(object parameter) => currentPage < TotalPages;


        private async Task AddSpecialistAsync()
        {
            if (!string.IsNullOrEmpty(SelectedSpecialist.Specialist_Name))
            {
                await _specialistsRepository.AddAsync(SelectedSpecialist);
                await LoadSpecialistsAsync();
                SelectedSpecialist = new Specialists();
            }
        }

        private async Task SaveSpecialistAsync()
        {
            if (SelectedSpecialist != null && SelectedSpecialist.Specialists_ID > 0)
            {
                await _specialistsRepository.UpdateAsync(SelectedSpecialist);
                await LoadSpecialistsAsync();
                SelectedSpecialist = new Specialists();
                if (string.IsNullOrEmpty(SelectedSpecialist.Photo))
                {
                    EmptyPhoto();
                }
            }
        }

        private async Task DeleteSpecialistAsync()
        {
            if (SelectedSpecialist != null && SelectedSpecialist.Specialists_ID > 0)
            {
                await _specialistsRepository.DeleteAsync(SelectedSpecialist.Specialists_ID);
                await LoadSpecialistsAsync();
                SelectedSpecialist = new Specialists();
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
                SelectedSpecialist.Photo = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedSpecialist));
            }

        }

        private void EmptyPhoto()
        {
            SelectedSpecialist.Photo = "D:/BSTU3_1/CourseProject/Beauty_Salon/Beauty_Salon/Views/pic/no-pic.png";
            OnPropertyChanged(nameof(SelectedSpecialist));
        }

        public async Task UpdateAverageGrade(int specialistId)
        {
            using (var dbContext = new DatabaseContext()) 
            {
                var gradesRepository = new GradesRepository(dbContext);
                var grades = await gradesRepository.FindAsync(g => g.Specialists_ID == specialistId);
                if (grades != null && grades.Any())
                {
                    var specialist = Specialists.FirstOrDefault(s => s.Specialists_ID == specialistId);
                    if (specialist != null)
                    {
                        specialist.Average_Grade = (float)Math.Round(grades.Average(g => g.Grade), 1);
                        if (SelectedSpecialist.Specialists_ID == specialistId)
                        {
                            SelectedSpecialist = specialist;
                            OnPropertyChanged(nameof(SelectedSpecialist));
                            
                        }
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            OnPropertyChanged(nameof(Specialists));
                            OnPropertyChanged(nameof(PaginatedSpecialists));
                        });
                        var specialists = await _specialistsRepository.GetAllAsync();

                        await LoadSpecialistsAsync();


                    }
                }
            }
        }

    }
}

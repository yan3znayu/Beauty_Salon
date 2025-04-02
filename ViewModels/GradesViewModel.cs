using Beauty_Salon.Models;
using Beauty_Salon.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Beauty_Salon.DAL;
using Beauty_Salon.DAL.Repositories.TablesRepositories;
using System.Windows.Media;
using System.Windows;

namespace Beauty_Salon.ViewModels
{
    public class GradesViewModel : BaseViewModel
    {
        private readonly GradesRepository _gradesRepository;
        private readonly SpecialistsViewModel _specialistsViewModel;

        private ObservableCollection<Grades> _grades;
        private Grades _selectedGrade;

        private int _currentGrade; // Оценка, которую пользователь поставил через слайдер

        public GradesViewModel()
        {
            _gradesRepository = new GradesRepository(new DatabaseContext());
            LoadGradesCommand = new RelayCommand(async (param) => await LoadGradesAsync());
            DeleteGradeCommand = new RelayCommand(async (param) => await DeleteGradeAsync());
            
            SelectedGrade = new Grades();
            //CurrentGrade = 1; 
        }

        public GradesViewModel(SpecialistsViewModel specialistsViewModel)
        {
            LoadGradesCommand = new RelayCommand(async (param) => await LoadGradesAsync());
            _specialistsViewModel = specialistsViewModel;
            _gradesRepository = new GradesRepository(new DatabaseContext());
            AddOrUpdateGradeCommand = new RelayCommand(async (param) => await AddOrUpdateGradeAsync(), param => CanAddGrade());
            _ = LoadExistingGradeAsync();
            LoadGradesCommand = new RelayCommand(async (param) => await LoadGradesAsync());
        }

        public ObservableCollection<Grades> Grades
        {
            get => _grades;
            set
            {
                _grades = value;
                OnPropertyChanged(nameof(Grades));
            }
        }

        public Grades SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                OnPropertyChanged(nameof(SelectedGrade));
            }
        }

        public int CurrentGrade
        {
            get => _currentGrade;
            set
            {
                if (_currentGrade != value)
                {
                    _currentGrade = value;
                    OnPropertyChanged(nameof(CurrentGrade));
                    UpdateStarFillBrushes(CurrentGrade);
                }
            }
        }

        private ObservableCollection<Brush> _starFillBrushes;

        public ObservableCollection<Brush> StarFillBrushes
        {
            get => _starFillBrushes;
            set
            {
                _starFillBrushes = value;
                OnPropertyChanged(nameof(StarFillBrushes));

            }
        }

        public ICommand LoadGradesCommand { get; set; }
        public ICommand AddOrUpdateGradeCommand { get; set; }
        public ICommand DeleteGradeCommand { get; set; }

        private async Task LoadGradesAsync()
        {
            var grades = await _gradesRepository.GetAllAsync();
            Grades = new ObservableCollection<Grades>(grades);
        }

        private async Task AddOrUpdateGradeAsync()
        {
            var existingGrade = await _gradesRepository.FindAsync(g =>
                g.User_ID == SessionManager.Instance.UserID && g.Specialists_ID == SessionManager.Instance.SpecialistID);

            if (existingGrade.Count > 0)
            {
                var gradeToUpdate = existingGrade[0];
                gradeToUpdate.Grade = CurrentGrade;

                await _gradesRepository.UpdateAsync(gradeToUpdate);
            }
            else
            {
                var newGrade = new Grades
                {
                    User_ID = SessionManager.Instance.UserID,
                    Specialists_ID = SessionManager.Instance.SpecialistID,
                    Grade = CurrentGrade
                };

                await _gradesRepository.AddAsync(newGrade);
            }

            UpdateStarFillBrushes(CurrentGrade);

            await _specialistsViewModel.LoadSpecialistAsync();
            await LoadGradesAsync();
            await _specialistsViewModel.UpdateAverageGrade(SessionManager.Instance.SpecialistID);


        }

        private bool CanAddGrade()
        {
            return SessionManager.Instance.UserName != "Guest";
        }

        private async Task LoadExistingGradeAsync()
        {
            var existingGrade = await _gradesRepository.FindAsync(g =>
                g.User_ID == SessionManager.Instance.UserID && g.Specialists_ID == SessionManager.Instance.SpecialistID);

            if (existingGrade.Count > 0)
            {
                CurrentGrade = existingGrade[0].Grade;
            }
            else
            {
                CurrentGrade = 1;
            }

            UpdateStarFillBrushes(CurrentGrade);
        }

        private async Task DeleteGradeAsync()
        {
            if (SelectedGrade != null && SelectedGrade.Grades_ID > 0)
            {
                await _gradesRepository.DeleteAsync(SelectedGrade.Grades_ID);
                await LoadGradesAsync();
                SelectedGrade = new Grades();
            }
        }

        private void UpdateStarFillBrushes(int grade)
        {
            var brushes = new ObservableCollection<Brush>();

            // Меняем цвет в зависимости от оценки
            for (int i = 1; i <= 5; i++)
            {
                if (i <= grade)
                {
                    brushes.Add(new SolidColorBrush(Colors.Gold)); // Заполненные звезды
                }
                else
                {
                    brushes.Add(new SolidColorBrush(Colors.Gray)); // Незаполненные звезды
                }
            }

            StarFillBrushes = brushes;
        }
    }
}

using Beauty_Salon.Models;
using Beauty_Salon.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Beauty_Salon.DAL;
using Microsoft.Win32;
using Beauty_Salon.DAL.Repositories.TablesRepositories;
using System.Windows;

namespace Beauty_Salon.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        private readonly StatusesRepository _statusesRepository;
        private readonly UsersRepository _usersRepository;
        private readonly RolesRepository _rolesRepository;
        private Users _selectedUser;
        private Users _originalUser;
        private ObservableCollection<Users> _users;
        private ObservableCollection<Statuses> _statuses;
        private ObservableCollection<Roles> _roles;

        public UsersViewModel()
        {
            _usersRepository = new UsersRepository(new DatabaseContext());
            _statusesRepository = new StatusesRepository(new DatabaseContext());
            _rolesRepository = new RolesRepository(new DatabaseContext());
            LoadUsersCommand = new RelayCommand(async (param) => await LoadUsersAsync());
            AddUserCommand = new RelayCommand(async (param) => await AddUserAsync());
            SaveUserCommand = new RelayCommand(async (param) => await SaveUserAsync(), CanSaveUserAsync);
            DeleteUserCommand = new RelayCommand(async (param) => await DeleteUserAsync());
            ChooseImageCommand = new RelayCommand(param => ChooseImage());
            ResetSelectedUserCommand = new RelayCommand(async (param) => await ResetSelectedUser());
            SelectedUser = new Users();

            EmptyPhoto();
            Initialize();
        }

        private async void Initialize()
        {
            await LoadStatusesAsync();
            await LoadRolesAsync();
            await LoadUsersAsync();
        }

        public ObservableCollection<Users> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }


        public Users SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public Users OriginalUser
        {
            get => _originalUser;
            set
            {
                _originalUser = value;
                OnPropertyChanged(nameof(OriginalUser));
            }
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

        public ObservableCollection<Roles> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }

        public ICommand LoadUsersCommand { get; set; }
        public ICommand AddUserCommand { get; set; }
        public ICommand SaveUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }
        public ICommand ResetSelectedUserCommand { get; set; }
        private async Task ResetSelectedUser()
        {
            if (SelectedUser != null)
            {
                using (var context = new DatabaseContext())
                {
                    _originalUser = context.Users.FirstOrDefault(u => u.User_ID == SelectedUser.User_ID);
                }

                if (_originalUser != null)
                {
                    SelectedUser.User_ID = _originalUser.User_ID;
                    SelectedUser.User_Name = _originalUser.User_Name;
                    SelectedUser.User_Fullname = _originalUser.User_Fullname;
                    SelectedUser.Email = _originalUser.Email;
                    SelectedUser.Phone = _originalUser.Phone;
                    SelectedUser.Description = _originalUser.Description;
                    SelectedUser.Status_ID = _originalUser.Status_ID;
                    if(!string.IsNullOrEmpty(_originalUser.Image))
                    {
                        SelectedUser.Image = _originalUser.Image;
                    }
                    else
                        EmptyPhoto();
                    

                    await LoadUsersAsync();
                }
            }
        }

        private async Task LoadUsersAsync()
        {
            var users = await _usersRepository.GetAllAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Users = new ObservableCollection<Users>(users);
            });
           
        }

        private async Task LoadStatusesAsync()
        {
            var statuses = await _statusesRepository.GetAllAsync();
            Statuses = new ObservableCollection<Statuses>(statuses);
        }

        private async Task LoadRolesAsync()
        {
            var roles = await _rolesRepository.GetAllAsync();
            Roles = new ObservableCollection<Roles>(roles);
        }


        private async Task AddUserAsync()
        {
            if (!string.IsNullOrEmpty(SelectedUser.User_Name) &&
                !string.IsNullOrEmpty(SelectedUser.Password_Hash) &&
                SelectedUser.Status_ID > 0)
            {
                if (await InputValidationClass.UserNameExistsInDatabaseAsync(SelectedUser.User_Name))
                {
                    MessageBox.Show("Логин уже существует в базе данных.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var salt = _usersRepository.GenerateSalt();
                SelectedUser.Password_Hash = _usersRepository.HashPassword(SelectedUser.Password_Hash, salt);
                SelectedUser.Password_Salt = salt;
                SelectedUser.Create_Date = DateTime.Now.ToUniversalTime();
                await _usersRepository.AddAsync(SelectedUser);
                //await LoadUsersAsync();
                SelectedUser = new Users();
                MessageBox.Show("Add");
            }
        }

        private bool CanSaveUserAsync(object parameter)
        {
            if (SelectedUser == null)
                return false;


            if (string.IsNullOrWhiteSpace(SelectedUser.User_Name) ||
                string.IsNullOrWhiteSpace(SelectedUser.Password_Hash))
                return false;


            return true;
        }

        private async Task SaveUserAsync()
        {
            if (SelectedUser != null && SelectedUser.User_ID > 0)
            {
                using (var context = new DatabaseContext())
                {
                    var originalUser = context.Users.FirstOrDefault(u => u.User_ID == SelectedUser.User_ID);

                    if (!string.Equals(originalUser.User_Name, SelectedUser.User_Name, StringComparison.OrdinalIgnoreCase) &&
                        await InputValidationClass.UserNameExistsInDatabaseAsync(SelectedUser.User_Name))
                    {
                        MessageBox.Show("Логин уже существует в базе данных.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (!string.IsNullOrEmpty(SelectedUser.Password_Hash) &&
                        !string.Equals(originalUser.Password_Hash, SelectedUser.Password_Hash, StringComparison.OrdinalIgnoreCase))
                    {
                        var salt = _usersRepository.GenerateSalt();
                        SelectedUser.Password_Hash = _usersRepository.HashPassword(SelectedUser.Password_Hash, salt);
                        SelectedUser.Password_Salt = salt;
                    }
                }
                    

                await _usersRepository.UpdateAsync(SelectedUser);

                MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                SelectedUser = new Users();
                if (string.IsNullOrEmpty(SelectedUser.Image))
                {
                    EmptyPhoto();
                }
            }
        }


        private async Task DeleteUserAsync()
        {
            if (SelectedUser != null && SelectedUser.User_ID > 0)
            {
                await _usersRepository.DeleteAsync(SelectedUser.User_ID);
                await LoadUsersAsync();
                SelectedUser = new Users();
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
                SelectedUser.Image = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        private void EmptyPhoto()
        {
            SelectedUser.Image = "D:/BSTU3_1/CourseProject/Beauty_Salon/Beauty_Salon/Views/pic/no-pic.png";
            OnPropertyChanged(nameof(SelectedUser));

        }
    }
}

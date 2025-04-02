using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Beauty_Salon.Commands;
using Beauty_Salon.DAL;
using Beauty_Salon.DAL.Repositories.TablesRepositories;
using Beauty_Salon.Models;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Beauty_Salon.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        //public string Image => SessionManager.Instance.Image;
        public string Login => SessionManager.Instance.UserName;

        private readonly UsersRepository _usersRepository;
        private Users _currentUser;
        private Users _newUser;
        private ObservableCollection<Users> _users;

        public UserProfileViewModel()
        {
            CurrentUser = new Users();
            _usersRepository = new UsersRepository(new DatabaseContext());
            DeleteUserCommand = new RelayCommand(async (param) => await DeleteUserAsync());
            LoadUsersInfoCommand = new RelayCommand(async (param) => await LoadUsersInfoAsync());
            SaveUsersInfoCommand = new RelayCommand(async (param) => await SaveUserInfoAsync());
            ToggleReadOnlyCommand = new RelayCommand(ExecuteToggleReadOnly);
            ChooseImageCommand = new RelayCommand(param => ChooseImage());
            _ = LoadUsersInfoAsync();
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

        public Users CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public ICommand LoadUsersInfoCommand { get; set; }
        public ICommand SaveUsersInfoCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand ToggleReadOnlyCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }
        private async Task LoadUsersInfoAsync()
        {
            var user = await _usersRepository.GetUserByUserNameAsync(Login);

            if (user != null)
            {
                CurrentUser = new Users()
                {
                    User_Name = user.User_Name,
                    User_Fullname = user.User_Fullname,
                    Email = user.Email,
                    Phone = user.Phone,
                    Description = user.Description,
                    Create_Date = user.Create_Date.Date,
                    Image = user.Image,

                    Role_ID = user.Role_ID,
                    Status_ID = user.Status_ID,
                    User_ID = user.User_ID,
                    Password_Hash = user.Password_Hash,
                    Password_Salt = user.Password_Salt,
                };
            }
        }

        private async Task SaveUserInfoAsync()
        {
            try
            {
                var originalUser = await _usersRepository.GetUserByUserNameAsync(SessionManager.Instance.UserName);

                if (!string.Equals(CurrentUser.Email, originalUser.Email, StringComparison.OrdinalIgnoreCase))
                {
                    if (!await InputValidationClass.ValidateEmailAsync(CurrentUser.Email))
                    {
                        await ResetCurrentUserAsync();
                        return;
                    }
                }

                if (!string.Equals(CurrentUser.User_Name, originalUser.User_Name, StringComparison.OrdinalIgnoreCase))
                {
                    if (await InputValidationClass.UserNameExistsInDatabaseAsync(CurrentUser.User_Name))
                    {
                        MessageBox.Show("Логин уже существует в базе данных.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                        await ResetCurrentUserAsync();
                        return;
                    }
                }

                if (!string.Equals(CurrentUser.Phone, originalUser.Phone, StringComparison.OrdinalIgnoreCase))
                {
                    if (!await InputValidationClass.ValidatePhoneNumberAsync(CurrentUser.Phone))
                    {
                        await ResetCurrentUserAsync();
                        return;
                    }

                    var phoneExists = await _usersRepository.FindAsync(u => u.Phone == CurrentUser.Phone && u.User_ID != CurrentUser.User_ID);
                    if (phoneExists.Any())
                    {
                        MessageBox.Show("Данный номер телефона уже используется другим пользователем. Пожалуйста, выберите другой номер.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                        await ResetCurrentUserAsync();
                        return;
                    }
                }

                using (var context = new DatabaseContext())
                {
                    var repository = new UsersRepository(context);
                    await repository.UpdateAsync(CurrentUser);
                }

                MessageBox.Show("Данные успешно обновлены!");

                await ResetCurrentUserAsync();
                await LoadUsersInfoAsync();

                SessionManager.Instance.UserName = CurrentUser.User_Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                // Обновляем данные в интерфейсе
                await ResetCurrentUserAsync();
            }
        }

        private async Task ResetCurrentUserAsync()
        {
            var originalUser = await _usersRepository.GetUserByUserNameAsync(SessionManager.Instance.UserName);

            if (originalUser != null)
            {
                CurrentUser = new Users
                {
                    User_Name = originalUser.User_Name,
                    Email = originalUser.Email,
                    Phone = originalUser.Phone,
                    User_Fullname = originalUser.User_Fullname,
                    Description = originalUser.Description,
                    Image = originalUser.Image,
                    Role_ID = originalUser.Role_ID,
                    Status_ID = originalUser.Status_ID,
                    User_ID = originalUser.User_ID,
                    Password_Hash = originalUser.Password_Hash,
                    Password_Salt = originalUser.Password_Salt,
                    Create_Date = originalUser.Create_Date.Date,
                };

                OnPropertyChanged(nameof(CurrentUser));
            }
        }





        private async Task DeleteUserAsync()
        {
            if (CurrentUser != null && CurrentUser.User_ID > 0)
            {
                await _usersRepository.DeleteAsync(CurrentUser.User_ID);
                CurrentUser = new Users();
            }
        }

        private void ExecuteToggleReadOnly(object parameter)
        {
            if (parameter is TextBox textBox)
            {
                textBox.IsReadOnly = false;
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
                CurrentUser.Image = openFileDialog.FileName;
                OnPropertyChanged(nameof(CurrentUser));
                SessionManager.Instance.Image = openFileDialog.FileName;
            }
        }


    }
}

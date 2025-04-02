using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Text;
using Beauty_Salon.DAL;
using Beauty_Salon.Models;
using Beauty_Salon.Commands;
using Beauty_Salon.Views;
using Beauty_Salon.DAL.Repositories.TablesRepositories;

namespace Beauty_Salon.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private string _fio;
        private string _login;
        private string _email;
        private string _password;
        private string _repeatPassword;
        private string _passwordError;
        private string _loginError;
        private string _emailError;
        private string _repeatPasswordError;

        private readonly UsersRepository _usersRepository;
        private Users? _user;

        public Users? User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        public string FIO
        {
            get => _fio;
            set { _fio = value; OnPropertyChanged(nameof(FIO)); }
        }

        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(nameof(Login)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); ValidateEmail(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); ValidatePassword(); }
        }

        public string RepeatPassword
        {
            get => _repeatPassword;
            set { _repeatPassword = value; OnPropertyChanged(nameof(RepeatPassword)); ValidatePassword(); }
        }

        public string PasswordError
        {
            get => _passwordError;
            set { _passwordError = value; OnPropertyChanged(nameof(PasswordError)); }
        }

        public string LoginError
        {
            get => _loginError;
            set { _loginError = value; OnPropertyChanged(nameof(LoginError)); }
        }

        public string EmailError
        {
            get => _emailError;
            set { _emailError = value; OnPropertyChanged(nameof(EmailError)); }
        }

        public ICommand SignUpCommand { get; }

        public SignUpViewModel()
        {
            SignUpCommand = new RelayCommand(ExecuteSignUp, CanExecuteSignUp);
            _usersRepository = new UsersRepository(new DatabaseContext());
        }

        private bool CanExecuteSignUp(object parameter) =>
            !string.IsNullOrWhiteSpace(FIO) &&
            !string.IsNullOrWhiteSpace(Login) &&
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(RepeatPassword) &&
            IsValidEmail() &&
            isValidatePassword() &&
            Password == RepeatPassword;

        private void ExecuteSignUp(object parameter)
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    if (context.Users.Any(u => u.User_Name == Login))
                    {
                        LoginError = "Пользователь с таким логином уже существует.";
                        return;
                    }

                    if (context.Users.Any(u => u.Email == Email))
                    {
                        EmailError = "Пользователь с таким email уже существует.";
                        return;
                    }

                    var password_salt = _usersRepository.GenerateSalt();

                    var newUser = new Users
                    {
                        User_Name = Login,
                        User_Fullname = FIO,
                        Email = Email,
                        Password_Salt = password_salt,
                        Password_Hash = _usersRepository.HashPassword(Password, password_salt),
                        Phone = "",
                        Description = "",
                        Image = null,
                        Role_ID = 6,
                        Status_ID = 2,
                        Create_Date = DateTime.UtcNow,

                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();

                    SessionManager.Instance.Image = null;
                    SessionManager.Instance.UserID = GetUserID();
                }
                SessionManager.Instance.UserName = Login;
                //MessageBox.Show("Пользователь добавлен!");
                new MenuWindow().Show();

                if (parameter is Window currentWindow)
                    currentWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}");
            }
        }

        private void ValidateEmail()
        {
            EmailError = string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                ? "Неправильный формат email."
                : string.Empty;
        }

        private bool IsValidEmail() => string.IsNullOrEmpty(EmailError);

        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
                PasswordError = "Пароль должен быть не менее 8 символов.";
            else if (!Password.Any(char.IsUpper))
                PasswordError = "Пароль должен содержать хотя бы одну заглавную букву.";
            else if (!Password.Any(char.IsDigit))
                PasswordError = "Пароль должен содержать хотя бы одну цифру.";
            else if (!Password.Any(ch => "!@#$%^&*()-_=+[]{}|;:'\",.<>?/`~".Contains(ch)))
                PasswordError = "Пароль должен содержать хотя бы один специальный символ.";
            else if (Password != RepeatPassword)
                PasswordError = "Пароли не совпадают.";
            else
                PasswordError = string.Empty;
        }

        private bool isValidatePassword() => string.IsNullOrEmpty(PasswordError);

        private int GetUserID()
        {
            using (var context = new DatabaseContext())
            {
                User = context.Users.FirstOrDefault(u => u.User_Name == Login) ?? new Users();

            }
            return User.User_ID;

        }


    }
}

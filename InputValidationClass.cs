using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Beauty_Salon.DAL.Repositories.TablesRepositories;
using Beauty_Salon.DAL;

namespace Beauty_Salon
{
    public class InputValidationClass
    {
        private static readonly UsersRepository _usersRepository = new UsersRepository(new DatabaseContext());

        public InputValidationClass() { }

        public static async Task<bool> ValidateEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email не может быть пустым.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(email))
            {
                MessageBox.Show("Неправильный формат email.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (await EmailExistsInDatabaseAsync(email))
            {
                MessageBox.Show("Email уже существует в базе данных.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public static async Task<bool> ValidatePhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Номер телефона не может быть пустым.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var phoneRegex = new Regex(@"^(\+375|80)\s?\(?\d{2}\)?\s?\d{3}[- ]?\d{2}[- ]?\d{2}$");
            if (!phoneRegex.IsMatch(phoneNumber))
            {
                MessageBox.Show("Неправильный формат номера телефона. Пример: +375 (XX) XXX-XX-XX\r\n", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (await PhoneNumberExistsInDatabaseAsync(phoneNumber))
            {
                MessageBox.Show("Номер телефона уже существует в базе данных.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public static async Task<bool> EmailExistsInDatabaseAsync(string email)
        {
            var user = await _usersRepository.FindAsync(u => u.Email == email);
            return user != null && user.Count > 0;
        }

        public static async Task<bool> PhoneNumberExistsInDatabaseAsync(string phoneNumber)
        {
            var user = await _usersRepository.FindAsync(u => u.Phone == phoneNumber);
            return user != null && user.Count > 0;
        }

        public static async Task<bool> UserNameExistsInDatabaseAsync(string userName)
        {
            var user = await _usersRepository.FindAsync(u => u.User_Name == userName);
            return user != null && user.Count > 0;
        }
    }
}

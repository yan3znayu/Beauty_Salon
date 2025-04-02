using Beauty_Salon.Models;
using Beauty_Salon.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Beauty_Salon.DAL.Repositories.TablesRepositories
{
    public class UsersRepository : IRepository<Users>
    {
        private readonly IRepository<Users> _genericRepository;

        public UsersRepository(DatabaseContext context)
        {
            _genericRepository = new GenericRepository<Users>(context);
        }

        public async Task<List<Users>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<Users> GetByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Users user)
        {
            await _genericRepository.AddAsync(user);
        }

        public async Task UpdateAsync(Users user)
        {
            await _genericRepository.UpdateAsync(user);
        }


        public async Task DeleteAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<List<Users>> FindAsync(System.Linq.Expressions.Expression<Func<Users, bool>> predicate)
        {
            return await _genericRepository.FindAsync(predicate);
        }
        public async Task<string?> GetUserImageByUserNameAsync(string userName)
        {
            var users = await _genericRepository.FindAsync(u => u.User_Name == userName);
            var user = users.FirstOrDefault();
            return user?.Image;
        }
        public async Task<Users?> GetUserByUserNameAsync(string userName)
        {
            using (var context = new DatabaseContext())
            {
                return await context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.User_Name == userName);
            }

        }

        public string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var saltedPassword = password + salt;
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hashBytes);
        }
    }
}

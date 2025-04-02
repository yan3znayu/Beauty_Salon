using Beauty_Salon.Models;
using Beauty_Salon.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Salon.DAL.Repositories.TablesRepositories
{
    public class FavoritesRepository : IRepository<Favorites>
    {
        private readonly IRepository<Favorites> _genericRepository;

        public FavoritesRepository(DatabaseContext context)
        {
            _genericRepository = new GenericRepository<Favorites>(context);
        }

        public async Task<List<Favorites>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<Favorites> GetByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Favorites favorite)
        {
            await _genericRepository.AddAsync(favorite);
        }

        public async Task UpdateAsync(Favorites favorite)
        {
            await _genericRepository.UpdateAsync(favorite);
        }

        public async Task DeleteAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<List<Favorites>> FindAsync(System.Linq.Expressions.Expression<Func<Favorites, bool>> predicate)
        {
            return await _genericRepository.FindAsync(predicate);
        }

        public async Task<Favorites> GetFavoriteAsync(int userId, int serviceId)
        {
            return (await FindAsync(f => f.User_ID == userId && f.Service_ID == serviceId)).FirstOrDefault();
        }
    }
}

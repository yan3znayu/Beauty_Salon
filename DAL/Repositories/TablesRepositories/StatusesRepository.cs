using Beauty_Salon.Models;
using Beauty_Salon.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beauty_Salon.DAL.Repositories.TablesRepositories
{
    public class StatusesRepository : IRepository<Statuses>
    {
        private readonly IRepository<Statuses> _genericRepository;

        public StatusesRepository(DatabaseContext context)
        {
            _genericRepository = new GenericRepository<Statuses>(context);
        }

        public async Task<List<Statuses>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<Statuses> GetByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Statuses statuse)
        {
            await _genericRepository.AddAsync(statuse);
        }

        public async Task UpdateAsync(Statuses statuse)
        {
            await _genericRepository.UpdateAsync(statuse);
        }

        public async Task DeleteAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<List<Statuses>> FindAsync(System.Linq.Expressions.Expression<Func<Statuses, bool>> predicate)
        {
            return await _genericRepository.FindAsync(predicate);
        }
    }
}

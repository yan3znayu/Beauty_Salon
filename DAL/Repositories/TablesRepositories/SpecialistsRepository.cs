using Beauty_Salon.Models;
using Beauty_Salon.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beauty_Salon.DAL.Repositories.TablesRepositories
{
    public class SpecialistsRepository : IRepository<Specialists>
    {
        private readonly IRepository<Specialists> _genericRepository;

        public SpecialistsRepository(DatabaseContext context)
        {
            _genericRepository = new GenericRepository<Specialists>(context);
        }

        public async Task<List<Specialists>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<Specialists> GetByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Specialists specialist)
        {
            await _genericRepository.AddAsync(specialist);
        }

        public async Task UpdateAsync(Specialists specialist)
        {
            await _genericRepository.UpdateAsync(specialist);
        }

        public async Task DeleteAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<List<Specialists>> FindAsync(System.Linq.Expressions.Expression<Func<Specialists, bool>> predicate)
        {
            return await _genericRepository.FindAsync(predicate);
        }
    }
}

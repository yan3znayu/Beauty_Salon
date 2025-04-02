using Beauty_Salon.Models;
using Beauty_Salon.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beauty_Salon.DAL.Repositories.TablesRepositories
{
    public class ServicesRepository : IRepository<Services>
    {
        private readonly IRepository<Services> _genericRepository;

        public ServicesRepository(DatabaseContext context)
        {
            _genericRepository = new GenericRepository<Services>(context);
        }

        public async Task<List<Services>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<Services> GetByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Services service)
        {
            await _genericRepository.AddAsync(service);
        }

        public async Task UpdateAsync(Services service)
        {
            await _genericRepository.UpdateAsync(service);
        }

        public async Task DeleteAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<List<Services>> FindAsync(System.Linq.Expressions.Expression<Func<Services, bool>> predicate)
        {
            return await _genericRepository.FindAsync(predicate);
        }
    }
}

using Beauty_Salon.Models;
using Beauty_Salon.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beauty_Salon.DAL.Repositories.TablesRepositories
{
    public class RolesRepository : IRepository<Roles>
    {
        private readonly IRepository<Roles> _genericRepository;

        public RolesRepository(DatabaseContext context)
        {
            _genericRepository = new GenericRepository<Roles>(context);
        }

        public async Task<List<Roles>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<Roles> GetByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Roles role)
        {
            await _genericRepository.AddAsync(role);
        }

        public async Task UpdateAsync(Roles role)
        {
            await _genericRepository.UpdateAsync(role);
        }

        public async Task DeleteAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<List<Roles>> FindAsync(System.Linq.Expressions.Expression<Func<Roles, bool>> predicate)
        {
            return await _genericRepository.FindAsync(predicate);
        }
    }
}

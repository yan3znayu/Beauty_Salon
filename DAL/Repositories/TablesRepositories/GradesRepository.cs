using Beauty_Salon.Models;
using Beauty_Salon.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beauty_Salon.DAL.Repositories.TablesRepositories
{
    public class GradesRepository : IRepository<Grades>
    {
        private readonly IRepository<Grades> _genericRepository;

        public GradesRepository(DatabaseContext context)
        {
            _genericRepository = new GenericRepository<Grades>(context);
        }

        public async Task<List<Grades>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<Grades> GetByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Grades grade)
        {
            await _genericRepository.AddAsync(grade);
        }

        public async Task UpdateAsync(Grades grade)
        {
            await _genericRepository.UpdateAsync(grade);
        }

        public async Task DeleteAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<List<Grades>> FindAsync(System.Linq.Expressions.Expression<Func<Grades, bool>> predicate)
        {
            return await _genericRepository.FindAsync(predicate);
        }
    }
}

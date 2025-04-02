using Beauty_Salon.Models;
using Beauty_Salon.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Beauty_Salon.DAL.Repositories.TablesRepositories
{
    public class ReservationsRepository : IRepository<Reservations>
    {
        private readonly IRepository<Reservations> _genericRepository;


        public ReservationsRepository(DatabaseContext context)
        {
            _genericRepository = new GenericRepository<Reservations>(context);
        }

        public async Task<List<Reservations>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public async Task<Reservations> GetByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Reservations reservation)
        {
            await _genericRepository.AddAsync(reservation);
        }

        public async Task UpdateAsync(Reservations reservation)
        {
            await _genericRepository.UpdateAsync(reservation);
        }

        public async Task DeleteAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<List<Reservations>> FindAsync(System.Linq.Expressions.Expression<Func<Reservations, bool>> predicate)
        {
            return await _genericRepository.FindAsync(predicate);
        }

    }
}

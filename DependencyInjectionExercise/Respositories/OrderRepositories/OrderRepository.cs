using DependencyInjectionExercise.Data;
using DependencyInjectionExercise.Models;
using Microsoft.EntityFrameworkCore;

namespace DependencyInjectionExercise.Respositories.OrderRepositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(BookStoreContext context) : base(context)
        {            
        }

        public async Task<IEnumerable<Order>> GetAsync()
        {
            return await _dbSet.OrderByDescending(o => o.OrderDate).ToListAsync(); 
        }
    }
}

using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Respositories.OrderRepositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetAsync();
    }
}

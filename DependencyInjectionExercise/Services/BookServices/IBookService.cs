using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services.BookServices
{
    public interface IBookService
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetAsync(int id);
        Task<List<Book>> GetByCategoryAsync(string category);

        Task<Book> CreateAsync(Book book);

        Task UpdateStockAsync(int id, int quantity);
    }
}

using DependencyInjectionExercise.Data;
using DependencyInjectionExercise.Models;
using Microsoft.EntityFrameworkCore;

namespace DependencyInjectionExercise.Respositories.BookRepositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(BookStoreContext context) : base(context)
        {
        }
    }
}

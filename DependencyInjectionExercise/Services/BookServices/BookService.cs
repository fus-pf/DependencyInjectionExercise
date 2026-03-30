using DependencyInjectionExercise.Models;
using DependencyInjectionExercise.Respositories;
using DependencyInjectionExercise.Respositories.BookRepositories;

namespace DependencyInjectionExercise.Services.BookServices
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            var result = await _bookRepository.GetAsync();
            return result.ToList();
        }
        public async Task<Book?> GetAsync(int id)
        {
            var result = await _bookRepository.GetOneAsync(b => b.Id == id);
            return result;
        }
        public async Task<List<Book>> GetByCategoryAsync(string category)
        {
            if (category != "fiction" && category != "non-fiction")
                throw new ArgumentException("Category must be 'fiction' or 'non-fiction'");

            var result = await _bookRepository.GetAsync(b => b.Category == category);
            return result.ToList();
        }

        public async Task<Book> CreateAsync(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Title is required");
            if (book.Price <= 0)
                throw new ArgumentException("Price must be greater than zero");
            if (book.Stock < 0)
                throw new ArgumentException("Stock cannot be negative");

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            return book;
        }

        public async Task UpdateStockAsync(int id, int quantity)
        {
            var book = await _bookRepository.GetOneAsync(b => b.Id == id);

            if (book == null)
                throw new KeyNotFoundException("Book not found");
            if (book.Stock + quantity < 0)
                throw new InvalidOperationException("Not enough stock");

            book.Stock += quantity;
            await _bookRepository.SaveChangesAsync();
        }
    }
}

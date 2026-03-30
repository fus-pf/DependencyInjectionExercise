using DependencyInjectionExercise.Data;
using DependencyInjectionExercise.Models;
using DependencyInjectionExercise.Services.BookServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DependencyInjectionExercise.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetBooks()
    {
        return await _bookService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await _bookService.GetAsync(id);
        if (book == null) return NotFound();
        return book;
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<List<Book>>> GetBooksByCategory(string category)
    {       
        return await _bookService.GetByCategoryAsync(category);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        var result = await _bookService.CreateAsync(book);
        return CreatedAtAction(nameof(GetBook), new { id = result.Id }, result);
    }

    [HttpPut("{id}/stock")]
    public async Task<IActionResult> UpdateStock(int id, [FromBody] int quantity)
    {
        await _bookService.UpdateStockAsync(id, quantity);
        return NoContent();
    }
}

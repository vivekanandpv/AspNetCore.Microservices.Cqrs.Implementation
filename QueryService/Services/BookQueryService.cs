using Microsoft.EntityFrameworkCore;
using QueryService.DataAccess;
using QueryService.Models;

namespace QueryService.Services;

class BookQueryService : IBookQueryService
{
    private readonly BookContext _context;

    public BookQueryService(BookContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        return await _context.Books.FirstAsync(b => b.BookId == id);
    }
}
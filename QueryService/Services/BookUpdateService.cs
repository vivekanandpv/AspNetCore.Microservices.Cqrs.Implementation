using CommandService.ViewModels;
using Microsoft.EntityFrameworkCore;
using QueryService.DataAccess;
using QueryService.Models;

namespace QueryService.Services;

class BookUpdateService : IBookUpdateService
{
    private readonly BookContext _context;

    public BookUpdateService(BookContext context)
    {
        _context = context;
    }

    public async Task UpdateAsync(BookMessageViewModel viewModel)
    {
        throw new NotImplementedException();
    }

    private async Task CreateFromMessageAsync(BookMessageViewModel viewModel)
    {
        var book = new Book
        {
            BookId = viewModel.BookId,
            Pages = viewModel.Pages.Value,
            Price = viewModel.Price.Value,
            Title = viewModel.Title,
            PricePerPage = viewModel.Pages.Value / viewModel.Price.Value
        };

        await _context.Books.AddAsync(book);

        await _context.SaveChangesAsync();
    }

    private async Task UpdateFromMessageAsync(BookMessageViewModel viewModel)
    {
        var bookDb = await GetSingleAsync(viewModel.BookId);

        if (bookDb == null)
        {
            return;
        }

        bookDb.Pages = viewModel.Pages.Value;
        bookDb.Price = viewModel.Price.Value;
        bookDb.Title = viewModel.Title;
        bookDb.PricePerPage = viewModel.Pages.Value / viewModel.Pages.Value;

        await _context.SaveChangesAsync();
    }

    private async Task DeleteFromMessageAsync(BookMessageViewModel viewModel)
    {
        var bookDb = await GetSingleAsync(viewModel.BookId);

        if (bookDb == null)
        {
            return;
        }

        _context.Books.Remove(bookDb);

        await _context.SaveChangesAsync();
    }

    private async Task<Book?> GetSingleAsync(int id)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
    }
}
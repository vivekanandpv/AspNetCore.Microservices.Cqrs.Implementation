using CommandService.ViewModels;
using Microsoft.EntityFrameworkCore;
using QueryService.DataAccess;
using QueryService.Models;

namespace QueryService.Services;

class BookUpdateService : IBookUpdateService
{
    private readonly BookUpdateContext _context;

    public BookUpdateService(BookUpdateContext context)
    {
        _context = context;
    }

    public async Task UpdateAsync(BookMessageViewModel viewModel)
    {
        switch (viewModel.CommandType)
        {
            case CommandType.Create:
            {
                await CreateFromMessageAsync(viewModel);
                return;
            }
            case CommandType.Update:
            {
                await UpdateFromMessageAsync(viewModel);
                return;
            }
            case CommandType.Delete:
            {
                await DeleteFromMessageAsync(viewModel);
                return;
            }
            default:
            {
                return;
            }
                
        }
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
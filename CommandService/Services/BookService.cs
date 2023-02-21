using CommandService.DataAccess;
using CommandService.Models;
using CommandService.MQ;
using CommandService.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Services;

class BookService : IBookService
{
    private readonly BookContext _context;
    private readonly IRabbitMQManager _rabbitMqManager;

    public BookService(BookContext context, IRabbitMQManager rabbitMqManager)
    {
        _context = context;
        _rabbitMqManager = rabbitMqManager;
    }

    public async Task CreateAsync(BookCreateViewModel viewModel)
    {
        var book = new Book
        {
            Title = viewModel.Title,
            Pages = viewModel.Pages,
            Price = viewModel.Price
        };

        await _context.AddAsync(book);

        await _context.SaveChangesAsync();

        var message = new BookMessageViewModel
        {
            CommandType = CommandType.Create,
            Pages = book.Pages,
            Price = book.Price,
            Title = book.Title,
            BookId = book.BookId
        };
        
        _rabbitMqManager.Publish(message, "ms-exchange", "topic", "cqrs");
    }

    public async Task UpdateAsync(int id, BookUpdateViewModel viewModel)
    {
        var book = await GetOneById(id);

        book.Pages = viewModel.Pages;
        book.Title = viewModel.Title;
        book.Price = viewModel.Price;

        await _context.SaveChangesAsync();
        
        var message = new BookMessageViewModel
        {
            CommandType = CommandType.Update,
            Pages = book.Pages,
            Price = book.Price,
            Title = book.Title,
            BookId = book.BookId
        };
        
        _rabbitMqManager.Publish(message, "ms-exchange", "topic", "cqrs");
    }

    public async Task DeleteAsync(int id)
    {
        var book = await GetOneById(id);

        _context.Books.Remove(book);

        await _context.SaveChangesAsync();
        
        var message = new BookMessageViewModel
        {
            CommandType = CommandType.Delete,
            BookId = book.BookId
        };
        
        _rabbitMqManager.Publish(message, "ms-exchange", "topic", "cqrs");
    }

    private async Task<Book> GetOneById(int id)
    {
        var bookDb = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

        if (bookDb == null)
        {
            throw new Exception("Could not find");
        }

        return bookDb;
    }
}
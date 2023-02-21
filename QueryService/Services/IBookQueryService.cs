using QueryService.Models;

namespace QueryService.Services;

public interface IBookQueryService
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book> GetByIdAsync(int id);
}
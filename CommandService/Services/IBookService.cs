using CommandService.ViewModels;

namespace CommandService.Services;

public interface IBookService
{
    Task CreateAsync(BookCreateViewModel viewModel);
    Task UpdateAsync(int id, BookUpdateViewModel viewModel);
    Task DeleteAsync(int id);
}
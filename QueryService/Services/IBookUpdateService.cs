using CommandService.ViewModels;

namespace QueryService.Services;

public interface IBookUpdateService
{
    Task UpdateAsync(BookMessageViewModel viewModel);
}
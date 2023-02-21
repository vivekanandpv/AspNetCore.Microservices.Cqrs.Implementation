using CommandService.Services;
using CommandService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(BookCreateViewModel viewModel)
    {
        await _bookService.CreateAsync(viewModel);
        return Ok();
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BookUpdateViewModel viewModel)
    {
        await _bookService.UpdateAsync(id, viewModel);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _bookService.DeleteAsync(id);
        return Ok();
    }
}
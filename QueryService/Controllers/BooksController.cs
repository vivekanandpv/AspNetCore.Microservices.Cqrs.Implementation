using Microsoft.AspNetCore.Mvc;
using QueryService.Models;
using QueryService.Services;

namespace QueryService.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BooksController: ControllerBase
{
    private readonly IBookQueryService _queryService;

    public BooksController(IBookQueryService queryService)
    {
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAsync()
    {
        return Ok(await _queryService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetByIdAsync(int id)
    {
        return Ok(await _queryService.GetByIdAsync(id));
    }
}
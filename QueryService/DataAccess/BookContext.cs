using Microsoft.EntityFrameworkCore;
using QueryService.Models;

namespace QueryService.DataAccess;

public class BookContext : DbContext
{
    public BookContext(DbContextOptions<BookContext> options) : base(options)
    {
        
    }

    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .ToTable(nameof(Book));
    }
}
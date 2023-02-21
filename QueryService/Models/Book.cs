namespace QueryService.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public int Pages { get; set; }
    public double Price { get; set; }
    public double PricePerPage { get; set; }
}
namespace CommandService.ViewModels;

public class BookMessageViewModel
{
    public CommandType CommandType { get; set; }
    public int BookId { get; set; }
    public string Title { get; set; }
    public int? Pages { get; set; }
    public double? Price { get; set; }
}
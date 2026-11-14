namespace DotNetBookstore.Models;

public class Book
{
    public int BookId { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}

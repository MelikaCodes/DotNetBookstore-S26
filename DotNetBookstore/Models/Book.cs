namespace DotNetBookstore.Models;

public class Book
{
    public int BookId { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public int PriceCents { get; set; }
    public string PriceLabel => FormatPrice(PriceCents);

    public static string FormatPrice(long cents) => FormattableString.Invariant($"CAD ${cents / 100m:0.00}");

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}

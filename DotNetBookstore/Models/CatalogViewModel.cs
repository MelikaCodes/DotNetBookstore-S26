namespace DotNetBookstore.Models;

public record CatalogViewModel(Category? Category, string Search, IReadOnlyList<Book> Books);

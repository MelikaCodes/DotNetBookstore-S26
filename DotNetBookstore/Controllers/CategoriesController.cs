using DotNetBookstore.Data;
using DotNetBookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetBookstore.Controllers;

public class CategoriesController(ApplicationDbContext database) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await database.Categories.AsNoTracking()
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken));
    }

    public async Task<IActionResult> Browse(string? category, string? query, CancellationToken cancellationToken)
    {
        var books = database.Books.AsNoTracking().Include(book => book.Category).AsQueryable();
        Category? selectedCategory = null;

        if (!string.IsNullOrWhiteSpace(category))
        {
            var categoryName = category.Trim();
            selectedCategory = await database.Categories.AsNoTracking()
                .SingleOrDefaultAsync(item => item.Name == categoryName, cancellationToken);

            if (selectedCategory is null)
            {
                return NotFound();
            }

            books = books.Where(book => book.CategoryId == selectedCategory.CategoryId);
        }

        var search = query?.Trim() ?? string.Empty;
        if (search.Length > 0)
        {
            var term = search.ToLowerInvariant();
            books = books.Where(book => book.Title.ToLower().Contains(term) ||
                book.Author.ToLower().Contains(term));
        }

        var results = await books.OrderBy(book => book.Title).ToListAsync(cancellationToken);
        return View(new CatalogViewModel(selectedCategory, search, results));
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var book = await database.Books.AsNoTracking().Include(item => item.Category)
            .SingleOrDefaultAsync(item => item.BookId == id, cancellationToken);

        return book is null ? NotFound() : View(book);
    }
}

using DotNetBookstore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetBookstore.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Book> Books => Set<Book>();

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Order>().HasIndex(order => new { order.CustomerId, order.CheckoutId }).IsUnique();

        builder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Fiction" },
            new Category { CategoryId = 2, Name = "Science Fiction" },
            new Category { CategoryId = 3, Name = "History" });

        builder.Entity<Book>().HasData(
            new Book { BookId = 1, PriceCents = 1299, CategoryId = 1, Title = "Pride and Prejudice", Author = "Jane Austen",
                Description = "Elizabeth Bennet reconsiders her first impressions of Mr. Darcy as family expectations put both of them to the test." },
            new Book { BookId = 2, PriceCents = 1099, CategoryId = 1, Title = "Little Women", Author = "Louisa May Alcott",
                Description = "Four sisters grow up during the American Civil War and find different ways to pursue the lives they want." },
            new Book { BookId = 3, PriceCents = 999, CategoryId = 2, Title = "Frankenstein", Author = "Mary Shelley",
                Description = "A scientist brings a creature to life and must face the consequences of abandoning his creation." },
            new Book { BookId = 4, PriceCents = 899, CategoryId = 2, Title = "The Time Machine", Author = "H. G. Wells",
                Description = "An inventor travels into the distant future and discovers a world whose apparent peace hides a darker reality." },
            new Book { BookId = 5, PriceCents = 1599, CategoryId = 3, Title = "The Histories", Author = "Herodotus",
                Description = "An account of the Greek and Persian worlds that traces the events and rivalries behind their wars." },
            new Book { BookId = 6, PriceCents = 1499, CategoryId = 3, Title = "The History of the Peloponnesian War", Author = "Thucydides",
                Description = "An account of the war between Athens and Sparta and the choices that drew their allies into the conflict." });
    }
}

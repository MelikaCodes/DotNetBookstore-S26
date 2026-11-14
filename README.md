# DotNet Bookstore

An ASP.NET Core MVC bookstore demo based on Dario Hesami’s COMP2084 classroom starter. Browse and search a SQLite catalog, add books to a cart, and sign in to place demo orders and see your order history. Checkout uses sample prices. The app does not collect payments or shipping information.

## Run locally

Install the .NET 10 SDK. From the repository root, run:

```sh
dotnet run --project DotNetBookstore --launch-profile http
```

Open [the Bookstore on localhost](http://localhost:5170). The app applies its SQLite migrations on startup. A new database includes six sample books in three categories. Git ignores the local database.

To try checkout, use a test email address to register a demo account. Add a book from its details page, open the cart, and place a demo order. The cart stays with the browser session, and unit prices and quantities are saved as part of each demo order.

## Code

- `Controllers/CategoriesController.cs` handles browsing, search, and book details.
- `Controllers/CartController.cs` and `Controllers/OrdersController.cs` handle cart changes, checkout, and order history.
- `Data/ApplicationDbContext.cs` configures the catalog, order data, and ASP.NET Core Identity.
- `Data/Migrations/` contains the SQLite schema and sample data.

The catalog uses cover images from these Project Gutenberg editions: [Pride and Prejudice](https://www.gutenberg.org/ebooks/1342), [Little Women](https://www.gutenberg.org/ebooks/514), [Frankenstein](https://www.gutenberg.org/ebooks/84), [The Time Machine](https://www.gutenberg.org/ebooks/35), [The History of Herodotus](https://www.gutenberg.org/ebooks/2707), and [The History of the Peloponnesian War](https://www.gutenberg.org/ebooks/7142).

See [LICENSE.txt](LICENSE.txt) for the classroom starter’s educational-use terms.

# DotNet Bookstore

An ASP.NET Core MVC bookstore demo based on the COMP2084 classroom starter by Dario Hesami. This version adds a SQLite catalog. Visitors can browse by category, search by title or author, and open a book's details.

## Run locally

Install the .NET 10 SDK. From the repository root, run:

```sh
dotnet run --project DotNetBookstore --launch-profile http
```

Open the localhost address printed by the app. On its first run, the app creates `DotNetBookstore/bookstore.db` and loads the sample books. The database is ignored by Git.

## Explore the code

- `Controllers/CategoriesController.cs` handles listing, search, and details.
- `Views/Categories/` contains the Razor views.
- `Data/ApplicationDbContext.cs` defines the catalog and seed data.
- `Data/Migrations/` creates the catalog and the existing ASP.NET Core Identity tables in SQLite.

The demo focuses on browsing books. See [LICENSE.txt](LICENSE.txt) for the classroom starter’s terms.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DotNetBookstore.Data;

public class PostgresBookstoreContext(DbContextOptions<PostgresBookstoreContext> options)
    : ApplicationDbContext(options);

public class PostgresBookstoreContextFactory : IDesignTimeDbContextFactory<PostgresBookstoreContext>
{
    public PostgresBookstoreContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Postgres")
            ?? "Host=localhost;Database=bookstore";
        var options = new DbContextOptionsBuilder<PostgresBookstoreContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new PostgresBookstoreContext(options);
    }
}

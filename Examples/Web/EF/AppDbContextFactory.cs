using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Web.EF;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[]? args = default)
    {
        const string connectionString = "Host=localhost;Port=5432;Database=autofilter;Username=postgres;Password=postgres;TrustServerCertificate=true";

        var options = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString);

        return new AppDbContext(options.Options);
    }
}
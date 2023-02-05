using AutoFixture;
using DynamicFilter.Tests.Common.EF;
using Microsoft.EntityFrameworkCore;

namespace DynamicFilter.Tests.Common;

public class DatabaseFixture : IAsyncLifetime
{
    public AppDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        DbContext = new AppDbContextFactory().CreateDbContext();

        await DbContext.Database.MigrateAsync();

        await DbContext.Products.ExecuteDeleteAsync();

        await SeedData(DbContext);
    }

    private static async Task SeedData(AppDbContext dbcontext)
    {
        var rnd = new Random();

        var fixture = new Fixture();

        var names = new[] { "Snickers", "Mars" };
        var bools = new[] { true, false };

        var products = fixture.Build<Product>().OmitAutoProperties()
            .With(x => x.Id)
            .With(x => x.Name, () => names[rnd.Next(0, names.Length)] + Guid.NewGuid())
            .With(x => x.Price)
            .With(x => x.IsInStock, () => bools[rnd.Next(0, bools.Length)])
            .With(x => x.IsForSale, () => bools[rnd.Next(0, bools.Length)])
            .With(x => x.ExpireDate, () => DateTime.UtcNow.Add(TimeSpan.FromDays(rnd.Next(1, 20)) + fixture.Create<TimeSpan>()))
            .CreateMany(1000);

        await dbcontext.Products.AddRangeAsync(products);
        await dbcontext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }
}
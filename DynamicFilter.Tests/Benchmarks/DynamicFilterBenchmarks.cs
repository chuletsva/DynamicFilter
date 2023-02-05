using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using DynamicFilter.Arguments;
using DynamicFilter.Models;
using DynamicFilter.Tests.Common;
using DynamicFilter.Tests.Common.EF;
using Microsoft.EntityFrameworkCore;

namespace DynamicFilter.Tests.Benchmarks;

[MinColumn]
[MaxColumn]
[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class DynamicFilterBenchmarks
{
    private (Expression<Func<Product, bool>> Expression, Operation DynamicFilter) _where;
    private (Expression<Func<Product, Dictionary<string, object>>> Expression, Operation DynamicFilter) _select;

    private DatabaseFixture _db = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        _where = 
        (
            x => (x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= DateTime.UtcNow && (x.IsForSale || x.IsInStock),

            new Operation("Where", new WhereArgs
            (
                new[]
                {
                    new Condition
                    (
                        Field: nameof(Product.Name),
                        Operator: SearchOperator.StartsWith,
                        Value: new[] { "Snickers" }
                    ),
                    new Condition
                    (
                        Logic: LogicOperator.Or,
                        Field: nameof(Product.Name),
                        Operator: SearchOperator.Contains,
                        Value: new[] { "Mars" }
                    ),
                    new Condition
                    (
                        Logic: LogicOperator.And,
                        Field: nameof(Product.ExpireDate),
                        Operator: SearchOperator.GreaterOrEqual,
                        Value: new[] { DateTime.UtcNow.ToString("s") }
                    ),
                    new Condition
                    (
                        Logic: LogicOperator.And,
                        Field: nameof(Product.IsForSale),
                        Operator: SearchOperator.Equals,
                        Value: new[] { "true" }
                    ),
                    new Condition
                    (
                        Logic: LogicOperator.Or,
                        Field: nameof(Product.IsInStock),
                        Operator: SearchOperator.Equals,
                        Value: new[] { "true" }
                    ),
                },

                new[]
                {
                    new Group
                    (
                        Start: 1,
                        End: 2,
                        Level: 1
                    ),
                    new Group
                    (
                        Start: 4,
                        End: 5,
                        Level: 1
                    )
                }
            ))
        );

        _select = 
        (
            x => new Dictionary<string, object>()
            {
                {"Id", x.Id},
                {"Name", x.Name}
            },

            new Operation("Select", new SelectArgs(new[]
            {
                nameof(Product.Id),
                nameof(Product.Name),
            }))
        );

        _db = new DatabaseFixture();

        await _db.InitializeAsync();

        _db.DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _db.DisposeAsync();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Where")]
    public Task Where_Expression()
    {
        return _db.DbContext.Products.AsNoTracking()
            .Where(_where.Expression)
            .OfType<object>()
            .ToArrayAsync();
    }

    [Benchmark]
    [BenchmarkCategory("Where")]
    public Task Where_DynamicFilter()
    {
        return _db.DbContext.Products.AsNoTracking()
            .ApplyDynamicFilter(_where.DynamicFilter)
            .OfType<object>()
            .ToArrayAsync();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Select")]
    public Task Select_Expression()
    {
        return _db.DbContext.Products.AsNoTracking()
            .Select(_select.Expression)
            .OfType<object>().ToArrayAsync();
    }

    [Benchmark]
    [BenchmarkCategory("Select")]
    public Task Select_DynamicFilter()
    {
        return _db.DbContext.Products.AsNoTracking()
            .ApplyDynamicFilter(_select.DynamicFilter)
            .OfType<object>().ToArrayAsync();
    }
}
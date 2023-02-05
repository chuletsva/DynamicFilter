using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using DynamicFilter.Operations;
using DynamicFilter.Tests.Common;
using DynamicFilter.Tests.Common.EF;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DynamicFilter.Tests.Benchmarks;

[MinColumn]
[MaxColumn]
[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class DynamicFilterBenchmarks
{
    private Filter _filter_with_select;
    private Filter _filter;
    private DatabaseFixture _db;

    [GlobalSetup]
    public async Task Setup()
    {
        var operations = new List<OperationDescription>()
        {
            new
            (
                Name: "Where",
                Arguments: JObject.FromObject(new WhereOperation
                (
                    new[]
                    {
                        new Condition
                        (
                            Name: nameof(Product.Name),
                            SearchOperator: SearchOperator.StartsWith,
                            Value: new[] { "Snickers" }
                        ),
                        new Condition
                        (
                            LogicOperator: LogicOperator.Or,
                            Name: nameof(Product.Name),
                            SearchOperator: SearchOperator.Contains,
                            Value: new[] { "Mars" }
                        ),
                        new Condition
                        (
                            LogicOperator: LogicOperator.And,
                            Name: nameof(Product.ExpireDate),
                            SearchOperator: SearchOperator.GreaterOrEqual,
                            Value: new[] { DateTime.UtcNow.ToString("s") }
                        ),
                        new Condition
                        (
                            LogicOperator: LogicOperator.And,
                            Name: nameof(Product.IsForSale),
                            SearchOperator: SearchOperator.Equals,
                            Value: new[] { "true" }
                        ),
                        new Condition
                        (
                            LogicOperator: LogicOperator.Or,
                            Name: nameof(Product.IsInStock),
                            SearchOperator: SearchOperator.Equals,
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
                            Start: 1,
                            End: 3,
                            Level: 2
                        ),
                        new Group
                        (
                            Start: 4,
                            End: 5,
                            Level: 2
                        )
                    }
                ))
            )
        };

        _filter = new Filter(operations.ToArray());

        operations.Add(new OperationDescription
        (
            Name: "Select",
            Arguments: JArray.FromObject(new []
            {
                nameof(Product.Id),
                nameof(Product.Name),
            })
        ));

        _filter_with_select = new Filter(operations.ToArray());

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
    [BenchmarkCategory("ApplyFilter")]
    public Task Plain()
    {
        return _db.DbContext.Products.AsNoTracking()
            .Where(x => (x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= DateTime.UtcNow && (x.IsForSale || x.IsInStock))
            .OfType<object>()
            .ToArrayAsync();
    }

    [Benchmark]
    [BenchmarkCategory("ApplyFilter")]
    public Task AutoFilter()
    {
        return _db.DbContext.Products.AsNoTracking()
            .ApplyDynamicFilter(_filter)
            .OfType<object>().ToArrayAsync();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ApplyFilterAndSelect")]
    public Task Plain_Select()
    {
        return _db.DbContext.Products.AsNoTracking()
            .Where(x => (x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= DateTime.UtcNow && (x.IsForSale || x.IsInStock))
            .Select(x => new { x.Id, x.Name })
            .OfType<object>().ToArrayAsync();
    }

    [Benchmark]
    [BenchmarkCategory("ApplyFilterAndSelect")]
    public Task AutoFilter_Select()
    {
        return _db.DbContext.Products.AsNoTracking()
            .ApplyDynamicFilter(_filter_with_select)
            .OfType<object>().ToArrayAsync();
    }
}
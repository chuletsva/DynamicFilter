using DynamicFilter.Operations;
using DynamicFilter.Tests.Common;
using DynamicFilter.Tests.Common.EF;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DynamicFilter.Tests;

public class EntityFrameworkTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _db;

    public EntityFrameworkTests(DatabaseFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task ApplyFilter()
    {
        var filter = CreateDefaultFilter();

        var products = await _db.DbContext.Products.ApplyDynamicFilter(filter).OfType<object>().ToArrayAsync();

        products.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ApplyFilterAndSelect()
    {
        var defaultFilter = CreateDefaultFilter();

        var properties = new[]
        {
            nameof(Product.Id),
            nameof(Product.Name)
        };

        var selectOperation = new OperationDescription("Select", JToken.FromObject(properties));

        var filter = new Filter(defaultFilter.Operations.Append(selectOperation).ToArray());

        var products = await _db.DbContext.Products
            .ApplyDynamicFilter(filter)
            .OfType<Dictionary<string, object>>().ToArrayAsync();

        products.Should().NotBeEmpty();

        foreach (var keys in products.Select(x => x.Keys))
        {
            keys.Should().BeEquivalentTo(properties);
        }
    }

    [Fact]
    public async Task AutoFilterShouldProduceSameResultAsPlainExpression()
    {
        var filter = new Filter
        (
            Operations: new []
            {
                new OperationDescription
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
                ),

                new OperationDescription
                (
                    Name: "OrderBy",
                    Arguments: nameof(Product.ExpireDate)
                ),

                new OperationDescription
                (
                    Name: "Select",
                    Arguments: JArray.FromObject(new[]
                    {
                        nameof(Product.Id),
                        nameof(Product.Name)
                    })
                )
            }
        );

        var autofilterEntities = await _db.DbContext.Products
            .ApplyDynamicFilter(filter)
            .OfType<Dictionary<string, object>>().ToArrayAsync();

        var plainExprEntities = await _db.DbContext.Products.Where(x =>
                (x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= DateTime.UtcNow && (x.IsForSale || x.IsInStock))
            .OrderBy(x => x.ExpireDate)
            .Select(x => new Dictionary<string, object>()
            {
                {nameof(Product.Id), x.Id},
                {nameof(Product.Name), x.Name},
            })
            .ToArrayAsync();

        autofilterEntities.Should().BeEquivalentTo(plainExprEntities, x => x.WithStrictOrdering());
    }

    private static Filter CreateDefaultFilter()
    {
        return new Filter
        (
            Operations: new []
            {
                new OperationDescription
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
                ),

                new OperationDescription
                (
                    Name: "OrderBy",
                    Arguments: nameof(Product.ExpireDate)
                )
            }
        );
    }
}
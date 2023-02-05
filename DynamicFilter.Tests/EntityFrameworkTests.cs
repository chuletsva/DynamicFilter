using DynamicFilter.Arguments;
using DynamicFilter.Models;
using DynamicFilter.Tests.Common;
using DynamicFilter.Tests.Common.EF;
using Microsoft.EntityFrameworkCore;

namespace DynamicFilter.Tests;

public class EntityFrameworkTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _db;

    public EntityFrameworkTests(DatabaseFixture db)
    {
        _db = db;
    }

    [Theory]
    [MemberData(nameof(Filters))]
    public async Task ApplyFilter(Operation[] filter)
    {
        _ = await _db.DbContext.Products.ApplyDynamicFilter(filter)
            .OfType<object>().ToArrayAsync();
    }

    public static IEnumerable<object[]> Filters
    {
        get
        {
            yield return new object[]
            {
                new Operation[]
                {
                    new
                    (
                        Name: "Where",
                        Arguments: new WhereArgs
                        (
                            new[]
                            {
                                new Condition
                                (
                                    Field: nameof(Product.Name),
                                    Operator: SearchOperator.StartsWith,
                                    Value: new[] { "Snickers" }
                                )
                            }
                        )
                    ),
                }
            };

            yield return new object[]
            {
                new Operation[] { new("Distinct", new DistinctArgs()) }
            };

            yield return new object[]
            {
                new Operation[] { new("Skip", new SkipArgs(1)) }
            };

            yield return new object[]
            {
                new Operation[] { new("Take", new TakeArgs(1)) }
            };

            yield return new object[]
            {
                new Operation[] { new("OrderBy", new OrderByArgs(nameof(Product.ExpireDate))) }
            };

            yield return new object[]
            {
                new Operation[] { new("OrderByDescending", new OrderByDescendingArgs(nameof(Product.ExpireDate))) }
            };

            yield return new object[]
            {
                new Operation[] 
                { 
                    new("OrderBy", new OrderByArgs(nameof(Product.ExpireDate))), 
                    new("ThenBy", new ThenByArgs(nameof(Product.ExpireDate)))
                }
            };

            yield return new object[]
            {
                new Operation[]
                {
                    new("OrderBy", new OrderByArgs(nameof(Product.ExpireDate))), 
                    new("ThenByDescending", new ThenByDescendingArgs(nameof(Product.ExpireDate)))
                }
            };

            yield return new object[]
            {
                new Operation[] { new("Select", new SelectArgs(new[] { nameof(Product.Id) })) }
            };
        }
    }
}
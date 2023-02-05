using DynamicFilter.Arguments;
using DynamicFilter.Models;
using FluentAssertions;

namespace DynamicFilter.Tests;

public class DynamicFilterLinqExtensionsTests
{
    [Fact]
    public void ShouldFilter()
    {
        var obj = new TestClass();

        var queryable = new[] { obj, new() }.AsQueryable();

        var filter = new Operation[]
        {
            new
            (
                Name: "Where",
                Arguments: new WhereArgs
                (
                    Conditions: new[]
                    {
                        new Condition(nameof(TestClass.Id), new[] { obj.Id.ToString() }, SearchOperator.Equals)
                    }
                )
            )
        };

        var resultQueryable = queryable.ApplyDynamicFilter(filter);

        resultQueryable.OfType<object>().Should().ContainSingle(x => x == obj);
    }

    [Fact]
    public void ShouldSelect()
    {
        var obj = new TestClass();

        var queryable = new[] { obj }.AsQueryable();

        var filter = new Operation[]
        {
            new
            (
                Name: "Select",
                Arguments: new SelectArgs(new[] { nameof(TestClass.Id) })
            )
        };

        var resultQueryable = (IQueryable<Dictionary<string, object>>) queryable.ApplyDynamicFilter(filter);

        resultQueryable.Should().ContainSingle();

        resultQueryable.Single().Keys.Should().ContainSingle(nameof(TestClass.Id));

        resultQueryable.Single().Values.Should().ContainSingle(obj.Id.ToString());
    }

    private class TestClass
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public bool Prop { get; init; }
    }
}

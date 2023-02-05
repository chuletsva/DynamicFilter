using DynamicFilter.Operations;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace DynamicFilter.Tests;

public class DynamicFilterLinqExtensionsTests
{
    [Fact]
    public void ShouldFilter()
    {
        var matchObject = new TestClass();

        var queryable = new[] { matchObject, new() }.AsQueryable();

        var filter = new Filter
        (
            Operations: new OperationDescription[]
            {
                new 
                (
                    Name: "Where",
                    Arguments: JObject.FromObject(new WhereOperation
                    (
                        Conditions: new[]
                        {
                            new Condition(nameof(TestClass.Id), new [] { matchObject.Id.ToString() }, SearchOperator.Equals)
                        }
                    ))
                )
            }
        );

        var resultQueryable = queryable.ApplyDynamicFilter(filter);

        resultQueryable.OfType<object>().Should().ContainSingle(x => x == matchObject);
    }

    [Fact]
    public void ShouldSelect()
    {
        var obj = new TestClass();
        var queryable = new[] { obj }.AsQueryable();

        var filter = new Filter
        (
            Operations: new[]
            {
                new OperationDescription
                (
                    Name: "Select",
                    Arguments: JArray.FromObject(new [] { nameof(TestClass.Id) })
                )
            }
        );

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

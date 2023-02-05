using AutoFixture;
using DynamicFilter.Arguments;
using FluentAssertions;

namespace DynamicFilter.Tests;

public class OperationHandlerTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Dictinct()
    {
        var obj = new TestClass();

        var queryable = new[] { obj, obj }.AsQueryable();

        queryable = (IQueryable<TestClass>)OperationHandler.Distinct(queryable);

        queryable.Should().HaveCount(1);
    }

    [Fact]
    public void Select()
    {
        IQueryable query = new Fixture().CreateMany<TestClass>().AsQueryable();

        var args = new SelectArgs(new[] { nameof(TestClass.Bool) });

        query = OperationHandler.Select(query, args);

        query.ElementType.Should().Be(typeof(Dictionary<string, object>));
    }

    [Fact]
    public void Skip()
    {
        var sourceQuery = new Fixture().CreateMany<int>(1000).AsQueryable();

        var expectedQuery = sourceQuery.Skip(500);

        var args = new SkipArgs(500);

        IQueryable actualQuery = OperationHandler.Skip(sourceQuery, args);

        actualQuery.Should().BeEquivalentTo(expectedQuery, options => options.WithStrictOrdering());
    }

    [Fact]
    public void Take()
    {
        var sourceQuery = new Fixture().CreateMany<int>(1000).AsQueryable();

        var expectedQuery = sourceQuery.Take(500);

        var args = new TakeArgs(500);

        var actualQuery = OperationHandler.Take(sourceQuery, args);

        actualQuery.Should().BeEquivalentTo(expectedQuery, options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderBy()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var args = new OrderByArgs(nameof(TestClass.Int1));

        query = (IQueryable<TestClass>)OperationHandler.OrderBy(query, args);

        query.Should().BeInAscendingOrder(x => x.Int1);
    }

    [Fact]
    public void OrderByDescending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var args = new OrderByDescendingArgs(nameof(TestClass.Int1));

        query = (IQueryable<TestClass>)OperationHandler.OrderByDescending(query, args);

        query.Should().BeInDescendingOrder(x => x.Int1);
    }

    [Fact]
    public void ThenBy()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var orderByArgs = new OrderByArgs(nameof(TestClass.Int1));

        query = (IQueryable<TestClass>)OperationHandler.OrderBy(query, orderByArgs);

        var thenByArgs = new ThenByArgs(nameof(TestClass.Int2));

        query = (IQueryable<TestClass>)OperationHandler.ThenBy(query, thenByArgs);

        query.Should().BeInAscendingOrder(x => x.Int1).And.ThenBeInAscendingOrder(x => x.Int2);
    }

    [Fact]
    public void ThenByDescending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var orderByArgs = new OrderByArgs(nameof(TestClass.Int1));

        query = (IQueryable<TestClass>)OperationHandler.OrderBy(query, orderByArgs);

        var thenByDescendingArgs = new ThenByDescendingArgs(nameof(TestClass.Int2));

        query = (IQueryable<TestClass>)OperationHandler.ThenByDescending(query, thenByDescendingArgs);

        query.Should().BeInAscendingOrder(x => x.Int1).And.ThenBeInDescendingOrder(x => x.Int2);
    }

    private class TestClass
    {
        public bool Bool { get; init; }
        public int Int1 { get; init; }
        public int Int2 { get; init; }
    }
}

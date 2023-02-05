using AutoFixture;
using DynamicFilter.Operations;
using FluentAssertions;

namespace DynamicFilter.Tests;

public class OperationProcessorTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Dictinct()
    {
        var obj = new TestClass();

        var queryable = new[] { obj, obj }.AsQueryable();

        var operation = new DistinctOperation();

        queryable = (IQueryable<TestClass>)OperationProcessor.Distinct(queryable, operation);

        queryable.Should().HaveCount(1);
    }

    [Fact]
    public void Select()
    {
        IQueryable query = new Fixture().CreateMany<TestClass>().AsQueryable();

        var operation = new SelectOperation(new[] { nameof(TestClass.Bool) });

        query = OperationProcessor.Select(query, operation);

        query.ElementType.Should().Be(typeof(Dictionary<string, object>));
    }

    [Fact]
    public void Skip()
    {
        var sourceQuery = new Fixture().CreateMany<int>(1000).AsQueryable();

        var expectedQuery = sourceQuery.Skip(500);

        var operation = new SkipOperation(500);

        IQueryable actualQuery = OperationProcessor.Skip(sourceQuery, operation);

        actualQuery.Should().BeEquivalentTo(expectedQuery, options => options.WithStrictOrdering());
    }

    [Fact]
    public void Take()
    {
        var sourceQuery = new Fixture().CreateMany<int>(1000).AsQueryable();

        var expectedQuery = sourceQuery.Take(500);

        var operation = new TakeOperation(500);

        var actualQuery = OperationProcessor.Take(sourceQuery, operation);

        actualQuery.Should().BeEquivalentTo(expectedQuery, options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderBy()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var operation = new OrderByOperation(nameof(TestClass.Int1));

        query = (IQueryable<TestClass>)OperationProcessor.OrderBy(query, operation);

        query.Should().BeInAscendingOrder(x => x.Int1);
    }

    [Fact]
    public void OrderByDescending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var operation = new OrderByDescendingOperation(nameof(TestClass.Int1));

        query = (IQueryable<TestClass>)OperationProcessor.OrderByDescending(query, operation);

        query.Should().BeInDescendingOrder(x => x.Int1);
    }

    [Fact]
    public void ThenBy()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var orderBy = new OrderByOperation(nameof(TestClass.Int1));

        query = (IQueryable<TestClass>)OperationProcessor.OrderBy(query, orderBy);

        var thenBy = new ThenByOperation(nameof(TestClass.Int2));

        query = (IQueryable<TestClass>)OperationProcessor.ThenBy(query, thenBy);

        query.Should().BeInAscendingOrder(x => x.Int1).And.ThenBeInAscendingOrder(x => x.Int2);
    }

    [Fact]
    public void ThenByDescending()
    {
        var query = _fixture.CreateMany<TestClass>(1000).AsQueryable();

        var orderBy = new OrderByOperation(nameof(TestClass.Int1));

        query = (IQueryable<TestClass>)OperationProcessor.OrderBy(query, orderBy);

        var thenByDescending = new ThenByDescendingOperation(nameof(TestClass.Int2));

        query = (IQueryable<TestClass>)OperationProcessor.ThenByDescending(query, thenByDescending);

        query.Should().BeInAscendingOrder(x => x.Int1).And.ThenBeInDescendingOrder(x => x.Int2);
    }

    private class TestClass
    {
        public bool Bool { get; init; }
        public int Int1 { get; init; }
        public int Int2 { get; init; }
    }
}

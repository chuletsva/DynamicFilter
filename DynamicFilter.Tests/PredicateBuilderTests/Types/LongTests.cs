using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class LongTests
{
    [Theory]
    [MemberData(nameof(LongTestCases))]
    public void ShouldHandleLong(long objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Long = objValue };

        Condition condition = new(nameof(obj.Long), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(LongTestCases))]
    [MemberData(nameof(NullableLongTestCases))]
    public void ShouldHandleNullableLong(long? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableLong = objValue };

        Condition condition = new(nameof(obj.NullableLong), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> LongTestCases => new[]
    {
        new object[] { long.MinValue, new[] { long.MaxValue.ToString() }, SearchOperator.Equals, false },
        new object[] { long.MaxValue, new[] { long.MinValue.ToString() }, SearchOperator.Equals, false },
        new object[] { 0L, new[] { "0" }, SearchOperator.Equals, true },
        new object[] { long.MinValue, new[] { long.MinValue.ToString() }, SearchOperator.Equals, true },
        new object[] { long.MaxValue, new[] { long.MaxValue.ToString() }, SearchOperator.Equals, true },

        new object[] { long.MinValue, new[] { long.MaxValue.ToString() }, SearchOperator.NotEquals, true },
        new object[] { long.MaxValue, new[] { long.MinValue.ToString() }, SearchOperator.NotEquals, true },
        new object[] { 0L, new[] { "0" }, SearchOperator.NotEquals, false },
        new object[] { long.MinValue, new[] { long.MinValue.ToString() }, SearchOperator.NotEquals, false },
        new object[] { long.MaxValue, new[] { long.MaxValue.ToString() }, SearchOperator.NotEquals, false },

        new object[] { long.MaxValue, new[] { long.MinValue.ToString() }, SearchOperator.Greater, true },
        new object[] { long.MinValue, new[] { long.MaxValue.ToString() }, SearchOperator.Greater, false },
        new object[] { 0L, new[] { "0" }, SearchOperator.Greater, false },
        new object[] { long.MinValue, new[] { long.MinValue.ToString() }, SearchOperator.Greater, false },
        new object[] { long.MaxValue, new[] { long.MaxValue.ToString() }, SearchOperator.Greater, false },

        new object[] { long.MaxValue, new[] { long.MinValue.ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { long.MinValue, new[] { long.MaxValue.ToString() }, SearchOperator.GreaterOrEqual, false },
        new object[] { 0L, new[] { "0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { long.MinValue, new[] { long.MinValue.ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { long.MaxValue, new[] { long.MaxValue.ToString() }, SearchOperator.GreaterOrEqual, true },

        new object[] { long.MinValue, new[] { long.MaxValue.ToString() }, SearchOperator.Less, true },
        new object[] { long.MaxValue, new[] { long.MinValue.ToString() }, SearchOperator.Less, false },
        new object[] { 0L, new[] { "0" }, SearchOperator.Less, false },
        new object[] { long.MinValue, new[] { long.MinValue.ToString() }, SearchOperator.Less, false },
        new object[] { long.MaxValue, new[] { long.MaxValue.ToString() }, SearchOperator.Less, false },

        new object[] { long.MinValue, new[] { long.MaxValue.ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { long.MaxValue, new[] { long.MinValue.ToString() }, SearchOperator.LessOrEqual, false },
        new object[] { 0L, new[] { "0" }, SearchOperator.LessOrEqual, true },
        new object[] { long.MinValue, new[] { long.MinValue.ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { long.MaxValue, new[] { long.MaxValue.ToString() }, SearchOperator.LessOrEqual, true },

        new object[] { 0L, new[] { "0" }, SearchOperator.Any, true },
        new object[] { 0L, new[] { "1" }, SearchOperator.Any, false },
        new object[] { 0L, Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableLongTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { "0" }, SearchOperator.Equals, false },
        new object?[] { null, new[] { long.MaxValue.ToString() }, SearchOperator.Equals, false },
        new object?[] { 0L, new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { long.MaxValue, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { "0" }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { long.MaxValue.ToString() }, SearchOperator.NotEquals, true },
        new object?[] { 0L, new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { long.MaxValue, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Greater, false },
        new object?[] { null, new[] { long.MinValue.ToString() }, SearchOperator.Greater, false },
        new object?[] { 0L, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { long.MaxValue, new string?[] { null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { long.MinValue.ToString() }, SearchOperator.GreaterOrEqual, false },
        new object?[] { 0L, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { long.MaxValue, new string?[] { null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Less, false },
        new object?[] { null, new[] { long.MaxValue.ToString() }, SearchOperator.Less, false },
        new object?[] { 0L, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { long.MinValue, new string?[] { null }, SearchOperator.Less, false },

        new object?[] { null, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { long.MaxValue.ToString() }, SearchOperator.LessOrEqual, false },
        new object?[] { 0L, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { long.MinValue, new string?[] { null }, SearchOperator.LessOrEqual, false },

        new object?[] { long.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { 0L, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { long.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { 0L, null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public long Long { get; init; }
        public long? NullableLong { get; init; }
    }
}
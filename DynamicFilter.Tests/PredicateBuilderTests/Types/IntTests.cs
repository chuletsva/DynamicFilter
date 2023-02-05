using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Models;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class IntTests
{
    [Theory]
    [MemberData(nameof(IntTestCases))]
    public void ShouldHandleInt(int objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Int = objValue };

        Condition condition = new(nameof(obj.Int), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(IntTestCases))]
    [MemberData(nameof(NullableIntTestCases))]
    public void ShouldHandleNullableInt(int? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableInt = objValue };

        Condition condition = new(nameof(obj.NullableInt), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> IntTestCases => new[]
    {
        new object[] { int.MinValue, new[] { int.MaxValue.ToString() }, SearchOperator.Equals, false },
        new object[] { int.MaxValue, new[] { int.MinValue.ToString() }, SearchOperator.Equals, false },
        new object[] { 0, new[] { "0" }, SearchOperator.Equals, true },
        new object[] { int.MinValue, new[] { int.MinValue.ToString() }, SearchOperator.Equals, true },
        new object[] { int.MaxValue, new[] { int.MaxValue.ToString() }, SearchOperator.Equals, true },

        new object[] { int.MinValue, new[] { int.MaxValue.ToString() }, SearchOperator.NotEquals, true },
        new object[] { int.MaxValue, new[] { int.MinValue.ToString() }, SearchOperator.NotEquals, true },
        new object[] { 0, new[] { "0" }, SearchOperator.NotEquals, false },
        new object[] { int.MinValue, new[] { int.MinValue.ToString() }, SearchOperator.NotEquals, false },
        new object[] { int.MaxValue, new[] { int.MaxValue.ToString() }, SearchOperator.NotEquals, false },

        new object[] { int.MaxValue, new[] { int.MinValue.ToString() }, SearchOperator.Greater, true },
        new object[] { int.MinValue, new[] { int.MaxValue.ToString() }, SearchOperator.Greater, false },
        new object[] { 0, new[] { "0" }, SearchOperator.Greater, false },
        new object[] { int.MinValue, new[] { int.MinValue.ToString() }, SearchOperator.Greater, false },
        new object[] { int.MaxValue, new[] { int.MaxValue.ToString() }, SearchOperator.Greater, false },

        new object[] { int.MaxValue, new[] { int.MinValue.ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { int.MinValue, new[] { int.MaxValue.ToString() }, SearchOperator.GreaterOrEqual, false },
        new object[] { 0, new[] { "0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { int.MinValue, new[] { int.MinValue.ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { int.MaxValue, new[] { int.MaxValue.ToString() }, SearchOperator.GreaterOrEqual, true },

        new object[] { int.MinValue, new[] { int.MaxValue.ToString() }, SearchOperator.Less, true },
        new object[] { int.MaxValue, new[] { int.MinValue.ToString() }, SearchOperator.Less, false },
        new object[] { 0, new[] { "0" }, SearchOperator.Less, false },
        new object[] { int.MinValue, new[] { int.MinValue.ToString() }, SearchOperator.Less, false },
        new object[] { int.MaxValue, new[] { int.MaxValue.ToString() }, SearchOperator.Less, false },

        new object[] { int.MinValue, new[] { int.MaxValue.ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { int.MaxValue, new[] { int.MinValue.ToString() }, SearchOperator.LessOrEqual, false },
        new object[] { 0, new[] { "0" }, SearchOperator.LessOrEqual, true },
        new object[] { int.MinValue, new[] { int.MinValue.ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { int.MaxValue, new[] { int.MaxValue.ToString() }, SearchOperator.LessOrEqual, true },

        new object[] { 0, new[] { "0" }, SearchOperator.Any, true },
        new object[] { 0, new[] { "1" }, SearchOperator.Any, false },
        new object[] { 0, Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableIntTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { "0" }, SearchOperator.Equals, false },
        new object?[] { null, new[] { int.MaxValue.ToString() }, SearchOperator.Equals, false },
        new object?[] { 0, new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { int.MaxValue, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { "0" }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { int.MaxValue.ToString() }, SearchOperator.NotEquals, true },
        new object?[] { 0, new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { int.MaxValue, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Greater, false },
        new object?[] { null, new[] { int.MinValue.ToString() }, SearchOperator.Greater, false },
        new object?[] { 0, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { int.MaxValue, new string?[] { null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { int.MinValue.ToString() }, SearchOperator.GreaterOrEqual, false },
        new object?[] { 0, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { int.MaxValue, new string?[] { null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Less, false },
        new object?[] { null, new[] { int.MaxValue.ToString() }, SearchOperator.Less, false },
        new object?[] { 0, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { int.MinValue, new string?[] { null }, SearchOperator.Less, false },

        new object?[] { null, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { int.MaxValue.ToString() }, SearchOperator.LessOrEqual, false },
        new object?[] { 0, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { int.MinValue, new string?[] { null }, SearchOperator.LessOrEqual, false },

        new object?[] { int.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { 0, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { int.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { 0, null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public int Int { get; init; }
        public int? NullableInt { get; init; }
    }
}
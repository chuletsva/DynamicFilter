using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class ShortTests
{
    [Theory]
    [MemberData(nameof(ShortTestCases))]
    public void ShouldHandleShort(short objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Short = objValue };

        Condition condition = new(nameof(obj.Short), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(ShortTestCases))]
    [MemberData(nameof(NullableShortTestCases))]
    public void ShouldHandleNullableShort(short? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableShort = objValue };

        Condition condition = new(nameof(obj.NullableShort), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> ShortTestCases => new[]
    {
        new object[] { short.MinValue, new[] { short.MaxValue.ToString() }, SearchOperator.Equals, false },
        new object[] { short.MaxValue, new[] { short.MinValue.ToString() }, SearchOperator.Equals, false },
        new object[] { (short)0, new[] { "0" }, SearchOperator.Equals, true },
        new object[] { short.MinValue, new[] { short.MinValue.ToString() }, SearchOperator.Equals, true },
        new object[] { short.MaxValue, new[] { short.MaxValue.ToString() }, SearchOperator.Equals, true },

        new object[] { short.MinValue, new[] { short.MaxValue.ToString() }, SearchOperator.NotEquals, true },
        new object[] { short.MaxValue, new[] { short.MinValue.ToString() }, SearchOperator.NotEquals, true },
        new object[] { (short)0, new[] { "0" }, SearchOperator.NotEquals, false },
        new object[] { short.MinValue, new[] { short.MinValue.ToString() }, SearchOperator.NotEquals, false },
        new object[] { short.MaxValue, new[] { short.MaxValue.ToString() }, SearchOperator.NotEquals, false },

        new object[] { short.MaxValue, new[] { short.MinValue.ToString() }, SearchOperator.Greater, true },
        new object[] { short.MinValue, new[] { short.MaxValue.ToString() }, SearchOperator.Greater, false },
        new object[] { (short)0, new[] { "0" }, SearchOperator.Greater, false },
        new object[] { short.MinValue, new[] { short.MinValue.ToString() }, SearchOperator.Greater, false },
        new object[] { short.MaxValue, new[] { short.MaxValue.ToString() }, SearchOperator.Greater, false },

        new object[] { short.MaxValue, new[] { short.MinValue.ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { short.MinValue, new[] { short.MaxValue.ToString() }, SearchOperator.GreaterOrEqual, false },
        new object[] { (short)0, new[] { "0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { short.MinValue, new[] { short.MinValue.ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { short.MaxValue, new[] { short.MaxValue.ToString() }, SearchOperator.GreaterOrEqual, true },

        new object[] { short.MinValue, new[] { short.MaxValue.ToString() }, SearchOperator.Less, true },
        new object[] { short.MaxValue, new[] { short.MinValue.ToString() }, SearchOperator.Less, false },
        new object[] { (short)0, new[] { "0" }, SearchOperator.Less, false },
        new object[] { short.MinValue, new[] { short.MinValue.ToString() }, SearchOperator.Less, false },
        new object[] { short.MaxValue, new[] { short.MaxValue.ToString() }, SearchOperator.Less, false },

        new object[] { short.MinValue, new[] { short.MaxValue.ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { short.MaxValue, new[] { short.MinValue.ToString() }, SearchOperator.LessOrEqual, false },
        new object[] { (short)0, new[] { "0" }, SearchOperator.LessOrEqual, true },
        new object[] { short.MinValue, new[] { short.MinValue.ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { short.MaxValue, new[] { short.MaxValue.ToString() }, SearchOperator.LessOrEqual, true },

        new object[] { (short)0, new[] { "0" }, SearchOperator.Any, true },
        new object[] { (short)0, new[] { "1" }, SearchOperator.Any, false },
        new object[] { (short)0, Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableShortTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { "0" }, SearchOperator.Equals, false },
        new object?[] { null, new[] { short.MaxValue.ToString() }, SearchOperator.Equals, false },
        new object?[] { (short)0, new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { short.MaxValue, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { "0" }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { short.MaxValue.ToString() }, SearchOperator.NotEquals, true },
        new object?[] { (short)0, new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { short.MaxValue, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Greater, false },
        new object?[] { null, new[] { short.MinValue.ToString() }, SearchOperator.Greater, false },
        new object?[] { (short)0, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { short.MaxValue, new string?[] { null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { short.MinValue.ToString() }, SearchOperator.GreaterOrEqual, false },
        new object?[] { (short)0, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { short.MaxValue, new string?[] { null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Less, false },
        new object?[] { null, new[] { short.MaxValue.ToString() }, SearchOperator.Less, false },
        new object?[] { (short)0, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { short.MinValue, new string?[] { null }, SearchOperator.Less, false },

        new object?[] { null, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { short.MaxValue.ToString() }, SearchOperator.LessOrEqual, false },
        new object?[] { (short)0, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { short.MinValue, new string?[] { null }, SearchOperator.LessOrEqual, false },

        new object?[] { short.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { (short)0, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { short.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { (short)0, null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public short Short { get; init; }
        public short? NullableShort { get; init; }
    }
}

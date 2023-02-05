using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Models;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class CharTests
{
    [Theory]
    [MemberData(nameof(CharTestCases))]
    public void ShouldHandleChar(char objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Char = objValue };

        Condition condition = new(nameof(obj.Char), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(CharTestCases))]
    [MemberData(nameof(NullableCharTestCases))]
    public void ShouldHandleNullableChar(char? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableChar = objValue };

        Condition condition = new(nameof(obj.NullableChar), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> CharTestCases => new[]
    {
        new object[] { default(char), new[] { default(char).ToString() }, SearchOperator.Equals, true },
        new object[] { char.MinValue, new[] { char.MinValue.ToString() }, SearchOperator.Equals, true },
        new object[] { char.MaxValue, new[] { char.MaxValue.ToString() }, SearchOperator.Equals, true },
        new object[] { char.MinValue, new[] { char.MaxValue.ToString() }, SearchOperator.Equals, false },
        new object[] { char.MaxValue, new[] { char.MinValue.ToString() }, SearchOperator.Equals, false },

        new object[] { default(char), new[] { default(char).ToString() }, SearchOperator.NotEquals, false },
        new object[] { char.MinValue, new[] { char.MinValue.ToString() }, SearchOperator.NotEquals, false },
        new object[] { char.MaxValue, new[] { char.MaxValue.ToString() }, SearchOperator.NotEquals, false },
        new object[] { char.MinValue, new[] { char.MaxValue.ToString() }, SearchOperator.NotEquals, true },
        new object[] { char.MaxValue, new[] { char.MinValue.ToString() }, SearchOperator.NotEquals, true },

        new object[] { char.MaxValue, new[] { char.MinValue.ToString() }, SearchOperator.Greater, true },
        new object[] { char.MinValue, new[] { char.MaxValue.ToString() }, SearchOperator.Greater, false },
        new object[] { default(char), new[] { default(char).ToString() }, SearchOperator.Greater, false },
        new object[] { char.MinValue, new[] { char.MinValue.ToString() }, SearchOperator.Greater, false },
        new object[] { char.MaxValue, new[] { char.MaxValue.ToString() }, SearchOperator.Greater, false },

        new object[] { char.MaxValue, new[] { char.MinValue.ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { char.MinValue, new[] { char.MaxValue.ToString() }, SearchOperator.GreaterOrEqual, false },
        new object[] { default(char), new[] { default(char).ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { char.MinValue, new[] { char.MinValue.ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { char.MaxValue, new[] { char.MaxValue.ToString() }, SearchOperator.GreaterOrEqual, true },

        new object[] { char.MinValue, new[] { char.MaxValue.ToString() }, SearchOperator.Less, true },
        new object[] { char.MaxValue, new[] { char.MinValue.ToString() }, SearchOperator.Less, false },
        new object[] { default(char), new[] { default(char).ToString() }, SearchOperator.Less, false },
        new object[] { char.MinValue, new[] { char.MinValue.ToString() }, SearchOperator.Less, false },
        new object[] { char.MaxValue, new[] { char.MaxValue.ToString() }, SearchOperator.Less, false },

        new object[] { char.MinValue, new[] { char.MaxValue.ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { char.MaxValue, new[] { char.MinValue.ToString() }, SearchOperator.LessOrEqual, false },
        new object[] { default(char), new[] { default(char).ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { char.MinValue, new[] { char.MinValue.ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { char.MaxValue, new[] { char.MaxValue.ToString() }, SearchOperator.LessOrEqual, true },

        new object[] { default(char), new[] { default(char).ToString() }, SearchOperator.Any, true },
        new object[] { default(char), new[] { "1" }, SearchOperator.Any, false },
        new object[] { default(char), Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableCharTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[]{ default(char).ToString() }, SearchOperator.Equals, false },
        new object?[] { null, new[]{ char.MaxValue.ToString() }, SearchOperator.Equals, false },
        new object?[] { default(char), new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { char.MaxValue, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[]{ default(char).ToString() }, SearchOperator.NotEquals, true },
        new object?[] { null, new[]{ char.MaxValue.ToString() }, SearchOperator.NotEquals, true },
        new object?[] { default(char), new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { char.MaxValue, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { null, new[]{ default(char).ToString() }, SearchOperator.Greater, false },
        new object?[] { null, new[]{ char.MinValue.ToString() }, SearchOperator.Greater, false },
        new object?[] { default(char), new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { char.MaxValue, new string?[] { null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[]{ default(char).ToString() }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { char.MinValue.ToString() }, SearchOperator.GreaterOrEqual, false },
        new object?[] { default(char), new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { char.MaxValue, new string?[] { null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { null, new[] { default(char).ToString() }, SearchOperator.Less, false },
        new object?[] { null, new[] { char.MaxValue.ToString() }, SearchOperator.Less, false },
        new object?[] { default(char), new string?[] { null }, SearchOperator.Less, false },
        new object?[] { char.MinValue, new string?[] { null }, SearchOperator.Less, false },

        new object?[] { null, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { default(char).ToString() }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { char.MaxValue.ToString() }, SearchOperator.LessOrEqual, false },
        new object?[] { default(char), new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { char.MinValue, new string?[] { null }, SearchOperator.LessOrEqual, false },

        new object?[] { char.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { default(char), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { char.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { default(char), null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { default(char).ToString() }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public char Char { get; init; }
        public char? NullableChar { get; init; }
    }
}
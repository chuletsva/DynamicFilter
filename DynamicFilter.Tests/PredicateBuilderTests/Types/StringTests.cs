using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class StringTests
{
    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldHandleString(string? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { String = objValue };

        Condition condition = new(nameof(obj.String), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object?[]> TestCases => new[]
    {
        // Equals
        new object?[] { Guid.Empty.ToString(), new[] {  Guid.Empty.ToString() }, SearchOperator.Equals, true },
        new object?[] { string.Empty, new[] { string.Empty }, SearchOperator.Equals, true },
        new object?[] { Guid.NewGuid().ToString(), new[] { Guid.NewGuid().ToString() }, SearchOperator.Equals, false },

        new object?[] { " ", new[] { string.Empty }, SearchOperator.Equals, false },
        new object?[] { string.Empty, new[] { " " }, SearchOperator.Equals, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { string.Empty }, SearchOperator.Equals, false },
        new object?[] { null, new[] { Guid.Empty.ToString() }, SearchOperator.Equals, false },
        new object?[] { Guid.Empty.ToString(), new string?[]{ null }, SearchOperator.Equals, false },   
        
        // NotEquals
        new object?[] { Guid.Empty.ToString(), new[] { Guid.Empty.ToString() }, SearchOperator.NotEquals, false },
        new object?[] { string.Empty, new[] { string.Empty }, SearchOperator.NotEquals, false },
        new object?[] { Guid.NewGuid().ToString(), new[] { Guid.NewGuid().ToString() }, SearchOperator.NotEquals, true },

        new object?[] { " ", new[] { string.Empty }, SearchOperator.NotEquals, true },
        new object?[] { string.Empty, new[] { " " }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[]{ null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { string.Empty }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { Guid.Empty.ToString() }, SearchOperator.NotEquals, true },
        new object?[] { Guid.Empty.ToString(), new string?[]{ null }, SearchOperator.NotEquals, true },

        // Other
        new object?[] { string.Empty, null, SearchOperator.Exists, true },
        new object?[] { Guid.Empty.ToString(), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { Guid.Empty.ToString(), null, SearchOperator.NotExists, false },

        new object?[] { "ab", new[]{ "a" }, SearchOperator.StartsWith, true },
        new object?[] { "ab", new[] { "b" }, SearchOperator.StartsWith, false },

        new object?[] { "ab", new[] { "b" }, SearchOperator.EndsWith, true },
        new object?[] { "ab", new[] { "a" }, SearchOperator.EndsWith, false },

        new object?[] { "a", new[] { "a" }, SearchOperator.Contains, true },
        new object?[] { "abc", new[] { "b" }, SearchOperator.Contains, true },
        new object?[] { "a", new[] { "b" }, SearchOperator.Contains, false },

        new object?[] { "abc", new[] { "d" }, SearchOperator.NotContains, true },
        new object?[] { "abc", new[] { "b" }, SearchOperator.NotContains, false },

        new object[] { "a", new[] { "a" }, SearchOperator.Any, true },
        new object[] { "a", new[] { "b" }, SearchOperator.Any, false },
        new object[] { "a", Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { "a" }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public string? String { get; init; }
    }
}
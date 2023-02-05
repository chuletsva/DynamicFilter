using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class BoolTests
{
    [Theory]
    [MemberData(nameof(BoolTestCases))]
    public void ShouldHandleBool(bool objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Bool = objValue };

        Condition condition = new(nameof(obj.Bool), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(BoolTestCases))]
    [MemberData(nameof(NullableBoolTestCases))]
    public void ShouldHandleNullableBool(bool? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableBool = objValue };

        Condition condition = new(nameof(obj.NullableBool), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ShouldThrow_WhenNotComparableValue(string? searchValue)
    {
        Condition condition = new(nameof(TestClass.Bool), new[] { searchValue }, SearchOperator.Equals);

        FluentActions.Invoking(() => PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition }))
            .Should().Throw<Exception>().Which.Message
            .Should().StartWith($"Property '{nameof(TestClass.Bool)}' from type '{nameof(TestClass)}'");
    }

    public static IEnumerable<object[]> BoolTestCases => new[]
    {
        new object[] { true, new[] { "true" }, SearchOperator.Equals, true },
        new object[] { false, new[]{ "false" }, SearchOperator.Equals, true },
        new object[] { true, new[] { "false" }, SearchOperator.Equals, false },
        new object[] { false, new[] { "true" }, SearchOperator.Equals, false },

        new object[] { true, new[] { "true" }, SearchOperator.NotEquals, false },
        new object[] { false, new[] { "false" }, SearchOperator.NotEquals, false },
        new object[] { true, new[] { "false" }, SearchOperator.NotEquals, true },
        new object[] { false, new[] { "true" }, SearchOperator.NotEquals, true },

        new object[] { true, new[] { "true" }, SearchOperator.Any, true },
        new object[] { false, new[] { "false" }, SearchOperator.Any, true },
        new object[] { true, new[] { "false" }, SearchOperator.Any, false },
        new object[] { false, new[] { "true" }, SearchOperator.Any, false },
        new object[] { true, Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableBoolTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { "true" }, SearchOperator.Equals, false },
        new object?[] { null, new[] { "false" }, SearchOperator.Equals, false },
        new object?[] { true, new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { false, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { "true" }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { "false" }, SearchOperator.NotEquals, true },
        new object?[] { true, new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { false, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { true, null, SearchOperator.Exists, true },
        new object?[] { false, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { true, null, SearchOperator.NotExists, false },
        new object?[] { false, null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { "true" }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public bool Bool { get; set; }
        public bool? NullableBool { get; set; }
    }
}
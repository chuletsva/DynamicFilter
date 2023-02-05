using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class DateTimeOffsetTests
{
    [Theory]
    [MemberData(nameof(DateTimeOffsetOffsetTestCases))]
    public void ShouldHandleDateTimeOffset(DateTimeOffset objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        objValue = new DateTimeOffset(
            objValue.Year, objValue.Month, objValue.Day,
            objValue.Hour, objValue.Minute, objValue.Second, objValue.Offset);

        TestClass obj = new() { DateTimeOffset = objValue };

        Condition condition = new(nameof(obj.DateTimeOffset), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetOffsetTestCases))]
    [MemberData(nameof(NullableDateTimeOffsetOffsetTestCases))]
    public void ShouldHandleNullableDateTimeOffset(DateTimeOffset? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        if (objValue is not null)
        {
            objValue = new DateTimeOffset(
                objValue.Value.Year, objValue.Value.Month, objValue.Value.Day,
                objValue.Value.Hour, objValue.Value.Minute, objValue.Value.Second, objValue.Value.Offset);
        }

        TestClass obj = new() { NullableDateTimeOffset = objValue };

        Condition condition = new(nameof(obj.NullableDateTimeOffset), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object?[]> DateTimeOffsetOffsetTestCases => new[]
    {
        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.Equals, true },
        new object[] { DateTimeOffset.UtcNow, new[] { DateTimeOffset.UtcNow.ToString("u") }, SearchOperator.Equals, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.Equals, true },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.Equals, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.Equals, false },

        new object[] { default(DateTimeOffset), new[]{ default(DateTimeOffset).ToString("u") }, SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.UtcNow, new[] { DateTimeOffset.UtcNow.ToString("u") }, SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.NotEquals, false },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.NotEquals, true },

        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.Greater, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.Greater, false },
        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.Greater, false },
        new object[] { DateTimeOffset.UtcNow, new[] { DateTimeOffset.UtcNow.ToString("u") }, SearchOperator.Greater, false },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.Greater, false },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.Greater, false },

        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.GreaterOrEqual, false },
        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.UtcNow, new[] { DateTimeOffset.UtcNow.ToString("u") }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.GreaterOrEqual, true },

        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.Less, true },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.Less, false },
        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.Less, false },
        new object[] { DateTimeOffset.UtcNow, new[] { DateTimeOffset.UtcNow.ToString("u") }, SearchOperator.Less, false },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.Less, false },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.Less, false },

        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.LessOrEqual, false },
        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.UtcNow, new[] { DateTimeOffset.UtcNow.ToString("u") }, SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.MinValue, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.LessOrEqual, true },
        new object[] { DateTimeOffset.MaxValue, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.LessOrEqual, true },

        new object[] { default(DateTimeOffset), new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.Any, true },
        new object[] { default(DateTimeOffset), new[] { DateTimeOffset.UtcNow.ToString("u") }, SearchOperator.Any, false },
        new object[] { default(DateTimeOffset), Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableDateTimeOffsetOffsetTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.Equals, false },
        new object?[] { null, new[] { DateTimeOffset.UtcNow.ToString("u") }, SearchOperator.Equals, false },
        new object?[] { default(DateTimeOffset), new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { DateTimeOffset.MaxValue, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { DateTimeOffset.UtcNow.ToString("u") }, SearchOperator.NotEquals, true },
        new object?[] { default(DateTimeOffset), new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { DateTimeOffset.MaxValue, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.Greater, false },
        new object?[] { null, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.Greater, false },
        new object?[] { default(DateTimeOffset), new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { DateTimeOffset.MaxValue, new string?[] { null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { DateTimeOffset.MinValue.ToString("u") }, SearchOperator.GreaterOrEqual, false },
        new object?[] { default(DateTimeOffset), new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { DateTimeOffset.MaxValue, new string?[] { null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.Less, false },
        new object?[] { null, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.Less, false },
        new object?[] { default(DateTimeOffset), new string?[] { null }, SearchOperator.Less, false },
        new object?[] { DateTimeOffset.MinValue, new string?[] { null }, SearchOperator.Less, false },

        new object?[] { null, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { DateTimeOffset.MaxValue.ToString("u") }, SearchOperator.LessOrEqual, false },
        new object?[] { default(DateTimeOffset), new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { DateTimeOffset.MinValue, new string?[] { null }, SearchOperator.LessOrEqual, false },

        new object?[] { DateTimeOffset.UtcNow, null, SearchOperator.Exists, true },
        new object?[] { default(DateTimeOffset), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { DateTimeOffset.UtcNow, null, SearchOperator.NotExists, false },
        new object?[] { default(DateTimeOffset), null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { default(DateTimeOffset).ToString("u") }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public DateTimeOffset DateTimeOffset { get; init; }
        public DateTimeOffset? NullableDateTimeOffset { get; init; }
    }
}

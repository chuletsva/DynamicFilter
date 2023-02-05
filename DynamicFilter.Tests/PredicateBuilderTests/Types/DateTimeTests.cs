using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class DateTimeTests
{
    [Theory]
    [MemberData(nameof(DateTimeTestCases))]
    public void ShouldHandleDateTime(DateTime objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        objValue = new DateTime(objValue.Year, objValue.Month, objValue.Day, objValue.Hour, objValue.Minute, objValue.Second, objValue.Kind);

        TestClass obj = new() { DateTime = objValue };

        Condition condition = new(nameof(obj.DateTime), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DateTimeTestCases))]
    [MemberData(nameof(NullableDateTimeTestCases))]
    public void ShouldHandleNullableDateTime(DateTime? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        if (objValue is not null)
        {
            objValue = new DateTime(objValue.Value.Year, objValue.Value.Month, objValue.Value.Day,
                objValue.Value.Hour, objValue.Value.Minute, objValue.Value.Second, objValue.Value.Kind);
        }

        TestClass obj = new() { NullableDateTime = objValue };

        Condition condition = new(nameof(obj.NullableDateTime), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object?[]> DateTimeTestCases => new[]
    {
        new object[] { default(DateTime), new[] { default(DateTime).ToString("u") }, SearchOperator.Equals, true },
        new object[] { DateTime.UtcNow, new[] { DateTime.UtcNow.ToString("u") }, SearchOperator.Equals, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.Equals, true },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.Equals, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.Equals, false },

        new object[] { default(DateTime), new[]{ default(DateTime).ToString("u") }, SearchOperator.NotEquals, false },
        new object[] { DateTime.UtcNow, new[] { DateTime.UtcNow.ToString("u") }, SearchOperator.NotEquals, false },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.NotEquals, false },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.NotEquals, false },
        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.NotEquals, true },

        new object[] { DateTime.MaxValue, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.Greater, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.Greater, false },
        new object[] { default(DateTime), new[] { default(DateTime).ToString("u") }, SearchOperator.Greater, false },
        new object[] { DateTime.UtcNow, new[] { DateTime.UtcNow.ToString("u") }, SearchOperator.Greater, false },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.Greater, false },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.Greater, false },

        new object[] { DateTime.MaxValue, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.GreaterOrEqual, false },
        new object[] { default(DateTime), new[] { default(DateTime).ToString("u") }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.UtcNow, new[] { DateTime.UtcNow.ToString("u") }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.GreaterOrEqual, true },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.GreaterOrEqual, true },

        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.Less, true },
        new object[] { DateTime.MaxValue, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.Less, false },
        new object[] { default(DateTime), new[] { default(DateTime).ToString("u") }, SearchOperator.Less, false },
        new object[] { DateTime.UtcNow, new[] { DateTime.UtcNow.ToString("u") }, SearchOperator.Less, false },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.Less, false },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.Less, false },

        new object[] { DateTime.MinValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.LessOrEqual, true },
        new object[] { DateTime.MaxValue, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.LessOrEqual, false },
        new object[] { default(DateTime), new[] { default(DateTime).ToString("u") }, SearchOperator.LessOrEqual, true },
        new object[] { DateTime.UtcNow, new[] { DateTime.UtcNow.ToString("u") }, SearchOperator.LessOrEqual, true },
        new object[] { DateTime.MinValue, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.LessOrEqual, true },
        new object[] { DateTime.MaxValue, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.LessOrEqual, true },

        new object[] { default(DateTime), new[] { default(DateTime).ToString("u") }, SearchOperator.Any, true },
        new object[] { default(DateTime), new[] { DateTime.UtcNow.ToString("u") }, SearchOperator.Any, false },
        new object[] { default(DateTime), Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableDateTimeTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { default(DateTime).ToString("u") }, SearchOperator.Equals, false },
        new object?[] { null, new[] { DateTime.UtcNow.ToString("u") }, SearchOperator.Equals, false },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { DateTime.MaxValue, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { default(DateTime).ToString("u") }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { DateTime.UtcNow.ToString("u") }, SearchOperator.NotEquals, true },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { DateTime.MaxValue, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { null, new[] { default(DateTime).ToString("u") }, SearchOperator.Greater, false },
        new object?[] { null, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.Greater, false },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { DateTime.MaxValue, new string?[] { null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { default(DateTime).ToString("u") }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { DateTime.MinValue.ToString("u") }, SearchOperator.GreaterOrEqual, false },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { DateTime.MaxValue, new string?[] { null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { null, new[] { default(DateTime).ToString("u") }, SearchOperator.Less, false },
        new object?[] { null, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.Less, false },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.Less, false },
        new object?[] { DateTime.MinValue, new string?[] { null }, SearchOperator.Less, false },

        new object?[] { null, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { default(DateTime).ToString("u") }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { DateTime.MaxValue.ToString("u") }, SearchOperator.LessOrEqual, false },
        new object?[] { default(DateTime), new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { DateTime.MinValue, new string?[] { null }, SearchOperator.LessOrEqual, false },

        new object?[] { DateTime.UtcNow, null, SearchOperator.Exists, true },
        new object?[] { default(DateTime), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { DateTime.UtcNow, null, SearchOperator.NotExists, false },
        new object?[] { default(DateTime), null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { default(DateTime).ToString("u") }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public DateTime DateTime { get; init; }
        public DateTime? NullableDateTime { get; init; }
    }
}
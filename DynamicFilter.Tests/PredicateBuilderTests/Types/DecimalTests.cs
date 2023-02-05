using System.Globalization;
using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Models;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class DecimalTests
{
    [Theory]
    [MemberData(nameof(DecimalTestCases))]
    public void ShouldHandleDecimal(decimal objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Decimal = objValue };

        Condition condition = new(nameof(obj.Decimal), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DecimalTestCases))]
    [MemberData(nameof(NullableDecimalTestCases))]
    public void ShouldHandleNullableDecimal(decimal? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableDecimal = objValue };

        Condition condition = new(nameof(obj.NullableDecimal), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> DecimalTestCases => new[]
    {
        new object[] { 0M, new[] { "0" }, SearchOperator.Equals, true },
        new object[] { decimal.Zero, new[] { decimal.Zero.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { decimal.One, new[] { decimal.One.ToString(CultureInfo.InvariantCulture)}, SearchOperator.Equals, true },
        new object[] { decimal.MinusOne, new[] { decimal.MinusOne.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { decimal.MinValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { decimal.MaxValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { decimal.MinValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object[] { decimal.MaxValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object[] { 1M, new[] { "1.0" }, SearchOperator.Equals, true },

        new object[] { 0M, new[] { "0" }, SearchOperator.NotEquals, false },
        new object[] { decimal.Zero, new[] { decimal.Zero.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { decimal.One, new[] { decimal.One.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { decimal.MinusOne, new[] { decimal.MinusOne.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { decimal.MinValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { decimal.MaxValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { decimal.MinValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object[] { decimal.MaxValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object[] { 1M, new[] { "1.0" }, SearchOperator.NotEquals, false },

        new object[] { decimal.MaxValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, true },
        new object[] { decimal.MinValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { 0M, new[] { "0" }, SearchOperator.Greater, false },
        new object[] { decimal.Zero, new[] { decimal.Zero.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { decimal.One, new[] { decimal.One.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { decimal.MinusOne, new[] { decimal.MinusOne.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { decimal.MinValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { decimal.MaxValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { 1.1M, new[] { "1.0" }, SearchOperator.Greater, true },

        new object[] { decimal.MaxValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MinValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object[] { 0M, new[] { "0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.Zero, new[] { decimal.Zero.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.One, new[] { decimal.One.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MinusOne, new[] { decimal.MinusOne.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MinValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { decimal.MaxValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { 1.1M, new[] { "1.0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { 1M, new[] { "1.0" }, SearchOperator.GreaterOrEqual, true },

        new object[] { decimal.MinValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, true },
        new object[] { decimal.MaxValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { 0M, new[] { "0" }, SearchOperator.Less, false },
        new object[] { decimal.Zero, new[] { decimal.Zero.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { decimal.One, new[] { decimal.One.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { decimal.MinusOne, new[] { decimal.MinusOne.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { decimal.MinValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { decimal.MaxValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { 1.0M, new[] { "1.1" }, SearchOperator.Less, true },

        new object[] { decimal.MinValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { decimal.MaxValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object[] { 0M, new[] { "0" }, SearchOperator.LessOrEqual, true },
        new object[] { decimal.Zero, new[] { decimal.Zero.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { decimal.One, new[] { decimal.One.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { decimal.MinusOne, new[] { decimal.MinusOne.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { decimal.MinValue, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { decimal.MaxValue, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { 1.0M, new[] { "1.1" }, SearchOperator.LessOrEqual, true },
        new object[] { 1M, new[] { "1.0" }, SearchOperator.LessOrEqual, true },

        new object[] { 0M, new[] { "0" }, SearchOperator.Any, true },
        new object[] { 0M, new[] { "1" }, SearchOperator.Any, false },
        new object[] { 0M, Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableDecimalTestCases => new[]
    {
        new object?[] { null, new string?[]{ null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { "0" }, SearchOperator.Equals, false },
        new object?[] { null, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object?[] { 0M, new string?[]{ null }, SearchOperator.Equals, false },
        new object?[] { decimal.MaxValue, new string?[]{ null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { "0" }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object?[] { 0M, new string?[]{ null }, SearchOperator.NotEquals, true },
        new object?[] { decimal.MaxValue, new string?[]{ null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[]{ null }, SearchOperator.Greater, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Greater, false },
        new object?[] { null, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object?[] { 0M, new string?[]{ null }, SearchOperator.Greater, false },
        new object?[] { decimal.MaxValue, new string?[]{ null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { decimal.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object?[] { 0M, new string?[]{ null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { decimal.MaxValue, new string?[]{ null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.Less, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Less, false },
        new object?[] { null, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object?[] { 0M, new string?[]{ null }, SearchOperator.Less, false },
        new object?[] { decimal.MinValue, new string?[]{ null }, SearchOperator.Less, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { decimal.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object?[] { 0M, new string?[]{ null }, SearchOperator.LessOrEqual, false },
        new object?[] { decimal.MinValue, new string?[]{ null }, SearchOperator.LessOrEqual, false },

        new object?[] { decimal.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { 0M, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { decimal.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { 0M, null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public decimal Decimal { get; init; }
        public decimal? NullableDecimal { get; init; }
    }
}
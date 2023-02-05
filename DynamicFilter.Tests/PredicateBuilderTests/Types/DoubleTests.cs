using System.Globalization;
using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Models;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class DoubleTests
{
    [Theory]
    [MemberData(nameof(DoubleTestCases))]
    public void ShouldHandleDouble(double objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Double = objValue };

        Condition condition = new(nameof(obj.Double), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(DoubleTestCases))]
    [MemberData(nameof(NullableDoubleTestCases))]
    public void ShouldHandleNullableDouble(double? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableDouble = objValue };

        Condition condition = new(nameof(obj.NullableDouble), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> DoubleTestCases => new[]
    {
        new object[] { 0D, new[] { "0" }, SearchOperator.Equals, true },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object[] { 1D, new[] { "1.0" }, SearchOperator.Equals, true },

        new object[] { 0D, new[] { "0" }, SearchOperator.NotEquals, false },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object[] { 1D, new[] { "1.0" }, SearchOperator.NotEquals, false },

        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, true },
        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { double.PositiveInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, true },
        new object[] { 0D, new[] { "0" }, SearchOperator.Greater, false },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { 1.1D, new[] { "1.0" }, SearchOperator.Greater, true },

        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object[] { double.PositiveInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { 0D, new[] { "0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { 1.1D, new[] { "1.0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { 1D, new[] { "1.0" }, SearchOperator.GreaterOrEqual, true },

        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, true },
        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { double.NegativeInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, true },
        new object[] { 0D, new[] { "0" }, SearchOperator.Less, false },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { 1.0D, new[] { "1.1" }, SearchOperator.Less, true },

        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object[] { double.NegativeInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { 0D, new[] { "0" }, SearchOperator.LessOrEqual, true },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { 1.0D, new[]{ "1.1" }, SearchOperator.LessOrEqual, true },
        new object[] { 1D, new[] { "1.0" }, SearchOperator.LessOrEqual, true },

        new object[] { 0D, new[] { "0" }, SearchOperator.Any, true },
        new object[] { 0D, new[] { "1" }, SearchOperator.Any, false },
        new object[] { 0D, Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableDoubleTestCases => new[]
    {
        new object[] { 0D, new[] { "0" }, SearchOperator.Equals, true },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object[] { 1D, new[] { "1.0" }, SearchOperator.Equals, true },

        new object[] { 0D, new[] { "0" }, SearchOperator.NotEquals, false },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object[] { 1D, new[] { "1.0" }, SearchOperator.NotEquals, false },

        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, true },
        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { double.PositiveInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, true },
        new object[] { 0D, new[] { "0" }, SearchOperator.Greater, false },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { 1.1D, new[] { "1.0" }, SearchOperator.Greater, true },

        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object[] { double.PositiveInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { 0D, new[] { "0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { 1.1D, new[] { "1.0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { 1D, new[] { "1.0" }, SearchOperator.GreaterOrEqual, true },

        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, true },
        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { double.NegativeInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, true },
        new object[] { 0D,  new[] { "0" }, SearchOperator.Less, false },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { 1.0D, new[] { "1.1" }, SearchOperator.Less, true },

        new object[] { double.MinValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { double.MaxValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object[] { double.NegativeInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { 0D, new[] { "0" }, SearchOperator.LessOrEqual, true },
        new object[] { double.MinValue, new[] { double.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { double.MaxValue, new[] { double.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { double.NegativeInfinity, new[] { double.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { double.PositiveInfinity, new[] { double.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { double.Epsilon, new[] { double.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { 1.0D, new[] { "1.1" }, SearchOperator.LessOrEqual, true },
        new object[] { 1D, new[] { "1.0" }, SearchOperator.LessOrEqual, true },

        new object?[] { double.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { 0D, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { double.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { 0D, null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public double Double { get; init; }
        public double? NullableDouble { get; init; }
    }
}
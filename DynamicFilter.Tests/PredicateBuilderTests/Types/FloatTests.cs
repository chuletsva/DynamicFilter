using System.Globalization;
using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class FloatTests
{
    [Theory]
    [MemberData(nameof(FloatTestCases))]
    public void ShouldHandleFloat(float objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Float = objValue };

        Condition condition = new(nameof(obj.Float), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(FloatTestCases))]
    [MemberData(nameof(NullableFloatTestCases))]
    public void ShouldHandleNullableFloat(float? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableFloat = objValue };

        Condition condition = new(nameof(obj.NullableFloat), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> FloatTestCases => new[]
    {
        new object[] { 0F, new [] { "0" }, SearchOperator.Equals, true },
        new object[] { float.MinValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { float.MaxValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { float.NegativeInfinity, new[] { float.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { float.PositiveInfinity, new[] { float.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { float.Epsilon, new[] { float.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, true },
        new object[] { float.MinValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object[] { float.MaxValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object[] { 1F, new[] { "1.0" }, SearchOperator.Equals, true },

        new object[] { 0F, new [] { "0" }, SearchOperator.NotEquals, false },
        new object[] { float.MinValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { float.MaxValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { float.NegativeInfinity, new[] { float.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { float.PositiveInfinity, new[] { float.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { float.Epsilon, new[] { float.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, false },
        new object[] { float.MinValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object[] { float.MaxValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object[] { 1F, new[] { "1.0" }, SearchOperator.NotEquals, false },

        new object[] { float.MaxValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, true },
        new object[] { float.MinValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { float.PositiveInfinity, new[] { float.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, true },
        new object[] { 0F, new [] { "0" }, SearchOperator.Greater, false },
        new object[] { float.MinValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { float.MaxValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { float.NegativeInfinity, new[] { float.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { float.PositiveInfinity, new[] { float.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { float.Epsilon, new[] { float.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object[] { 1.1F, new[] { "1.0" }, SearchOperator.Greater, true },

        new object[] { float.MaxValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { float.MinValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object[] { float.PositiveInfinity, new[] { float.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { 0F, new [] { "0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { float.MinValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { float.MaxValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { float.NegativeInfinity, new[] { float.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { float.PositiveInfinity, new[] { float.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { float.Epsilon, new[] { float.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, true },
        new object[] { 1.1F, new[] { "1.0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { 1F, new[] { "1.0" }, SearchOperator.GreaterOrEqual, true },

        new object[] { float.MinValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, true },
        new object[] { float.MaxValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { float.NegativeInfinity, new[] { float.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, true },
        new object[] { 0F, new [] { "0" }, SearchOperator.Less, false },
        new object[] { float.MinValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { float.MaxValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { float.NegativeInfinity, new[] { float.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { float.PositiveInfinity, new[] { float.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { float.Epsilon, new[] { float.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object[] { 1.0F, new[] { "1.1" }, SearchOperator.Less, true },

        new object[] { float.MinValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { float.MaxValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object[] { float.NegativeInfinity, new[] { float.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { 0F, new [] { "0" }, SearchOperator.LessOrEqual, true },
        new object[] { float.MinValue, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { float.MaxValue, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { float.NegativeInfinity, new[] { float.NegativeInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { float.PositiveInfinity, new[] { float.PositiveInfinity.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { float.Epsilon, new[] { float.Epsilon.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, true },
        new object[] { 1.0F, new[] { "1.1" }, SearchOperator.LessOrEqual, true },
        new object[] { 1F, new[] { "1.0" }, SearchOperator.LessOrEqual, true },

        new object[] { 0F, new[] { "0" }, SearchOperator.Any, true },
        new object[] { 0F, new[] { "1" }, SearchOperator.Any, false },
        new object[] { 0F, Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableFloatTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { "0" }, SearchOperator.Equals, false },
        new object?[] { null, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Equals, false },
        new object?[] { 0F, new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { float.MaxValue, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { "0" }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.NotEquals, true },
        new object?[] { 0F, new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { float.MaxValue, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Greater, false },
        new object?[] { null, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Greater, false },
        new object?[] { 0F, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { float.MaxValue, new string?[] { null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { float.MaxValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.GreaterOrEqual, false },
        new object?[] { 0F, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { float.MinValue, new string?[] { null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Less, false },
        new object?[] { null, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.Less, false },
        new object?[] { 0F, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { float.MaxValue, new string?[] { null }, SearchOperator.Less, false },

        new object?[] { null, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { float.MinValue.ToString(CultureInfo.InvariantCulture) }, SearchOperator.LessOrEqual, false },
        new object?[] { 0F, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { float.MaxValue, new string?[] { null }, SearchOperator.LessOrEqual, false },

        new object?[] { float.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { 0F, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { float.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { 0F, null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public float Float { get; init; }
        public float? NullableFloat { get; init; }
    }
}
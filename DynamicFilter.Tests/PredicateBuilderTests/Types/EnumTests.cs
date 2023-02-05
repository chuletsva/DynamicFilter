using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class EnumTests
{
    [Theory]
    [MemberData(nameof(EnumTestCases))]
    public void ShouldHandleEnum(TestEnum objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Enum = objValue };

        Condition condition = new(nameof(obj.Enum), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(EnumTestCases))]
    [MemberData(nameof(NullableEnumTestCases))]
    public void ShouldHandleNullableEnum(TestEnum? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableEnum = objValue };

        Condition condition = new(nameof(obj.NullableEnum), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> EnumTestCases => new[]
    {
        new object[] { TestEnum.One, new[] { nameof(TestEnum.One)}, SearchOperator.Equals, true },
        new object[] { TestEnum.One, new[] { nameof(TestEnum.Two) }, SearchOperator.Equals, false },

        new object[] { TestEnum.One, new[] { nameof(TestEnum.One) }, SearchOperator.NotEquals, false },
        new object[] { TestEnum.One, new[] { nameof(TestEnum.Two) }, SearchOperator.NotEquals, true },

        new object[] { TestEnum.One, new[] { nameof(TestEnum.One) }, SearchOperator.Any, true },
        new object[] { TestEnum.One, new[] { nameof(TestEnum.Two) }, SearchOperator.Any, false },
        new object[] { TestEnum.One, Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableEnumTestCases => new[]
    {
        new object?[] { null, new string?[]{ null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { default(TestEnum).ToString() }, SearchOperator.Equals, false },
        new object?[] { null, new[] { nameof(TestEnum.One) }, SearchOperator.Equals, false },
        new object?[] { default(TestEnum), new string?[]{ null }, SearchOperator.Equals, false },
        new object?[] { TestEnum.One, new string?[]{ null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { default(TestEnum).ToString() }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { nameof(TestEnum.One) }, SearchOperator.NotEquals, true },
        new object?[] { default(TestEnum), new string?[]{ null }, SearchOperator.NotEquals, true },
        new object?[] { TestEnum.One, new string?[]{ null }, SearchOperator.NotEquals, true },

        new object?[] { TestEnum.One, new string?[]{ null }, SearchOperator.Exists, true },
        new object?[] { default(TestEnum), new string?[]{ null }, SearchOperator.Exists, true },
        new object?[] { null, new string?[]{ null }, SearchOperator.Exists, false },

        new object?[] { null, new string?[]{ null }, SearchOperator.NotExists, true },
        new object?[] { TestEnum.One, new string?[]{ null }, SearchOperator.NotExists, false },
        new object?[] { default(TestEnum), new string?[]{ null }, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { nameof(TestEnum.One) }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    public enum TestEnum { One, Two }

    private class TestClass
    {
        public TestEnum Enum { get; init; }
        public TestEnum? NullableEnum { get; init; }
    }
}
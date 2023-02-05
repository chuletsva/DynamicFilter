using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Models;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class ByteTests
{
    [Theory]
    [MemberData(nameof(ByteTestCases))]
    public void ShouldHandleByte(byte objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Byte = objValue };

        Condition condition = new(nameof(obj.Byte), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(ByteTestCases))]
    [MemberData(nameof(NullableByteTestCases))]
    public void ShouldHandleNullableByte(byte? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableByte = objValue };

        Condition condition = new(nameof(obj.NullableByte), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> ByteTestCases => new[]
    {
        new object[] { (byte)0, new[] { "0" }, SearchOperator.Equals, true },
        new object[] { byte.MinValue, new[] { byte.MinValue.ToString() }, SearchOperator.Equals, true },
        new object[] { byte.MaxValue, new[] { byte.MaxValue.ToString() }, SearchOperator.Equals, true },
        new object[] { byte.MinValue, new[] { byte.MaxValue.ToString() }, SearchOperator.Equals, false },
        new object[] { byte.MaxValue, new[] { byte.MinValue.ToString() }, SearchOperator.Equals, false },

        new object[] { (byte)0, new[] { "0" }, SearchOperator.NotEquals, false },
        new object[] { byte.MinValue, new[] { byte.MinValue.ToString() }, SearchOperator.NotEquals, false },
        new object[] { byte.MaxValue, new[] { byte.MaxValue.ToString() }, SearchOperator.NotEquals, false },
        new object[] { byte.MinValue, new[] { byte.MaxValue.ToString() }, SearchOperator.NotEquals, true },
        new object[] { byte.MaxValue, new[] { byte.MinValue.ToString() }, SearchOperator.NotEquals, true },

        new object[] { byte.MaxValue, new[] { byte.MinValue.ToString() }, SearchOperator.Greater, true },
        new object[] { byte.MinValue, new[] { byte.MaxValue.ToString() }, SearchOperator.Greater, false },
        new object[] { (byte)0, new[] { "0" }, SearchOperator.Greater, false },
        new object[] { byte.MinValue, new[] { byte.MinValue.ToString() }, SearchOperator.Greater, false },
        new object[] { byte.MaxValue, new[] { byte.MaxValue.ToString() }, SearchOperator.Greater, false },

        new object[] { byte.MaxValue, new[] { byte.MinValue.ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { byte.MinValue, new[] { byte.MaxValue.ToString() }, SearchOperator.GreaterOrEqual, false },
        new object[] { (byte)0, new[] { "0" }, SearchOperator.GreaterOrEqual, true },
        new object[] { byte.MinValue, new[] { byte.MinValue.ToString() }, SearchOperator.GreaterOrEqual, true },
        new object[] { byte.MaxValue, new[] { byte.MaxValue.ToString() }, SearchOperator.GreaterOrEqual, true },

        new object[] { byte.MinValue, new[] { byte.MaxValue.ToString() }, SearchOperator.Less, true },
        new object[] { byte.MaxValue, new[] { byte.MinValue.ToString() }, SearchOperator.Less, false },
        new object[] { (byte)0, new[] { "0" }, SearchOperator.Less, false },
        new object[] { byte.MinValue, new[] { byte.MinValue.ToString() }, SearchOperator.Less, false },
        new object[] { byte.MaxValue, new[] { byte.MaxValue.ToString() }, SearchOperator.Less, false },

        new object[] { byte.MinValue, new[] { byte.MaxValue.ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { byte.MaxValue, new[] { byte.MinValue.ToString() }, SearchOperator.LessOrEqual, false },
        new object[] { (byte)0, new[] { "0" }, SearchOperator.LessOrEqual, true },
        new object[] { byte.MinValue, new[] { byte.MinValue.ToString() }, SearchOperator.LessOrEqual, true },
        new object[] { byte.MaxValue, new[] { byte.MaxValue.ToString() }, SearchOperator.LessOrEqual, true },

        new object[] { (byte)0, new[] { "0" }, SearchOperator.Any, true },
        new object[] { (byte)0, new[] { "1" }, SearchOperator.Any, false },
        new object[] { (byte)0, Array.Empty<string?>(), SearchOperator.Any, false },
    };

    public static IEnumerable<object?[]> NullableByteTestCases => new[]
    {
        new object?[] { null, new string?[]{ null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { "0" }, SearchOperator.Equals, false },
        new object?[] { null, new[] { byte.MaxValue.ToString() }, SearchOperator.Equals, false },
        new object?[] { (byte)0, new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { byte.MaxValue, new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { "0" }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { byte.MaxValue.ToString() }, SearchOperator.NotEquals, true },
        new object?[] { (byte)0, new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { byte.MaxValue, new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { null, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Greater, false },
        new object?[] { null, new[] { byte.MinValue.ToString() }, SearchOperator.Greater, false },
        new object?[] { (byte)0, new string?[] { null }, SearchOperator.Greater, false },
        new object?[] { byte.MaxValue, new string?[] { null }, SearchOperator.Greater, false },

        new object?[] { null, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.GreaterOrEqual, false },
        new object?[] { null, new[] { byte.MinValue.ToString() }, SearchOperator.GreaterOrEqual, false },
        new object?[] { (byte)0, new string?[] { null }, SearchOperator.GreaterOrEqual, false },
        new object?[] { byte.MaxValue, new string?[] { null }, SearchOperator.GreaterOrEqual, false },

        new object?[] { null, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Less, false },
        new object?[] { null, new[] { byte.MaxValue.ToString() }, SearchOperator.Less, false },
        new object?[] { (byte)0, new string?[] { null }, SearchOperator.Less, false },
        new object?[] { byte.MinValue, new string?[] { null }, SearchOperator.Less, false },

        new object?[] { null, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { "0" }, SearchOperator.LessOrEqual, false },
        new object?[] { null, new[] { byte.MaxValue.ToString() }, SearchOperator.LessOrEqual, false },
        new object?[] { (byte)0, new string?[] { null }, SearchOperator.LessOrEqual, false },
        new object?[] { byte.MinValue, new string?[] { null }, SearchOperator.LessOrEqual, false },

        new object?[] { byte.MaxValue, null, SearchOperator.Exists, true },
        new object?[] { (byte)0, null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { byte.MaxValue, null, SearchOperator.NotExists, false },
        new object?[] { (byte)0, null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { "0" }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public byte Byte { get; set; }
        public byte? NullableByte { get; set; }
    }
}
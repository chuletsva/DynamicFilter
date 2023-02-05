using System.Linq.Expressions;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;
using FluentAssertions;

namespace DynamicFilter.Tests.PredicateBuilderTests.Types;

public class GuidTests
{
    [Theory]
    [MemberData(nameof(GuidTestCases))]
    public void ShouldHandleGuid(Guid objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { Guid = objValue };

        Condition condition = new(nameof(obj.Guid), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(GuidTestCases))]
    [MemberData(nameof(NullableGuidTestCases))]
    public void ShouldHandleNullableGuid(Guid? objValue, string?[] searchValue, SearchOperator searchOperator, bool result)
    {
        TestClass obj = new() { NullableGuid = objValue };

        Condition condition = new(nameof(obj.NullableGuid), searchValue, searchOperator);

        var lambda = (Expression<Func<TestClass, bool>>)PredicateBuilder.BuildPredicate(typeof(TestClass), new[] { condition });

        Func<TestClass, bool> func = lambda.Compile();

        func(obj).Should().Be(result);
    }

    public static IEnumerable<object[]> GuidTestCases
    {
        get
        {
            Guid guid = Guid.NewGuid();

            yield return new object[] { guid, new[] { guid.ToString() }, SearchOperator.Equals, true };
            yield return new object[] { Guid.Empty, new[] { Guid.Empty.ToString() }, SearchOperator.Equals, true };
            yield return new object[] { Guid.NewGuid(), new[] { Guid.NewGuid().ToString() }, SearchOperator.Equals, false };

            yield return new object[] { guid, new[] { guid.ToString() }, SearchOperator.NotEquals, false };
            yield return new object[] { Guid.Empty, new[] { Guid.Empty.ToString() }, SearchOperator.NotEquals, false };
            yield return new object[] { Guid.NewGuid(), new[] { Guid.NewGuid().ToString() }, SearchOperator.NotEquals, true };

            yield return new object[] { guid, new[] { guid.ToString() }, SearchOperator.Any, true };
            yield return new object[] { guid, new[] { Guid.NewGuid().ToString() }, SearchOperator.Any, false };
            yield return new object[] { guid, Array.Empty<string?>(), SearchOperator.Any, false };
        }
    }

    public static IEnumerable<object?[]> NullableGuidTestCases => new[]
    {
        new object?[] { null, new string?[] { null }, SearchOperator.Equals, true },
        new object?[] { null, new[] { default(Guid).ToString() }, SearchOperator.Equals, false },
        new object?[] { null, new[] { Guid.NewGuid().ToString() }, SearchOperator.Equals, false },
        new object?[] { default(Guid), new string?[] { null }, SearchOperator.Equals, false },
        new object?[] { Guid.NewGuid(), new string?[] { null }, SearchOperator.Equals, false },

        new object?[] { null, new string?[] { null }, SearchOperator.NotEquals, false },
        new object?[] { null, new[] { default(Guid).ToString() }, SearchOperator.NotEquals, true },
        new object?[] { null, new[] { Guid.NewGuid().ToString() }, SearchOperator.NotEquals, true },
        new object?[] { default(Guid), new string?[] { null }, SearchOperator.NotEquals, true },
        new object?[] { Guid.NewGuid(), new string?[] { null }, SearchOperator.NotEquals, true },

        new object?[] { Guid.NewGuid(), null, SearchOperator.Exists, true },
        new object?[] { default(Guid), null, SearchOperator.Exists, true },
        new object?[] { null, null, SearchOperator.Exists, false },

        new object?[] { null, null, SearchOperator.NotExists, true },
        new object?[] { Guid.NewGuid(), null, SearchOperator.NotExists, false },
        new object?[] { default(Guid), null, SearchOperator.NotExists, false },

        new object?[] { null, Array.Empty<string?>(), SearchOperator.Any, false },
        new object?[] { null, new[] { Guid.NewGuid().ToString() }, SearchOperator.Any, false },
        new object?[] { null, new string?[] { null }, SearchOperator.Any, true }
    };

    private class TestClass
    {
        public Guid Guid { get; init; }
        public Guid? NullableGuid { get; init; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;

namespace DynamicFilter;

internal static class OperationProcessor
{
    public static IQueryable ApplyOperation(IQueryable queryable, OperationBase operation)
    {
        return operation switch
        {
            WhereOperation x => Where(queryable, x),
            DistinctOperation x => Distinct(queryable, x),
            SkipOperation x => Skip(queryable, x),
            TakeOperation x => Take(queryable, x),
            OrderByOperation x => OrderBy(queryable, x),
            OrderByDescendingOperation x => OrderByDescending(queryable, x),
            ThenByOperation x => ThenBy(queryable, x),
            ThenByDescendingOperation x => ThenByDescending(queryable, x),
            SelectOperation x => Select(queryable, x),
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        };
    }

    internal static IQueryable Where(IQueryable queryable, WhereOperation operation)
    {
        LambdaExpression lambdaExpr = PredicateBuilder.BuildPredicate(queryable.ElementType, operation.Conditions, operation.Groups);

        var method = QueryableMethods.Where(queryable.ElementType);

        var filteredQueryable = method.Invoke(null, new object[] { queryable, lambdaExpr }) ?? throw new NullReferenceException();

        return (IQueryable)filteredQueryable;
    }

    internal static IQueryable Distinct(IQueryable queryable, DistinctOperation operation)
    {
        var method = QueryableMethods.Distinct(queryable.ElementType);

        var distinctQueryable = method.Invoke(null, new object[] { queryable }) ?? throw new NullReferenceException();

        return (IQueryable)distinctQueryable;
    }

    internal static IQueryable Skip(IQueryable queryable, SkipOperation operation)
    {
        var method = QueryableMethods.Skip(queryable.ElementType);

        var skipQueryable = method.Invoke(null, new object[] { queryable, operation.Count }) ?? throw new NullReferenceException();

        return (IQueryable)skipQueryable;
    }

    internal static IQueryable Take(IQueryable queryable, TakeOperation operation)
    {
        var method = QueryableMethods.Take(queryable.ElementType);

        var topQueryable = method.Invoke(null, new object[] { queryable, operation.Count }) ?? throw new NullReferenceException();

        return (IQueryable)topQueryable;
    }

    internal static IQueryable OrderBy(IQueryable queryable, OrderByOperation operation)
    {
        PropertyInfo property = ReflectionHelper.GetProperty(queryable.ElementType, operation.PropertyName);

        MethodInfo method = QueryableMethods.OrderBy(queryable.ElementType, property.PropertyType);

        return ApplySorting(queryable, method, property);
    }

    internal static IQueryable OrderByDescending(IQueryable queryable, OrderByDescendingOperation operation)
    {
        PropertyInfo property = ReflectionHelper.GetProperty(queryable.ElementType, operation.PropertyName);

        MethodInfo method = QueryableMethods.OrderByDescending(queryable.ElementType, property.PropertyType);

        return ApplySorting(queryable, method, property);
    }

    internal static IQueryable ThenBy(IQueryable queryable, ThenByOperation operation)
    {
        PropertyInfo property = ReflectionHelper.GetProperty(queryable.ElementType, operation.PropertyName);

        MethodInfo method = QueryableMethods.ThenBy(queryable.ElementType, property.PropertyType);

        return ApplySorting(queryable, method, property);
    }

    internal static IQueryable ThenByDescending(IQueryable queryable, ThenByDescendingOperation operation)
    {
        PropertyInfo property = ReflectionHelper.GetProperty(queryable.ElementType, operation.PropertyName);

        MethodInfo method = QueryableMethods.ThenByDescending(queryable.ElementType, property.PropertyType);

        return ApplySorting(queryable, method, property);
    }

    internal static IQueryable ApplySorting(IQueryable queryable, MethodInfo method, PropertyInfo property)
    {
        ParameterExpression paramExpr = Expression.Parameter(queryable.ElementType, "x");
        MemberExpression propExpr = Expression.Property(paramExpr, property);
        Expression keySelector = Expression.Lambda(propExpr, paramExpr);

        var sortedQueryable = method.Invoke(null, new object[] { queryable, keySelector }) ?? throw new NullReferenceException();

        return (IQueryable)sortedQueryable;
    }

    internal static IQueryable Select(IQueryable queryable, SelectOperation operation)
    {
        var properties = ReflectionHelper.GetProperties(queryable.ElementType, operation.Properties).ToArray();

        ParameterExpression paramExpr = Expression.Parameter(queryable.ElementType, "x");

        Type dictType = typeof(Dictionary<string, object>);

        var addMethod = dictType.GetMethod("Add") ?? throw new NullReferenceException();

        ListInitExpression bodyExpr = Expression.ListInit(
            Expression.New(dictType),
            properties.Select(x => Expression.ElementInit(
                addMethod,
                Expression.Constant(x.Name),
                Expression.Convert(Expression.Property(paramExpr, x), typeof(object)))));

        MethodInfo method = QueryableMethods.Select(queryable.ElementType, dictType);

        LambdaExpression lambdaExpr = Expression.Lambda(bodyExpr, paramExpr);

        var selectQueryable = method.Invoke(null, new object[] { queryable, lambdaExpr }) ?? throw new NullReferenceException();

        return (IQueryable)selectQueryable;
    }
}
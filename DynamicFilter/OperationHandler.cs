using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DynamicFilter.Arguments;
using DynamicFilter.Exceptions;
using DynamicFilter.Helpers;
using DynamicFilter.Models;

namespace DynamicFilter;

internal static class OperationHandler
{
    public static IQueryable ApplyOperation(IQueryable queryable, Operation operation)
    {
        return operation.Arguments switch
        {
            WhereArgs x => Where(queryable, x),
            DistinctArgs => Distinct(queryable),
            SkipArgs x => Skip(queryable, x),
            TakeArgs x => Take(queryable, x),
            OrderByArgs x => OrderBy(queryable, x),
            OrderByDescendingArgs x => OrderByDescending(queryable, x),
            ThenByArgs x => ThenBy(queryable, x),
            ThenByDescendingArgs x => ThenByDescending(queryable, x),
            SelectArgs x => Select(queryable, x),
            _ => throw new ArgumentOutOfRangeException(nameof(operation.Arguments))
        };
    }

    internal static IQueryable Where(IQueryable queryable, WhereArgs args)
    {
        if (args.Conditions.Length is 0)
        {
            throw new DynamicFilterException("Where arguments must contain at least one condition");
        }

        LambdaExpression lambdaExpr = PredicateBuilder.BuildPredicate(queryable.ElementType, args.Conditions, args.Groups);

        var method = QueryableMethods.Where(queryable.ElementType);

        var filteredQueryable = method.Invoke(null, new object[] { queryable, lambdaExpr }) ?? throw new NullReferenceException();

        return (IQueryable)filteredQueryable;
    }

    internal static IQueryable Distinct(IQueryable queryable)
    {
        var method = QueryableMethods.Distinct(queryable.ElementType);

        var distinctQueryable = method.Invoke(null, new object[] { queryable }) ?? throw new NullReferenceException();

        return (IQueryable)distinctQueryable;
    }

    internal static IQueryable Skip(IQueryable queryable, SkipArgs args)
    {
        if (args.Count < 1)
        {
            throw new DynamicFilterException("Skip argument must be greather than 0");
        }

        var method = QueryableMethods.Skip(queryable.ElementType);

        var skipQueryable = method.Invoke(null, new object[] { queryable, args.Count }) ?? throw new NullReferenceException();

        return (IQueryable)skipQueryable;
    }

    internal static IQueryable Take(IQueryable queryable, TakeArgs args)
    {
        if (args.Count < 1)
        {
            throw new DynamicFilterException("Take argument must be greather than 0");
        }

        var method = QueryableMethods.Take(queryable.ElementType);

        var topQueryable = method.Invoke(null, new object[] { queryable, args.Count }) ?? throw new NullReferenceException();

        return (IQueryable)topQueryable;
    }

    internal static IQueryable OrderBy(IQueryable queryable, OrderByArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.PropertyName))
        {
            throw new DynamicFilterException("OrderBy argument should not be empty");
        }

        PropertyInfo property = ReflectionHelper.GetProperty(queryable.ElementType, args.PropertyName);

        MethodInfo method = QueryableMethods.OrderBy(queryable.ElementType, property.PropertyType);

        return ApplySorting(queryable, method, property);
    }

    internal static IQueryable OrderByDescending(IQueryable queryable, OrderByDescendingArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.PropertyName))
        {
            throw new DynamicFilterException("OrderByDescending argument should not be empty");
        }

        PropertyInfo property = ReflectionHelper.GetProperty(queryable.ElementType, args.PropertyName);

        MethodInfo method = QueryableMethods.OrderByDescending(queryable.ElementType, property.PropertyType);

        return ApplySorting(queryable, method, property);
    }

    internal static IQueryable ThenBy(IQueryable queryable, ThenByArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.PropertyName))
        {
            throw new DynamicFilterException("ThenBy argument should not be empty");
        }

        PropertyInfo property = ReflectionHelper.GetProperty(queryable.ElementType, args.PropertyName);

        MethodInfo method = QueryableMethods.ThenBy(queryable.ElementType, property.PropertyType);

        return ApplySorting(queryable, method, property);
    }

    internal static IQueryable ThenByDescending(IQueryable queryable, ThenByDescendingArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.PropertyName))
        {
            throw new DynamicFilterException("ThenByDescending argument should not be empty");
        }

        PropertyInfo property = ReflectionHelper.GetProperty(queryable.ElementType, args.PropertyName);

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

    internal static IQueryable Select(IQueryable queryable, SelectArgs args)
    {
        if (args.Properties.Length is 0)
        {
            throw new DynamicFilterException("Select arguments must contain at least one value");
        }

        if (args.Properties.Any(string.IsNullOrWhiteSpace))
        {
            throw new DynamicFilterException("Select arguments should not contain empty values");
        }

        var properties = ReflectionHelper.GetProperties(queryable.ElementType, args.Properties).ToArray();

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
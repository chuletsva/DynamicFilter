using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace DynamicFilter.Helpers;

internal static class QueryableMethods
{
    private static readonly ConcurrentDictionary<(string MethodName, Type Type1, Type Type2), MethodInfo> Cache = new();

    public static MethodInfo Where(Type elementType)
    {
        return Cache.GetOrAdd(("Where", elementType, null!), key =>
        {
            return typeof(Queryable).GetMethods()
                .First(x => x.Name == key.MethodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(key.Type1);
        });
    }

    public static MethodInfo OrderBy(Type elementType, Type elementPropertyType)
    {
        return Cache.GetOrAdd(("OrderBy", elementType, elementPropertyType), key =>
        {
            return typeof(Queryable).GetMethods()
                .Single(x => x.Name == key.MethodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(key.Type1, key.Type2);
        });
    }

    public static MethodInfo OrderByDescending(Type elementType, Type elementPropertyType)
    {
        return Cache.GetOrAdd(("OrderByDescending", elementType, elementPropertyType), key =>
        {
            return typeof(Queryable).GetMethods()
                .Single(x => x.Name == key.MethodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(key.Type1, key.Type2);
        });
    }

    public static MethodInfo ThenBy(Type elementType, Type elementPropertyType)
    {
        return Cache.GetOrAdd(("ThenBy", elementType, elementPropertyType), key =>
        {
            return typeof(Queryable).GetMethods()
                .Single(x => x.Name == key.MethodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(key.Type1, key.Type2);
        });
    }

    public static MethodInfo ThenByDescending(Type elementType, Type elementPropertyType)
    {
        return Cache.GetOrAdd(("ThenByDescending", elementType, elementPropertyType), key =>
        {
            return typeof(Queryable).GetMethods()
                .Single(x => x.Name == key.MethodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(key.Type1, key.Type2);
        });
    }

    public static MethodInfo Skip(Type elementType)
    {
        return Cache.GetOrAdd(("Skip", elementType, null!), key =>
        {
            return typeof(Queryable).GetMethods()
                .Single(x => x.Name == key.MethodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(key.Type1);
        });
    }

    public static MethodInfo Take(Type elementType)
    {
        return Cache.GetOrAdd(("Take", elementType, null!), key =>
        {
            return typeof(Queryable).GetMethods()
                .First(x => x.Name == key.MethodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(key.Type1);
        });
    }

    public static MethodInfo Distinct(Type elementType)
    {
        return Cache.GetOrAdd(("Distinct", elementType, null!), key =>
        {
            return typeof(Queryable).GetMethods()
                .Single(x => x.Name == key.MethodName && x.GetParameters().Length is 1)
                .MakeGenericMethod(key.Type1);
        });
    }

    public static MethodInfo DistinctBy(Type elementType, Type elementPropertyType)
    {
        return Cache.GetOrAdd(("DistinctBy", elementType, elementPropertyType), key =>
        {
            return typeof(Queryable).GetMethods()
                .Single(x => x.Name == "DistinctBy" && x.GetParameters().Length == 2)
                .MakeGenericMethod(key.Type1, key.Type2);
        });
    }

    public static MethodInfo Select(Type sourceElementType, Type destinationElementType)
    {
        return Cache.GetOrAdd(("Select", sourceElementType, destinationElementType), key =>
        {
            return typeof(Queryable).GetMethods()
                .First(x => x.Name == key.MethodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(key.Type1, key.Type2);
        });
    }
}
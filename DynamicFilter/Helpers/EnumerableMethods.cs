using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace DynamicFilter.Helpers;

internal static class EnumerableMethods
{
    private static readonly ConcurrentDictionary<(string MethodName, Type Type1, Type Type2), MethodInfo> Cache = new();

    public static MethodInfo Contains(Type elementType)
    {
        return Cache.GetOrAdd(("Contains", elementType, null!), key =>
        {
            return typeof(Enumerable).GetMethods()
                .First(x => x.Name == key.MethodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(key.Type1);
        });
    }
}

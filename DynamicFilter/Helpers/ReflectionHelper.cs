using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DynamicFilter.Helpers;

internal static class ReflectionHelper
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, PropertyInfo>> PropertiesCache = new();

    public static PropertyInfo GetProperty(Type type, string propertyName)
    {
        var properties = PropertiesCache.GetOrAdd(type, t => t.GetProperties().ToDictionary(p => p.Name));

        if (!properties.TryGetValue(propertyName, out var property))
        {
            throw new Exception($"Property '{propertyName}' not found");
        }

        return property;
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type type, string[] propertyNames)
    {
        var properties = PropertiesCache.GetOrAdd(type, t => t.GetProperties().ToDictionary(p => p.Name));

        foreach (var propertyName in propertyNames)
        {
            if (!properties.TryGetValue(propertyName, out var property))
            {
                throw new Exception($"Property '{propertyName}' not found");
            }

            yield return property;
        }
    }

    public static bool IsComparable(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        return typeof(IComparable).IsAssignableFrom(type);
    }

    public static bool CanBeNull(Type type)
    {
        return type.IsClass || Nullable.GetUnderlyingType(type) is not null;
    }
}
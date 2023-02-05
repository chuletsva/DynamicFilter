using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace DynamicFilter.Helpers;

internal static class ValueConverter
{
    private static readonly Dictionary<Type, Func<string, object>> Converters = new()
    {
        [typeof(DateTime)] = ConvertDateTime,
        [typeof(DateTime?)] = ConvertDateTime,
        [typeof(DateTimeOffset)] = ConvertDateTimeOffset,
        [typeof(DateTimeOffset?)] = ConvertDateTimeOffset
    };

    public static object ConvertArray(string[] value, Type elementType)
    {
        var arr = Array.CreateInstance(elementType, value.Length);

        for (int i = 0; i < value.Length; i++)
        {
            var condValue = ConvertValue(value[i], elementType);

            arr.SetValue(condValue, i);
        }

        return arr;
    }

    public static object ConvertValue(string value, Type destinationType)
    {
        if (value is null) return null;

        try
        {
            var converter = GetConverter(destinationType);

            return converter.Invoke(value);
        }
        catch
        {
            throw new Exception($"Cannot convert value '{value}' to type {destinationType.Name}");
        }
    }

    private static Func<string, object> GetConverter(Type destinationType)
    {
        if (Converters.TryGetValue(destinationType, out var converterFunc))
        {
            return converterFunc;
        }

        TypeConverter defaultConverter = TypeDescriptor.GetConverter(destinationType);

        return x => defaultConverter.ConvertFromInvariantString(x) ?? throw new NullReferenceException();
    }

    private static object ConvertDateTime(string value)
    {
        return DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal);
    }

    private static object ConvertDateTimeOffset(string value)
    {
        return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal);
    }
}
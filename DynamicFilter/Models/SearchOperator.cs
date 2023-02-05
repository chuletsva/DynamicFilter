namespace DynamicFilter.Models;

public enum SearchOperator
{
    // All
    Equals = 1,
    NotEquals = 2,
    Any = 3,

    // Long, Int, Short, Decimal, Double, Float, DateTime, DateTimeOffset, Char, Byte, Nullable
    Greater = 4,
    GreaterOrEqual = 5,
    Less = 6,
    LessOrEqual = 7,

    // Nullable, String
    Exists = 8,
    NotExists = 9,

    // String
    StartsWith = 10,
    EndsWith = 11,
    Contains = 12,
    NotContains = 13
}
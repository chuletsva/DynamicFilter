namespace DynamicFilter.Operations;

public sealed record WhereOperation(Condition[] Conditions, Group[] Groups = default) : OperationBase;

public sealed record Condition(string Name, string[] Value, SearchOperator SearchOperator, LogicOperator? LogicOperator = default);

public sealed record Group(int Start, int End, int Level);

public enum LogicOperator
{
    And = 1,
    Or = 2
}

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

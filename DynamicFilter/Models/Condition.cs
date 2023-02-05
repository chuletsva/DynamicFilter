namespace DynamicFilter.Models;

public sealed record Condition(string Field, string?[] Value, SearchOperator Operator, LogicOperator? Logic = default);
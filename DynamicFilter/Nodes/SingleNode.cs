using System;
using System.Linq.Expressions;
using System.Reflection;
using DynamicFilter.Helpers;
using DynamicFilter.Operations;
using static DynamicFilter.Helpers.ValueConverter;

namespace DynamicFilter.Nodes;

internal sealed class SingleNode : INode
{
    private readonly Condition _condition;
    private readonly ParameterExpression _paramExpr;

    public LogicOperator? Operator => _condition.LogicOperator;

    public SingleNode(Condition condition, ParameterExpression paramExpr)
    {
        _condition = condition;
        _paramExpr = paramExpr;
    }

    public Expression BuildExpression()
    {
        PropertyInfo property = ReflectionHelper.GetProperty(_paramExpr.Type, _condition.Name);

        MemberExpression propExpr = Expression.Property(_paramExpr, property);

        Expression predicateExpr;

        switch (_condition.SearchOperator)
        {
            case SearchOperator.Equals:
                {
                    var (value, valueExpr) = BuildSingleValueExpression(property);

                    if (property.PropertyType == typeof(bool))
                    {
                        predicateExpr = value switch
                        {
                            true => propExpr,
                            false => Expression.Not(propExpr),
                            _ => throw new ArgumentOutOfRangeException(nameof(value))
                        };
                    }
                    else
                    {
                        predicateExpr = Expression.Equal(propExpr, valueExpr);
                    }

                    break;
                }
            case SearchOperator.NotEquals:
                {
                    var (value, valueExpr) = BuildSingleValueExpression(property);

                    if (property.PropertyType == typeof(bool))
                    {
                        predicateExpr = value switch
                        {
                            true => Expression.Not(propExpr),
                            false => propExpr,
                            _ => throw new ArgumentOutOfRangeException(nameof(value))
                        };
                    }
                    else
                    {
                        predicateExpr = Expression.NotEqual(propExpr, valueExpr);
                    }

                    break;
                }

            case SearchOperator.Greater when ReflectionHelper.IsComparable(property.PropertyType):
                {
                    var (_, valueExpr) = BuildSingleValueExpression(property);

                    predicateExpr = Expression.GreaterThan(propExpr, valueExpr);

                    break;
                }

            case SearchOperator.GreaterOrEqual when ReflectionHelper.IsComparable(property.PropertyType):
                {
                    var (_, valueExpr) = BuildSingleValueExpression(property);

                    predicateExpr = Expression.GreaterThanOrEqual(propExpr, valueExpr);

                    break;
                }

            case SearchOperator.Less when ReflectionHelper.IsComparable(property.PropertyType):
                {
                    var (_, valueExpr) = BuildSingleValueExpression(property);

                    predicateExpr = Expression.LessThan(propExpr, valueExpr);

                    break;
                }

            case SearchOperator.LessOrEqual when ReflectionHelper.IsComparable(property.PropertyType):
                {
                    var (_, valueExpr) = BuildSingleValueExpression(property);

                    predicateExpr = Expression.LessThanOrEqual(propExpr, valueExpr);

                    break;
                }

            case SearchOperator.Exists when ReflectionHelper.CanBeNull(property.PropertyType):
                {
                    predicateExpr = Expression.NotEqual(propExpr, Expression.Constant(null));

                    break;
                }

            case SearchOperator.NotExists when ReflectionHelper.CanBeNull(property.PropertyType):
                {
                    predicateExpr = Expression.Equal(propExpr, Expression.Constant(null));

                    break;
                }

            case SearchOperator.StartsWith when property.PropertyType == typeof(string):
                {
                    var (_, valueExpr) = BuildSingleValueExpression(property);

                    predicateExpr = Expression.Call(propExpr, "StartsWith", null, valueExpr);

                    break;
                }

            case SearchOperator.EndsWith when property.PropertyType == typeof(string):
                {
                    var (_, valueExpr) = BuildSingleValueExpression(property);

                    predicateExpr = Expression.Call(propExpr, "EndsWith", null, valueExpr);

                    break;
                }

            case SearchOperator.Contains when property.PropertyType == typeof(string):
                {
                    var (_, valueExpr) = BuildSingleValueExpression(property);

                    predicateExpr = Expression.Call(propExpr, "Contains", null, valueExpr);

                    break;
                }

            case SearchOperator.NotContains when property.PropertyType == typeof(string):
                {
                    var (_, valueExpr) = BuildSingleValueExpression(property);

                    predicateExpr = Expression.Not(Expression.Call(propExpr, "Contains", null, valueExpr));

                    break;
                }

            case SearchOperator.Any:
                {
                    var (_, valueExpr) = BuildArrayExpression(property);

                    predicateExpr = Expression.Call(null, EnumerableMethods.Contains(property.PropertyType), valueExpr, propExpr);

                    break;
                }

            default:
                throw new Exception($"Operator '{_condition.SearchOperator}' is not supported for type '{property.PropertyType.Name}'");
        }

        return predicateExpr;
    }

    private (object Value, Expression ValueExpr) BuildSingleValueExpression(PropertyInfo property)
    {
        string originalValue = _condition.Value[0];

        try
        {
            object value = ConvertValue(originalValue, property.PropertyType);

            Expression valueExpr = Expression.Constant(value, property.PropertyType);

            return (value, valueExpr);
        }
        catch
        {
            throw new Exception($"Property '{property.Name}' from type '{property.DeclaringType?.Name}' is not compatible with {getInvalidValueAlias(originalValue)}");
        }

        static string getInvalidValueAlias(object value)
        {
            return value switch
            {
                null => "null",
                _ when string.IsNullOrWhiteSpace(value.ToString()) => "empty string",
                _ => $"value '{value}'"
            };
        }
    }

    private (object Value, Expression ValueExpr) BuildArrayExpression(PropertyInfo property)
    {
        try
        {
            object value = ConvertArray(_condition.Value, property.PropertyType);

            Expression valueExpr = Expression.Constant(value, value.GetType());

            return (value, valueExpr);
        }
        catch (Exception ex)
        {
            throw new Exception($"Property '{property.Name}' of type '{property.PropertyType.Name}' is not compatible with some array values", ex);
        }
    }
}
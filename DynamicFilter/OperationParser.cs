using System;
using DynamicFilter.Operations;

namespace DynamicFilter;

internal static class OperationParser
{
    public static OperationBase Parse(OperationDescription description)
    {
        string operationName = description.Name.ToLower();

        switch (operationName)
        {
            case "where":
            {
                return description.Arguments.ToObject<WhereOperation>();
            }

            case "distinct":
            {
                return new DistinctOperation();
            }

            case "skip":
            {
                var count = description.Arguments.ToObject<int>();

                return new SkipOperation(count);
            }

            case "take":
            {
                var count = description.Arguments.ToObject<int>();

                return new TakeOperation(count);
            }

            case "orderby":
            {
                var propertyName = description.Arguments.ToObject<string>();

                return new OrderByOperation(propertyName);
            }

            case "orderbydescending":
            {
                var propertyName = description.Arguments.ToObject<string>();

                return new OrderByDescendingOperation(propertyName);
            }

            case "thenby":
            {
                var propertyName = description.Arguments.ToObject<string>();

                return new ThenByOperation(propertyName);
            }

            case "thenbydescending":
            {
                var propertyName = description.Arguments.ToObject<string>();

                return new ThenByDescendingOperation(propertyName);
            }

            case "select":
            {
                var properties = description.Arguments.ToObject<string[]>();

                return new SelectOperation(properties);
            }

            default: throw new ArgumentOutOfRangeException();
        }
    }
}
using System;
using DynamicFilter.Arguments;
using DynamicFilter.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicFilter.Converters;

internal class OperationNewtonsoftJsonConverter : JsonConverter<Operation>
{
    public override Operation? ReadJson(JsonReader reader, Type objectType, Operation? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jobject = JObject.Load(reader);

        string name = jobject.GetValue(nameof(Operation.Name), StringComparison.OrdinalIgnoreCase)?.ToString().ToLower() ?? throw new JsonException();

        JToken? argumentsJson = jobject.GetValue(nameof(Operation.Arguments), StringComparison.OrdinalIgnoreCase);

        ArgsBase arguments = ParseArguments(name, argumentsJson);

        return new Operation(name, arguments);
    }

    private static ArgsBase ParseArguments(string operationName, JToken? argumentsJson)
    {
        switch (operationName)
        {
            case "where":
            {
                return getValue<WhereArgs>();
            }

            case "distinct":
            {
                return new DistinctArgs();
            }

            case "skip":
            {
                var count = getValue<int>();

                return new SkipArgs(count);
            }

            case "take":
            {
                var count = getValue<int>();

                return new TakeArgs(count);
            }

            case "orderby":
            {
                if (argumentsJson is null)
                {
                    return new OrderByArgs();
                }

                var fieldName = getValue<string>();

                return new OrderByArgs(fieldName);
            }

            case "orderbydescending":
            {
                if (argumentsJson is null)
                {
                    return new OrderByDescendingArgs();
                }

                var fieldName = getValue<string>();

                return new OrderByDescendingArgs(fieldName);
            }

            case "thenby":
            {
                if (argumentsJson is null)
                {
                    return new ThenByArgs();
                }

                var fieldName = getValue<string>();

                return new ThenByArgs(fieldName);
            }

            case "thenbydescending":
            {
                if (argumentsJson is null)
                {
                    return new ThenByDescendingArgs();
                }

                var fieldName = getValue<string>();

                return new ThenByDescendingArgs(fieldName);
            }

            case "select":
            {
                switch (argumentsJson?.Type)
                {
                    case JTokenType.Array:
                    {
                        var fields = getValue<string[]>();

                        return new SelectArgs(fields);
                    }
                    case JTokenType.String:
                    {
                        var field = getValue<string>();

                        return new SelectArgs(new[] { field }, true);
                    }
                    default:
                        throw new JsonException();
                }
            }

            default: throw new ArgumentOutOfRangeException(nameof(operationName));
        }

        T getValue<T>()
        {
            if (argumentsJson is null)
            {
                throw new JsonException();
            }

            return argumentsJson.ToObject<T>() ?? throw new JsonException();
        }
    }

    public override void WriteJson(JsonWriter writer, Operation? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

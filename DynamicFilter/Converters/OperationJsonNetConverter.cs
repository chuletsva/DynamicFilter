using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using DynamicFilter.Arguments;
using DynamicFilter.Models;

namespace DynamicFilter.Converters;

internal class OperationJsonNetConverter : JsonConverter<Operation>
{
    internal static readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    private static readonly JsonNodeOptions _nodeOptions = new()
    {
        PropertyNameCaseInsensitive = _serializerOptions.PropertyNameCaseInsensitive
    };

    public override Operation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonNode = JsonNode.Parse(ref reader, _nodeOptions) ?? throw new JsonException();

        string name = jsonNode[nameof(Operation.Name)]?.ToString().ToLower() ?? throw new JsonException();

        JsonNode? argumentsJson = jsonNode[nameof(Operation.Arguments)];

        ArgsBase arguments = ParseArguments(name, argumentsJson);

        return new Operation(name, arguments);
    }

    private static ArgsBase ParseArguments(string operationName, JsonNode? argumentsJson)
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

                var fieldName = getValue<string?>();

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
                switch (argumentsJson)
                {
                    case JsonArray:
                    {
                        var fields = getValue<string[]>();

                        return new SelectArgs(fields);
                    }
                    case JsonValue:
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
            return argumentsJson.Deserialize<T>(_serializerOptions) ?? throw new JsonException();
        }
    }

    public override void Write(Utf8JsonWriter writer, Operation value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
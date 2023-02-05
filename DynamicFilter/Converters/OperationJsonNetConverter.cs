using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using DynamicFilter.Arguments;
using DynamicFilter.Models;

namespace DynamicFilter.Converters;

public class OperationJsonNetConverter : JsonConverter<Operation>
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

        JsonNode argumentsJson = jsonNode[nameof(Operation.Arguments)] ?? throw new JsonException();

        ArgsBase arguments = ParseArguments(name, argumentsJson);

        return new Operation(name, arguments);
    }

    private static ArgsBase ParseArguments(string operationName, JsonNode argumentsJson)
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
                var propertyName = getValue<string>();

                return new OrderByArgs(propertyName);
            }

            case "orderbydescending":
            {
                var propertyName = getValue<string>();

                return new OrderByDescendingArgs(propertyName);
            }

            case "thenby":
            {
                var propertyName = getValue<string>();

                return new ThenByArgs(propertyName);
            }

            case "thenbydescending":
            {
                var propertyName = getValue<string>();

                return new ThenByDescendingArgs(propertyName);
            }

            case "select":
            {
                var properties = getValue<string[]>();

                return new SelectArgs(properties);
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
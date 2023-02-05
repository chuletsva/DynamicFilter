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
        var jobject = JObject.Load(reader);

        string name = jobject.GetValue(nameof(Operation.Name), StringComparison.OrdinalIgnoreCase)?.ToString().ToLower() ?? throw new JsonException();

        var argumentsJson = jobject.GetValue(nameof(Operation.Arguments), StringComparison.OrdinalIgnoreCase) ?? throw new JsonException();

        ArgsBase arguments = ParseArguments(name, argumentsJson);

        return new Operation(name, arguments);
    }

    private static ArgsBase ParseArguments(string operationName, JToken argumentsJson)
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
            return argumentsJson.ToObject<T>() ?? throw new JsonException();
        }
    }

    public override void WriteJson(JsonWriter writer, Operation? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

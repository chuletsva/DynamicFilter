using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace DynamicFilter.Converters;

public class DynamicFilterJsonConverter : JsonConverter<Filter>
{
    public override Filter? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string json = JsonNode.Parse(ref reader)?.ToJsonString() ?? throw new JsonException();

        var filter = JObject.Parse(json).ToObject<Filter>();

        return filter;
    }

    public override void Write(Utf8JsonWriter writer, Filter value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

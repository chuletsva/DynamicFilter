using DynamicFilter.Arguments;
using DynamicFilter.Converters;

namespace DynamicFilter.Models;

[System.Text.Json.Serialization.JsonConverter(typeof(OperationJsonNetConverter))]
[Newtonsoft.Json.JsonConverter(typeof(OperationNewtonsoftJsonConverter))]
public sealed record Operation(string Name, ArgsBase Arguments);
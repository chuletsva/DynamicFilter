using Newtonsoft.Json.Linq;

namespace DynamicFilter;

public sealed record OperationDescription(string Name, JToken Arguments);
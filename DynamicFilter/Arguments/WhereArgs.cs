using DynamicFilter.Models;

namespace DynamicFilter.Arguments;

public sealed record WhereArgs(Condition[] Conditions, Group[]? Groups = default) : ArgsBase;

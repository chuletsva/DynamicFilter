namespace DynamicFilter.Arguments;

public sealed record SelectArgs(string[] Fields, bool SingleField = false) : ArgsBase;

using System;

namespace DynamicFilter.Exceptions;

public sealed class DynamicFilterException : Exception
{
    internal DynamicFilterException(string message) : base(message)
    {
    }

    internal DynamicFilterException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
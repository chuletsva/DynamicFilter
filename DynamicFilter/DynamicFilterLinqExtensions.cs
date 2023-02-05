using System.Linq;
using DynamicFilter.Models;

namespace DynamicFilter;

public static class DynamicFilterLinqExtensions
{
    public static IQueryable ApplyDynamicFilter(this IQueryable queryable, params Operation[] filter)
    {
        return filter.Aggregate(queryable, OperationHandler.ApplyOperation);
    }
}
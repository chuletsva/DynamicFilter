using System.Linq;

namespace DynamicFilter;

public static class DynamicFilterLinqExtensions
{
    public static IQueryable ApplyDynamicFilter(this IQueryable queryable, Filter filter)
    {
        return filter.Operations.Aggregate(queryable, (query, info) =>
        {
            var operation = OperationParser.Parse(info);

            return OperationProcessor.ApplyOperation(query, operation);
        });
    }
}
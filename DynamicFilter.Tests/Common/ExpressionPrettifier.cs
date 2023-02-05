using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DynamicFilter.Tests.Common;

internal static class ExpressionPrettifier
{
    public static string PrettifyLambda(LambdaExpression lambdaExpr)
    {
        string lambda = lambdaExpr.ToString();

        int i = 0;

        lambda = Regex.Replace(lambda, @"x.\w+", _ => (++i).ToString());
        lambda = Regex.Replace(lambda, @"OrElse", _ => "or");
        lambda = Regex.Replace(lambda, @"AndAlso", _ => "and");
        lambda = Regex.Replace(lambda, @"x => ", _ => "");
        lambda = Regex.Replace(lambda, @"^\((?'content'.+)\)$", match => match.Groups["content"].Value);

        return lambda;
    }
}
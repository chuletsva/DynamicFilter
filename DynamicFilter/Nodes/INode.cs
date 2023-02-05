using System.Linq.Expressions;
using DynamicFilter.Operations;

namespace DynamicFilter.Nodes;

internal interface INode
{
    LogicOperator? Operator { get; }
    Expression BuildExpression();
}
using System.Linq.Expressions;
using DynamicFilter.Models;

namespace DynamicFilter.Nodes;

internal interface INode
{
    LogicOperator? Logic { get; }
    Expression BuildExpression();
}
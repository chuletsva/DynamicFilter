using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DynamicFilter.Operations;

namespace DynamicFilter.Nodes;

internal sealed class GroupNode : INode
{
    private readonly IReadOnlyList<INode> _children;

    public GroupNode(IReadOnlyList<INode> children, LogicOperator? logicOperator)
    {
        (_children, Operator) = (children, logicOperator);
    }

    public LogicOperator? Operator { get; }

    public Expression BuildExpression()
    {
        Expression leftOperand = _children[0].BuildExpression();

        for (int i = 1; i < _children.Count; i++)
        {
            switch (_children[i].Operator)
            {
                case LogicOperator.Or:
                    int j = i;

                    Expression rightOperand = _children[i].BuildExpression();

                    while (j + 1 < _children.Count && _children[j + 1].Operator == LogicOperator.And)
                        rightOperand = Expression.AndAlso(rightOperand, _children[++j].BuildExpression());

                    leftOperand = Expression.OrElse(leftOperand, rightOperand);

                    i = j;
                    break;
                case LogicOperator.And:
                    leftOperand = Expression.AndAlso(leftOperand, _children[i].BuildExpression());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return leftOperand;
    }
}
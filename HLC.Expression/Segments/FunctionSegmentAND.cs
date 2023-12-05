using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentAND : FunctionSegment
    {
        public override string MatchString { get; } = "AND(";
        public override Operator MatchOperator { get; } = Operator.FunctionAnd;
        public override ExpressionType ExpressionType => ExpressionType.AND;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            foreach (var child in children)
            {
                if (child is VariableExpression ve && !parameters.ContainsKey(ve.Key))
                {
                    return Expression.Result(false);
                }

                if (!child.Invoke(parameters).BooleanResult)
                {
                    return Expression.Result(false);
                }
            }
            return Expression.Result(true);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Boolean, "AND()", "逻辑与运算")
            {
                Demo = "AND(true, true, false)  AND(1 > 2, 2 > 4, false)",
                Input = "Boolean值，或结果为Boolean的表达式。输入可以为多个，用逗号分隔。",
                Output = "逻辑值。输出对所有输入参数的与运算结果。",
            };
        }
    }

}

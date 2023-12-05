using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentOR : FunctionSegment
    {
        public override string MatchString { get; } = "OR(";
        public override Operator MatchOperator { get; } = Operator.FunctionOr;
        public override ExpressionType ExpressionType => ExpressionType.OR;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            foreach (var child in children)
            {
                if (child is VariableExpression ve && !parameters.ContainsKey(ve.Key))
                {
                    continue;
                }

                if (child.Invoke(parameters).BooleanResult)
                {
                    return Expression.Result(true);
                }
            }

            return Expression.Result(false);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Boolean, "OR()", "逻辑或运算")
            {
                Demo = "OR(true, true, false)  OR(1 > 2, 2 > 4, false)",
                Input = "Boolean值，或结果为Boolean的表达式。输入可以为多个，用逗号分隔。",
                Output = "逻辑值。输出对所有输入参数的或运算结果。",
            };
        }
    }

}

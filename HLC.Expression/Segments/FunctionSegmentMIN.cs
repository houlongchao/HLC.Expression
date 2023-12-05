using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentMIN : FunctionSegment
    {
        public override string MatchString { get; } = "MIN(";
        public override Operator MatchOperator { get; } = Operator.MIN;
        public override ExpressionType ExpressionType => ExpressionType.MIN;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var min = children[0].Invoke(parameters).NumberResult;
            for (int i = 1; i < children.Count; i++)
            {
                min = Math.Min(min, children[i].Invoke(parameters).NumberResult);
            }
            return Expression.Result(min);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Number, "MIN()", "取最小值")
            {
                Demo = "MIN(1.1, 2, 3)  MIN(1.1+2.2, 3, 4)",
                Input = "数值，或结果为数值的表达式。输入可以为多个，用逗号分隔。",
                Output = "数值。输出最小的输入结果。",
            };
        }
    }

}

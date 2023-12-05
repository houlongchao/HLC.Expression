using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentMAX : FunctionSegment
    {
        public override string MatchString { get; } = "MAX(";
        public override Operator MatchOperator { get; } = Operator.MAX;
        public override ExpressionType ExpressionType => ExpressionType.MAX;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var max = children[0].Invoke(parameters).NumberResult;
            for (int i = 1; i < children.Count; i++)
            {
                max = Math.Max(max, children[i].Invoke(parameters).NumberResult);
            }
            return Expression.Result(max);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Number, "MAX()", "取最大值")
            {
                Demo = "MAX(1.1, 2, 3)  MAX(1.1+2.2, 3, 4)",
                Input = "数值，或结果为数值的表达式。输入可以为多个，用逗号分隔。",
                Output = "数值。输出最大的输入结果。",
            };
        }
    }

}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentFLOORING : FunctionSegment
    {
        public override string MatchString { get; } = "FLOORING(";
        public override Operator MatchOperator { get; } = Operator.FLOORING;
        public override ExpressionType ExpressionType => ExpressionType.FLOORING;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            ResultExpression invoke = children[0].Invoke(parameters);
            var result = invoke.NumberResult;
            return Expression.Result(Math.Floor(result));
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Number, "FLOORING()", "向下取整")
            {
                Demo = "FLOORING(1.1)  FLOORING(1.1+2.2)",
                Input = "数值，或结果为数值的表达式",
                Output = "整数。输出比输入参数小的最大整数。"
            };
        }
    }

}

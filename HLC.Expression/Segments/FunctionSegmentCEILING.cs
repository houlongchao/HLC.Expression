using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentCEILING : FunctionSegment
    {
        public override string MatchString { get; } = "CEILING(";
        public override Operator MatchOperator { get; } = Operator.CEILING;
        public override ExpressionType ExpressionType => ExpressionType.CEILING;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            ResultExpression invoke = children[0].Invoke(parameters);
            var result = invoke.NumberResult;
            return Expression.Result(Math.Ceiling(result));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Number, "CEILING()", "向上取整")
            {
                Demo = "CEILING(1.1)  CEILING(1.1+2.2)",
                Input = "数值，或结果为数值的表达式",
                Output = "整数。输出比输入参数大的最小整数。"
            };
        }
    }

}

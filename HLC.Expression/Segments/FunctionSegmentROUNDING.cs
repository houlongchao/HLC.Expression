using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentROUNDING : FunctionSegment
    {
        public override string MatchString { get; } = "ROUNDING(";
        public override Operator MatchOperator { get; } = Operator.ROUNDING;
        public override ExpressionType ExpressionType => ExpressionType.ROUNDING;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            if (children.Count <= 1)
            {
                var result = Math.Round(value, MidpointRounding.AwayFromZero);
                return Expression.Result(result);
            }
            else
            {
                var digits = children[1].Invoke(parameters).NumberResult;
                var result = Math.Round(value, (int)digits, MidpointRounding.AwayFromZero);
                return Expression.Result(result);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Number, "ROUNDING()", "四舍五入")
            {
                Demo = "ROUNDING(1.1)  ROUNDING(1.1+2.2, 1)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值。输出保留指定位数的四舍五入值。"
            };
        }
    }

}

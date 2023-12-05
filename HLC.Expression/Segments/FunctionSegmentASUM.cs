using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentASUM : FunctionSegment
    {
        public override string MatchString { get; } = "ASUM(";
        public override Operator MatchOperator { get; } = Operator.ASUM;
        public override ExpressionType ExpressionType => ExpressionType.ASUM;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var valueExpression = children[0].Invoke(parameters);
            if (valueExpression.IsList())
            {
                decimal result = 0;
                foreach (var item in valueExpression.ListResult)
                {
                    if (decimal.TryParse(item.ToString(), out var d))
                    {
                        result += d;
                    }
                }
                return Expression.Result(result);
            }
            else
            {
                return Expression.Result(valueExpression.NumberResult);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Array, "ASUM()", "数组求和")
            {
                Demo = "ASUM({Array})",
                Input = "数值数组参数，或者结果为数值数组的表达式",
                Output = "数值。输出输入数值数值参数的求和。"
            };
        }
    }

}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentAMAXDIFF : FunctionSegment
    {
        public override string MatchString { get; } = "AMAXDIFF(";
        public override Operator MatchOperator { get; } = Operator.AMAXDIFF;
        public override ExpressionType ExpressionType => ExpressionType.AMAXDIFF;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var valueExpression = children[0].Invoke(parameters);
            if (valueExpression.IsList())
            {
                var values = valueExpression.ListResult;
                if (values.Count <= 1)
                {
                    return Expression.Result(0);
                }
                decimal result = 0;
                for (int i = 1; i < values.Count; i++)
                {
                    result = Math.Max(Math.Abs(values[i].ToDecimal(0) - values[i - 1].ToDecimal(0)), result);
                }
                return Expression.Result(result);
            }
            else
            {
                return Expression.Result(0);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Array, "AMAXDIFF()", "数组最大相邻差")
            {
                Demo = "AMAXDIFF({Array})",
                Input = "数值数组参数，或者结果为数值数组的表达式",
                Output = "数值。输出数组中向量两参数差中的最大值。"
            };
        }
    }

}

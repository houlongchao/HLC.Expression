using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentAMINDIFF : FunctionSegment
    {
        public override string MatchString { get; } = "AMINDIFF(";
        public override Operator MatchOperator { get; } = Operator.AMINDIFF;
        public override ExpressionType ExpressionType => ExpressionType.AMINDIFF;

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
                decimal result = decimal.MaxValue;
                for (int i = 1; i < values.Count; i++)
                {
                    result = Math.Min(Math.Abs(values[i].ToDecimal(0) - values[i - 1].ToDecimal(0)), result);
                }
                return Expression.Result(result);
            }
            else
            {
                return Expression.Result(0);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Array, "AMINDIFF()", "数组最小相邻差")
            {
                Demo = "AMINDIFF({Array})",
                Input = "数值数组参数，或者结果为数值数组的表达式",
                Output = "数值。输出数组中向量两参数差中的最小值。"
            };
        }
    }
}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentAMAX : FunctionSegment
    {
        public override string MatchString { get; } = "AMAX(";
        public override Operator MatchOperator { get; } = Operator.AMAX;
        public override ExpressionType ExpressionType => ExpressionType.AMAX;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var valueExpression = children[0].Invoke(parameters);
            if (valueExpression.IsList())
            {
                var values = valueExpression.ListResult;
                if (values.Count <= 0)
                {
                    return Expression.Result(0);
                }
                var result = values[0].ToDecimal(0);
                foreach (var item in values)
                {
                    result = Math.Max(item.ToDecimal(0), result);
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
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Array, "AMAX()", "数组最大值")
            {
                Demo = "AMAX({Array})",
                Input = "数值数组参数，或者结果为数值数组的表达式",
                Output = "数值。输出数组中的最大值 。"
            };
        }
    }

}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentAMIN : FunctionSegment
    {
        public override string MatchString { get; } = "AMIN(";
        public override Operator MatchOperator { get; } = Operator.AMIN;
        public override ExpressionType ExpressionType => ExpressionType.AMIN;

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
                    result = Math.Min(item.ToDecimal(0), result);
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
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Array, "AMIN()", "数组最小值")
            {
                Demo = "AMIN({Array})",
                Input = "数值数组参数，或者结果为数值数组的表达式",
                Output = "数值。输出数组中的最小值。"
            };
        }
    }

}

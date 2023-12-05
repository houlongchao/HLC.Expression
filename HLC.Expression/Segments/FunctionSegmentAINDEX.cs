using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentAINDEX : FunctionSegment
    {
        public override string MatchString { get; } = "AINDEX(";
        public override Operator MatchOperator { get; } = Operator.AINDEX;
        public override ExpressionType ExpressionType => ExpressionType.AINDEX;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var valueExpression = children[0].Invoke(parameters);
            var index = children[1].Invoke(parameters).NumberResult;
            if (index < 0)
            {
                return Expression.Result("");
            }

            if (!valueExpression.IsList())
            {
                return Expression.Result(valueExpression.StringResult);
            }

            if (valueExpression.IsNumberList())
            {
                var values = valueExpression.NumberListResult;
                if (values.Count > index)
                {
                    return Expression.Result(values[(int)index]);
                }
                else
                {
                    return Expression.Result("");
                }
            }
            else
            {
                var values = valueExpression.ListResult;
                if (values.Count > index)
                {
                    return Expression.Result(values[(int)index].ToString());
                }
                else
                {
                    return Expression.Result("");
                }
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Array, "AINDEX()", "获取指定下标数据")
            {
                Demo = "AINDEX({Array}, 1)  AINDEX({Array}, 1)",
                Input = "输入参数必须2个。第一个为数组参数，或结果为数组的表达式，第二个为指定下标（从0开始）",
                Output = "任意值。输出指定下标位置的值。没有数据返回空字符串。"
            };
        }
    }

}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentAMATCH : FunctionSegment
    {
        public override string MatchString { get; } = "AMATCH(";
        public override Operator MatchOperator { get; } = Operator.AMATCH;
        public override ExpressionType ExpressionType => ExpressionType.AMATCH;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var valueExpression = children[0].Invoke(parameters);
            if (!valueExpression.IsList())
            {
                return Expression.Result(-1);
            }
            var matchExpression = children[1].Invoke(parameters);
            if (valueExpression.IsNumberList() && matchExpression.IsNumber())
            {
                var values = valueExpression.NumberListResult;
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i] == matchExpression.NumberResult)
                    {
                        return Expression.Result(i);
                    }
                }
                return Expression.Result(-1);
            }
            else
            {
                var values = valueExpression.ListResult;
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i]?.ToString() == matchExpression.StringResult)
                    {
                        return Expression.Result(i);
                    }
                }
                return Expression.Result(-1);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Array, "AMATCH()", "下标搜索")
            {
                Demo = "AMATCH({Array}, 100)  AMATCH({Array}, '123')",
                Input = "输入参数必须2个。第一个为数组参数，或结果为数组的表达式，第二个为要匹配的数据",
                Output = "整数。输出匹配到的数据下标(从0开始)。没匹配到返回-1 。"
            };
        }
    }

}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentDATETIME : FunctionSegment
    {
        public override string MatchString { get; } = "DATETIME(";
        public override Operator MatchOperator { get; } = Operator.DATETIME;
        public override ExpressionType ExpressionType => ExpressionType.DATETIME;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).StringResult;
            if (children.Count < 2)
            {
                return Expression.Result(DateTime.Parse(value));
            }
            else
            {
                return Expression.Result(DateTime.ParseExact(value, children[1].Invoke(parameters).StringResult, null));
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Value, "DATETIME()", "字符串转日期时间")
            {
                Demo = "DATETIME('2021-02-01 12:00')  DATETIME('202102011200', 'yyyyMMddHHmm')",
                Input = "一个可以转换为日期时间的字符串, 第二个参数为可选格式化字符串",
                Output = "日期类型",
            };
        }
    }

}

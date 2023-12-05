using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentFIND : FunctionSegment
    {
        public override string MatchString { get; } = "FIND(";
        public override Operator MatchOperator { get; } = Operator.FIND;
        public override ExpressionType ExpressionType => ExpressionType.FIND;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).StringResult;
            var matchValue = children[1].Invoke(parameters).StringResult;
            var result = value.IndexOf(matchValue);
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Text, "FIND()", "获取下标")
            {
                Demo = "FIND({OPT:A}, '2')  FIND('123456', '23')",
                Input = "输入参数必须2个，第一个参数为要匹配的源字符串，第二个为匹配字符。",
                Output = "整数。输出匹配到的位置下标（下标从0开始）。",
            };
        }
    }

}

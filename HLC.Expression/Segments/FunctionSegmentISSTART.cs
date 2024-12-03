using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentISSTART : FunctionSegment
    {
        public override string MatchString { get; } = "ISSTART(";
        public override Operator MatchOperator { get; } = Operator.ISSTART;
        public override ExpressionType ExpressionType => ExpressionType.ISSTART;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var valueExpression = children[0].Invoke(parameters);
            var matchExpression = children[1].Invoke(parameters);
            var result = valueExpression.StringResult.StartsWith(matchExpression.StringResult);
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Boolean, "ISSTART()", "是否以指定字符串开始")
            {
                Demo = "ISSTART({OPT:A}, 'a')  ISSTART({OPT:A}, '123')",
                Input = "输入参数必须为2个，第一个为字符串或计算结果为字符串的表达式，第二个为要匹配的字符串",
                Output = "逻辑值。输出结果为第一个参数是否以第二个参数开始",
            };
        }
    }

}

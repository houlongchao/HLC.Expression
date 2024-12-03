using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentRIGHT : FunctionSegment
    {
        public override string MatchString { get; } = "RIGHT(";
        public override Operator MatchOperator { get; } = Operator.RIGHT;
        public override ExpressionType ExpressionType => ExpressionType.RIGHT;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).StringResult;
            var length = children[1].Invoke(parameters).NumberResult;
            var result = value.Substring(value.Length - (int)length);
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Text, "RIGHT()", "从右侧取指定长度子串")
            {
                Demo = "RIGHT({OPT:A}, 2)  RIGHT('123456', 3)",
                Input = "输入参数必须2个，第一个参数为要截取的源字符串，第二个为截取长度。",
                Output = "字符串。输出从右侧截取的字符串结果。",
            };
        }
    }

}

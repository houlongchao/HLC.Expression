using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentLEFT : FunctionSegment
    {
        public override string MatchString { get; } = "LEFT(";
        public override Operator MatchOperator { get; } = Operator.LEFT;
        public override ExpressionType ExpressionType => ExpressionType.LEFT;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).StringResult;
            var length = children[1].Invoke(parameters).NumberResult;
            var result = value.Substring(0, (int)length);
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Text, "LEFT()", "从左侧取指定长度子串")
            {
                Demo = "LEFT({OPT:A}, 2)  LEFT('123456', 3)",
                Input = "输入参数必须2个，第一个参数为要截取的源字符串，第二个为截取长度。",
                Output = "字符串。输出从左侧截取的字符串结果。",
            };
        }
    }

}

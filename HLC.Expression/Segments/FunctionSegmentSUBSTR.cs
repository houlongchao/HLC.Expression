using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentSUBSTR : FunctionSegment
    {
        public override string MatchString { get; } = "SUBSTR(";
        public override Operator MatchOperator { get; } = Operator.SUBSTR;
        public override ExpressionType ExpressionType => ExpressionType.SUBSTR;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).StringResult;
            var startIndex = children[1].Invoke(parameters).NumberResult;
            if (children.Count > 2)
            {
                var length = children[2].Invoke(parameters).NumberResult;
                var result = value.Substring((int)startIndex, (int)length);
                return Expression.Result(result);
            }
            else
            {
                var result = value.Substring((int)startIndex);
                return Expression.Result(result);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Text, "SUBSTR()", "取子串")
            {
                Demo = "SUBSTR({OPT:A}, 1, 2)  SWITCH({OPT:A}, 0, 2)",
                Input = "输入参数必须3个，第一个参数为要截取的源字符串，第二个为开始截取坐标，第三个为截取长度。",
                Output = "字符串。输出截取的字符串结果。",
            };
        }
    }

}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentSUBNUM : FunctionSegment
    {
        public override string MatchString { get; } = "SUBNUM(";
        public override Operator MatchOperator { get; } = Operator.SUBNUM;
        public override ExpressionType ExpressionType => ExpressionType.SUBNUM;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).StringResult;
            var startIndex = children[1].Invoke(parameters).NumberResult;
            if (children.Count > 2)
            {
                var length = children[2].Invoke(parameters).NumberResult;
                var substr = value.Substring((int)startIndex, (int)length);
                var result = decimal.Parse(substr);
                return Expression.Result(result);
            }
            else
            {
                var substr = value.Substring((int)startIndex);
                var result = decimal.Parse(substr);
                return Expression.Result(result);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Text, "SUBNUM()", "取子串转数值")
            {
                Demo = "SUBNUM({OPT:A}, 1, 2)  SUBNUM({OPT:A}, 0, 2)",
                Input = "输入参数必须3个，第一个参数为要截取的源字符串，第二个为开始截取坐标，第三个为截取长度。",
                Output = "字符串。输出截取的字符串结果转为数值。",
            };
        }
    }

}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentREVERSE : FunctionSegment
    {
        public override string MatchString { get; } = "REVERSE(";
        public override Operator MatchOperator { get; } = Operator.REVERSE;
        public override ExpressionType ExpressionType => ExpressionType.REVERSE;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).StringResult;
            var result = string.Join("", value.Reverse());
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Text, "REVERSE()", "反转字符串")
            {
                Demo = "REVERSE({OPT:A})  REVERSE('123456')",
                Input = "字符串或计算结果为字符串。",
                Output = "字符串。输出输入字符串的反转字符串。",
            };
        }
    }

}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentLENGTH : FunctionSegment
    {
        public override string MatchString { get; } = "LENGTH(";
        public override Operator MatchOperator { get; } = Operator.Length;
        public override ExpressionType ExpressionType => ExpressionType.Length;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).StringResult;
            var result = value.Length;
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Text, "LENGTH()", "获取字符串长度")
            {
                Demo = "LENGTH({OPT:A})  LENGTH('123456')",
                Input = "字符串或计算结果为字符串。",
                Output = "整数。输出输入字符串的长度。",
            };
        }
    }

}

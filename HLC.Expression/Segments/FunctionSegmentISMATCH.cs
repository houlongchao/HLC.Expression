using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentISMATCH : FunctionSegment
    {
        public override string MatchString { get; } = "ISMATCH(";
        public override Operator MatchOperator { get; } = Operator.ISMATCH;
        public override ExpressionType ExpressionType => ExpressionType.ISMATCH;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var valueExpression = children[0].Invoke(parameters);
            var matchExpression = children[1].Invoke(parameters);
            var result = Regex.IsMatch(valueExpression.StringResult, matchExpression.StringResult);
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Boolean, "ISMATCH()", "是否可以用指定正则匹配")
            {
                Demo = "ISMATCH('123456', '^\\d*$')  ISMATCH('123456', '^1\\d{5}$')",
                Input = "输入参数必须为2个，第一个为字符串或计算结果为字符串的表达式，第二个为要匹配的正则字符串",
                Output = "逻辑值。输出结果为第一个参数是否可以用第二个正则表达式进行匹配",
            };
        }
    }

}

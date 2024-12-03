using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentISNUMBER : FunctionSegment
    {
        public override string MatchString { get; } = "ISNUMBER(";
        public override Operator MatchOperator { get; } = Operator.ISNUMBER;
        public override ExpressionType ExpressionType => ExpressionType.ISNUMBER;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            ResultExpression invoke = children[0].Invoke(parameters);
            var result = invoke.IsNumber();
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Boolean, "ISNUMBER()", "是否为数值")
            {
                Demo = "ISNUMBER({OPT:A})  ISNUMBER({OPT:A}+{OPT:B})",
                Input = "参数变量，或一个表达式。",
                Output = "逻辑值。输出结果是否为数值",
            };
        }
    }

}

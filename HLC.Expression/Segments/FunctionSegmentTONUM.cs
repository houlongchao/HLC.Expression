using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;
namespace HLC.Expression.Segments
{
    public class FunctionSegmentTONUM : FunctionSegment
    {
        public override string MatchString { get; } = "TONUM(";
        public override Operator MatchOperator { get; } = Operator.TONUM;
        public override ExpressionType ExpressionType => ExpressionType.TONUM;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var result = children[0].Invoke(parameters);
            return Expression.Result(result.NumberResult);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Value, "TONUM()", "转为数字")
            {
                Demo = "TONUM({OPT:NUM1})   TONUM('123')",
                Input = "一个输入参数",
                Output = "结果数字",
            };
        }
    }

}

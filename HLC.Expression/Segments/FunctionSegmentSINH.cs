using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentSINH : FunctionSegment
    {
        public override string MatchString { get; } = "SINH(";
        public override Operator MatchOperator { get; } = Operator.SINH;
        public override ExpressionType ExpressionType => ExpressionType.SINH;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Sinh((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "SINH()", "双曲正弦函数")
            {
                Demo = "SINH(1.1)  SINH(1.1+2.2)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值"
            };
        }
    }

}

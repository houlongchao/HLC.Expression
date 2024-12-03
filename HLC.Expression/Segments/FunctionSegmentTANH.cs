using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentTANH : FunctionSegment
    {
        public override string MatchString { get; } = "TANH(";
        public override Operator MatchOperator { get; } = Operator.TANH;
        public override ExpressionType ExpressionType => ExpressionType.TANH;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Tanh((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "TANH()", "双曲正切函数")
            {
                Demo = "TANH(1.1)  TANH(1.1+2.2)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值"
            };
        }
    }

}

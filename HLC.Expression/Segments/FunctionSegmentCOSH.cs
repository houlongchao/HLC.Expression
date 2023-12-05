using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentCOSH : FunctionSegment
    {
        public override string MatchString { get; } = "COSH(";
        public override Operator MatchOperator { get; } = Operator.COSH;
        public override ExpressionType ExpressionType => ExpressionType.COSH;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Cosh((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "COSH()", "双曲余弦函数")
            {
                Demo = "COSH(1.1)  COSH(1.1+2.2)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值"
            };
        }
    }

}

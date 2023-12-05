using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentSQRT : FunctionSegment
    {
        public override string MatchString { get; } = "SQRT(";
        public override Operator MatchOperator { get; } = Operator.SQRT;
        public override ExpressionType ExpressionType => ExpressionType.SQRT;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Sqrt((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "SQRT()", "平方根")
            {
                Demo = "SQRT(20)  SQRT(1.1+2.2)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值"
            };
        }
    }

}

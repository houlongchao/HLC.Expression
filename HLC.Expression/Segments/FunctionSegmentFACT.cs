using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentFACT : FunctionSegment
    {
        public override string MatchString { get; } = "FACT(";
        public override Operator MatchOperator { get; } = Operator.FACT;
        public override ExpressionType ExpressionType => ExpressionType.FACT;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            decimal result = 1;
            for (int i = 2; i <= value; i++)
            {
                result *= i;
            }
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "FACT()", "阶乘")
            {
                Demo = "FACT(5)  FACT(1+2)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值"
            };
        }
    }

}

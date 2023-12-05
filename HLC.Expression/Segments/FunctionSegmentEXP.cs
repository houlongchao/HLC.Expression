using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentEXP : FunctionSegment
    {
        public override string MatchString { get; } = "EXP(";
        public override Operator MatchOperator { get; } = Operator.EXP;
        public override ExpressionType ExpressionType => ExpressionType.EXP;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Exp((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "EXP()", "e的指数次幂")
            {
                Demo = "EXP(1.1)  EXP(1.1+2.2)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值"
            };
        }
    }

}

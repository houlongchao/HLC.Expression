using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentPI : FunctionSegment
    {
        public override string MatchString { get; } = "PI(";
        public override Operator MatchOperator { get; } = Operator.PI;
        public override ExpressionType ExpressionType => ExpressionType.PI;

        public override bool IsFreeArg => true;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            return Expression.Result((decimal)Math.PI);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "PI()", "圆周率PI")
            {
                Demo = "PI()",
                Input = "无",
                Output = "常量圆周率PI"
            };
        }
    }

}

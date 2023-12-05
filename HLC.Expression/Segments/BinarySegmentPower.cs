using HLC.Expression.Definitions;
using System;

namespace HLC.Expression.Segments
{
    public class BinarySegmentPower : BinarySegment
    {
        public override string MatchString => "^";

        public override Operator MatchOperator => Operator.Power;

        public override ExpressionType ExpressionType => ExpressionType.Power;

        public override ExpressionSymbolDefinitionItem GetDefinistion()
        {
            return new ExpressionSymbolDefinitionItem("^", "指数") { Demo = "5^2", Details = "左右只能是数字或表达式结果为数字" };
        }

        public override ResultExpression Invoke(Expression left, Expression right, Parameters parameters)
        {
            ResultExpression leftResult = left.Invoke(parameters);
            ResultExpression rightResult = right.Invoke(parameters);
            var result = Math.Pow((double)leftResult.NumberResult, (double)rightResult.NumberResult);
            return Expression.Result((decimal)result);
        }
    }

}

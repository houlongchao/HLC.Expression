using HLC.Expression.Definitions;

namespace HLC.Expression.Segments
{
    public class BinarySegmentBooleanOr : BinarySegment
    {
        public override string MatchString => "||";

        public override Operator MatchOperator => Operator.BooleanOr;

        public override ExpressionType ExpressionType => ExpressionType.BooleanOr;

        public override ExpressionSymbolDefinitionItem GetDefinistion()
        {
            return new ExpressionSymbolDefinitionItem("||", "或运算") { Demo = "true || false", Details = "左右只能是Boolean或表达式结果为Boolean" };
        }

        public override ResultExpression Invoke(Expression left, Expression right, Parameters parameters)
        {
            ResultExpression leftResult = (left is VariableExpression vel && !parameters.ContainsKey(vel.Key))
                ? Expression.Result(false)
                : left.Invoke(parameters);
            ResultExpression rightResult = (right is VariableExpression ver && !parameters.ContainsKey(ver.Key))
                ? Expression.Result(false)
                : right.Invoke(parameters);
            bool result = leftResult.BooleanResult || rightResult.BooleanResult;
            return Expression.Result(result);
        }
    }
}

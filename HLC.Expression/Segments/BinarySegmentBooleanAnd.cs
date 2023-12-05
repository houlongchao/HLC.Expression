using HLC.Expression.Definitions;

namespace HLC.Expression.Segments
{
    public class BinarySegmentBooleanAnd : BinarySegment
    {
        public override string MatchString => "&&";

        public override Operator MatchOperator => Operator.BooleanAnd;

        public override ExpressionType ExpressionType => ExpressionType.BooleanAnd;

        public override ExpressionSymbolDefinitionItem GetDefinistion()
        {
            return new ExpressionSymbolDefinitionItem("&&", "与运算"){ Demo = "true && false", Details = "左右只能是Boolean或表达式结果为Boolean"};
        }

        public override ResultExpression Invoke(Expression left, Expression right, Parameters parameters)
        {
            ResultExpression leftResult = (left is VariableExpression vel && !parameters.ContainsKey(vel.Key))
                ? Expression.Result(false)
                : left.Invoke(parameters);
            ResultExpression rightResult = (right is VariableExpression ver && !parameters.ContainsKey(ver.Key))
                ? Expression.Result(false)
                : right.Invoke(parameters);
            bool result = leftResult.BooleanResult && rightResult.BooleanResult;
            return Expression.Result(result);
        }
    }

}

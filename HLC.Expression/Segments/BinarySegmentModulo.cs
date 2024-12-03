using HLC.Expression.Definitions;

namespace HLC.Expression.Segments
{
    public class BinarySegmentModulo : BinarySegment
    {
        public override string MatchString => "%";

        public override Operator MatchOperator => Operator.Modulo;

        public override ExpressionType ExpressionType => ExpressionType.Modulo;

        public override ExpressionSymbolDefinitionItem GetDefinition()
        {
            return new ExpressionSymbolDefinitionItem("%", "取模") { Demo = "5%2", Details = "左右只能是整数或表达式结果为整数" };
        }

        public override ResultExpression Invoke(Expression left, Expression right, Parameters parameters)
        {
            ResultExpression leftResult = left.Invoke(parameters);
            ResultExpression rightResult = right.Invoke(parameters);
            int result = (int)leftResult.NumberResult % (int)rightResult.NumberResult;
            return Expression.Result(result);
        }
    }

}

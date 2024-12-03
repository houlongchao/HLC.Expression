using HLC.Expression.Definitions;

namespace HLC.Expression.Segments
{
    public class BinarySegmentSubtract : BinarySegment
    {
        public override string MatchString => "-";

        public override Operator MatchOperator => Operator.Subtract;

        public override ExpressionType ExpressionType => ExpressionType.Subtract;

        public override ExpressionSymbolDefinitionItem GetDefinition()
        {
            return new ExpressionSymbolDefinitionItem("-", "减法") { Demo = "5-2", Details = "左右只能是数字或表达式结果为数字" };
        }

        public override ResultExpression Invoke(Expression left, Expression right, Parameters parameters)
        {
            ResultExpression leftResult = left.Invoke(parameters);
            ResultExpression rightResult = right.Invoke(parameters);
            var result = leftResult.NumberResult - rightResult.NumberResult;
            return Expression.Result(result);
        }
    }

}

using HLC.Expression.Definitions;

namespace HLC.Expression.Segments
{
    public class BinarySegmentGreater : BinarySegment
    {
        public override string MatchString => ">";

        public override Operator MatchOperator => Operator.Greater;

        public override ExpressionType ExpressionType => ExpressionType.Greater;

        public override ExpressionSymbolDefinitionItem GetDefinition()
        {
            return new ExpressionSymbolDefinitionItem(">", "大于") { Demo = "5>2", Details = "左右只能是数字或表达式结果为数字" };
        }

        public override ResultExpression Invoke(Expression left, Expression right, Parameters parameters)
        {
            ResultExpression leftResult = left.Invoke(parameters);
            ResultExpression rightResult = right.Invoke(parameters);
            if (leftResult.IsDateTime())
            {
                bool result = leftResult.DateTimeResult > rightResult.DateTimeResult;
                return Expression.Result(result);
            }
            {
                bool result = leftResult.NumberResult > rightResult.NumberResult;
                return Expression.Result(result);
            }
        }
    }

}

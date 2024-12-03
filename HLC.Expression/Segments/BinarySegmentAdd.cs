using HLC.Expression.Definitions;

namespace HLC.Expression.Segments
{
    public class BinarySegmentAdd : BinarySegment
    {
        public override string MatchString => "+";

        public override Operator MatchOperator => Operator.Add;

        public override ExpressionType ExpressionType => ExpressionType.Add;

        public override ExpressionSymbolDefinitionItem GetDefinition()
        {
            return new ExpressionSymbolDefinitionItem("+", "加法") { Demo = "5+2", Details = "左右只能是数字或表达式结果为数字" };
        }

        public override ResultExpression Invoke(Expression left, Expression right, Parameters parameters)
        {
            ResultExpression leftResult = left.Invoke(parameters);
            ResultExpression rightResult = right.Invoke(parameters);
            try
            {
                var result = leftResult.NumberResult + rightResult.NumberResult;
                return Expression.Result(result);
            }
            catch
            {
                string result = leftResult.StringResult + rightResult.StringResult;
                return Expression.Result(result);
            }
        }
    }

}

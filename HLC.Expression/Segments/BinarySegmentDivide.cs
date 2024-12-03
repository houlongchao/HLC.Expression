using HLC.Expression.Definitions;

namespace HLC.Expression.Segments
{
    public class BinarySegmentDivide : BinarySegment
    {
        public override string MatchString => "/";

        public override Operator MatchOperator => Operator.Divide;

        public override ExpressionType ExpressionType => ExpressionType.Divide;

        public override ExpressionSymbolDefinitionItem GetDefinition()
        {
            return new ExpressionSymbolDefinitionItem("/", "除法") { Demo = "5/2", Details = "左右只能是数字或表达式结果为数字" };
        }

        public override ResultExpression Invoke(Expression left, Expression right, Parameters parameters)
        {
            ResultExpression leftResult = left.Invoke(parameters);
            ResultExpression rightResult = right.Invoke(parameters);
            if (rightResult.NumberResult == 0)
            {
                switch (ExpressionSetting.Instance.DivideZero)
                {
                    case DivideZero.ReturnZero:
                        return Expression.Result(0);
                    case DivideZero.Throw:
                        throw new ExpressionCalculateException(ToString(), "除0异常");
                    default:
                        var result = leftResult.NumberResult / rightResult.NumberResult;
                        return Expression.Result(result);
                }
            }
            else
            {
                var result = leftResult.NumberResult / rightResult.NumberResult;
                return Expression.Result(result);
            }
        }
    }

}

using HLC.Expression.Definitions;

namespace HLC.Expression.Segments
{
    public class BinarySegmentNotEqual : BinarySegment
    {
        public override string MatchString => "!=";

        public override Operator MatchOperator => Operator.NotEqual;

        public override ExpressionType ExpressionType => ExpressionType.NotEqual;

        public override ExpressionSymbolDefinitionItem GetDefinition()
        {
            return new ExpressionSymbolDefinitionItem("!=", "不等于") { Demo = "5!=2", Details = "如果左侧为数字，右侧可以是一个区间，如{A}==[1,10]" };
        }

        public override ResultExpression Invoke(Expression left, Expression right, Parameters parameters)
        {
            if (left is VariableExpression vel && !parameters.ContainsKey(vel.Key))
            {
                return Expression.Result(true);
            }
            if (right is VariableExpression ver && !parameters.ContainsKey(ver.Key))
            {
                return Expression.Result(true);
            }

            ResultExpression leftResult = left.Invoke(parameters);
            ResultExpression rightResult = right.Invoke(parameters);

            if (leftResult.IsNumber() && rightResult.IsNumber())
            {
                bool result = !ExpressionSetting.Instance.AreEquals(leftResult.NumberResult, rightResult.NumberResult);
                return Expression.Result(result);
            }
            else if (leftResult.IsNumber() && rightResult.IsRange())
            {
                bool result = !RangeUtils.IsInRange(leftResult.NumberResult, rightResult.ToString());
                return Expression.Result(result);
            }
            else if (leftResult.IsDateTime() || rightResult.IsDateTime())
            {
                bool result = leftResult.DateTimeResult != rightResult.DateTimeResult;
                return Expression.Result(result);
            }
            else
            {
                bool result = leftResult.ToString() != rightResult.ToString();
                return Expression.Result(result);
            }
        }
    }

}

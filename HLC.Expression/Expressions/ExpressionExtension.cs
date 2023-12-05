using System.Collections.Generic;

namespace HLC.Expression
{
    public static class ExpressionExtension
    {

        #region 检查返回值

        public static bool IsBoolResult(this Expression expression)
        {
            switch (expression.Type)
            {
                case ExpressionType.Variable:
                case ExpressionType.BooleanConstant:
                case ExpressionType.IN:
                case ExpressionType.Greater:
                case ExpressionType.GreaterEqual:
                case ExpressionType.Less:
                case ExpressionType.LessEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.BooleanAnd:
                case ExpressionType.BooleanOr:
                case ExpressionType.AND:
                case ExpressionType.OR:
                case ExpressionType.NOT:
                    return true;
                case ExpressionType.IF:
                    return (expression is FunctionExpression ifExpression) &&
                           ifExpression.Children[0].IsBoolResult() &&
                           ifExpression.Children[1].IsBoolResult() &&
                           ifExpression.Children[2].IsBoolResult();
                default:
                    return false;
            }
        }
        public static bool IsNumberResult(this Expression expression)
        {
            switch (expression.Type)
            {
                case ExpressionType.Variable:
                case ExpressionType.NumberConstant:
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.CEILING:
                case ExpressionType.FLOORING:
                case ExpressionType.ROUNDING:
                case ExpressionType.MAX:
                case ExpressionType.MIN:
                case ExpressionType.Power:
                case ExpressionType.Modulo:
                    return true;
                case ExpressionType.IF:
                    return (expression is FunctionExpression ifExpression) &&
                           ifExpression.Children[0].IsBoolResult() &&
                           ifExpression.Children[1].IsNumberResult() &&
                           ifExpression.Children[2].IsNumberResult();
                default:
                    return false;
            }
        }

        public static bool IsStringResult(this Expression expression)
        {
            switch (expression.Type)
            {
                case ExpressionType.Variable:
                case ExpressionType.StringConstant:
                case ExpressionType.CONCAT:
                    return true;
                case ExpressionType.IF:
                    return (expression is FunctionExpression ifExpression) &&
                           ifExpression.Children[0].IsBoolResult() &&
                           ifExpression.Children[1].IsStringResult() &&
                           ifExpression.Children[2].IsStringResult();
                default:
                    return false;
            }
        }

        #endregion

        #region 检查参数

        public static bool IsStringExpression(this Expression expression, ICollection<string> checkStringList = null, bool ignoreCase = true)
        {
            if (!(expression is ConstantExpression constantExpression))
            {
                return false;
            }
            else
            {
                if (constantExpression.Type != ExpressionType.StringConstant)
                {
                    return false;
                }

                if (checkStringList != null)
                {
                    if (ignoreCase && !checkStringList.Contains(constantExpression.Value.ToString().ToLower()))
                    {
                        return false;
                    }
                    if (!ignoreCase && !checkStringList.Contains(constantExpression.Value.ToString()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsVariableExpression(this Expression expression)
        {
            if (!(expression is VariableExpression))
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}

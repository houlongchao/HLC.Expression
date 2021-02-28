using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLC.Expression.Utils;

namespace HLC.Expression
{
    /// <summary>
    /// 二元运算符表达式
    /// </summary>
    public class BinaryExpression : Expression
    {
        public Expression Left { get; set; }
        public Expression Right { get; set; }

        public BinaryExpression(Expression left, ExpressionType type, Expression right) : base(type)
        {
            Left = left;
            Right = right;
        }

        public override ResultExpression Invoke(Parameters parameters)
        {
            if (Type == ExpressionType.Add)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                double result = leftResult.NumberResult + rightResult.NumberResult;
                return Result(result);
            }
            if (Type == ExpressionType.Subtract)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                double result = leftResult.NumberResult - rightResult.NumberResult;
                return Result(result);
            }
            if (Type == ExpressionType.Multiply)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                double result = leftResult.NumberResult * rightResult.NumberResult;
                return Result(result);
            }
            if (Type == ExpressionType.Divide)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                double result = leftResult.NumberResult / rightResult.NumberResult;
                return Result(result);
            }
            if (Type == ExpressionType.Greater)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                bool result = leftResult.NumberResult > rightResult.NumberResult;
                return Result(result);
            }
            if (Type == ExpressionType.GreaterEqual)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                bool result = leftResult.NumberResult >= rightResult.NumberResult;
                return Result(result);
            }
            if (Type == ExpressionType.Less)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                bool result = leftResult.NumberResult < rightResult.NumberResult;
                return Result(result);
            }
            if (Type == ExpressionType.LessEqual)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                bool result = leftResult.NumberResult <= rightResult.NumberResult;
                return Result(result);
            }
            if (Type == ExpressionType.Equal)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);

                if (leftResult.IsNumber() && rightResult.IsNumber())
                {
                    bool result = ExpressionSetting.Instance.AreEquals(leftResult.NumberResult, rightResult.NumberResult);
                    return Result(result);
                }
                else if (leftResult.IsNumber() && rightResult.DataType == ResultType.Range)
                {
                    bool result = RangeUtil.IsInRange(leftResult.NumberResult, rightResult.Data.ToString());
                    return Result(result);
                }
                else
                {
                    bool result = leftResult.Data == rightResult.Data;
                    return Result(result);
                }
            }
            if (Type == ExpressionType.NotEqual)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);

                if (leftResult.IsNumber() && rightResult.IsNumber())
                {
                    bool result = !ExpressionSetting.Instance.AreEquals(leftResult.NumberResult, rightResult.NumberResult);
                    return Result(result);
                }
                else if (leftResult.IsNumber() && rightResult.DataType == ResultType.Range)
                {
                    bool result = !RangeUtil.IsInRange(leftResult.NumberResult, rightResult.Data.ToString());
                    return Result(result);
                }
                else
                {
                    bool result = leftResult.Data != rightResult.Data;
                    return Result(result);
                }
            }

            if (Type == ExpressionType.Power)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                double result = Math.Pow(leftResult.NumberResult, rightResult.NumberResult);
                return Result(result);
            }

            if (Type == ExpressionType.Modulo)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                int result = (int)leftResult.NumberResult % (int)rightResult.NumberResult;
                return Result(result);
            }

            if (Type == ExpressionType.BooleanAnd)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                bool result = leftResult.BooleanResult && rightResult.BooleanResult;
                return Result(result);
            }

            if (Type == ExpressionType.BooleanOr)
            {
                ResultExpression leftResult = Left.Invoke(parameters);
                ResultExpression rightResult = Right.Invoke(parameters);
                bool result = leftResult.BooleanResult || rightResult.BooleanResult;
                return Result(result);
            }
            return null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Left.Type.GetPriority() < Type.GetPriority())
            {
                sb.Append("(");
                sb.Append(Left);
                sb.Append(")");
            }
            else
            {
                sb.Append(Left);
            }

            if (Type == ExpressionType.Add)
            {
                sb.Append(" + ");
            }
            else if (Type == ExpressionType.Subtract)
            {
                sb.Append(" - ");
            }
            else if (Type == ExpressionType.Multiply)
            {
                sb.Append(" * ");
            }
            else if (Type == ExpressionType.Divide)
            {
                sb.Append(" / ");
            }
            else if (Type == ExpressionType.Greater)
            {
                sb.Append(" > ");
            }
            else if (Type == ExpressionType.GreaterEqual)
            {
                sb.Append(" >= ");
            }
            else if (Type == ExpressionType.Less)
            {
                sb.Append(" < ");
            }
            else if (Type == ExpressionType.LessEqual)
            {
                sb.Append(" <= ");
            }
            else if (Type == ExpressionType.Equal)
            {
                sb.Append(" == ");
            }
            else if (Type == ExpressionType.NotEqual)
            {
                sb.Append(" != ");
            }
            else if (Type == ExpressionType.Modulo)
            {
                sb.Append(" % ");
            }
            else if (Type == ExpressionType.BooleanAnd)
            {
                sb.Append(" && ");
            }
            else if (Type == ExpressionType.BooleanOr)
            {
                sb.Append(" || ");
            }
            else if (Type == ExpressionType.Power)
            {
                sb.Append("^");
            }

            if (Right.Type.GetPriority() < Type.GetPriority())
            {
                sb.Append("(");
                sb.Append(Right);
                sb.Append(")");
            }
            else
            {
                sb.Append(Right);
            }
            return sb.ToString();
        }

        public override IList<string> GetVariableKeys()
        {
            IList<string> leftVariableKeys = Left.GetVariableKeys();
            IList<string> rightVariableKeys = Right.GetVariableKeys();
            return leftVariableKeys.Concat(rightVariableKeys).ToList();
        }

    }
}
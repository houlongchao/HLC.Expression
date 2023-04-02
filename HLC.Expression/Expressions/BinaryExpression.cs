using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            switch (Type)
            {
                case ExpressionType.Add:
                    {
                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);
                        try
                        {
                            var result = leftResult.NumberResult + rightResult.NumberResult;
                            InvokeResult = Result(result);
                            break;
                        }
                        catch
                        {
                            string result = leftResult.StringResult + rightResult.StringResult;
                            InvokeResult = Result(result);
                            break;
                        }
                    }
                case ExpressionType.Subtract:
                    {
                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);
                        var result = leftResult.NumberResult - rightResult.NumberResult;
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.Multiply:
                    {
                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);
                        var result = leftResult.NumberResult * rightResult.NumberResult;
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.Divide:
                    {
                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);
                        if (rightResult.NumberResult == 0)
                        {
                            switch (ExpressionSetting.Instance.DivideZero)
                            {
                                case DivideZero.ReturnZero:
                                    InvokeResult = Result(0);
                                    break;
                                case DivideZero.Throw:
                                    throw new ExpressionCalculateException(ToString(), "除0异常");
                                default:
                                    var result = leftResult.NumberResult / rightResult.NumberResult;
                                    InvokeResult = Result(result);
                                    break;
                            }
                            break;
                        }
                        else
                        {
                            var result = leftResult.NumberResult / rightResult.NumberResult;
                            InvokeResult = Result(result);
                            break;
                        }
                    }
                case ExpressionType.Greater:
                    {
                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);
                        if (leftResult.IsDateTime())
                        {
                            bool result = leftResult.DateTimeResult > rightResult.DateTimeResult;
                            InvokeResult = Result(result);
                            break;
                        }
                        {
                            bool result = leftResult.NumberResult > rightResult.NumberResult;
                            InvokeResult = Result(result);
                            break;
                        }
                    }
                case ExpressionType.GreaterEqual:
                    {
                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);
                        if (leftResult.IsDateTime())
                        {
                            bool result = leftResult.DateTimeResult >= rightResult.DateTimeResult;
                            InvokeResult = Result(result);
                            break;
                        }
                        {
                            bool result = leftResult.NumberResult >= rightResult.NumberResult;
                            InvokeResult = Result(result);
                            break;
                        }
                    }
                case ExpressionType.Less:
                    {
                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);

                        if (leftResult.IsDateTime())
                        {
                            bool result = leftResult.DateTimeResult < rightResult.DateTimeResult;
                            InvokeResult = Result(result);
                            break;
                        }
                        {
                            bool result = leftResult.NumberResult < rightResult.NumberResult;
                            InvokeResult = Result(result);
                            break;
                        }
                    }
                case ExpressionType.LessEqual:
                    {
                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);
                        if (leftResult.IsDateTime())
                        {
                            bool result = leftResult.DateTimeResult <= rightResult.DateTimeResult;
                            InvokeResult = Result(result);
                            break;
                        }
                        {
                            bool result = leftResult.NumberResult <= rightResult.NumberResult;
                            InvokeResult = Result(result);
                            break;
                        }
                    }
                case ExpressionType.Equal:
                    {
                        if (Left is VariableExpression vel && !parameters.ContainsKey(vel.Key))
                        {
                            InvokeResult = Result(false);
                            break;
                        }
                        if (Right is VariableExpression ver && !parameters.ContainsKey(ver.Key))
                        {
                            InvokeResult = Result(false);
                            break;
                        }

                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);

                        if (leftResult.IsNumber() && rightResult.IsNumber())
                        {
                            bool result = ExpressionSetting.Instance.AreEquals(leftResult.NumberResult, rightResult.NumberResult);
                            InvokeResult = Result(result);
                            break;
                        }
                        else if (leftResult.IsNumber() && rightResult.IsRange())
                        {
                            bool result = RangeUtils.IsInRange(leftResult.NumberResult, rightResult.Data.ToString());
                            InvokeResult = Result(result);
                            break;
                        }
                        else if (leftResult.IsDateTime() || rightResult.IsDateTime())
                        {
                            bool result = leftResult.DateTimeResult == rightResult.DateTimeResult;
                            InvokeResult = Result(result);
                            break;
                        }
                        else
                        {
                            bool result = leftResult.ToString() == rightResult.ToString();
                            InvokeResult = Result(result);
                            break;
                        }
                    }
                case ExpressionType.NotEqual:
                    {
                        if (Left is VariableExpression vel && !parameters.ContainsKey(vel.Key))
                        {
                            InvokeResult = Result(true);
                            break;
                        }
                        if (Right is VariableExpression ver && !parameters.ContainsKey(ver.Key))
                        {
                            InvokeResult = Result(true);
                            break;
                        }

                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);

                        if (leftResult.IsNumber() && rightResult.IsNumber())
                        {
                            bool result = !ExpressionSetting.Instance.AreEquals(leftResult.NumberResult, rightResult.NumberResult);
                            InvokeResult = Result(result);
                            break;
                        }
                        else if (leftResult.IsNumber() && rightResult.IsRange())
                        {
                            bool result = !RangeUtils.IsInRange(leftResult.NumberResult, rightResult.ToString());
                            InvokeResult = Result(result);
                            break;
                        }
                        else if (leftResult.IsDateTime() || rightResult.IsDateTime())
                        {
                            bool result = leftResult.DateTimeResult != rightResult.DateTimeResult;
                            InvokeResult = Result(result);
                            break;
                        }
                        else
                        {
                            bool result = leftResult.ToString() != rightResult.ToString();
                            InvokeResult = Result(result);
                            break;
                        }
                    }
                case ExpressionType.Power:
                    {
                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);
                        var result = Math.Pow((double)leftResult.NumberResult, (double)rightResult.NumberResult);
                        InvokeResult = Result((decimal)result);
                        break;
                    }
                case ExpressionType.Modulo:
                    {
                        ResultExpression leftResult = Left.Invoke(parameters);
                        ResultExpression rightResult = Right.Invoke(parameters);
                        int result = (int)leftResult.NumberResult % (int)rightResult.NumberResult;
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.BooleanAnd:
                    {
                        ResultExpression leftResult = (Left is VariableExpression vel && !parameters.ContainsKey(vel.Key)) ? Result(false) : Left.Invoke(parameters);
                        ResultExpression rightResult = (Right is VariableExpression ver && !parameters.ContainsKey(ver.Key)) ? Result(false) : Right.Invoke(parameters);
                        bool result = leftResult.BooleanResult && rightResult.BooleanResult;
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.BooleanOr:
                    {
                        ResultExpression leftResult = (Left is VariableExpression vel && !parameters.ContainsKey(vel.Key)) ? Result(false) : Left.Invoke(parameters);
                        ResultExpression rightResult = (Right is VariableExpression ver && !parameters.ContainsKey(ver.Key)) ? Result(false) : Right.Invoke(parameters);
                        bool result = leftResult.BooleanResult || rightResult.BooleanResult;
                        InvokeResult = Result(result);
                        break;
                    }
                default:
                    {
                        throw new NotSupportedException($"Not Supported {Type} In BinaryExpression");
                    }
            }

            return InvokeResult;
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

            switch (Type)
            {
                case ExpressionType.Add:
                {
                    sb.Append(" + ");
                    break;
                }
                case ExpressionType.Subtract:
                {
                    sb.Append(" - ");
                    break;
                }
                case ExpressionType.Multiply:
                {
                    sb.Append(" * ");
                    break;
                }
                case ExpressionType.Divide:
                {
                    sb.Append(" / ");
                    break;
                }
                case ExpressionType.Greater:
                {
                    sb.Append(" > ");
                    break;
                }
                case ExpressionType.GreaterEqual:
                {
                    sb.Append(" >= ");
                    break;
                }
                case ExpressionType.Less:
                {
                    sb.Append(" < ");
                    break;
                }
                case ExpressionType.LessEqual:
                {
                    sb.Append(" <= ");
                    break;
                }
                case ExpressionType.Equal:
                {
                    sb.Append(" == ");
                    break;
                }
                case ExpressionType.NotEqual:
                {
                    sb.Append(" != ");
                    break;
                }
                case ExpressionType.Modulo:
                {
                    sb.Append(" % ");
                    break;
                }
                case ExpressionType.BooleanAnd:
                {
                    sb.Append(" && ");
                    break;
                }
                case ExpressionType.BooleanOr:
                {
                    sb.Append(" || ");
                    break;
                }
                case ExpressionType.Power:
                {
                    sb.Append("^");
                    break;
                }
                default:
                {
                    throw new NotSupportedException($"Not Supported {Type} In BinaryExpression");
                }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HLC.Expression
{
    /// <summary>
    /// 函数表达式
    /// </summary>
    public class FunctionExpression : Expression
    {
        public IList<Expression> Children { get; set; }

        public FunctionExpression(ExpressionType type, IList<Expression> children) : base(type)
        {
            Children = children;
        }

        public override ResultExpression Invoke(Parameters parameters)
        {
            switch (Type)
            {
                #region 非函数，不处理

                case ExpressionType.Data:
                case ExpressionType.Variable:
                case ExpressionType.StringConstant:
                case ExpressionType.RangeConstant:
                case ExpressionType.NumberConstant:
                case ExpressionType.BooleanConstant:
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.Greater:
                case ExpressionType.GreaterEqual:
                case ExpressionType.Less:
                case ExpressionType.LessEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Power:
                case ExpressionType.Modulo:
                case ExpressionType.BooleanAnd:
                case ExpressionType.BooleanOr:
                    {
                        // 非函数，不处理
                        break;
                    }

                #endregion

                #region 判断函数

                case ExpressionType.If:
                    {
                        if (Children[0] is VariableExpression ve && !parameters.ContainsKey(ve.Key))
                        {
                            InvokeResult = Children[2].Invoke(parameters);
                            break;
                        }

                        ResultExpression conditionInvoke = Children[0].Invoke(parameters);
                        if (conditionInvoke.BooleanResult)
                        {
                            InvokeResult = Children[1].Invoke(parameters);
                            break;
                        }
                        else
                        {
                            InvokeResult = Children[2].Invoke(parameters);
                            break;
                        }
                    }
                case ExpressionType.Switch:
                case ExpressionType.SwitchC:
                    {
                        // switch 有专门的处理类
                        break;
                    }

                #endregion

                #region 数值函数

                case ExpressionType.Rounding:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        var digits = Children[1].Invoke(parameters).NumberResult;
                        var result = Math.Round(value, (int)digits, MidpointRounding.AwayFromZero);
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.Ceiling:
                    {
                        ResultExpression invoke = Children[0].Invoke(parameters);
                        var result = invoke.NumberResult;
                        InvokeResult = Result(Math.Ceiling(result));
                        break;
                    }
                case ExpressionType.Flooring:
                    {
                        ResultExpression invoke = Children[0].Invoke(parameters);
                        var result = invoke.NumberResult;
                        InvokeResult = Result(Math.Floor(result));
                        break;
                    }
                case ExpressionType.Max:
                    {
                        var max = Children[0].Invoke(parameters).NumberResult;
                        for (int i = 1; i < Children.Count; i++)
                        {
                            max = Math.Max(max, Children[i].Invoke(parameters).NumberResult);
                        }
                        InvokeResult = Result(max);
                        break;
                    }
                case ExpressionType.Min:
                    {
                        var min = Children[0].Invoke(parameters).NumberResult;
                        for (int i = 1; i < Children.Count; i++)
                        {
                            min = Math.Min(min, Children[i].Invoke(parameters).NumberResult);
                        }
                        InvokeResult = Result(min);
                        break;
                    }

                #endregion

                #region 文本函数

                case ExpressionType.Concat:
                    {
                        var sb = new StringBuilder();
                        foreach (var child in Children)
                        {
                            var result = child.Invoke(parameters);
                            if (result.IsList())
                            {
                                foreach (var item in result.ListResult)
                                {
                                    sb.Append(item);
                                }
                            }
                            else
                            {
                                sb.Append(result.Data);
                            }
                        }
                        InvokeResult = Result(sb.ToString());
                        break;
                    }
                case ExpressionType.SubStr:
                    {
                        var value = Children[0].Invoke(parameters).StringResult;
                        var startIndex = Children[1].Invoke(parameters).NumberResult;
                        if (Children.Count > 2)
                        {
                            var length = Children[2].Invoke(parameters).NumberResult;
                            var result = value.Substring((int)startIndex, (int)length);
                            InvokeResult = Result(result);
                            break;
                        }
                        else
                        {
                            var result = value.Substring((int)startIndex);
                            InvokeResult = Result(result);
                            break;
                        }

                    }
                case ExpressionType.SubNum:
                    {
                        var value = Children[0].Invoke(parameters).StringResult;
                        var startIndex = Children[1].Invoke(parameters).NumberResult;
                        if (Children.Count > 2)
                        {
                            var length = Children[2].Invoke(parameters).NumberResult;
                            var substr = value.Substring((int)startIndex, (int)length);
                            var result = decimal.Parse(substr);
                            InvokeResult = Result(result);
                            break;
                        }
                        else
                        {
                            var substr = value.Substring((int)startIndex);
                            var result = decimal.Parse(substr);
                            InvokeResult = Result(result);
                            break;
                        }
                    }
                case ExpressionType.Left:
                    {
                        var value = Children[0].Invoke(parameters).StringResult;
                        var length = Children[1].Invoke(parameters).NumberResult;
                        var result = value.Substring(0, (int)length);
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.Right:
                    {
                        var value = Children[0].Invoke(parameters).StringResult;
                        var length = Children[1].Invoke(parameters).NumberResult;
                        var result = value.Substring(value.Length - (int)length);
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.Reverse:
                    {
                        var value = Children[0].Invoke(parameters).StringResult;
                        var result = string.Join("", value.Reverse());
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.Length:
                    {
                        var value = Children[0].Invoke(parameters).StringResult;
                        var result = value.Length;
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.FIND:
                    {
                        var value = Children[0].Invoke(parameters).StringResult;
                        var matchValue = Children[1].Invoke(parameters).StringResult;
                        var result = value.IndexOf(matchValue);
                        InvokeResult = Result(result);
                        break;
                    }

                #endregion

                #region 逻辑函数

                case ExpressionType.Not:
                    {
                        if (Children[0] is VariableExpression ve && !parameters.ContainsKey(ve.Key))
                        {
                            InvokeResult = Result(true);
                            break;
                        }
                        else
                        {
                            ResultExpression invoke = Children[0].Invoke(parameters);
                            var result = invoke.BooleanResult;
                            InvokeResult = Result(!result);
                        }

                        break;
                    }
                case ExpressionType.FunctionAnd:
                    {
                        InvokeResult = Result(true);
                        foreach (var child in Children)
                        {
                            if (child is VariableExpression ve && !parameters.ContainsKey(ve.Key))
                            {
                                InvokeResult = Result(false);
                                break;
                            }

                            if (!child.Invoke(parameters).BooleanResult)
                            {
                                InvokeResult = Result(false);
                                break;
                            }
                        }
                        break;
                    }
                case ExpressionType.FunctionOr:
                    {
                        InvokeResult = Result(false);
                        foreach (var child in Children)
                        {
                            if (child is VariableExpression ve && !parameters.ContainsKey(ve.Key))
                            {
                                continue;
                            }

                            if (child.Invoke(parameters).BooleanResult)
                            {
                                InvokeResult = Result(true);
                                break;
                            }
                        }
                        break;
                    }
                case ExpressionType.In:
                    {
                        if (Children[0] is VariableExpression ve && !parameters.ContainsKey(ve.Key))
                        {
                            InvokeResult = Result(false);
                            break;
                        }
                        ResultExpression valueInvoke = Children[0].Invoke(parameters);

                        bool isNumModel = valueInvoke.IsNumber();

                        LinkedList<ResultExpression> invokeResults = new LinkedList<ResultExpression>();
                        for (int i = 1; i < Children.Count; i++)
                        {
                            if (Children[i] is VariableExpression ve2 && !parameters.ContainsKey(ve2.Key))
                            {
                                continue;
                            }
                            ResultExpression invoke = Children[i].Invoke(parameters);
                            if (!(invoke.IsNumber() || invoke.IsNumberList()))
                            {
                                isNumModel = false;
                            }

                            invokeResults.AddLast(invoke);
                        }

                        if (isNumModel)
                        {
                            InvokeResult = Result(false);
                            foreach (var result in invokeResults)
                            {
                                if (result.IsNumber() && ExpressionSetting.Instance.AreEquals(valueInvoke.NumberResult, result.NumberResult))
                                {
                                    InvokeResult = Result(true);
                                    break;
                                }

                                if (result.IsNumberList())
                                {
                                    foreach (var res in result.NumberListResult)
                                    {
                                        if (ExpressionSetting.Instance.AreEquals(valueInvoke.NumberResult, res))
                                        {
                                            InvokeResult = Result(true);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            InvokeResult = Result(false);
                            foreach (var result in invokeResults)
                            {
                                if (result.IsList())
                                {
                                    foreach (object res in result.ListResult)
                                    {
                                        if (valueInvoke.Data.Equals(res))
                                        {
                                            InvokeResult = Result(true);
                                            break;
                                        }
                                    }
                                }
                                if (valueInvoke.Data.Equals(result.Data))
                                {
                                    InvokeResult = Result(true);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case ExpressionType.HasValue:
                    {
                        if (Children[0] is VariableExpression ve)
                        {
                            if (parameters.TryGetValue(ve.Key, out var value))
                            {
                                InvokeResult = Result(!string.IsNullOrEmpty(value.Data?.ToString()));
                                break;
                            }
                            else
                            {
                                InvokeResult = Result(false);
                                break;
                            }
                        }
                        else
                        {
                            ResultExpression invoke = Children[0].Invoke(parameters);
                            InvokeResult = Result(!string.IsNullOrEmpty(invoke.Data?.ToString()));
                        }

                        break;
                    }
                case ExpressionType.IsNumber:
                    {
                        ResultExpression invoke = Children[0].Invoke(parameters);
                        var result = invoke.IsNumber();
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.IsStart:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        var matchExpression = Children[1].Invoke(parameters);
                        var result = valueExpression.StringResult.StartsWith(matchExpression.StringResult);
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.IsEnd:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        var matchExpression = Children[1].Invoke(parameters);
                        var result = valueExpression.StringResult.EndsWith(matchExpression.StringResult);
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.IsMatch:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        var matchExpression = Children[1].Invoke(parameters);
                        var result = Regex.IsMatch(valueExpression.StringResult, matchExpression.StringResult);
                        InvokeResult = Result(result);
                        break;
                    }

                #endregion

                #region 参数函数

                case ExpressionType.DateTime:
                    {
                        var value = Children[0].Invoke(parameters).StringResult;
                        if (Children.Count < 2)
                        {
                            InvokeResult = Result(DateTime.Parse(value));
                            break;
                        }
                        else
                        {
                            InvokeResult = Result(DateTime.ParseExact(value, Children[1].Invoke(parameters).StringResult, null));
                            break;
                        }
                    }
                case ExpressionType.Get:
                    {
                        if (Children[0] is VariableExpression ve && parameters.ContainsKey(ve.Key))
                        {
                            InvokeResult = Children[0].Invoke(parameters);
                            break;
                        }
                        else
                        {
                            InvokeResult = Children[1].Invoke(parameters);
                            break;
                        }
                    }
                case ExpressionType.AsNum:
                case ExpressionType.ToNum:
                    {
                        var result = Children[0].Invoke(parameters);
                        InvokeResult = Result(result.NumberResult);
                        break;
                    }
                case ExpressionType.ToStr:
                    {
                        var result = Children[0].Invoke(parameters);

                        switch (result.DataType)
                        {
                            case ResultType.Number:
                                {
                                    InvokeResult = Children.Count < 2 ? Result(result.NumberResult.ToString()) : Result(result.NumberResult.ToString(Children[1].Invoke(parameters).StringResult));
                                    break;
                                }
                            case ResultType.DateTime:
                                {
                                    InvokeResult = Children.Count < 2 ? Result(ExpressionSetting.Instance.ToString(result.DateTimeResult)) : Result(result.DateTimeResult.ToString(Children[1].Invoke(parameters).StringResult));
                                    break;
                                }
                            case ResultType.Boolean:
                                {
                                    InvokeResult = Result(ExpressionSetting.Instance.ToString(result.BooleanResult));
                                    break;
                                }
                            default:
                                {
                                    InvokeResult = Result(result.ToString());
                                    break;
                                }
                        }
                        break;
                    }
                case ExpressionType.META:
                    {
                        var value = Children[0] as VariableExpression;
                        if (!parameters.ContainsKey(value.Key))
                        {
                            InvokeResult = new ResultExpression(ResultType.Empty, null);
                            break;
                        }
                        var parameter = parameters[value.Key];

                        var metadata = Children[1] as ConstantExpression;

                        var formate = "txt";
                        if (Children.Count > 2)
                        {
                            formate = (Children[2] as ConstantExpression).Value.ToString();
                        }

                        if (!parameter.Metadata.TryGetValue(metadata.Value.ToString(), out string v))
                        {
                            if (ExpressionSetting.Instance.CheckVariableExist)
                            {
                                throw new ExpressionParameterException(value.Key, $"Not Found Metadata {metadata.Value}");
                            }

                            InvokeResult = new ResultExpression(ResultType.Empty, null);
                            break;
                        }

                        switch (formate)
                        {
                            case "bool":
                                {
                                    InvokeResult = Result(Convert.ToBoolean(v));
                                    break;
                                }
                            case "num":
                                {
                                    InvokeResult = Result(Convert.ToDecimal(v));
                                    break;
                                }
                            case "txt":
                            default:
                                {
                                    InvokeResult = Result(v);
                                    break;
                                }
                        }
                        break;
                    }
                case ExpressionType.DATAMETA:
                    {
                        var value = Children[0] as VariableExpression;
                        if (!parameters.ContainsKey(value.Key))
                        {
                            InvokeResult = new ResultExpression(ResultType.Empty, null);
                            break;
                        }
                        var parameter = parameters[value.Key];

                        var metadata = Children[1] as ConstantExpression;

                        var formate = "txt";
                        if (Children.Count > 2)
                        {
                            formate = (Children[2] as ConstantExpression).Value.ToString();
                        }

                        if (!parameter.DataMetadata.TryGetDataValue(parameter.Data.ToString(), metadata.Value.ToString(), out string v))
                        {
                            if (ExpressionSetting.Instance.CheckVariableExist)
                            {
                                throw new ExpressionParameterException(value.Key, $"Not Found DataMetadata {parameter.Data} {metadata.Value}");
                            }

                            InvokeResult = new ResultExpression(ResultType.Empty, null);
                            break;
                        }

                        switch (formate)
                        {
                            case "bool":
                                {
                                    InvokeResult = Result(Convert.ToBoolean(v));
                                    break;
                                }
                            case "num":
                                {
                                    InvokeResult = Result(Convert.ToDecimal(v));
                                    break;
                                }
                            case "txt":
                            default:
                                {
                                    InvokeResult = Result(v);
                                    break;
                                }
                        }
                        break;
                    }

                #endregion

                #region 数学函数

                case ExpressionType.ABS:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result(Math.Abs(value));
                        break;
                    }
                case ExpressionType.SIN:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Sin((double)value));
                        break;
                    }
                case ExpressionType.ASIN:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Asin((double)value));
                        break;
                    }
                case ExpressionType.COS:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Cos((double)value));
                        break;
                    }
                case ExpressionType.ACOS:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Acos((double)value));
                        break;
                    }
                case ExpressionType.TAN:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Tan((double)value));
                        break;
                    }
                case ExpressionType.ATAN:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Atan((double)value));
                        break;
                    }
                case ExpressionType.ATAN2:
                    {
                        var y = Children[0].Invoke(parameters).NumberResult;
                        var x = Children[1].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Atan2((double)y, (double)x));
                        break;
                    }
                case ExpressionType.SINH:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Sinh((double)value));
                        break;
                    }
                case ExpressionType.COSH:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Cosh((double)value));
                        break;
                    }
                case ExpressionType.TANH:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Tanh((double)value));
                        break;
                    }
                case ExpressionType.RAD:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        var radians = ((decimal)Math.PI / 180) * value;
                        InvokeResult = Result(radians);
                        break;
                    }
                case ExpressionType.DEG:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        var degrees = (180 / (decimal)Math.PI) * value;
                        InvokeResult = Result(degrees);
                        break;
                    }
                case ExpressionType.LOG:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        if (Children.Count > 1)
                        {
                            var newBase = Children[1].Invoke(parameters).NumberResult;
                            InvokeResult = Result((decimal)Math.Log((double)value, (double)newBase));
                        }
                        else
                        {
                            InvokeResult = Result((decimal)Math.Log((double)value));
                        }
                        break;
                    }
                case ExpressionType.LOG10:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Log10((double)value));
                        break;
                    }
                case ExpressionType.EXP:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Exp((double)value));
                        break;
                    }
                case ExpressionType.FACT:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        decimal result = 1;
                        for (int i = 2; i <= value; i++)
                        {
                            result *= i;
                        }
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.SQRT:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        InvokeResult = Result((decimal)Math.Sqrt((double)value));
                        break;
                    }
                case ExpressionType.MOD:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        var value2 = Children[1].Invoke(parameters).NumberResult;
                        int result = (int)value % (int)value2;
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.POW:
                    {
                        var value = Children[0].Invoke(parameters).NumberResult;
                        var value2 = Children[1].Invoke(parameters).NumberResult;
                        var result = (decimal)Math.Pow((double)value, (double)value2);
                        InvokeResult = Result(result);
                        break;
                    }
                case ExpressionType.PI:
                    {
                        InvokeResult = Result((decimal)Math.PI);
                        break;
                    }

                #endregion

                #region 数组函数

                case ExpressionType.ASUM:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        if (valueExpression.IsList())
                        {
                            decimal result = 0;
                            foreach (var item in valueExpression.ListResult)
                            {
                                if (decimal.TryParse(item.ToString(), out var d))
                                {
                                    result += d;
                                }
                            }
                            InvokeResult = Result(result);
                            break;
                        }
                        else
                        {
                            InvokeResult = Result(valueExpression.NumberResult);
                            break;
                        }
                    }
                case ExpressionType.ACOUNT:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        if (valueExpression.IsList())
                        {
                            var values = valueExpression.ListResult;
                            var result = values.Count;
                            InvokeResult = Result(result);
                            break;
                        }
                        else
                        {
                            InvokeResult = Result(1);
                            break;

                        }
                    }
                case ExpressionType.AINDEX:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        var index = Children[1].Invoke(parameters).NumberResult;
                        if (index < 0)
                        {
                            InvokeResult = Result("");
                            break;
                        }

                        if (!valueExpression.IsList())
                        {
                            InvokeResult = Result(valueExpression.StringResult);
                            break;
                        }

                        if (valueExpression.IsNumberList())
                        {
                            var values = valueExpression.NumberListResult;
                            if (values.Count > index)
                            {
                                InvokeResult = Result(values[(int)index]);
                                break;
                            }
                            else
                            {
                                InvokeResult = Result("");
                                break;
                            }
                        }
                        else
                        {
                            var values = valueExpression.ListResult;
                            if (values.Count > index)
                            {
                                InvokeResult = Result(values[(int)index].ToString());
                                break;
                            }
                            else
                            {
                                InvokeResult = Result("");
                                break;
                            }
                        }
                    }
                case ExpressionType.AMATCH:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        if (!valueExpression.IsList())
                        {
                            InvokeResult = Result(-1);
                            break;
                        }
                        var matchExpression = Children[1].Invoke(parameters);
                        if (valueExpression.IsNumberList() && matchExpression.IsNumber())
                        {
                            var values = valueExpression.NumberListResult;
                            InvokeResult = Result(-1);
                            for (int i = 0; i < values.Count; i++)
                            {
                                if (values[i] == matchExpression.NumberResult)
                                {
                                    InvokeResult = Result(i);
                                    break;
                                }
                            }
                            break;
                        }
                        else
                        {
                            var values = valueExpression.ListResult;
                            InvokeResult = Result(-1);
                            for (int i = 0; i < values.Count; i++)
                            {
                                if (values[i]?.ToString() == matchExpression.StringResult)
                                {
                                    InvokeResult = Result(i);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                case ExpressionType.AMAX:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        if (valueExpression.IsList())
                        {
                            var values = valueExpression.ListResult;
                            if (values.Count <= 0)
                            {
                                InvokeResult = Result(0);
                                break;
                            }
                            var result = values[0].ToDecimal(0);
                            foreach (var item in values)
                            {
                                result = Math.Max(item.ToDecimal(0), result);
                            }
                            InvokeResult = Result(result);
                            break;
                        }
                        else
                        {
                            InvokeResult = Result(valueExpression.NumberResult);
                            break;
                        }
                    }
                case ExpressionType.AMIN:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        if (valueExpression.IsList())
                        {
                            var values = valueExpression.ListResult;
                            if (values.Count <= 0)
                            {
                                InvokeResult = Result(0);
                                break;
                            }
                            var result = values[0].ToDecimal(0);
                            foreach (var item in values)
                            {
                                result = Math.Min(item.ToDecimal(0), result);
                            }
                            InvokeResult = Result(result);
                            break;
                        }
                        else
                        {
                            InvokeResult = Result(valueExpression.NumberResult);
                            break;
                        }
                    }
                case ExpressionType.AMAXDIFF:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        if (valueExpression.IsList())
                        {
                            var values = valueExpression.ListResult;
                            if (values.Count <= 1)
                            {
                                InvokeResult = Result(0);
                                break;
                            }
                            decimal result = 0;
                            for (int i = 1; i < values.Count; i++)
                            {
                                result = Math.Max(Math.Abs(values[i].ToDecimal(0) - values[i - 1].ToDecimal(0)), result);
                            }
                            InvokeResult = Result(result);
                            break;
                        }
                        else
                        {
                            InvokeResult = Result(0);
                            break;
                        }
                    }
                case ExpressionType.AMINDIFF:
                    {
                        var valueExpression = Children[0].Invoke(parameters);
                        if (valueExpression.IsList())
                        {
                            var values = valueExpression.ListResult;
                            if (values.Count <= 1)
                            {
                                InvokeResult = Result(0);
                                break;
                            }
                            decimal result = decimal.MaxValue;
                            for (int i = 1; i < values.Count; i++)
                            {
                                result = Math.Min(Math.Abs(values[i].ToDecimal(0) - values[i - 1].ToDecimal(0)), result);
                            }
                            InvokeResult = Result(result);
                            break;
                        }
                        else
                        {
                            InvokeResult = Result(0);
                            break;
                        }
                    }

                #endregion

                default:
                    {
                        throw new NotSupportedException($"Not Supported {Type} in FunctionExpression");
                    }
            }

            return InvokeResult;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            switch (this.Type)
            {
                #region 非函数，不处理

                case ExpressionType.Data:
                case ExpressionType.Variable:
                case ExpressionType.StringConstant:
                case ExpressionType.RangeConstant:
                case ExpressionType.NumberConstant:
                case ExpressionType.BooleanConstant:
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.Greater:
                case ExpressionType.GreaterEqual:
                case ExpressionType.Less:
                case ExpressionType.LessEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Power:
                case ExpressionType.Modulo:
                case ExpressionType.BooleanAnd:
                case ExpressionType.BooleanOr:
                    {
                        // 非函数，不处理
                        break;
                    }

                #endregion

                #region 判断函数

                case ExpressionType.If:
                    {
                        return $"IF({Children[0]}, {Children[1]}, {Children[2]})";
                    }
                case ExpressionType.Switch:
                case ExpressionType.SwitchC:
                    {
                        // switch 有专门的处理类
                        break;
                    }

                #endregion

                #region 数值函数

                case ExpressionType.Ceiling:
                    {
                        return $"CEILING({Children.FirstOrDefault()})";
                    }
                case ExpressionType.Flooring:
                    {
                        return $"FLOORING({Children.FirstOrDefault()})";
                    }
                case ExpressionType.Rounding:
                    {
                        return $"ROUNDING({Children.FirstOrDefault()})";
                    }
                case ExpressionType.Max:
                    {
                        sb.Append("MAX(");
                        break;
                    }
                case ExpressionType.Min:
                    {
                        sb.Append("MIN(");
                        break;
                    }

                #endregion

                #region 文本函数

                case ExpressionType.Concat:
                    {
                        sb.Append("CONCAT(");
                        break;
                    }
                case ExpressionType.SubStr:
                    {
                        return $"SUBSTR({Children[0]}, {Children[1]}, {Children[2]})";
                    }
                case ExpressionType.SubNum:
                    {
                        return $"SUBNUM({Children[0]}, {Children[1]}, {Children[2]})";
                    }

                case ExpressionType.Left:
                    {
                        sb.Append("LEFT(");
                        break;
                    }
                case ExpressionType.Right:
                    {
                        sb.Append("RIGHT(");
                        break;
                    }
                case ExpressionType.Reverse:
                    {
                        sb.Append("REVERSE(");
                        break;
                    }
                case ExpressionType.FIND:
                    {
                        sb.Append("FIND(");
                        break;
                    }
                case ExpressionType.Length:
                    {
                        sb.Append("LENGTH(");
                        break;
                    }

                #endregion

                #region 逻辑函数


                case ExpressionType.Not:
                    {
                        return $"NOT({Children.FirstOrDefault()})";
                    }
                case ExpressionType.FunctionAnd:
                    {
                        sb.Append("AND(");
                        break;
                    }
                case ExpressionType.FunctionOr:
                    {
                        sb.Append("OR(");
                        break;
                    }
                case ExpressionType.In:
                    {
                        sb.Append("IN(");
                        break;
                    }
                case ExpressionType.HasValue:
                    {
                        return $"HASVALUE({Children.FirstOrDefault()})";
                    }
                case ExpressionType.IsNumber:
                    {
                        return $"ISNUMBER({Children.FirstOrDefault()})";
                    }
                case ExpressionType.IsStart:
                    {
                        sb.Append("ISSTART(");
                        break;
                    }
                case ExpressionType.IsEnd:
                    {
                        sb.Append("ISEND(");
                        break;
                    }
                case ExpressionType.IsMatch:
                    {
                        sb.Append("ISMATCH(");
                        break;
                    }

                #endregion

                #region 参数函数

                case ExpressionType.DateTime:
                    {
                        sb.Append("DATETIME(");
                        break;
                    }
                case ExpressionType.Get:
                    {
                        return $"GET({Children[0]}, {Children[1]})";
                    }
                case ExpressionType.AsNum:
                    {
                        return $"ASNUM({Children[0]})";
                    }
                case ExpressionType.ToNum:
                    {
                        sb.Append("TONUM(");
                        break;
                    }
                case ExpressionType.ToStr:
                    {
                        sb.Append("TOSTR(");
                        break;
                    }
                case ExpressionType.META:
                    {
                        sb.Append("META(");
                        break;
                    }
                case ExpressionType.DATAMETA:
                    {
                        sb.Append("DATAMETA(");
                        break;
                    }

                #endregion

                #region 数学函数

                case ExpressionType.ABS:
                    {
                        return $"ABS({Children[0]})";
                    }
                case ExpressionType.SIN:
                    {
                        return $"SIN({Children[0]})";
                    }
                case ExpressionType.ASIN:
                    {
                        sb.Append("ASIN(");
                        break;
                    }
                case ExpressionType.COS:
                    {
                        sb.Append("COS(");
                        break;
                    }
                case ExpressionType.ACOS:
                    {
                        sb.Append("ACOS(");
                        break;
                    }
                case ExpressionType.TAN:
                    {
                        sb.Append("TAN(");
                        break;
                    }
                case ExpressionType.ATAN:
                    {
                        sb.Append("ATAN(");
                        break;
                    }
                case ExpressionType.ATAN2:
                    {
                        sb.Append("ATAN2(");
                        break;
                    }
                case ExpressionType.SINH:
                    {
                        sb.Append("SINH(");
                        break;
                    }
                case ExpressionType.COSH:
                    {
                        sb.Append("COSH(");
                        break;
                    }
                case ExpressionType.TANH:
                    {
                        sb.Append("TANH(");
                        break;
                    }
                case ExpressionType.RAD:
                    {
                        sb.Append("DEG2RAD(");
                        break;
                    }
                case ExpressionType.DEG:
                    {
                        sb.Append("RAD2DEG(");
                        break;
                    }
                case ExpressionType.LOG:
                    {
                        sb.Append("LOG(");
                        break;
                    }
                case ExpressionType.LOG10:
                    {
                        sb.Append("LOG10(");
                        break;
                    }
                case ExpressionType.EXP:
                    {
                        sb.Append("EXP(");
                        break;
                    }
                case ExpressionType.FACT:
                    {
                        sb.Append("FACT(");
                        break;
                    }
                case ExpressionType.SQRT:
                    {
                        sb.Append("SQRT(");
                        break;
                    }
                case ExpressionType.MOD:
                    {
                        sb.Append("MOD(");
                        break;
                    }
                case ExpressionType.POW:
                    {
                        sb.Append("POW(");
                        break;
                    }
                case ExpressionType.PI:
                    {
                        return $"PI()";
                    }

                #endregion

                #region 数组函数

                case ExpressionType.ASUM:
                    {
                        sb.Append("ASUM(");
                        break;
                    }
                case ExpressionType.AINDEX:
                    {
                        sb.Append("AINDEX(");
                        break;
                    }
                case ExpressionType.AMATCH:
                    {
                        sb.Append("AMATCH(");
                        break;
                    }
                case ExpressionType.ACOUNT:
                    {
                        sb.Append("ACOUNT(");
                        break;
                    }
                case ExpressionType.AMAX:
                    {
                        sb.Append("AMAX(");
                        break;
                    }
                case ExpressionType.AMIN:
                    {
                        sb.Append("AMIN(");
                        break;
                    }
                case ExpressionType.AMAXDIFF:
                    {
                        sb.Append("AMAXDIFF(");
                        break;
                    }
                case ExpressionType.AMINDIFF:
                    {
                        sb.Append("AMINDIFF(");
                        break;
                    }

                #endregion

                default:
                    {
                        throw new NotSupportedException($"Not Supported {Type} in FunctionExpression");
                    }
            }
            if (Children.Count >= 0)
            {
                sb.Append(Children[0]);
                for (int i = 1; i < Children.Count; i++)
                {
                    sb.Append(", " + Children[i]);
                }
            }

            sb.Append(")");
            return sb.ToString();
        }

        public override IList<string> GetVariableKeys()
        {
            IEnumerable<string> result = new List<string>();
            for (int i = 0; i < Children.Count; i++)
            {
                result = result.Concat(Children[i].GetVariableKeys());
            }
            return result.ToList();
        }
    }
}

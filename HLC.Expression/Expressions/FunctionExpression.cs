using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            if (Type == ExpressionType.Max)
            {
                double max = Children[0].Invoke(parameters).NumberResult;
                for (int i = 1; i < Children.Count; i++)
                {
                    max = Math.Max(max, Children[i].Invoke(parameters).NumberResult);
                }
                return Result(max);
            }
            if (Type == ExpressionType.Min)
            {
                double min = Children[0].Invoke(parameters).NumberResult;
                for (int i = 1; i < Children.Count; i++)
                {
                    min = Math.Min(min, Children[i].Invoke(parameters).NumberResult);
                }
                return Result(min);
            }
            if (Type == ExpressionType.In)
            {

                ResultExpression valueInvoke = Children[0].Invoke(parameters);

                bool isNumModel = valueInvoke.IsNumber();

                LinkedList<ResultExpression> invokeResults = new LinkedList<ResultExpression>();
                for (int i = 1; i < Children.Count; i++)
                {
                    ResultExpression invoke = Children[i].Invoke(parameters);
                    if (!(invoke.IsNumber() || invoke.IsNumberList()))
                    {
                        isNumModel = false;
                    }

                    invokeResults.AddLast(invoke);
                }

                if (isNumModel)
                {
                    foreach (var result in invokeResults)
                    {
                        if (result.IsNumber() && ExpressionSetting.Instance.AreEquals(valueInvoke.NumberResult, result.NumberResult))
                        {
                            return Result(true);
                        }

                        if (result.IsNumberList())
                        {
                            foreach (var res in result.NumberListResult)
                            {
                                if (ExpressionSetting.Instance.AreEquals(valueInvoke.NumberResult, res))
                                {
                                    return Result(true);
                                }
                            }
                        }
                    }
                    return Result(false);
                }
                else
                {
                    foreach (var result in invokeResults)
                    {
                        if (result.IsList())
                        {
                            foreach (object res in result.ListResult)
                            {
                                if (valueInvoke.Data.Equals(res))
                                {
                                    return Result(true);
                                }
                            }
                        }
                        if (valueInvoke.Data.Equals(result.Data))
                        {
                            return Result(true);
                        }
                    }
                    return Result(false);
                }
            }
            if (Type == ExpressionType.Rounding)
            {
                var value = Children[0].Invoke(parameters).NumberResult;
                var digits = Children[1].Invoke(parameters).NumberResult;
                var result = Math.Round(value, (int)digits, MidpointRounding.AwayFromZero);
                return Result(result);
            }
            if (Type == ExpressionType.Ceiling)
            {
                ResultExpression invoke = Children[0].Invoke(parameters);
                double result = invoke.NumberResult;
                return Result(Math.Ceiling(result));
            }
            if (Type == ExpressionType.Flooring)
            {
                ResultExpression invoke = Children[0].Invoke(parameters);
                double result = invoke.NumberResult;
                return Result(Math.Floor(result));
            }
            if (Type == ExpressionType.Not)
            {
                ResultExpression invoke = Children[0].Invoke(parameters);
                var result = invoke.BooleanResult;
                return Result(!result);
            }
            if (Type == ExpressionType.If)
            {
                ResultExpression conditionInvoke = Children[0].Invoke(parameters);
                if (conditionInvoke.BooleanResult)
                {
                    return Children[1].Invoke(parameters);
                }
                else
                {
                    return Children[2].Invoke(parameters);
                }
            }
            if (Type == ExpressionType.FunctionAnd)
            {
                foreach (var child in Children)
                {
                    if (!child.Invoke(parameters).BooleanResult)
                    {
                        return Result(false);
                    }
                }

                return Result(true);
            }
            if (Type == ExpressionType.FunctionOr)
            {
                foreach (var child in Children)
                {
                    if (child.Invoke(parameters).BooleanResult)
                    {
                        return Result(true);
                    }
                }

                return Result(false);
            }
            return null;
        }

        public override string ToString()
        {
            if (Type == ExpressionType.Ceiling)
            {
                return $"CEILING({Children.FirstOrDefault()})";
            }
            if (Type == ExpressionType.Flooring)
            {
                return $"FLOORING({Children.FirstOrDefault()})";
            }
            if (Type == ExpressionType.Rounding)
            {
                return $"ROUNDING({Children.FirstOrDefault()})";
            }
            if (Type == ExpressionType.Not)
            {
                return $"NOT({Children.FirstOrDefault()})";
            }

            if (Type == ExpressionType.If)
            {
                return $"IF( {Children[0]}, {Children[1]}, {Children[2]} )";
            }

            StringBuilder sb = new StringBuilder();
            if (Type == ExpressionType.Max)
            {
                sb.Append("MAX(");
            }
            else if (Type == ExpressionType.Min)
            {
                sb.Append("MIN(");
            }
            else if (Type == ExpressionType.In)
            {
                sb.Append("IN(");
            }
            else if (Type == ExpressionType.FunctionAnd)
            {
                sb.Append("AND(");
            }
            else if (Type == ExpressionType.FunctionOr)
            {
                sb.Append("OR(");
            }
            else
            {
                return $"ERROR - {Type}";
            }

            sb.Append(Children[0]);
            for (int i = 1; i < Children.Count; i++)
            {
                sb.Append(", " + Children[i]);
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override IList<string> GetVariableKeys()
        {
            IEnumerable<string> variableKeys = Children[0].GetVariableKeys();
            for (int i = 1; i < Children.Count; i++)
            {
                variableKeys = variableKeys.Concat(Children[i].GetVariableKeys());
            }
            return variableKeys.ToList();
        }

    }
}
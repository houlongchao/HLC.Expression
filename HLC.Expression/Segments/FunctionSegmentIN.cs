using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentIN : FunctionSegment
    {
        public override string MatchString { get; } = "IN(";
        public override Operator MatchOperator { get; } = Operator.IN;
        public override ExpressionType ExpressionType => ExpressionType.IN;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            if (children[0] is VariableExpression ve && !parameters.ContainsKey(ve.Key))
            {
                return Expression.Result(false);
            }
            ResultExpression valueInvoke = children[0].Invoke(parameters);

            bool isNumModel = valueInvoke.IsNumber();

            LinkedList<ResultExpression> invokeResults = new LinkedList<ResultExpression>();
            for (int i = 1; i < children.Count; i++)
            {
                if (children[i] is VariableExpression ve2 && !parameters.ContainsKey(ve2.Key))
                {
                    continue;
                }
                ResultExpression invoke = children[i].Invoke(parameters);
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
                        return Expression.Result(true);
                    }

                    if (result.IsNumberList())
                    {
                        foreach (var res in result.NumberListResult)
                        {
                            if (ExpressionSetting.Instance.AreEquals(valueInvoke.NumberResult, res))
                            {
                                return Expression.Result(true);
                            }
                        }
                    }
                }
                return Expression.Result(false);
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
                                return Expression.Result(true);
                            }
                        }
                    }
                    if (valueInvoke.Data.Equals(result.Data))
                    {
                        return Expression.Result(true);
                    }
                }
                return Expression.Result(false);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Boolean, "IN()", "包含判断")
            {
                Demo = "IN({OPT:A}, 1, 2)  IN({OPT:A}, 1, 2+3)",
                Input = "输入参数至少2个，判断第一个参数是否在后面参数列表中。",
                Output = "逻辑值。如果第一个参数在后面参数列表中则为true，否则为false。",
            };
        }
    }

}

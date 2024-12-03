using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;
namespace HLC.Expression.Segments
{
    public class FunctionSegmentHASVALUE : FunctionSegment
    {
        public override string MatchString { get; } = "HASVALUE(";
        public override Operator MatchOperator { get; } = Operator.HASVALUE;
        public override ExpressionType ExpressionType => ExpressionType.HASVALUE;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            if (children[0] is VariableExpression ve)
            {
                if (parameters.TryGetValue(ve.Key, out var value))
                {
                    return Expression.Result(!string.IsNullOrEmpty(value.Data?.ToString()));
                }
                else
                {
                    return Expression.Result(false);
                }
            }
            else
            {
                ResultExpression invoke = children[0].Invoke(parameters);
                return Expression.Result(!string.IsNullOrEmpty(invoke.Data?.ToString()));
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Boolean, "HASVALUE()", "是否有数据")
            {
                Demo = "HASVALUE({OPT:A})  HASVALUE({OPT:B})",
                Input = "一个参数变量",
                Output = "逻辑值。输出当前参数变量是否有数据。",
            };
        }
    }

}

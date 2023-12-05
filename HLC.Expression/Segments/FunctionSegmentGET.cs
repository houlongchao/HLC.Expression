using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentGET : FunctionSegment
    {
        public override string MatchString { get; } = "GET(";
        public override Operator MatchOperator { get; } = Operator.GET;
        public override ExpressionType ExpressionType => ExpressionType.GET;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            if (children[0] is VariableExpression ve && parameters.ContainsKey(ve.Key))
            {
                return children[0].Invoke(parameters);
            }
            else
            {
                return children[1].Invoke(parameters);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Value, "GET()", "变量为空返回默认值")
            {
                Demo = "GET({OPT:A}, 'AAA')   GET({OPT:NUM},123)",
                Input = "输入参数必须为2个，第一个为要获取变量，第二个为变量默认值",
                Output = "变量值或者默认值",
            };
        }
    }

}

using HLC.Expression.Definitions;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentIF : FunctionSegment
    {
        public override string MatchString { get; } = "IF(";
        public override Operator MatchOperator { get; } = Operator.IF;
        public override ExpressionType ExpressionType => ExpressionType.IF;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            if (children[0] is VariableExpression ve && !parameters.ContainsKey(ve.Key))
            {
                return children[2].Invoke(parameters);
            }

            ResultExpression conditionInvoke = children[0].Invoke(parameters);
            if (conditionInvoke.BooleanResult)
            {
                return children[1].Invoke(parameters);
            }
            else
            {
                return children[2].Invoke(parameters);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.If, "IF()", "逻辑判断")
            {
                Demo = "IF(1>2, true, false)  IF(1 > 2, 2 > 4, false)",
                Input = "输入参数必须为3个，第一个为逻辑值或逻辑结果公式，第二和第三个参数为任意值或任意公式。",
                Output = "任意值。当第一个参数为逻辑true时输出第二个参数结果，否则输出第三个参数结果",
            };
        }
    }

}

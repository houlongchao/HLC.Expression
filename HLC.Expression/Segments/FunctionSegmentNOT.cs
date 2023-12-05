using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentNOT : FunctionSegment
    {
        public override string MatchString { get; } = "NOT(";
        public override Operator MatchOperator { get; } = Operator.NOT;
        public override ExpressionType ExpressionType => ExpressionType.NOT;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            if (children[0] is VariableExpression ve && !parameters.ContainsKey(ve.Key))
            {
                return Expression.Result(true);
            }
            else
            {
                ResultExpression invoke = children[0].Invoke(parameters);
                var result = invoke.BooleanResult;
                return Expression.Result(!result);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Boolean, "NOT()", "逻辑取反")
            {
                Demo = "NOT(true)  NOT(true && false) NOT(1 > 2)",
                Input = "Boolean值，或结果为Boolean的表达式。",
                Output = "逻辑值。输出当前输入的非逻辑。",
            };
        }
    }

}

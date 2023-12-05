using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentMOD : FunctionSegment
    {
        public override string MatchString { get; } = "MOD(";
        public override Operator MatchOperator { get; } = Operator.MOD;
        public override ExpressionType ExpressionType => ExpressionType.MOD;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            var value2 = children[1].Invoke(parameters).NumberResult;
            int result = (int)value % (int)value2;
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "MOD()", "取余数")
            {
                Demo = "MOD(20，3)  MOD(10+20, 1+2)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值"
            };
        }
    }

}

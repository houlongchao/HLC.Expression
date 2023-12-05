using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentABS : FunctionSegment
    {
        public override string MatchString { get; } = "ABS(";
        public override Operator MatchOperator { get; } = Operator.ABS;
        public override ExpressionType ExpressionType => ExpressionType.ABS;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result(Math.Abs(value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "ABS()", "绝对值函数")
            {
                Demo = "ABS(1.1)  ABS(1.1+2.2)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值。输出为输入值的绝对值"
            };
        }
    }

}

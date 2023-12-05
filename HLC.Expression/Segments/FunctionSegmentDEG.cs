using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentDEG : FunctionSegment
    {
        public override string MatchString { get; } = "DEG(";
        public override Operator MatchOperator { get; } = Operator.DEG;
        public override ExpressionType ExpressionType => ExpressionType.DEG;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            var degrees = (180 / (decimal)Math.PI) * value;
            return Expression.Result(degrees);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "DEG()", "弧度转角度")
            {
                Demo = "DEG(1.1)  DEG(1.1+2.2)",
                Input = "数值(弧度)，或结果为数值(弧度)的表达式",
                Output = "数值(角度)。输出为输入弧度的角度值"
            };
        }
    }

}

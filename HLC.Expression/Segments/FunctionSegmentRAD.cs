using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentRAD : FunctionSegment
    {
        public override string MatchString { get; } = "RAD(";
        public override Operator MatchOperator { get; } = Operator.RAD;
        public override ExpressionType ExpressionType => ExpressionType.RAD;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            var radians = ((decimal)Math.PI / 180) * value;
            return Expression.Result(radians);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "RAD()", "角度转弧度")
            {
                Demo = "RAD(1.1)  RAD(1.1+2.2)",
                Input = "数值(角度)，或结果为数值(角度)的表达式",
                Output = "数值(弧度)。输出为输入角度的弧度值"
            };
        }
    }

}

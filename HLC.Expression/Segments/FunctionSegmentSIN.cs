using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentSIN : FunctionSegment
    {
        public override string MatchString { get; } = "SIN(";
        public override Operator MatchOperator { get; } = Operator.SIN;
        public override ExpressionType ExpressionType => ExpressionType.SIN;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Sin((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "SIN()", "正弦函数")
            {
                Demo = "SIN(1.1)  SIN(1.1+2.2)",
                Input = "数值(弧度)，或结果为数值(弧度)的表达式",
                Output = "数值(正弦值)。输出为输入弧度的正弦值"
            };
        }
    }

}

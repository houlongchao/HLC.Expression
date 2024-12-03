using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentASIN : FunctionSegment
    {
        public override string MatchString { get; } = "ASIN(";
        public override Operator MatchOperator { get; } = Operator.ASIN;
        public override ExpressionType ExpressionType => ExpressionType.ASIN;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Asin((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "ASIN()", "反正弦函数")
            {
                Demo = "ASIN(1.1)  ASIN(1.1+2.2)",
                Input = "数值(正弦值)，或结果为数值(正弦值)的表达式",
                Output = "数值(弧度)。输出为输入正弦值的弧度"
            };
        }
    }

}

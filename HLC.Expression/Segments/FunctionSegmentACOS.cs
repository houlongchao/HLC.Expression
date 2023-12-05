using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentACOS : FunctionSegment
    {
        public override string MatchString { get; } = "ACOS(";
        public override Operator MatchOperator { get; } = Operator.ACOS;
        public override ExpressionType ExpressionType => ExpressionType.ACOS;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Acos((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "ACOS()", "反余弦函数")
            {
                Demo = "ACOS(1.1)  ACOS(1.1+2.2)",
                Input = "数值(余弦值)，或结果为数值(余弦值)的表达式",
                Output = "数值(弧度)。输出为输入余弦值的弧度"
            };
        }
    }

}

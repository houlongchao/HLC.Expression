using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentATAN : FunctionSegment
    {
        public override string MatchString { get; } = "ATAN(";
        public override Operator MatchOperator { get; } = Operator.ATAN;
        public override ExpressionType ExpressionType => ExpressionType.ATAN;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Atan((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "ATAN()", "反正切函数")
            {
                Demo = "ATAN(1.1)  ATAN(1.1+2.2)",
                Input = "数值(正切值)，或结果为数值(正切值)的表达式",
                Output = "数值(弧度)。输出为输入正切值的弧度"
            };
        }
    }

}

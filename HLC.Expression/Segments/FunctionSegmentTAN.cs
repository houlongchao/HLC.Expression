using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentTAN : FunctionSegment
    {
        public override string MatchString { get; } = "TAN(";
        public override Operator MatchOperator { get; } = Operator.TAN;
        public override ExpressionType ExpressionType => ExpressionType.TAN;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Tan((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "TAN()", "正切函数")
            {
                Demo = "TAN(1.1)  TAN(1.1+2.2)",
                Input = "数值(弧度)，或结果为数值(弧度)的表达式",
                Output = "数值(正切值)。输出为输入弧度的正切值"
            };
        }
    }

}

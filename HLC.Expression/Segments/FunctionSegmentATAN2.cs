using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentATAN2 : FunctionSegment
    {
        public override string MatchString { get; } = "ATAN2(";
        public override Operator MatchOperator { get; } = Operator.ATAN2;
        public override ExpressionType ExpressionType => ExpressionType.ATAN2;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var x = children[0].Invoke(parameters).NumberResult;
            var y = children[1].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Atan2((double)y, (double)x));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "ATAN2()", "反余切函数")
            {
                Demo = "ATAN2(1,2)  ATAN2(1+1,2+2)",
                Input = "数值(X,Y坐标)，或结果为数值(X,Y坐标)的表达式",
                Output = "数值(弧度)。输出为输入坐标的反余切弧度"
            };
        }
    }

}

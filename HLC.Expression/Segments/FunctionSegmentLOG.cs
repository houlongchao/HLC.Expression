using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentLOG : FunctionSegment
    {
        public override string MatchString { get; } = "LOG(";
        public override Operator MatchOperator { get; } = Operator.LOG;
        public override ExpressionType ExpressionType => ExpressionType.LOG;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            if (children.Count > 1)
            {
                var newBase = children[1].Invoke(parameters).NumberResult;
                return Expression.Result((decimal)Math.Log((double)value, (double)newBase));
            }
            else
            {
                return Expression.Result((decimal)Math.Log((double)value));
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "LOG()", "自然对数")
            {
                Demo = "LOG(2)  LOG(2, 5)",
                Input = "数值，或结果为数值的表达式。支持一个参数或两个参数，第二个参数默认e",
                Output = "数值。输出为输入值的对数，默认对数底为e"
            };
        }
    }

}

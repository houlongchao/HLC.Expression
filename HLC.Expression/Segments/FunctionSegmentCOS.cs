using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentCOS : FunctionSegment
    {
        public override string MatchString { get; } = "COS(";
        public override Operator MatchOperator { get; } = Operator.COS;
        public override ExpressionType ExpressionType => ExpressionType.COS;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Cos((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "COS()", "余弦函数")
            {
                Demo = "COS(1.1)  COS(1.1+2.2)",
                Input = "数值(弧度)，或结果为数值(弧度)的表达式",
                Output = "数值(余弦值)。输出为输入弧度的余弦值"
            };
        }
    }

}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentLOG10 : FunctionSegment
    {
        public override string MatchString { get; } = "LOG10(";
        public override Operator MatchOperator { get; } = Operator.LOG10;
        public override ExpressionType ExpressionType => ExpressionType.LOG10;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            return Expression.Result((decimal)Math.Log10((double)value));
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "LOG10()", "以10为底对数")
            {
                Demo = "LOG10(2)  LOG10(2)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值。输出为输入值以10为底的对数"
            };
        }
    }

}

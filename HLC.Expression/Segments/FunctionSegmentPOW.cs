using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentPOW : FunctionSegment
    {
        public override string MatchString { get; } = "POW(";
        public override Operator MatchOperator { get; } = Operator.POW;
        public override ExpressionType ExpressionType => ExpressionType.POW;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0].Invoke(parameters).NumberResult;
            var value2 = children[1].Invoke(parameters).NumberResult;
            var result = (decimal)Math.Pow((double)value, (double)value2);
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Math, "POW()", "指数")
            {
                Demo = "POW(20, 3)  POW(10+20, 1+2)",
                Input = "数值，或结果为数值的表达式",
                Output = "数值"
            };
        }
    }

}

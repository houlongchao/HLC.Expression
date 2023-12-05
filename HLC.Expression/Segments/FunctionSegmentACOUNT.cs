using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentACOUNT : FunctionSegment
    {
        public override string MatchString { get; } = "ACOUNT(";
        public override Operator MatchOperator { get; } = Operator.ACOUNT;
        public override ExpressionType ExpressionType => ExpressionType.ACOUNT;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var valueExpression = children[0].Invoke(parameters);
            if (valueExpression.IsList())
            {
                var values = valueExpression.ListResult;
                var result = values.Count;
                return Expression.Result(result);
            }
            else
            {
                return Expression.Result(1);
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Array, "ACOUNT()", "数组长度")
            {
                Demo = "ACOUNT({Array})  ACOUNT({Array})",
                Input = "数值数组参数，或者结果为数值数组的表达式",
                Output = "整数。输出输入数组的长度。"
            };
        }
    }

}

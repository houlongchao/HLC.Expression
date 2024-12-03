using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentASNUM : FunctionSegment
    {
        public override string MatchString { get; } = "ASNUM(";
        public override Operator MatchOperator { get; } = Operator.ASNUM;
        public override ExpressionType ExpressionType => ExpressionType.ASNUM;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var result = children[0].Invoke(parameters);
            return Expression.Result(result.NumberResult);
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            // 准备移除该函数，所以不返回定义，但为了兼容保留解析和计算
            return null;
        }
    }

}

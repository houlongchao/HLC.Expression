using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentNOW : FunctionSegment
    {
        public override string MatchString { get; } = "NOW(";
        public override Operator MatchOperator { get; } = Operator.NOW;
        public override ExpressionType ExpressionType => ExpressionType.NOW;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            if (children.Count == 0)
            {
                return Expression.Result(DateTime.Now);
            }
            else
            {
                var format = children[0].Invoke(parameters).StringResult;
                if ("UTC".Equals(format, StringComparison.CurrentCultureIgnoreCase))
                {
                    return Expression.Result(DateTime.UtcNow);
                }
                else
                {
                    return Expression.Result(DateTime.Now);
                }
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Value, "NOW()", "获取当前系统时间")
            {
                Demo = "NOW()   NOW('UTC')",
                Input = "无参数时返回本地时间，参数为UTC时返回UTC时间",
                Output = "当前系统时间，日期类型",
            };
        }
    }

}

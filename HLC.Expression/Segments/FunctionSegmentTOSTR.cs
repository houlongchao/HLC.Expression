using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentTOSTR : FunctionSegment
    {
        public override string MatchString { get; } = "TOSTR(";
        public override Operator MatchOperator { get; } = Operator.TOSTR;
        public override ExpressionType ExpressionType => ExpressionType.TOSTR;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var result = children[0].Invoke(parameters);

            switch (result.DataType)
            {
                case ResultType.Number:
                    {
                        return children.Count < 2
                            ? Expression.Result(result.NumberResult.ToString())
                            : Expression.Result(result.NumberResult.ToString(children[1].Invoke(parameters).StringResult));
                    }
                case ResultType.DateTime:
                    {
                        return children.Count < 2
                            ? Expression.Result(ExpressionSetting.Instance.ToString(result.DateTimeResult))
                            : Expression.Result(result.DateTimeResult.ToString(children[1].Invoke(parameters).StringResult));
                    }
                case ResultType.Boolean:
                    {
                        return Expression.Result(ExpressionSetting.Instance.ToString(result.BooleanResult));
                    }
                default:
                    {
                        return Expression.Result(result.ToString());
                    }
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Value, "TOSTR()", "转为字符串")
            {
                Demo = "TOSTR({OPT:DATETIME})   TOSTR({OPT:DATETIME}, 'yyyyMMdd')",
                Input = "变量和一个可选格式化字符串",
                Output = "字符串",
            };
        }
    }

}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{

    public class FunctionSegmentSWITCH : FunctionSegment
    {
        public override string MatchString { get; } = "SWITCH(";
        public override Operator MatchOperator { get; } = Operator.SWITCH;
        public override ExpressionType ExpressionType => ExpressionType.SWITCH;
        public override bool IsPairArgs { get; } = true;

        public override ResultExpression InvokePair(Expression value, IList<Tuple<Expression, Expression>> children, Parameters parameters)
        {
            var valueResult = value.Invoke(parameters);
            if (valueResult == null)
            {
                throw new ExpressionCalculateException("Input value cannot be null", this.ToString());
            }
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                var item1 = child.Item1.Invoke(parameters);
                if (item1 == null)
                {
                    continue;
                }
                if (valueResult.IsNumber() && item1.IsNumber() && ExpressionSetting.Instance.AreEquals(valueResult.NumberResult, item1.NumberResult))
                {
                    return child.Item2.Invoke(parameters);
                }
                else if (valueResult.IsNumber() && item1.IsRange() && RangeUtils.IsInRange(valueResult.NumberResult, item1.Data.ToString()))
                {
                    return child.Item2.Invoke(parameters);
                }
                else if (string.Equals(valueResult.ToString(), item1.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return child.Item2.Invoke(parameters);
                }
                else if (i == children.Count - 1 && item1.StringResult == string.Empty)
                {
                    // 如果匹配到最后一个，并且匹配项是空字符串则作为默认匹配项返回
                    return child.Item2.Invoke(parameters);
                }
            }

            throw new ExpressionCalculateException(this.ToString(), "No match was found");
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.If, "SWITCH()", "精确开关匹配")
            {
                Demo = "SWITCH({OPT:A}, 1:'str', 2:'2')  SWITCH({OPT:A}, 'A':1, 'B':2)",
                Input = "输入参数至少2个，第一个参数为待匹配参数，后面的参数为匹配值和结果值(用冒号分隔)。",
                Output = "任意值。输出匹配参数中与待匹配参数相同匹配值的结果值。",
            };
        }
    }

}

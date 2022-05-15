using System.Collections.Generic;
using System.Text;

namespace HLC.Expression
{
    /// <summary>
    /// 常量表达式
    /// </summary>
    public class ConstantExpression : Expression
    {
        public ConstantExpression(ExpressionType type, object value) : base(type)
        {
            Value = value;
        }

        public object Value { get; set; }

        public override IList<string> GetVariableKeys()
        {
            return new List<string>();
        }

        public override ResultExpression Invoke(Parameters parameters)
        {
            if (Type == ExpressionType.BooleanConstant)
            {
                return Result((bool)Value);
            }

            if (Type == ExpressionType.NumberConstant)
            {
                return Result((decimal) Value);
            }

            if (Type == ExpressionType.RangeConstant)
            {
                return new ResultExpression(ResultType.Range, Value);
            }

            return Result(Value.ToString());
        }

        public override string ToString()
        {
            if (Type == ExpressionType.StringConstant)
            {
                var sb = new StringBuilder();
                if (ExpressionSetting.Instance.HasStringStartChar)
                {
                    sb.Append(ExpressionSetting.Instance.StringStartChar);
                }

                sb.Append(Value);

                if (ExpressionSetting.Instance.HasStringEndChar)
                {
                    sb.Append(ExpressionSetting.Instance.StringEndChar);
                }

                return sb.ToString();
            }
            return $"{Value}";
        }
    }
}

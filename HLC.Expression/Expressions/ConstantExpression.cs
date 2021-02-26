using System.Collections.Generic;

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
                return Result((double) Value);
            }

            return Result(Value.ToString());
        }

        public override string ToString()
        {
            if (Type == ExpressionType.StringConstant)
            {
                return $"{ExpressionSetting.Instance.StringStartChar}{Value}{ExpressionSetting.Instance.StringEndChar}";
            }
            return $"{Value}";
        }
    }
}
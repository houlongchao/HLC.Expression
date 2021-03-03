using System.Collections.Generic;
using System.Text;

namespace HLC.Expression
{
    /// <summary>
    /// 变量表达式
    /// </summary>
    public class VariableExpression : Expression
    {
        public VariableExpression(string key) : base(ExpressionType.Variable)
        {
            Key = key;
        }

        public string Key { get; set; }

        public override ResultExpression Invoke(Parameters parameters)
        {
            if (parameters == null || !parameters.ContainsKey(Key))
            {
                throw new ExpressionException($"Not found parameter: {Key}.");
            }
            var parameter = parameters[Key];

            if (parameter.Type == ParameterType.Number)
            {
                return new ResultExpression(ResultType.Number, parameter.Data);
            }
            if (parameter.Type == ParameterType.String)
            {
                return new ResultExpression(ResultType.String, parameter.Data);
            }
            if (parameter.Type == ParameterType.NumberList)
            {
                return new ResultExpression(ResultType.NumberList, parameter.Data);
            }
            if (parameter.Type == ParameterType.StringList)
            {
                return new ResultExpression(ResultType.StringList, parameter.Data);
            }
            if (parameter.Type == ParameterType.Range)
            {
                return new ResultExpression(ResultType.Range, parameter.Data);
            }
            throw new ExpressionException($"Not parse Parameter type:{parameter.Type} in VariableExpression Invoke. ");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (ExpressionSetting.Instance.HasVariableStartChar)
            {
                sb.Append(ExpressionSetting.Instance.VariableStartChar);
            }

            sb.Append(Key);

            if (ExpressionSetting.Instance.HasVariableEndChar)
            {
                sb.Append(ExpressionSetting.Instance.VariableEndChar);
            }

            return sb.ToString();
        }


        public override IList<string> GetVariableKeys()
        {
            return new List<string>() { Key };
        }

    }
}
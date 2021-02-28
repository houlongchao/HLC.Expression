using System.Collections.Generic;

namespace HLC.Expression
{
    /// <summary>
    /// 参数项
    /// </summary>
    public class Parameter
    {
        public Parameter(string data)
        {
            Type = ParameterType.String;
            Data = data;
        }

        public Parameter(string data, ParameterType type)
        {
            Type = type;
            Data = data;
        }

        public Parameter(double data)
        {
            Type = ParameterType.Number;
            Data = data;
        }

        public Parameter(IList<string> data)
        {
            Type = ParameterType.StringList;
            Data = data;
        }

        public Parameter(IList<double> data)
        {
            Type = ParameterType.NumberList;
            Data = data;
        }

        public ParameterType Type { get; private set; }

        public object Data { get; private set; }

        public TData GetData<TData>()
        {
            return (TData) Data;
        }

        public static implicit operator Parameter(double data)
        {
            return new Parameter(data);
        }

        public static implicit operator Parameter(string data)
        {
            return new Parameter(data);
        }

        public static implicit operator Parameter(List<double> data)
        {
            return new Parameter(data);
        }

        public static implicit operator Parameter(List<string> data)
        {
            return new Parameter(data);
        }
    }
}
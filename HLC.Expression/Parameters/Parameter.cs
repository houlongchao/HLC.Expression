using System;
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

        public Parameter(object data, ParameterType type)
        {
            Type = type;
            Data = data;
        }

        public Parameter(decimal data)
        {
            Type = ParameterType.Number;
            Data = data;
        }

        public Parameter(IList<string> data)
        {
            Type = ParameterType.StringList;
            Data = data;
        }

        public Parameter(IList<decimal> data)
        {
            Type = ParameterType.NumberList;
            Data = data;
        }

        public Parameter(bool data)
        {
            Type = ParameterType.Boolean;
            Data = data;
        }

        public Parameter(DateTime data)
        {
            Type = ParameterType.DateTime;
            Data = data;
        }

        /// <summary>
        /// 参数类型 <see cref="ParameterType"/>
        /// </summary>
        public ParameterType Type { get; private set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// 元数据
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        public TData GetData<TData>()
        {
            return (TData)Data;
        }

        public static implicit operator Parameter(decimal data)
        {
            return new Parameter(data);
        }

        public static implicit operator Parameter(double data)
        {
            return new Parameter((decimal)data);
        }

        public static implicit operator Parameter(int data)
        {
            return new Parameter((decimal)data);
        }

        public static implicit operator Parameter(string data)
        {
            return new Parameter(data);
        }

        public static implicit operator Parameter(bool data)
        {
            return new Parameter(data);
        }

        public static implicit operator Parameter(List<decimal> data)
        {
            return new Parameter(data);
        }

        public static implicit operator Parameter(List<string> data)
        {
            return new Parameter(data);
        }
    }
}

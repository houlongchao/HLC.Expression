using System.Collections;
using System.Collections.Generic;

namespace HLC.Expression
{
    /// <summary>
    /// 返回值表达式
    /// </summary>
    public class ResultExpression : Expression
    {
        public ResultExpression(bool data) : base(ExpressionType.Data)
        {
            DataType = ResultType.Boolean;
            Data = data;
        }
        public ResultExpression(double data) : base(ExpressionType.Data)
        {
            DataType = ResultType.Number;
            Data = data;
        }
        public ResultExpression(string data) : base(ExpressionType.Data)
        {
            DataType = ResultType.String;
            Data = data;
        }
        public ResultExpression(IList<double> data) : base(ExpressionType.Data)
        {
            DataType = ResultType.NumberList;
            Data = data;
        }
        public ResultExpression(IList<string> data) : base(ExpressionType.Data)
        {
            DataType = ResultType.StringList;
            Data = data;
        }

        public ResultExpression(ResultType dataType, object data) : base(ExpressionType.Data)
        {
            DataType = dataType;
            Data = data;
        }

        /// <summary>
        /// 返回值类型
        /// </summary>
        public ResultType DataType { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 判断返回值是否是数字
        /// </summary>
        /// <returns></returns>
        public bool IsNumber()
        {
            return DataType == ResultType.Number;
        }

        /// <summary>
        /// 判断返回值是否是数字列表
        /// </summary>
        /// <returns></returns>
        public bool IsNumberList()
        {
            return DataType == ResultType.NumberList;
        }

        /// <summary>
        /// 以double类型获取返回值
        /// </summary>
        public double NumberResult => (double)Data;

        /// <summary>
        /// 以double列表获取返回值
        /// </summary>
        public IList<double> NumberListResult => (IList<double>)Data;

        /// <summary>
        /// 判断返回值是否是一个列表
        /// </summary>
        /// <returns></returns>
        public bool IsList()
        {
            return DataType == ResultType.StringList || DataType == ResultType.NumberList;
        }

        /// <summary>
        /// 以列表形式获得返回值
        /// </summary>
        public IList ListResult => (IList)Data;

        /// <summary>
        /// 以bool类型获取返回值
        /// </summary>
        public bool BooleanResult
        {
            get
            {
                if (Data is bool)
                {
                    return (bool)Data;
                }

                return bool.Parse(Data.ToString());
            }
        }

        public override ResultExpression Invoke(Parameters parameters)
        {
            return this;
        }

        public override string ToString()
        {
            return $"{Data}";
        }

        public override IList<string> GetVariableKeys()
        {
            return new List<string>();
        }

    }
}
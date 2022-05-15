using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        public ResultExpression(decimal data) : base(ExpressionType.Data)
        {
            DataType = ResultType.Number;
            Data = data;
        }
        public ResultExpression(string data) : base(ExpressionType.Data)
        {
            DataType = ResultType.String;
            Data = data;
        }
        public ResultExpression(IList<decimal> data) : base(ExpressionType.Data)
        {
            DataType = ResultType.NumberList;
            Data = data;
        }
        public ResultExpression(IList<string> data) : base(ExpressionType.Data)
        {
            DataType = ResultType.StringList;
            Data = data;
        }
        public ResultExpression(DateTime data) : base(ExpressionType.Data)
        {
            DataType = ResultType.DateTime;
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

        private object _data;

        /// <summary>
        /// 返回值
        /// </summary>
        public object Data
        {
            get
            {
                switch (DataType)
                {
                    case ResultType.Number:
                        var num = _data is decimal d ? d : decimal.Parse(_data.ToString() ?? string.Empty);
                        return ExpressionSetting.Instance.ConvertDecimal(num);
                    case ResultType.NumberList:
                        var convertedData = new List<decimal>();
                        foreach (decimal item in _data as IList<decimal>)
                        {
                            convertedData.Add(ExpressionSetting.Instance.ConvertDecimal(item));
                        }
                        return convertedData;
                }
                return _data;
            }
            set { _data = value; }
        }

        /// <summary>
        /// 判断返回值是否是数字
        /// </summary>
        /// <returns></returns>
        public bool IsNumber()
        {
            return DataType == ResultType.Number || (DataType == ResultType.String && IsNumber(Data.ToString()));
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
        /// 返回值是否为区间
        /// </summary>
        /// <returns></returns>
        public bool IsRange()
        {
            return DataType == ResultType.Range;
        }

        /// <summary>
        /// 以double类型获取返回值
        /// </summary>
        public decimal NumberResult
        {
            get
            {
                var result = Data is decimal d ? d : decimal.Parse(Data?.ToString() ?? "0");

                return ExpressionSetting.Instance.ConvertDecimal(result);
            }
        }
        /// <summary>
        /// 以decimal列表获取返回值
        /// </summary>
        public IList<decimal> NumberListResult => (IList<decimal>)Data;

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
                if (Data is bool data)
                {
                    return data;
                }

                return bool.Parse(Data.ToString() ?? string.Empty);
            }
        }

        /// <summary>
        /// 是否为日期时间类型
        /// </summary>
        /// <returns></returns>
        public bool IsDateTime()
        {
            return DataType == ResultType.DateTime;
        }

        /// <summary>
        /// 日期时间
        /// </summary>
        public DateTime DateTimeResult
        {
            get
            {
                if (Data is DateTime d)
                {
                    return d;
                }

                return DateTime.Parse(Data.ToString() ?? string.Empty);
            }
        }

        public string StringResult
        {
            get
            {
                return ToString();
            }
        }

        public override ResultExpression Invoke(Parameters parameters)
        {
            return this;
        }

        public override string ToString()
        {
            switch (DataType)
            {
                case ResultType.DateTime:
                    return ExpressionSetting.Instance.ToString(DateTimeResult);
                case ResultType.Boolean:
                    return ExpressionSetting.Instance.ToString(BooleanResult);
            }
            return ExpressionSetting.Instance.ToString(Data); 
        }

        public override IList<string> GetVariableKeys()
        {
            return new List<string>();
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        private bool IsNumber(string checkStr)
        {
            if (string.IsNullOrEmpty(checkStr))
            {
                return false;
            }
            return Regex.IsMatch(checkStr, @"^[+-]?[0123456789]*[.]?[0123456789]*$", RegexOptions.Compiled);
        }
    }
}

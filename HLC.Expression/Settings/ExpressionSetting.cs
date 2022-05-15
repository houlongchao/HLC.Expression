using System;

namespace HLC.Expression
{
    /// <summary>
    /// 表达式设置
    /// </summary>
    public class ExpressionSetting
    {
        private static ExpressionSetting _instance;

        public static ExpressionSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExpressionSetting();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 自定义一个表达式设置
        /// </summary>
        /// <param name="setting"></param>
        public static void SetSetting(ExpressionSetting setting)
        {
            _instance = setting;
        }


        /// <summary>
        ///  比较两个数字是否相等
        /// </summary>
        /// <param name="valueA"></param>
        /// <param name="valueB"></param>
        /// <returns></returns>
        public virtual bool AreEquals(double valueA, double valueB)
        {
            return Math.Abs(valueA - valueB) < 0.000001;
        }

        /// <summary>
        ///  比较两个数字是否相等
        /// </summary>
        /// <param name="valueA"></param>
        /// <param name="valueB"></param>
        /// <returns></returns>
        public virtual bool AreEquals(decimal valueA, decimal valueB)
        {
            return valueA == valueB;
        }

        #region String

        /// <summary>
        ///  检查指定字符是否为一个有效字符串字符，如果没有字符串结束限定符时生效
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool IsStringChar(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_' || c == '.' || c > 128;
        }

        /// <summary>
        /// 是否含有字符串开始限定符，默认为<c>true</c>
        /// </summary>
        public virtual bool HasStringStartChar => true;
        /// <summary>
        /// 是否含有字符串结束限定符，默认为<c>true</c>
        /// </summary>
        public virtual bool HasStringEndChar => true;

        /// <summary>
        /// 字符串开始限定符，默认为英文单引号<c>'</c>
        /// </summary>
        public virtual char StringStartChar => '\'';
        /// <summary>
        /// 字符串结束限定符，默认为英文单引号<c>'</c>
        /// </summary>
        public virtual char StringEndChar => '\'';

        #endregion

        #region Variable

        /// <summary>
        /// 检查指定字符是否为变量名的有效字符，如果没有变量结束限定符时生效
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool IsVariableNameChar(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_' || c == '.';
        }

        /// <summary>
        /// 是否含有变量开始限定符，默认为<c>true</c>
        /// </summary>
        public virtual bool HasVariableStartChar => true;
        /// <summary>
        /// 是否含有变量结束限定符，默认为<c>true</c>
        /// </summary>
        public virtual bool HasVariableEndChar => true;

        /// <summary>
        /// 变量开始限定符，默认为左花括号<c>{</c>
        /// </summary>
        public virtual char VariableStartChar => '{';
        /// <summary>
        /// 变量结束限定符，默认为右花括号<c>{</c>
        /// </summary>
        public virtual char VariableEndChar => '}';

        #endregion

        #region Convert

        /// <summary>
        /// 转换decimal计算返回值
        /// 应用场景：计算出IsNaN或者为无穷值等特殊值时进行转换处理为有效值
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual decimal ConvertDecimal(decimal result)
        {
            return result;
        }

        #endregion

        #region ToString()

        /// <summary>
        /// 日期转字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public virtual string ToString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Boolean 转字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ToString(bool boolean)
        {
            return boolean.ToString().ToLower();
        }

        /// <summary>
        /// 对象转字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ToString(object obj)
        {
            return obj?.ToString() ?? "";
        }

        #endregion

        #region 模式设置

        /// <summary>
        /// 除0设置，默认整体返回0
        /// </summary>
        public virtual DivideZero DivideZero { get; set; } = DivideZero.ReturnZero;

        /// <summary>
        /// 检查变量是否存在
        /// </summary>
        public virtual bool CheckVariableExist { get; set; } = false;

        #endregion
    }

    public enum DivideZero
    {
        /// <summary>
        /// 除法整体返回0
        /// </summary>
        ReturnZero = 0,

        /// <summary>
        /// 抛异常
        /// </summary>
        Throw = 1,
    }
}

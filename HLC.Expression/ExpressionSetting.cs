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

        public virtual char StringStartChar => '\'';
        public virtual char StringEndChar => '\'';


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

        #region String

        /// <summary>
        ///  检查指定字符是否为一个有效字符串字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool IsStringChar(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_' || c == '.' || c > 128;
        }

        /// <summary>
        /// 是否有字符串开始字符
        /// </summary>
        /// <returns></returns>
        public virtual bool HasStringStartChar()
        {
            return true;
        }

        /// <summary>
        /// 是否是字符串开始字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool IsStringStartChar(char c)
        {
            return c == StringStartChar;
        }

        /// <summary>
        /// 是否有字符串结束字符
        /// </summary>
        /// <returns></returns>
        public virtual bool HasStringEndChar()
        {
            return true;
        }

        /// <summary>
        /// 是否是字符串结束字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool IsStringEndChar(char c)
        {
            return c == StringEndChar;
        }

        #endregion

        #region Variable

        /// <summary>
        /// 检查指定字符是否为变量名的有效字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool IsVariableNameChar(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_' || c == '.';
        }

        /// <summary>
        /// 检查指定字符是否为变量开始字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool IsVariableStartChar(char c)
        {
            return c == '{';
        }

        /// <summary>
        /// 是否存在变量结束字符
        /// </summary>
        /// <returns></returns>
        public virtual bool HasVariableEndChar()
        {
            return true;
        }

        /// <summary>
        /// 检查指定字符是否为变量结束字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool IsVariableEndChar(char c)
        {
            return c == '}';
        }

        #endregion
    }
}
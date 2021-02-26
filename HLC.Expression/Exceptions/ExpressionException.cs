using System;

namespace HLC.Expression
{
    /// <summary>
    /// 表达式异常
    /// </summary>
    public class ExpressionException : Exception
    {
        public ExpressionException(string message) : base(message)
        {
        }
    }
}
namespace HLC.Expression
{
    /// <summary>
    /// 表达式计算出错异常
    /// </summary>
    /// 
    public class ExpressionCalculateException : ExpressionException
    {
        /// <summary>
        /// 计算异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public ExpressionCalculateException(string message) : base(message, null)
        {
        }

        /// <summary>
        /// 计算异常
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="formula">被计算的表达式</param>
        public ExpressionCalculateException(string message, string formula) : base(message, formula)
        {
        }
    }
}

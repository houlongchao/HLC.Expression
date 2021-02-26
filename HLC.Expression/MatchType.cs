namespace HLC.Expression
{
    /// <summary>
    /// 从字符串中匹配出的类型
    /// </summary>
    public enum MatchType
    {
        /// <summary>
        /// 什么都没有匹配到呢
        /// </summary>
        None,

        /// <summary>
        /// 匹配了一个值
        /// </summary>
        Value,

        /// <summary>
        /// 匹配了一个方法
        /// </summary>
        Function,

        /// <summary>
        /// 匹配了一个左括号
        /// </summary>
        LeftBracket,

        /// <summary>
        /// 匹配了一个右括号
        /// </summary>
        RightBracket,

        /// <summary>
        /// 匹配了一个二元运算符
        /// </summary>
        BinaryOperator,

        /// <summary>
        /// 匹配了一个分隔符 ,
        /// </summary>
        Separator,
    }
}
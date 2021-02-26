namespace HLC.Expression
{
    /// <summary>
    /// 操作符
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// 加 +
        /// </summary>
        Add,

        /// <summary>
        /// 减 -
        /// </summary>
        Subtract,

        /// <summary>
        /// 乘 *
        /// </summary>
        Multiply,

        /// <summary>
        /// 除 /
        /// </summary>
        Divide,

        /// <summary>
        /// 向上取整 CEILING(
        /// </summary>
        Ceiling,

        /// <summary>
        /// 向下取整 FLOORING(
        /// </summary>
        Flooring,

        /// <summary>
        /// 四舍五入 ROUNDING(
        /// </summary>
        Rounding,

        /// <summary>
        /// 最大值 MAX(
        /// </summary>
        Max,

        /// <summary>
        /// 最小值 MIN(
        /// </summary>
        Min,

        /// <summary>
        /// 大于 >
        /// </summary>
        Greater,

        /// <summary>
        /// 大于等于 >=
        /// </summary>
        GreaterEqual,

        /// <summary>
        /// 小于 &lt;
        /// </summary>
        Less,

        /// <summary>
        /// 小于等于 &lt;=
        /// </summary>
        LessEqual,

        /// <summary>
        /// 等于 ==
        /// </summary>
        Equal,

        /// <summary>
        /// 不等于 !=
        /// </summary>
        NotEqual,

        /// <summary>
        /// 指数 ^
        /// </summary>
        Power,

        /// <summary>
        /// 取余 %
        /// </summary>
        Modulo,

        /// <summary>
        /// 与 &&
        /// </summary>
        BooleanAnd,

        /// <summary>
        /// 或 ||
        /// </summary>
        BooleanOr,

        /// <summary>
        /// 非 NOT(
        /// </summary>
        Not,

        /// <summary>
        /// 与运算 AND(
        /// </summary>
        FunctionAnd,

        /// <summary>
        /// 或运算 OR(
        /// </summary>
        FunctionOr,

        /// <summary>
        /// 条件判断 IF(
        /// </summary>
        If,

        /// <summary>
        /// 给的值是否在指定列表中 IN(
        /// </summary>
        In,

        /// <summary>
        /// 分隔符 ,
        /// </summary>
        Separator,

        /// <summary>
        /// 左括号
        /// </summary>
        LeftBracket,

        /// <summary>
        /// 右括号
        /// </summary>
        RightBracket,
    }
}
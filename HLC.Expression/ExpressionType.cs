namespace HLC.Expression
{
    /// <summary>
    /// 表达式类型
    /// </summary>
    public enum ExpressionType
    {
        /// <summary>
        /// 数据
        /// </summary>
        Data,

        /// <summary>
        /// 变量
        /// </summary>
        Variable,

        /// <summary>
        /// 字符串常量
        /// </summary>
        StringConstant,

        /// <summary>
        /// 区间范围
        /// </summary>
        RangeConstant,

        /// <summary>
        /// 数值常量
        /// </summary>
        NumberConstant,

        /// <summary>
        /// Boolean常量
        /// </summary>
        BooleanConstant,

        /// <summary>
        /// 加法
        /// </summary>
        Add,

        /// <summary>
        /// 减法
        /// </summary>
        Subtract,

        /// <summary>
        /// 乘法
        /// </summary>
        Multiply,

        /// <summary>
        /// 除法
        /// </summary>
        Divide,

        /// <summary>
        /// 向上取整
        /// </summary>
        Ceiling,

        /// <summary>
        /// 向下取整
        /// </summary>
        Flooring,

        /// <summary>
        /// 四舍五入
        /// </summary>
        Rounding,

        /// <summary>
        /// 最大值
        /// </summary>
        Max,

        /// <summary>
        /// 最小值
        /// </summary>
        Min,

        /// <summary>
        /// 是否存在于指定列表
        /// </summary>
        In,

        /// <summary>
        /// 大于
        /// </summary>
        Greater,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqual,

        /// <summary>
        /// 小于
        /// </summary>
        Less,

        /// <summary>
        /// 小于等于
        /// </summary>
        LessEqual,

        /// <summary>
        /// 等于
        /// </summary>
        Equal,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual,

        /// <summary>
        /// 指数
        /// </summary>
        Power,

        /// <summary>
        /// 取模
        /// </summary>
        Modulo,

        /// <summary>
        /// 与运算
        /// </summary>
        BooleanAnd,

        /// <summary>
        /// 或运算
        /// </summary>
        BooleanOr,

        /// <summary>
        /// 方法与
        /// </summary>
        FunctionAnd,

        /// <summary>
        /// 方法或
        /// </summary>
        FunctionOr,

        /// <summary>
        /// 非
        /// </summary>
        Not,

        /// <summary>
        /// 条件计算
        /// </summary>
        If
    }
}
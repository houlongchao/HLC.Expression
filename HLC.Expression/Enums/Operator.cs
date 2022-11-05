namespace HLC.Expression
{
    /// <summary>
    /// 操作符
    /// </summary>
    public enum Operator
    {
        #region 加减乘除

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

        #endregion

        #region 取整

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

        #endregion

        #region 最大最小

        /// <summary>
        /// 最大值 MAX(
        /// </summary>
        Max,

        /// <summary>
        /// 最小值 MIN(
        /// </summary>
        Min,

        #endregion

        #region 大小比较

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

        #endregion

        #region 指数取余

        /// <summary>
        /// 指数 ^
        /// </summary>
        Power,

        /// <summary>
        /// 取余 %
        /// </summary>
        Modulo,

        #endregion

        #region 逻辑运算

        /// <summary>
        /// 与 &amp;&amp;
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

        #endregion

        #region 逻辑语句

        /// <summary>
        /// 条件判断 IF(
        /// </summary>
        If,

        /// <summary>
        /// 给的值是否在指定列表中 IN(
        /// </summary>
        In,

        /// <summary>
        /// 匹配方法 SWITCH(
        /// </summary>
        Switch,

        /// <summary>
        /// 匹配方法 SWITCHC(
        /// </summary>
        SwitchC,

        #endregion

        #region 字符串处理

        /// <summary>
        /// 合并多个字符串 CONCAT(
        /// </summary>
        Concat,

        /// <summary>
        /// 字串获取 SUBSTR(
        /// </summary>
        SubStr,

        /// <summary>
        /// 字串转数字 SUBNUM(
        /// </summary>
        SubNum,

        /// <summary>
        /// 从左侧截取字符串 LEFT(
        /// </summary>
        Left,

        /// <summary>
        /// 从右侧截取字符串 RIGHT(
        /// </summary>
        Right,

        /// <summary>
        /// 获取字符串长度 LENGTH(
        /// </summary>
        Length,

        /// <summary>
        /// 获取指定字符下标 FIND(
        /// </summary>
        FIND,

        /// <summary>
        /// 字符串取反 REVERSE(
        /// </summary>
        Reverse,

        #endregion

        #region 数学函数

        /// <summary>
        /// 绝对值函数 ABS(
        /// </summary>
        ABS,

        /// <summary>
        /// 正弦函数 SIN(
        /// </summary>
        SIN,

        /// <summary>
        /// 反正弦函数 ASIN(
        /// </summary>
        ASIN,

        /// <summary>
        /// 余弦函数 COS(
        /// </summary>
        COS,

        /// <summary>
        /// 反余弦函数 ACOS(
        /// </summary>
        ACOS,

        /// <summary>
        /// 正切函数 TAN(
        /// </summary>
        TAN,

        /// <summary>
        /// 反正切函数 ATAN(
        /// </summary>
        ATAN,

        /// <summary>
        /// 反余切函数 ATAN2(
        /// </summary>
        ATAN2,

        /// <summary>
        /// 双曲正弦函数 SINH(
        /// </summary>
        SINH,

        /// <summary>
        /// 双曲余弦函数 COSH(
        /// </summary>
        COSH,

        /// <summary>
        /// 双曲正切函数 TANH(
        /// </summary>
        TANH,

        /// <summary>
        /// 角度转弧度 RAD(
        /// </summary>
        RAD,

        /// <summary>
        /// 弧度转角度 DEG(
        /// </summary>
        DEG,

        /// <summary>
        /// 自然对数 LOG(
        /// </summary>
        LOG,

        /// <summary>
        /// 常用对数 LOG(
        /// </summary>
        LOG10,

        /// <summary>
        /// e的指数次幂 EXP(
        /// </summary>
        EXP,

        /// <summary>
        /// 阶乘 FACT(
        /// </summary>
        FACT,

        /// <summary>
        /// 平方根 SQRT(
        /// </summary>
        SQRT,

        /// <summary>
        /// 取余数 MOD(
        /// </summary>
        MOD,

        /// <summary>
        /// 指数 POW(
        /// </summary>
        POW,

        /// <summary>
        /// 圆周率 PI(
        /// </summary>
        PI,

        #endregion

        #region 值处理

        /// <summary>
        /// 日期时间 DATETIME(
        /// </summary>
        DateTime,

        /// <summary>
        /// 获取值 GET(
        /// </summary>
        Get,

        /// <summary>
        /// 作为数字  ASNUM(
        /// </summary>
        AsNum,

        /// <summary>
        /// 转为数字  TONUM(
        /// </summary>
        ToNum,

        /// <summary>
        /// 转为字符串 TOSTR(
        /// </summary>
        ToStr,

        /// <summary>
        /// 获取属性 META(
        /// </summary>
        META,

        #endregion

        #region 数组

        /// <summary>
        /// 数组求和 ASUM(
        /// </summary>
        ASUM,

        /// <summary>
        /// 数据元素获取 AINDEX(
        /// </summary>
        AINDEX,

        /// <summary>
        /// 数组元素位置获取 AMATCH(
        /// </summary>
        AMATCH,

        /// <summary>
        /// 数组数量获取 ACOUNT(
        /// </summary>
        ACOUNT,

        #endregion

        #region 值判断

        /// <summary>
        /// 含有值 HASVALUE(
        /// </summary>
        HasValue,

        /// <summary>
        /// 是否为数字 ISNUMBER(
        /// </summary>
        IsNumber,

        /// <summary>
        /// 是否以指定字符串开始 ISSTART(
        /// </summary>
        IsStart,

        /// <summary>
        /// 是否以指定字符串结束 ISEND(
        /// </summary>
        IsEnd,

        /// <summary>
        /// 是否可以用指定正则匹配 ISMATCH(
        /// </summary>
        IsMatch,

        #endregion

        #region 辅助符号

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

        #endregion
    }
}

namespace HLC.Expression
{
    /// <summary>
    /// 表达式类型
    /// </summary>
    public enum ExpressionType
    {
        #region 值标识

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

        #endregion

        #region 加减乘除

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

        #endregion

        #region 取整

        /// <summary>
        /// 向上取整
        /// </summary>
        CEILING,

        /// <summary>
        /// 向下取整
        /// </summary>
        FLOORING,

        /// <summary>
        /// 四舍五入
        /// </summary>
        ROUNDING,

        #endregion

        #region 最大最小

        /// <summary>
        /// 最大值
        /// </summary>
        MAX,

        /// <summary>
        /// 最小值
        /// </summary>
        MIN,

        #endregion

        #region 大小比较

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

        #endregion

        #region 指数取余

        /// <summary>
        /// 指数
        /// </summary>
        Power,

        /// <summary>
        /// 取模
        /// </summary>
        Modulo,

        #endregion

        #region 逻辑运算

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
        AND,

        /// <summary>
        /// 方法或
        /// </summary>
        OR,

        /// <summary>
        /// 非
        /// </summary>
        NOT,

        #endregion

        #region 逻辑语句

        /// <summary>
        /// 条件计算
        /// </summary>
        IF,

        /// <summary>
        /// 是否存在于指定列表
        /// </summary>
        IN,

        /// <summary>
        /// 数据匹配 Switch
        /// </summary>
        SWITCH,

        /// <summary>
        /// 数据包含匹配 SwitchC
        /// </summary>
        SWITCHC,

        #endregion

        #region 字符串处理

        /// <summary>
        /// 合并字符串
        /// </summary>
        CONCAT,

        /// <summary>
        /// 子串获取
        /// </summary>
        SUBSTR,

        /// <summary>
        /// 子串转数字
        /// </summary>
        SUBNUM,

        /// <summary>
        /// 从左侧截取字符串
        /// </summary>
        LEFT,

        /// <summary>
        /// 从右侧截取字符串
        /// </summary>
        RIGHT,

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        Length,

        /// <summary>
        /// 获取指定字符下标
        /// </summary>
        FIND,

        /// <summary>
        /// 字符串取反 REVERSE(
        /// </summary>
        REVERSE,

        #endregion

        #region 参数值处理

        /// <summary>
        /// 日期时间
        /// </summary>
        DATETIME,

        /// <summary>
        /// 获取数据，如果没有则返回默认值
        /// </summary>
        GET,

        /// <summary>
        /// 作为数字
        /// </summary>
        ASNUM,

        /// <summary>
        /// 转为数字
        /// </summary>
        TONUM,

        /// <summary>
        /// 转为字符串
        /// </summary>
        TOSTR,

        /// <summary>
        /// 获取属性
        /// </summary>
        META,

        /// <summary>
        /// 获取数据属性
        /// </summary>
        DATAMETA,

        /// <summary>
        /// 当前系统时间
        /// </summary>
        NOW,

        #endregion

        #region 数学函数

        /// <summary>
        /// 绝对值函数
        /// </summary>
        ABS,

        /// <summary>
        /// 正弦函数
        /// </summary>
        SIN,

        /// <summary>
        /// 反正弦函数
        /// </summary>
        ASIN,

        /// <summary>
        /// 余弦函数
        /// </summary>
        COS,

        /// <summary>
        /// 反余弦函数
        /// </summary>
        ACOS,

        /// <summary>
        /// 正切函数
        /// </summary>
        TAN,

        /// <summary>
        /// 反正切函数 
        /// </summary>
        ATAN,

        /// <summary>
        /// 反余切函数
        /// </summary>
        ATAN2,

        /// <summary>
        /// 双曲正弦函数
        /// </summary>
        SINH,

        /// <summary>
        /// 双曲余弦函数
        /// </summary>
        COSH,

        /// <summary>
        /// 双曲正切函数
        /// </summary>
        TANH,

        /// <summary>
        /// 角度转弧度
        /// </summary>
        RAD,

        /// <summary>
        /// 弧度转角度 
        /// </summary>
        DEG,

        /// <summary>
        /// 自然对数
        /// </summary>
        LOG,

        /// <summary>
        /// 常用对数
        /// </summary>
        LOG10,

        /// <summary>
        /// 指数
        /// </summary>
        EXP,

        /// <summary>
        /// 阶乘
        /// </summary>
        FACT,

        /// <summary>
        /// 平方根 
        /// </summary>
        SQRT,

        /// <summary>
        /// 取余数函数
        /// </summary>
        MOD,

        /// <summary>
        /// 指数函数
        /// </summary>
        POW,

        /// <summary>
        /// 圆周率
        /// </summary>
        PI,

        #endregion

        #region 值判断

        /// <summary>
        /// 含有数据
        /// </summary>
        HASVALUE,

        /// <summary>
        /// 是否是数字
        /// </summary>
        ISNUMBER,

        /// <summary>
        /// 是否以指定字符串开始 ISSTART(
        /// </summary>
        ISSTART,

        /// <summary>
        /// 是否以指定字符串结束 ISEND(
        /// </summary>
        ISEND,

        /// <summary>
        /// 是否可以用指定正则匹配 ISMATCH(
        /// </summary>
        ISMATCH,

        #endregion

        #region 数组

        /// <summary>
        /// 数组求和
        /// </summary>
        ASUM,

        /// <summary>
        /// 数据元素获取
        /// </summary>
        AINDEX,

        /// <summary>
        /// 数组元素位置获取
        /// </summary>
        AMATCH,

        /// <summary>
        /// 数组数量获取
        /// </summary>
        ACOUNT,

        /// <summary>
        /// 数组最大值 AMAX(
        /// </summary>
        AMAX,

        /// <summary>
        /// 数组最小值 AMIN(
        /// </summary>
        AMIN,

        /// <summary>
        /// 数组最大相邻差 AMAXDIFF(
        /// </summary>
        AMAXDIFF,

        /// <summary>
        /// 数组最小相邻差 AMINDIFF(
        /// </summary>
        AMINDIFF,

        #endregion

        #region 函数调用

        /// <summary>
        /// 函数调用
        /// </summary>
        CALL,

        #endregion
    }
}

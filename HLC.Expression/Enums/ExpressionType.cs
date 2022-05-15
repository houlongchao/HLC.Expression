﻿namespace HLC.Expression
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
        Ceiling,

        /// <summary>
        /// 向下取整
        /// </summary>
        Flooring,

        /// <summary>
        /// 四舍五入
        /// </summary>
        Rounding,

        #endregion

        #region 最大最小

        /// <summary>
        /// 最大值
        /// </summary>
        Max,

        /// <summary>
        /// 最小值
        /// </summary>
        Min,

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
        FunctionAnd,

        /// <summary>
        /// 方法或
        /// </summary>
        FunctionOr,

        /// <summary>
        /// 非
        /// </summary>
        Not,

        #endregion

        #region 逻辑语句

        /// <summary>
        /// 条件计算
        /// </summary>
        If,

        /// <summary>
        /// 是否存在于指定列表
        /// </summary>
        In,

        /// <summary>
        /// 数据匹配 Switch
        /// </summary>
        Switch,

        /// <summary>
        /// 数据包含匹配 SwitchC
        /// </summary>
        SwitchC,

        #endregion

        #region 字符串处理

        /// <summary>
        /// 合并字符串
        /// </summary>
        Concat,

        /// <summary>
        /// 子串获取
        /// </summary>
        SubStr,

        /// <summary>
        /// 子串转数字
        /// </summary>
        SubNum,

        #endregion

        #region 参数值处理

        /// <summary>
        /// 日期时间
        /// </summary>
        DateTime,

        /// <summary>
        /// 含有数据
        /// </summary>
        HasValue,

        /// <summary>
        /// 是否是数字
        /// </summary>
        IsNumber,

        /// <summary>
        /// 获取数据，如果没有则返回默认值
        /// </summary>
        Get,

        /// <summary>
        /// 作为数字
        /// </summary>
        AsNum,

        /// <summary>
        /// 转为字符串
        /// </summary>
        ToStr,

        /// <summary>
        /// 获取属性
        /// </summary>
        META,

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
    }
}
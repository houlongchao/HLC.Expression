using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    /// <summary>
    /// 函数片段
    /// </summary>
    public abstract class FunctionSegment
    {
        /// <summary>
        /// 匹配的字符串前缀
        /// </summary>
        public abstract string MatchString { get; }

        /// <summary>
        /// 匹配的操作符定义
        /// </summary>
        public abstract Operator MatchOperator { get;}

        /// <summary>
        /// 表达式类型定义
        /// </summary>
        public abstract ExpressionType ExpressionType { get; }

        /// <summary>
        /// 字符串匹配长度
        /// </summary>
        public int MatchLength => MatchString.Length;

        /// <summary>
        /// 是否是成对参数
        /// </summary>
        public virtual bool IsPairArgs { get; }

        /// <summary>
        /// 是否是无参数函数
        /// </summary>
        public virtual bool IsFreeArg { get; }

        /// <summary>
        /// 构建成对参数函数表达式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="arrayList"></param>
        /// <returns></returns>
        public virtual Expression BuildPairFunctionExpression(Expression value, IList<Tuple<Expression, Expression>> arrayList)
        {
            return Expression.MakeFunctionPair(ExpressionType, value, arrayList);
        }

        /// <summary>
        /// 构建函数表达式
        /// </summary>
        /// <param name="arrayList"></param>
        /// <returns></returns>
        public virtual Expression BuildFunctionExpression(IList<Expression> arrayList)
        {
            return Expression.MakeFunction(ExpressionType, arrayList);
        }

        /// <summary>
        /// 构建函数表达式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual Expression BuildFunctionExpression(Expression data)
        {
            return Expression.MakeFunction(ExpressionType, data);
        }

        /// <summary>
        /// 执行表达式计算
        /// </summary>
        /// <param name="children"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            return null;
        }

        /// <summary>
        /// 执行表达式计算
        /// </summary>
        /// <param name="value"></param>
        /// <param name="children"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual ResultExpression InvokePair(Expression value, IList<Tuple<Expression, Expression>> children, Parameters parameters)
        {
            return null;
        }

        public abstract ExpressionFunctionDefinitionItem GetDefinistion();
    }
}

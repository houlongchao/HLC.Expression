using HLC.Expression.Definitions;

namespace HLC.Expression.Segments
{
    public abstract class BinarySegment
    {
        /// <summary>
        /// 匹配的字符串
        /// </summary>
        public abstract string MatchString { get; }

        /// <summary>
        /// 匹配的操作符定义
        /// </summary>
        public abstract Operator MatchOperator { get; }

        /// <summary>
        /// 表达式类型定义
        /// </summary>
        public abstract ExpressionType ExpressionType { get; }

        /// <summary>
        /// 字符串匹配长度
        /// </summary>
        public int MatchLength => MatchString.Length;

        public virtual BinaryExpression BuildBinaryExpression(Expression left, Expression right)
        {
            return Expression.MakeBinary(left, ExpressionType, right);
        }

        /// <summary>
        /// 执行表达式计算
        /// </summary>
        /// <returns></returns>
        public abstract ResultExpression Invoke(Expression left, Expression right, Parameters parameters);

        public abstract ExpressionSymbolDefinitionItem GetDefinistion();
    }
}

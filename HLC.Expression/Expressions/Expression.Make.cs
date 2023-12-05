using System;
using System.Collections.Generic;

namespace HLC.Expression
{
    /// <summary>
    /// 表达式树
    /// </summary>
    public abstract partial class Expression
    {
        #region 私有拼接表达式方法

        public static BinaryExpression MakeBinary(Expression left, ExpressionType type, Expression right)
        {
            return new BinaryExpression(left, type, right);
        }

        public static FunctionPairExpression MakeFunctionPair(ExpressionType type, Expression value, IList<Tuple<Expression, Expression>> children)
        {
            return new FunctionPairExpression(type, value, children);
        }

        public static FunctionExpression MakeFunction(ExpressionType type, Expression child)
        {
            var children = new List<Expression> { child };
            return MakeFunction(type, children);
        }

        public static FunctionExpression MakeFunction(ExpressionType type, IList<Expression> children)
        {
            return new FunctionExpression(type, children);
        }

        public static FunctionExpression MakeFunction(ExpressionType type)
        {
            return new FunctionExpression(type, new List<Expression>());
        }

        #endregion


        #region 静态 公共表达式树构建

        public static ConstantExpression NumConstant(decimal value)
        {
            return new ConstantExpression(ExpressionType.NumberConstant, value);
        }

        public static ConstantExpression StringConstant(string value)
        {
            return new ConstantExpression(ExpressionType.StringConstant, value);
        }

        public static ConstantExpression RangeConstant(string value)
        {
            return new ConstantExpression(ExpressionType.RangeConstant, value);
        }

        public static ConstantExpression BoolConstant(bool value)
        {
            return new ConstantExpression(ExpressionType.BooleanConstant, value);
        }

        public static VariableExpression Variable(string name)
        {
            return new VariableExpression(name);
        }

        public static ResultExpression ResultEmpty()
        {
            return new ResultExpression(ResultType.Empty, null);
        }

        public static ResultExpression Result(bool data)
        {
            return new ResultExpression(data);
        }

        public static ResultExpression Result(decimal data)
        {
            return new ResultExpression(data);
        }

        public static ResultExpression Result(string data)
        {
            return new ResultExpression(data);
        }

        public static ResultExpression Result(IList<decimal> data)
        {
            return new ResultExpression(data);
        }

        public static ResultExpression Result(IList<string> data)
        {
            return new ResultExpression(data);
        }

        public static ResultExpression Result(DateTime data)
        {
            return new ResultExpression(data);
        }

        public static ResultExpression Result(ResultType dataType, object data)
        {
            return new ResultExpression(dataType, data);
        }

        public static BinaryExpression Add(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.Add, right);
        }

        public static BinaryExpression Multiply(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.Multiply, right);
        }

        public static BinaryExpression Subtract(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.Subtract, right);
        }

        public static BinaryExpression Divide(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.Divide, right);
        }

        public static BinaryExpression Greater(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.Greater, right);
        }

        public static BinaryExpression GreaterEqual(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.GreaterEqual, right);
        }

        public static BinaryExpression Less(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.Less, right);
        }

        public static BinaryExpression LessEqual(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.LessEqual, right);
        }

        public static BinaryExpression Equal(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.Equal, right);
        }

        public static BinaryExpression NotEqual(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.NotEqual, right);
        }

        public static BinaryExpression Power(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.Power, right);
        }

        public static BinaryExpression Modulo(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.Modulo, right);
        }

        public static BinaryExpression BooleanAdd(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.BooleanAnd, right);
        }

        public static BinaryExpression BooleanOr(Expression left, Expression right)
        {
            return MakeBinary(left, ExpressionType.BooleanOr, right);
        }

        #endregion

    }
}

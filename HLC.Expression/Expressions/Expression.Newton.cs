using System;

namespace HLC.Expression
{
    /// <summary>
    /// 牛顿法解析
    /// </summary>
	public static class ExpressionNewton
	{
        /// <summary>
        /// 牛顿法求解一元函数
        /// </summary>
        /// <param name="expression">要求解的表达式</param>
        /// <param name="resultVariable">要求的变量</param>
        /// <param name="parameters">常量参数</param>
        /// <returns></returns>
        public static decimal NewtonSolve(this Expression expression, string resultVariable, Parameters parameters)
        {
            Parameter orgParameter = null;
            if (parameters.ContainsKey(resultVariable))
            {
                orgParameter = parameters[resultVariable];
            }
            var variables = expression.GetVariableKeys();
            bool hasDependentVariable = false;
            foreach (var variable in variables)
            {
                if (variable == resultVariable)
                {
                    hasDependentVariable = true;
                    continue;
                }
                if (!parameters.ContainsKey(variable))
                {
                    throw new ExpressionCalculateException($"Not found parameter {variable}");
                }
            }
            if (!hasDependentVariable)
            {
                throw new ExpressionCalculateException("The dependent variable not in the formula.");
            }

            decimal x1 = 0;
            decimal x2 = 1;
            int maxLoop = 100;
            while (Math.Abs(x2 - x1) > (decimal)0.00001)
            {
                x1 = x2;
                parameters[resultVariable] = x1;
                x2 = x1 - expression.Invoke(parameters).NumberResult / Derivative(expression, resultVariable, parameters).Invoke(parameters).NumberResult;
                if (--maxLoop < 0)
                {
                    throw new ExpressionCalculateException("Calculation for too long.");
                }
            }

            if (orgParameter == null)
            {
                parameters.Remove(resultVariable);
            }
            else
            {
                parameters[resultVariable] = orgParameter;
            }

            return x2;
        }

        /// <summary>
        /// 求表达式<paramref name="expression"/>的导函数
        /// </summary>
        /// <param name="expression">求导的表达式</param>
        /// <param name="resultVariable">表达式中的求解变量</param>
        /// <param name="parameters">其它参数</param>
        /// <returns></returns>
        private static Expression Derivative(Expression expression, string resultVariable, Parameters parameters)
        {
            if (expression.Type == ExpressionType.NumberConstant)
            {
                return Expression.NumConstant(0);
            }
            if (expression.Type == ExpressionType.Variable && expression is VariableExpression v)
            {
                if (v.Key == resultVariable)
                {
                    return Expression.NumConstant(1);
                }
                if (parameters.ContainsKey(v.Key))
                {
                    return Expression.NumConstant(0);
                }
                throw new ExpressionCalculateException($"Not found parameter {v.Key}");
            }
            if (expression.Type == ExpressionType.Add)
            {
                var b = expression as BinaryExpression;
                var left = Derivative(b.Left, resultVariable, parameters);
                var right = Derivative(b.Right, resultVariable, parameters);
                return new BinaryExpression(left, ExpressionType.Add, right);
            }
            if (expression.Type == ExpressionType.Subtract)
            {
                var b = expression as BinaryExpression;
                var left = Derivative(b.Left, resultVariable, parameters);
                var right = Derivative(b.Right, resultVariable, parameters);
                return new BinaryExpression(left, ExpressionType.Subtract, right);
            }
            if (expression.Type == ExpressionType.Multiply)
            {
                var b = expression as BinaryExpression;
                var left = new BinaryExpression(Derivative(b.Left, resultVariable, parameters), ExpressionType.Multiply, b.Right);
                var right = new BinaryExpression(b.Left, ExpressionType.Multiply, Derivative(b.Right, resultVariable, parameters));
                return new BinaryExpression(left, ExpressionType.Add, right);
            }
            if (expression.Type == ExpressionType.Divide)
            {
                var b = expression as BinaryExpression;
                var leftLeft = new BinaryExpression(Derivative(b.Left, resultVariable, parameters), ExpressionType.Multiply, b.Right);
                var leftRight = new BinaryExpression(b.Left, ExpressionType.Multiply, Derivative(b.Right, resultVariable, parameters));
                var left = new BinaryExpression(leftLeft, ExpressionType.Subtract, leftRight);
                var right = new BinaryExpression(b.Right, ExpressionType.Multiply, b.Right);
                return new BinaryExpression(left, ExpressionType.Divide, right);
            }
            throw new ExpressionCalculateException($"Not support derivative type {expression.Type}");
        }
    }
}

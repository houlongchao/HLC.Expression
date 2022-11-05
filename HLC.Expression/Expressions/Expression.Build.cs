using System;
using System.Collections.Generic;
using System.Linq;

namespace HLC.Expression
{
    /// <summary>
    /// 表达式树
    /// </summary>
    public abstract partial class Expression
    {
        #region 静态 表达式数构建

        /// <summary>
        /// 从字符串构建表达式树
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        private static Expression FromFormula(string formula)
        {
            Stack<Expression> expressionStack = new Stack<Expression>();
            Stack<Operator> operatorStack = new Stack<Operator>();

            int index = 0;
            MatchType lastMatchType = MatchType.None;

            int line = 0;
            int lineStartIndex = 0;

            try
            {
                while (index < formula.Length)
                {
                    // 移除空格
                    if (formula[index] == ' ' || formula[index] == '\r' || formula[index] == '\t')
                    {
                        ++index;
                        continue;
                    }
                    // 移除换行
                    if (formula[index] == '\n')
                    {
                        ++line;
                        lineStartIndex = index;
                        ++index;
                        continue;
                    }

                    // 移除注释
                    {
                        var newIndex = TryCommentTrim(formula, index);
                        if (newIndex > index)
                        {
                            index = newIndex;
                            continue;
                        }
                    }

                    // 匹配方法
                    if (lastMatchType == MatchType.None ||
                        lastMatchType == MatchType.BinaryOperator ||
                        lastMatchType == MatchType.LeftBracket ||
                        lastMatchType == MatchType.Function ||
                        lastMatchType == MatchType.Separator)
                    {

                        int newIndex = TryFunctionBuild(formula, index, operatorStack);
                        if (newIndex > index)
                        {

                            lastMatchType = MatchType.Function;
                            index = newIndex;
                            continue;
                        }
                    }

                    // 匹配区间，只有在二元运算等于号(==)右边才做匹配
                    if (lastMatchType == MatchType.BinaryOperator)
                    {
                        int newIndex = TryRangeBuild(formula, index, expressionStack);
                        if (newIndex > index)
                        {
                            lastMatchType = MatchType.Value;
                            index = newIndex;
                            continue;
                        }
                    }

                    // 匹配值
                    if (lastMatchType == MatchType.None ||
                        lastMatchType == MatchType.BinaryOperator ||
                        lastMatchType == MatchType.LeftBracket ||
                        lastMatchType == MatchType.Function ||
                        lastMatchType == MatchType.Separator)
                    {
                        {
                            int newIndex = TryNumBuild(formula, index, expressionStack);
                            if (newIndex > index)
                            {
                                lastMatchType = MatchType.Value;
                                index = newIndex;
                                continue;
                            }
                        }
                        {
                            int newIndex = TryBooleanBuild(formula, index, expressionStack);
                            if (newIndex > index)
                            {
                                lastMatchType = MatchType.Value;
                                index = newIndex;
                                continue;
                            }
                        }
                        {
                            int newIndex = TryStringBuild(formula, index, expressionStack);
                            if (newIndex > index)
                            {
                                lastMatchType = MatchType.Value;
                                index = newIndex;
                                continue;
                            }
                        }
                        {
                            int newIndex = TryVariableBuild(formula, index, expressionStack);
                            if (newIndex > index)
                            {
                                lastMatchType = MatchType.Value;
                                index = newIndex;
                                continue;
                            }
                        }
                        {
                            int newIndex = TryConstantBuild(formula, index, expressionStack);
                            if (newIndex > index)
                            {
                                lastMatchType = MatchType.Value;
                                index = newIndex;
                                continue;
                            }
                        }
                    }

                    // 匹配左括号
                    if (lastMatchType == MatchType.None ||
                        lastMatchType == MatchType.BinaryOperator ||
                        lastMatchType == MatchType.LeftBracket ||
                        lastMatchType == MatchType.Function ||
                        lastMatchType == MatchType.Separator)
                    {
                        if (formula[index] == '(')
                        {
                            operatorStack.Push(Operator.LeftBracket);
                            lastMatchType = MatchType.LeftBracket;
                            ++index;
                            continue;
                        }
                    }

                    // 匹配右括号
                    if (lastMatchType == MatchType.Value || lastMatchType == MatchType.RightBracket)
                    {
                        if (formula[index] == ')')
                        {
                            lastMatchType = MatchType.RightBracket;
                            if (operatorStack.Count <= 0)
                            {
                                throw new ExpressionException("extra bracket", formula);
                            }
                            Operator op = operatorStack.Pop();

                            // 如果栈顶操作符为左括号则不做处理
                            if (op == Operator.LeftBracket)
                            {
                                ++index;
                                continue;
                            }

                            // 尝试方法运算构建
                            if (TryFunctionBuild(op, expressionStack, operatorStack))
                            {
                                ++index;
                                continue;
                            }

                            // 尝试二元运算构建
                            if (TryBinaryBuild(op, expressionStack))
                            {
                                if (operatorStack.Count <= 0)
                                {
                                    throw new ExpressionException("extra bracket", formula);
                                }
                                // 上面弹出的是运算符，运算结束后再弹出一个，如果是左括号丢弃，如果是方法运算符则运算
                                Operator pop = operatorStack.Pop();
                                if (pop == Operator.LeftBracket || TryFunctionBuild(pop, expressionStack, operatorStack))
                                {
                                    // 不做处理
                                }
                                else
                                {
                                    // 如果是二元操作则继续判断，直到可以关闭右括号
                                    while (TryBinaryBuild(pop, expressionStack))
                                    {
                                        if (operatorStack.Count <= 0)
                                        {
                                            throw new ExpressionException("extra bracket", formula);
                                        }
                                        pop = operatorStack.Pop();
                                        if (pop == Operator.LeftBracket || TryFunctionBuild(pop, expressionStack, operatorStack))
                                        {
                                            continue;
                                        }
                                    }
                                }

                                ++index;
                                continue;
                            }
                        }
                    }

                    // 匹配分隔符
                    if (lastMatchType == MatchType.Value || lastMatchType == MatchType.RightBracket)
                    {
                        if (formula[index] == ',')
                        {
                            Operator topOperator = operatorStack.Peek();
                            // 前面的运算符优先级大于当前分隔符，先计算前面的
                            while (Operator.Separator.GetPriority() < topOperator.GetPriority())
                            {
                                var result = TryBinaryBuild(topOperator, expressionStack);
                                if (!result)
                                {
                                    throw new ExpressionException(line, index - lineStartIndex, "Missing Operator.", formula);
                                }

                                operatorStack.Pop();
                                if (operatorStack.Count <= 0)
                                {
                                    break;
                                }

                                topOperator = operatorStack.Peek();
                            }

                            operatorStack.Push(Operator.Separator);
                            lastMatchType = MatchType.Separator;
                            index += 1;
                            continue;
                        }

                        if (formula[index] == ':')
                        {
                            Operator topOperator = operatorStack.Peek();
                            // 前面的运算符优先级大于当前分隔符，先计算前面的
                            while (Operator.Separator.GetPriority() < topOperator.GetPriority())
                            {
                                var result = TryBinaryBuild(topOperator, expressionStack);
                                if (!result)
                                {
                                    throw new ExpressionException(line, index - lineStartIndex, "Missing Operator.", formula);
                                }

                                operatorStack.Pop();
                                if (operatorStack.Count <= 0)
                                {
                                    break;
                                }

                                topOperator = operatorStack.Peek();
                            }
                            //operatorStack.Push(Operator.Colon);
                            lastMatchType = MatchType.Separator;
                            index += 1;
                            continue;
                        }
                    }

                    // 匹配二元运算符
                    if (lastMatchType == MatchType.Value || lastMatchType == MatchType.RightBracket)
                    {
                        if (formula[index] == '+')
                        {
                            AnalysisBuild(Operator.Add, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 1;
                            continue;
                        }

                        if (formula[index] == '-')
                        {
                            AnalysisBuild(Operator.Subtract, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 1;
                            continue;
                        }

                        if (formula[index] == '*')
                        {
                            AnalysisBuild(Operator.Multiply, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 1;
                            continue;
                        }

                        if (formula[index] == '/')
                        {
                            AnalysisBuild(Operator.Divide, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 1;
                            continue;
                        }

                        if (index + 2 < formula.Length && formula[index] == '>' && formula[index + 1] == '=')
                        {
                            AnalysisBuild(Operator.GreaterEqual, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 2;
                            continue;
                        }

                        if (index + 2 < formula.Length && formula[index] == '<' && formula[index + 1] == '=')
                        {
                            AnalysisBuild(Operator.LessEqual, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 2;
                            continue;
                        }

                        if (index + 2 < formula.Length && formula[index] == '=' && formula[index + 1] == '=')
                        {
                            AnalysisBuild(Operator.Equal, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 2;
                            continue;
                        }

                        if (index + 2 < formula.Length && formula[index] == '!' && formula[index + 1] == '=')
                        {
                            AnalysisBuild(Operator.NotEqual, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 2;
                            continue;
                        }

                        if (formula[index] == '>')
                        {
                            AnalysisBuild(Operator.Greater, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 1;
                            continue;
                        }

                        if (formula[index] == '<')
                        {
                            AnalysisBuild(Operator.Less, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 1;
                            continue;
                        }

                        if (formula[index] == '%')
                        {
                            AnalysisBuild(Operator.Modulo, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 1;
                            continue;
                        }

                        if (formula[index] == '^')
                        {
                            AnalysisBuild(Operator.Power, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 1;
                            continue;
                        }

                        if (index + 2 < formula.Length && formula[index] == '&' && formula[index + 1] == '&')
                        {
                            AnalysisBuild(Operator.BooleanAnd, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 2;
                            continue;
                        }

                        if (index + 2 < formula.Length && formula[index] == '|' && formula[index + 1] == '|')
                        {
                            AnalysisBuild(Operator.BooleanOr, expressionStack, operatorStack);
                            lastMatchType = MatchType.BinaryOperator;
                            index += 2;
                            continue;
                        }
                    }

                    int count = formula.Length - index;
                    count = count > 20 ? 20 : count;
                    throw new ExpressionException($"Error near '{formula.Substring(index, count)}'", formula);
                }
            }
            catch (ExpressionException e)
            {
                e.Line = line;
                e.Column = index - lineStartIndex;
                e.Formula = formula;
                throw e;
            }

            try
            {
                while (operatorStack.Count > 0)
                {
                    Operator op = operatorStack.Pop();
                    var success = TryBinaryBuild(op, expressionStack);
                    if (!success)
                    {
                        throw new ExpressionException(line, index - lineStartIndex, $"Error formula: {formula}", formula);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                throw new ExpressionException(line, index - lineStartIndex, $"Error formula: {formula}", formula);
            }

            if (expressionStack.Count != 1)
            {
                throw new ExpressionException(line, index - lineStartIndex, $"Error formula: {formula}", formula);
            }
            return expressionStack.Pop();
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

        public static FunctionExpression Not(Expression child)
        {
            return MakeFunction(ExpressionType.Not, child);
        }

        public static FunctionExpression Ceiling(Expression child)
        {
            return MakeFunction(ExpressionType.Ceiling, child);
        }

        public static FunctionExpression Flooring(Expression child)
        {
            return MakeFunction(ExpressionType.Flooring, child);
        }

        public static FunctionExpression Rounding(Expression child, Expression digits = null)
        {
            var list = new List<Expression>()
            {
                child,
                digits??NumConstant(0)
            };
            return MakeFunction(ExpressionType.Rounding, list);
        }

        public static FunctionExpression Max(IList<Expression> children)
        {
            return MakeFunction(ExpressionType.Max, children);
        }

        public static FunctionExpression Min(IList<Expression> children)
        {
            return MakeFunction(ExpressionType.Min, children);
        }

        public static FunctionExpression Concat(IList<Expression> children)
        {
            return MakeFunction(ExpressionType.Concat, children);
        }

        public static FunctionExpression In(Expression value, IList<Expression> list)
        {
            list.Insert(0, value);
            return MakeFunction(ExpressionType.In, list);
        }

        public static SwitchExpression Switch(Expression value, IList<Tuple<Expression, Expression>> list)
        {
            return new SwitchExpression(ExpressionType.Switch, value, list);
        }

        public static SwitchExpression SwitchC(Expression value, IList<Tuple<Expression, Expression>> list)
        {
            return new SwitchExpression(ExpressionType.SwitchC, value, list);
        }

        public static FunctionExpression SwitchC(Expression value)
        {
            return new FunctionExpression(ExpressionType.SubStr, new List<Expression>() { value });
        }

        public static FunctionExpression If(Expression condition, Expression ifTrue, Expression ifFalse)
        {
            var children = new List<Expression>
            {
                condition,
                ifTrue,
                ifFalse
            };
            return MakeFunction(ExpressionType.If, children);
        }

        public static FunctionExpression FunctionAnd(IList<Expression> children)
        {
            return MakeFunction(ExpressionType.FunctionAnd, children);
        }

        public static FunctionExpression FunctionOr(IList<Expression> children)
        {
            return MakeFunction(ExpressionType.FunctionOr, children);
        }

        #endregion

        #region 私有解析构建表达式方法

        /// <summary>
        /// 分析构建二元运算符，里面会比较二元运算符优先级
        /// </summary>
        /// <param name="currentOperator"></param>
        /// <param name="expressionStack"></param>
        /// <param name="operatorStack"></param>
        /// <exception cref="ExpressionException"></exception>
        private static void AnalysisBuild(Operator currentOperator, Stack<Expression> expressionStack, Stack<Operator> operatorStack)
        {
            if (operatorStack.Count <= 0 || currentOperator.GetPriority() > operatorStack.Peek().GetPriority())
            {
                operatorStack.Push(currentOperator);
            }
            else
            {
                var topOperator = operatorStack.Peek();
                while (currentOperator.GetPriority() <= topOperator.GetPriority())
                {
                    var result = TryBinaryBuild(topOperator, expressionStack);
                    if (!result)
                    {
                        throw new ExpressionException("Missing Operator.");
                    }

                    operatorStack.Pop();
                    if (operatorStack.Count <= 0)
                    {
                        break;
                    }

                    topOperator = operatorStack.Peek();
                }

                operatorStack.Push(currentOperator);
            }
        }

        /// <summary>
        /// 尝试解析构建二元运算树
        /// </summary>
        /// <param name="op"></param>
        /// <param name="expressionStack"></param>
        /// <returns></returns>
        private static bool TryBinaryBuild(Operator op, Stack<Expression> expressionStack)
        {

            if (op == Operator.Add)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(Add(left, right));
                return true;
            }

            if (op == Operator.Subtract)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(Subtract(left, right));
                return true;
            }

            if (op == Operator.Multiply)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(Multiply(left, right));
                return true;
            }

            if (op == Operator.Divide)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(Divide(left, right));
                return true;
            }

            if (op == Operator.Greater)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(Greater(left, right));
                return true;
            }

            if (op == Operator.GreaterEqual)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(GreaterEqual(left, right));
                return true;
            }

            if (op == Operator.Less)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(Less(left, right));
                return true;
            }

            if (op == Operator.LessEqual)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(LessEqual(left, right));
                return true;
            }

            if (op == Operator.Equal)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(Equal(left, right));
                return true;
            }

            if (op == Operator.NotEqual)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(NotEqual(left, right));
                return true;
            }

            if (op == Operator.Power)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(Power(left, right));
                return true;
            }

            if (op == Operator.Modulo)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(Modulo(left, right));
                return true;
            }

            if (op == Operator.BooleanAnd)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(BooleanAdd(left, right));
                return true;
            }

            if (op == Operator.BooleanOr)
            {
                Expression right = expressionStack.Pop();
                Expression left = expressionStack.Pop();
                expressionStack.Push(BooleanOr(left, right));
                return true;
            }

            return false;
        }

        /// <summary>
        /// 尝试解析函数运算符
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="index"></param>
        /// <param name="operatorStack"></param>
        /// <returns></returns>
        private static int TryFunctionBuild(string formula, int index, Stack<Operator> operatorStack)
        {
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("NOT("))
            {
                operatorStack.Push(Operator.Not);
                return index + 4;
            }
            if (index + 9 < formula.Length && formula.Substring(index, 9).Equals("HASVALUE("))
            {
                operatorStack.Push(Operator.HasValue);
                return index + 9;
            }
            if (index + 9 < formula.Length && formula.Substring(index, 9).Equals("ISNUMBER("))
            {
                operatorStack.Push(Operator.IsNumber);
                return index + 9;
            }
            if (index + 8 < formula.Length && formula.Substring(index, 8).Equals("CEILING("))
            {
                operatorStack.Push(Operator.Ceiling);
                return index + 8;
            }
            if (index + 9 < formula.Length && formula.Substring(index, 9).Equals("FLOORING("))
            {
                operatorStack.Push(Operator.Flooring);
                return index + 9;
            }
            if (index + 9 < formula.Length && formula.Substring(index, 9).Equals("ROUNDING("))
            {
                operatorStack.Push(Operator.Rounding);
                return index + 9;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("MAX("))
            {
                operatorStack.Push(Operator.Max);
                return index + 4;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("MIN("))
            {
                operatorStack.Push(Operator.Min);
                return index + 4;
            }
            if (index + 3 < formula.Length && formula.Substring(index, 3).Equals("IF("))
            {
                operatorStack.Push(Operator.If);
                return index + 3;
            }
            if (index + 3 < formula.Length && formula.Substring(index, 3).Equals("IN("))
            {
                operatorStack.Push(Operator.In);
                return index + 3;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("AND("))
            {
                operatorStack.Push(Operator.FunctionAnd);
                return index + 4;
            }
            if (index + 3 < formula.Length && formula.Substring(index, 3).Equals("OR("))
            {
                operatorStack.Push(Operator.FunctionOr);
                return index + 3;
            }
            if (index + 7 < formula.Length && formula.Substring(index, 7).Equals("CONCAT("))
            {
                operatorStack.Push(Operator.Concat);
                return index + 7;
            }
            if (index + 7 < formula.Length && formula.Substring(index, 7).Equals("SWITCH("))
            {
                operatorStack.Push(Operator.Switch);
                return index + 7;
            }
            if (index + 8 < formula.Length && formula.Substring(index, 8).Equals("SWITCHC("))
            {
                operatorStack.Push(Operator.SwitchC);
                return index + 8;
            }
            if (index + 7 < formula.Length && formula.Substring(index, 7).Equals("SUBSTR("))
            {
                operatorStack.Push(Operator.SubStr);
                return index + 7;
            }
            if (index + 7 < formula.Length && formula.Substring(index, 7).Equals("SUBNUM("))
            {
                operatorStack.Push(Operator.SubNum);
                return index + 7;
            }
            if (index + 9 < formula.Length && formula.Substring(index, 9).Equals("DATETIME("))
            {
                operatorStack.Push(Operator.DateTime);
                return index + 9;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("GET("))
            {
                operatorStack.Push(Operator.Get);
                return index + 4;
            }
            if (index + 6 < formula.Length && formula.Substring(index, 6).Equals("ASNUM("))
            {
                operatorStack.Push(Operator.AsNum);
                return index + 6;
            }
            if (index + 6 < formula.Length && formula.Substring(index, 6).Equals("TOSTR("))
            {
                operatorStack.Push(Operator.ToStr);
                return index + 6;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("ABS("))
            {
                operatorStack.Push(Operator.ABS);
                return index + 4;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("SIN("))
            {
                operatorStack.Push(Operator.SIN);
                return index + 4;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("ASIN("))
            {
                operatorStack.Push(Operator.ASIN);
                return index + 5;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("COS("))
            {
                operatorStack.Push(Operator.COS);
                return index + 4;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("ACOS("))
            {
                operatorStack.Push(Operator.ACOS);
                return index + 5;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("TAN("))
            {
                operatorStack.Push(Operator.TAN);
                return index + 4;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("ATAN("))
            {
                operatorStack.Push(Operator.ATAN);
                return index + 5;
            }
            if (index + 6 < formula.Length && formula.Substring(index, 6).Equals("ATAN2("))
            {
                operatorStack.Push(Operator.ATAN2);
                return index + 6;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("SINH("))
            {
                operatorStack.Push(Operator.SINH);
                return index + 5;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("COSH("))
            {
                operatorStack.Push(Operator.COSH);
                return index + 5;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("TANH("))
            {
                operatorStack.Push(Operator.TANH);
                return index + 5;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("RAD("))
            {
                operatorStack.Push(Operator.RAD);
                return index + 4;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("DEG("))
            {
                operatorStack.Push(Operator.DEG);
                return index + 4;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("LOG("))
            {
                operatorStack.Push(Operator.LOG);
                return index + 4;
            }
            if (index + 6 < formula.Length && formula.Substring(index, 6).Equals("LOG10("))
            {
                operatorStack.Push(Operator.LOG10);
                return index + 6;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("EXP("))
            {
                operatorStack.Push(Operator.EXP);
                return index + 4;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("FACT("))
            {
                operatorStack.Push(Operator.FACT);
                return index + 5;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("SQRT("))
            {
                operatorStack.Push(Operator.SQRT);
                return index + 5;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("MOD("))
            {
                operatorStack.Push(Operator.MOD);
                return index + 4;
            }
            if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("POW("))
            {
                operatorStack.Push(Operator.POW);
                return index + 4;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("META("))
            {
                operatorStack.Push(Operator.META);
                return index + 5;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("LEFT("))
            {
                operatorStack.Push(Operator.Left);
                return index + 5;
            }
            if (index + 6 < formula.Length && formula.Substring(index, 6).Equals("RIGHT("))
            {
                operatorStack.Push(Operator.Right);
                return index + 6;
            }
            if (index + 8 < formula.Length && formula.Substring(index, 8).Equals("REVERSE("))
            {
                operatorStack.Push(Operator.Reverse);
                return index + 8;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("FIND("))
            {
                operatorStack.Push(Operator.FIND);
                return index + 5;
            }
            if (index + 7 < formula.Length && formula.Substring(index, 7).Equals("LENGTH("))
            {
                operatorStack.Push(Operator.Length);
                return index + 7;
            }
            if (index + 6 < formula.Length && formula.Substring(index, 6).Equals("TONUM("))
            {
                operatorStack.Push(Operator.ToNum);
                return index + 6;
            }
            if (index + 5 < formula.Length && formula.Substring(index, 5).Equals("ASUM("))
            {
                operatorStack.Push(Operator.ASUM);
                return index + 5;
            }
            if (index + 7 < formula.Length && formula.Substring(index, 7).Equals("AINDEX("))
            {
                operatorStack.Push(Operator.AINDEX);
                return index + 7;
            }
            if (index + 7 < formula.Length && formula.Substring(index, 7).Equals("AMATCH("))
            {
                operatorStack.Push(Operator.AMATCH);
                return index + 7;
            }
            if (index + 7 < formula.Length && formula.Substring(index, 7).Equals("ACOUNT("))
            {
                operatorStack.Push(Operator.ACOUNT);
                return index + 7;
            }
            if (index + 8 < formula.Length && formula.Substring(index, 8).Equals("ISSTART("))
            {
                operatorStack.Push(Operator.IsStart);
                return index + 8;
            }
            if (index + 6 < formula.Length && formula.Substring(index, 6).Equals("ISEND("))
            {
                operatorStack.Push(Operator.IsEnd);
                return index + 6;
            }
            if (index + 8 < formula.Length && formula.Substring(index, 8).Equals("ISMATCH("))
            {
                operatorStack.Push(Operator.IsMatch);
                return index + 8;
            }
            return index;
        }

        /// <summary>
        /// 尝试构建函数树
        /// </summary>
        /// <param name="op"></param>
        /// <param name="expressionStack"></param>
        /// <param name="operatorStack"></param>
        /// <returns></returns>
        private static bool TryFunctionBuild(Operator op, Stack<Expression> expressionStack, Stack<Operator> operatorStack)
        {
            switch (op)
            {
                case Operator.Ceiling:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(Ceiling(data));
                        return true;
                    }
                case Operator.Flooring:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(Flooring(data));
                        return true;
                    }
                case Operator.Rounding:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(Rounding(data));
                        return true;
                    }
                case Operator.Not:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(Not(data));
                        return true;
                    }
                case Operator.Concat:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.Concat, data));
                        return true;
                    }
                case Operator.ABS:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ABS, data));
                        return true;
                    }
                case Operator.SIN:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.SIN, data));
                        return true;
                    }
                case Operator.ASIN:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ASIN, data));
                        return true;
                    }
                case Operator.COS:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.COS, data));
                        return true;
                    }
                case Operator.ACOS:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ACOS, data));
                        return true;
                    }
                case Operator.TAN:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.TAN, data));
                        return true;
                    }
                case Operator.ATAN:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ATAN, data));
                        return true;
                    }
                case Operator.ATAN2:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ATAN2, data));
                        return true;
                    }
                case Operator.SINH:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.SINH, data));
                        return true;
                    }
                case Operator.COSH:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.COSH, data));
                        return true;
                    }
                case Operator.TANH:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.TANH, data));
                        return true;
                    }
                case Operator.RAD:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.RAD, data));
                        return true;
                    }
                case Operator.DEG:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.DEG, data));
                        return true;
                    }
                case Operator.LOG:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.LOG, data));
                        return true;
                    }
                case Operator.LOG10:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.LOG10, data));
                        return true;
                    }
                case Operator.EXP:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.EXP, data));
                        return true;
                    }
                case Operator.FACT:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.FACT, data));
                        return true;
                    }
                case Operator.SQRT:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.SQRT, data));
                        return true;
                    }
                case Operator.PI:
                    {
                        expressionStack.Push(MakeFunction(ExpressionType.PI));
                        return true;
                    }
                case Operator.DateTime:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.DateTime, data));
                        return true;
                    }
                case Operator.HasValue:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.HasValue, data));
                        return true;
                    }
                case Operator.IsNumber:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.IsNumber, data));
                        return true;
                    }
                case Operator.AsNum:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.AsNum, data));
                        return true;
                    }
                case Operator.ToStr:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ToStr, data));
                        return true;
                    }
                case Operator.Length:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.Length, data));
                        return true;
                    }
                case Operator.Reverse:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.Reverse, data));
                        return true;
                    }
                case Operator.ToNum:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ToNum, data));
                        return true;
                    }
                case Operator.ASUM:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ASUM, data));
                        return true;
                    }
                case Operator.ACOUNT:
                    {
                        Expression data = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ACOUNT, data));
                        return true;
                    }
                case Operator.Separator:
                    {
                        // 尝试处理多元函数
                        return TryFunctionSeparatorBuild(expressionStack, operatorStack);
                    }

                case Operator.Add:
                case Operator.Subtract:
                case Operator.Multiply:
                case Operator.Divide:
                case Operator.Greater:
                case Operator.GreaterEqual:
                case Operator.Less:
                case Operator.LessEqual:
                case Operator.Equal:
                case Operator.NotEqual:
                case Operator.Power:
                case Operator.Modulo:
                case Operator.BooleanAnd:
                case Operator.BooleanOr:
                    {
                        // 不是函数
                        return false;
                    }
                case Operator.Max:
                case Operator.Min:
                case Operator.FunctionAnd:
                case Operator.FunctionOr:
                case Operator.If:
                case Operator.In:
                case Operator.Switch:
                case Operator.SwitchC:
                case Operator.SubStr:
                case Operator.SubNum:
                case Operator.MOD:
                case Operator.POW:
                case Operator.Get:
                case Operator.Left:
                case Operator.Right:
                case Operator.FIND:
                case Operator.META:
                case Operator.AINDEX:
                case Operator.AMATCH:
                case Operator.IsStart:
                case Operator.IsEnd:
                case Operator.IsMatch:
                    {
                        // 多元函数需要单独处理
                        return false;
                    }
                case Operator.LeftBracket:
                case Operator.RightBracket:
                default:
                    {
                        // 其他
                        break;
                    }
            }

            return false;

        }

        /// <summary>
        /// 尝试构建多元函数树
        /// </summary>
        /// <param name="expressionStack"></param>
        /// <param name="operatorStack"></param>
        /// <returns></returns>
        private static bool TryFunctionSeparatorBuild(Stack<Expression> expressionStack, Stack<Operator> operatorStack)
        {
            int separatorCount = 1;
            Operator topOp = operatorStack.Pop();
            while (topOp == Operator.Separator)
            {
                separatorCount = separatorCount + 1;
                topOp = operatorStack.Pop();
            }
            switch (topOp)
            {
                case Operator.Rounding:
                    {
                        var digits = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(Rounding(value, digits));
                        return true;
                    }
                case Operator.Max:
                    {
                        IList<Expression> arrayList = new List<Expression>();
                        while (separatorCount-- >= 0)
                        {
                            arrayList.Add(expressionStack.Pop());
                        }
                        arrayList = arrayList.Reverse().ToList();
                        expressionStack.Push(Max(arrayList));
                        return true;
                    }
                case Operator.Min:
                    {
                        IList<Expression> arrayList = new List<Expression>();
                        while (separatorCount-- >= 0)
                        {
                            arrayList.Add(expressionStack.Pop());
                        }
                        arrayList = arrayList.Reverse().ToList();
                        expressionStack.Push(Min(arrayList));
                        return true;
                    }
                case Operator.FunctionAnd:
                    {
                        IList<Expression> arrayList = new List<Expression>();
                        while (separatorCount-- >= 0)
                        {
                            arrayList.Add(expressionStack.Pop());
                        }
                        arrayList = arrayList.Reverse().ToList();
                        expressionStack.Push(MakeFunction(ExpressionType.FunctionAnd, arrayList));
                        return true;
                    }
                case Operator.FunctionOr:
                    {
                        IList<Expression> arrayList = new List<Expression>();
                        while (separatorCount-- >= 0)
                        {
                            arrayList.Add(expressionStack.Pop());
                        }
                        arrayList = arrayList.Reverse().ToList();
                        expressionStack.Push(MakeFunction(ExpressionType.FunctionOr, arrayList));
                        return true;
                    }
                case Operator.If:
                    {
                        Expression ifFalse = expressionStack.Pop();
                        Expression ifTrue = expressionStack.Pop();
                        Expression condition = expressionStack.Pop();
                        expressionStack.Push(If(condition, ifTrue, ifFalse));
                        return true;
                    }
                case Operator.In:
                    {
                        IList<Expression> arrayList = new List<Expression>();
                        while (separatorCount-- > 0)
                        {
                            arrayList.Add(expressionStack.Pop());
                        }
                        arrayList = arrayList.Reverse().ToList();
                        Expression value = expressionStack.Pop();
                        expressionStack.Push(In(value, arrayList));
                        return true;
                    }
                case Operator.Switch:
                    {
                        IList<Tuple<Expression, Expression>> arrayList = new List<Tuple<Expression, Expression>>();
                        while (separatorCount-- > 0)
                        {
                            var i2 = expressionStack.Pop();
                            var i1 = expressionStack.Pop();
                            arrayList.Add(new Tuple<Expression, Expression>(i1, i2));
                        }
                        arrayList = arrayList.Reverse().ToList();
                        Expression value = expressionStack.Pop();
                        expressionStack.Push(Switch(value, arrayList));

                        return true;
                    }
                case Operator.SwitchC:
                    {
                        IList<Tuple<Expression, Expression>> arrayList = new List<Tuple<Expression, Expression>>();
                        while (separatorCount-- > 0)
                        {
                            var i2 = expressionStack.Pop();
                            var i1 = expressionStack.Pop();
                            arrayList.Add(new Tuple<Expression, Expression>(i1, i2));
                        }
                        arrayList = arrayList.Reverse().ToList();
                        Expression value = expressionStack.Pop();
                        expressionStack.Push(SwitchC(value, arrayList));

                        return true;
                    }
                case Operator.Concat:
                    {
                        IList<Expression> arrayList = new List<Expression>();
                        while (separatorCount-- >= 0)
                        {
                            arrayList.Add(expressionStack.Pop());
                        }
                        arrayList = arrayList.Reverse().ToList();
                        expressionStack.Push(Concat(arrayList));
                        return true;
                    }
                case Operator.SubStr:
                    {
                        if (separatorCount > 1)
                        {
                            Expression length = expressionStack.Pop();
                            Expression startIndex = expressionStack.Pop();
                            Expression data = expressionStack.Pop();
                            expressionStack.Push(MakeFunction(ExpressionType.SubStr, new List<Expression>() { data, startIndex, length }));
                            return true;
                        }
                        else
                        {
                            Expression startIndex = expressionStack.Pop();
                            Expression data = expressionStack.Pop();
                            expressionStack.Push(MakeFunction(ExpressionType.SubStr, new List<Expression>() { data, startIndex }));
                            return true;
                        }
                    }
                case Operator.SubNum:
                    {
                        if (separatorCount > 1)
                        {
                            Expression length = expressionStack.Pop();
                            Expression startIndex = expressionStack.Pop();
                            Expression data = expressionStack.Pop();
                            expressionStack.Push(MakeFunction(ExpressionType.SubNum, new List<Expression>() { data, startIndex, length }));
                            return true;
                        }
                        else
                        {
                            Expression startIndex = expressionStack.Pop();
                            Expression data = expressionStack.Pop();
                            expressionStack.Push(MakeFunction(ExpressionType.SubNum, new List<Expression>() { data, startIndex }));
                            return true;
                        }
                    }
                case Operator.ATAN2:
                    {
                        var y = expressionStack.Pop();
                        var x = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ATAN2, new List<Expression>() { y, x }));
                        return true;
                    }
                case Operator.LOG:
                    {
                        var newBase = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.LOG, new List<Expression>() { value, newBase }));
                        return true;
                    }
                case Operator.MOD:
                    {
                        var newBase = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.MOD, new List<Expression>() { value, newBase }));
                        return true;
                    }
                case Operator.POW:
                    {
                        var newBase = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.POW, new List<Expression>() { value, newBase }));
                        return true;
                    }
                case Operator.DateTime:
                    {
                        var format = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.DateTime, new List<Expression>() { value, format }));
                        return true;
                    }
                case Operator.Get:
                    {
                        var defaultExpression = expressionStack.Pop();
                        var variableExpression = expressionStack.Pop();
                        if (!variableExpression.IsVariableExpression())
                        {
                            return false;
                        }
                        expressionStack.Push(MakeFunction(ExpressionType.Get, new List<Expression>() { variableExpression, defaultExpression }));
                        return true;
                    }
                case Operator.ToStr:
                    {
                        var format = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.ToStr, new List<Expression>() { value, format }));
                        return true;
                    }
                case Operator.META:
                    {
                        if (separatorCount > 1)
                        {
                            Expression formate = expressionStack.Pop();
                            if (!formate.IsStringExpression(new List<string> { "bool", "txt", "num" }))
                            {
                                return false;
                            }
                            Expression metadata = expressionStack.Pop();
                            if (!metadata.IsStringExpression())
                            {
                                return false;
                            }
                            Expression data = expressionStack.Pop();
                            if (!data.IsVariableExpression())
                            {
                                return false;
                            }
                            expressionStack.Push(MakeFunction(ExpressionType.META, new List<Expression>() { data, metadata, formate }));
                            return true;
                        }
                        else
                        {
                            Expression metadata = expressionStack.Pop();
                            if (!metadata.IsStringExpression())
                            {
                                return false;
                            }
                            Expression data = expressionStack.Pop();
                            if (!data.IsVariableExpression())
                            {
                                return false;
                            }
                            expressionStack.Push(MakeFunction(ExpressionType.META, new List<Expression>() { data, metadata }));
                            return true;
                        }
                    }
                case Operator.Left:
                    {
                        var length = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.Left, new List<Expression>() { value, length }));
                        return true;
                    }
                case Operator.Right:
                    {
                        var length = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.Right, new List<Expression>() { value, length }));
                        return true;
                    }
                case Operator.FIND:
                    {
                        var matchValue = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.FIND, new List<Expression>() { value, matchValue }));
                        return true;
                    }
                case Operator.AINDEX:
                    {
                        var index = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.AINDEX, new List<Expression>() { value, index }));
                        return true;
                    }
                case Operator.AMATCH:
                    {
                        var matchValue = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.AMATCH, new List<Expression>() { value, matchValue }));
                        return true;
                    }
                case Operator.IsStart:
                    {
                        var matchValue = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.IsStart, new List<Expression>() { value, matchValue }));
                        return true;
                    }
                case Operator.IsEnd:
                    {
                        var matchValue = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.IsEnd, new List<Expression>() { value, matchValue }));
                        return true;
                    }
                case Operator.IsMatch:
                    {
                        var matchValue = expressionStack.Pop();
                        var value = expressionStack.Pop();
                        expressionStack.Push(MakeFunction(ExpressionType.IsMatch, new List<Expression>() { value, matchValue }));
                        return true;
                    }
                // 二元运算符
                case Operator.Greater:
                case Operator.GreaterEqual:
                case Operator.Less:
                case Operator.LessEqual:
                case Operator.Equal:
                case Operator.NotEqual:
                case Operator.Power:
                case Operator.Modulo:
                case Operator.BooleanAnd:
                case Operator.BooleanOr:
                    break;
                // 一元函数
                case Operator.Add:
                case Operator.Subtract:
                case Operator.Multiply:
                case Operator.Divide:
                case Operator.Ceiling:
                case Operator.Flooring:
                case Operator.Not:
                case Operator.ABS:
                case Operator.SIN:
                case Operator.ASIN:
                case Operator.COS:
                case Operator.ACOS:
                case Operator.TAN:
                case Operator.ATAN:
                case Operator.SINH:
                case Operator.COSH:
                case Operator.TANH:
                case Operator.RAD:
                case Operator.DEG:
                case Operator.LOG10:
                case Operator.EXP:
                case Operator.FACT:
                case Operator.SQRT:
                case Operator.PI:
                case Operator.HasValue:
                case Operator.IsNumber:
                case Operator.AsNum:
                case Operator.Length:
                case Operator.Reverse:
                case Operator.ToNum:
                case Operator.ASUM:
                case Operator.ACOUNT:
                    break;
                // 其它
                case Operator.Separator:
                case Operator.LeftBracket:
                case Operator.RightBracket:
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// 尝试解析出数字
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="index"></param>
        /// <param name="expressionStack"></param>
        /// <returns></returns>
        private static int TryNumBuild(string formula, int index, Stack<Expression> expressionStack)
        {
            int currentIndex = index;

            var isNegative = false;
            if (formula[currentIndex] == '-')
            {
                isNegative = true;
                currentIndex += 1;
            }

            int startIndex = currentIndex;

            while (currentIndex < formula.Length)
            {
                if (formula[currentIndex] >= '0' && formula[currentIndex] <= '9')
                {
                    ++currentIndex;
                }
                else if (formula[currentIndex] == '.')
                {
                    ++currentIndex;
                }
                else
                {
                    break;
                }
            }

            if (startIndex == currentIndex)
            {
                return index;
            }

            var value = isNegative
                ? -decimal.Parse(formula.Substring(startIndex, currentIndex - startIndex))
                : decimal.Parse(formula.Substring(startIndex, currentIndex - startIndex));

            expressionStack.Push(NumConstant(value));

            return currentIndex;
        }

        /// <summary>
        /// 尝试解析出Boolean
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="index"></param>
        /// <param name="expressionStack"></param>
        /// <returns></returns>
        private static int TryBooleanBuild(string formula, int index, Stack<Expression> expressionStack)
        {
            if (formula.Length >= index + 4 && formula.Substring(index, 4).Equals("true"))
            {
                expressionStack.Push(BoolConstant(true));
                return index + 4;
            }
            if (formula.Length >= index + 5 && formula.Substring(index, 5).Equals("false"))
            {
                expressionStack.Push(BoolConstant(false));
                return index + 5;
            }
            return index;
        }

        /// <summary>
        /// 尝试解析出常量，如PI()
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="index"></param>
        /// <param name="expressionStack"></param>
        /// <returns></returns>
        private static int TryConstantBuild(string formula, int index, Stack<Expression> expressionStack)
        {
            if (formula.Length >= index + 4 && formula.Substring(index, 4).Equals("PI()"))
            {
                expressionStack.Push(Expression.MakeFunction(ExpressionType.PI));
                return index + 4;
            }
            return index;
        }

        /// <summary>
        /// 尝试解析出字符串
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="index"></param>
        /// <param name="expressionStack"></param>
        /// <returns></returns>
        /// <exception cref="ExpressionException"></exception>
        private static int TryStringBuild(string formula, int index, Stack<Expression> expressionStack)
        {
            var currentIndex = index;
            if (ExpressionSetting.Instance.HasStringStartChar)
            {
                if (ExpressionSetting.Instance.StringStartChar.Equals(formula[currentIndex]))
                {
                    ++currentIndex;
                }
                else
                {
                    return index;
                }
            }

            int startIndex = currentIndex;

            while (currentIndex < formula.Length)
            {
                // 如果有结束字符，则可以接受所有字符，对包含的结束字符识别转义
                if (ExpressionSetting.Instance.HasStringEndChar)
                {
                    // 发现了结束字符
                    if (currentIndex > index && formula[currentIndex] == ExpressionSetting.Instance.StringEndChar)
                    {
                        //如果当前结束字符后面紧跟一个结束字符，则标识当前结束字符是被转义的，不作为结束字符
                        if (currentIndex + 1 < formula.Length && formula[currentIndex + 1] == ExpressionSetting.Instance.StringEndChar)
                        {
                            currentIndex += 2;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        ++currentIndex;
                    }
                }
                else if (ExpressionSetting.Instance.IsStringChar(formula[currentIndex])) // 如果没有结束字符，必须是严格要求的可识别字符
                {
                    ++currentIndex;
                }
                else // 否则就是错误的无法识别
                {
                    break;
                }
            }

            if (currentIndex == index)
            {
                return index;
            }

            var value = formula.Substring(startIndex, currentIndex - startIndex);
            if (ExpressionSetting.Instance.HasStringEndChar)
            {
                if (currentIndex >= formula.Length)
                {
                    throw new ExpressionException("lose string end char", formula);
                }

                if (ExpressionSetting.Instance.StringEndChar.Equals(formula[currentIndex]))
                {
                    ++currentIndex;
                    expressionStack.Push(StringConstant(ExpressionSetting.Instance.DecodeString(value)));
                    return currentIndex;
                }
                else
                {
                    return index;
                }
            }
            else
            {
                expressionStack.Push(StringConstant(value));
                return currentIndex;
            }
        }

        /// <summary>
        /// 尝试从字符串<paramref name="formula"/>中位置<paramref name="index"/>的位置开始构建变量
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="index"></param>
        /// <param name="expressionStack"></param>
        /// <returns>返回新的位置index，如果返回的位置与传入的相同则表示没有获取到</returns>
        private static int TryVariableBuild(string formula, int index, Stack<Expression> expressionStack)
        {
            int currentIndex = index;

            if (ExpressionSetting.Instance.HasVariableStartChar)
            {
                if (ExpressionSetting.Instance.VariableStartChar.Equals(formula[currentIndex]))
                {
                    ++currentIndex;
                }
                else
                {
                    return index;
                }
            }

            int startIndex = currentIndex;

            while (currentIndex < formula.Length)
            {
                // 如果有结束字符，则可以接受所有字符，对包含的结束字符识别转义
                if (ExpressionSetting.Instance.HasVariableEndChar)
                {
                    // 发现了结束字符
                    if (currentIndex > index + 1 && formula[currentIndex] == ExpressionSetting.Instance.VariableEndChar)
                    {
                        //如果当前结束字符后面紧跟一个结束字符，则标识当前结束字符是被转义的，不作为结束字符
                        if (currentIndex + 1 < formula.Length && formula[currentIndex + 1] == ExpressionSetting.Instance.VariableEndChar)
                        {
                            currentIndex += 2;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        ++currentIndex;
                    }
                }
                else if (ExpressionSetting.Instance.IsVariableNameChar(formula[currentIndex])) // 如果没有结束字符，必须是严格要求的可识别字符
                {
                    ++currentIndex;
                }
                else // 否则就是错误的无法识别
                {
                    break;
                }
            }

            if (currentIndex == index)
            {
                return index;
            }

            string value = formula.Substring(startIndex, currentIndex - startIndex);

            if (ExpressionSetting.Instance.HasVariableEndChar)
            {
                if (currentIndex >= formula.Length)
                {
                    throw new ExpressionException("lose variable end char", formula);
                }
                if (ExpressionSetting.Instance.VariableEndChar.Equals(formula[currentIndex]))
                {
                    ++currentIndex;
                    expressionStack.Push(Variable(ExpressionSetting.Instance.DecodeVariable(value)));
                    return currentIndex;
                }
                else
                {
                    return index;
                }
            }
            else
            {
                expressionStack.Push(Variable(value));
                return currentIndex;
            }
        }

        /// <summary>
        /// 尝试从字符串<paramref name="formula"/>中位置<paramref name="index"/>的位置开始构建区间
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="index"></param>
        /// <param name="expressionStack"></param>
        /// <returns>返回新的位置index，如果返回的位置与传入的相同则表示没有获取到</returns>
        private static int TryRangeBuild(string formula, int index, Stack<Expression> expressionStack)
        {
            if (!(formula[index] == '(' || formula[index] == '['))
            {
                return index;
            }

            var endIndex = Int32.MaxValue;

            var findIndex = formula.IndexOf(')', index);
            if (findIndex > 0)
            {
                endIndex = Math.Min(endIndex, findIndex);
            }

            findIndex = formula.IndexOf(']', index);
            if (findIndex > 0)
            {
                endIndex = Math.Min(endIndex, findIndex);
            }

            if (endIndex == Int32.MaxValue)
            {
                return index;
            }

            var range = formula.Substring(index, endIndex - index + 1);
            if (RangeUtils.IsRange(range))
            {
                expressionStack.Push(RangeConstant(range));
                return endIndex + 1;
            }

            return index;
        }

        /// <summary>
        /// 尝试去掉注释
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int TryCommentTrim(string formula, int index)
        {
            var currentIndex = index;
            if ('#'.Equals(formula[currentIndex]))
            {
                ++currentIndex;
            }
            else
            {
                return index;
            }

            // #号后面全是注释，遇到换行结束
            while (currentIndex < formula.Length)
            {
                if (formula[currentIndex] == '\n')
                {
                    return currentIndex;
                }
                ++currentIndex;
            }

            return currentIndex;
        }

        #endregion

        #region 私有拼接表达式方法


        private static BinaryExpression MakeBinary(Expression left, ExpressionType type, Expression right)
        {
            return new BinaryExpression(left, type, right);
        }

        private static FunctionExpression MakeFunction(ExpressionType type, Expression child)
        {
            var children = new List<Expression> { child };
            return MakeFunction(type, children);
        }

        private static FunctionExpression MakeFunction(ExpressionType type, IList<Expression> children)
        {
            return new FunctionExpression(type, children);
        }

        private static FunctionExpression MakeFunction(ExpressionType type)
        {
            return new FunctionExpression(type, new List<Expression>());
        }

        #endregion
    }
}

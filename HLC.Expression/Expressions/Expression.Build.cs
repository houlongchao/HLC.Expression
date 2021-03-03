using System;
using System.Collections.Generic;
using System.Linq;
using HLC.Expression.Utils;

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

            while (index < formula.Length)
            {
                // 移除空格
                if (formula[index] == ' ')
                {
                    ++index;
                    continue;
                }

                // 匹配方法
                if (lastMatchType == MatchType.None ||
                    lastMatchType == MatchType.BinaryOperator ||
                    lastMatchType == MatchType.LeftBracket ||
                    lastMatchType == MatchType.Function ||
                    lastMatchType == MatchType.Separator)
                {
                    if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("NOT("))
                    {
                        operatorStack.Push(Operator.Not);
                        lastMatchType = MatchType.Function;
                        index += 4;
                        continue;
                    }

                    if (index + 8 < formula.Length && formula.Substring(index, 8).Equals("CEILING("))
                    {
                        operatorStack.Push(Operator.Ceiling);
                        lastMatchType = MatchType.Function;
                        index += 8;
                        continue;
                    }

                    if (index + 9 < formula.Length && formula.Substring(index, 9).Equals("FLOORING("))
                    {
                        operatorStack.Push(Operator.Flooring);
                        lastMatchType = MatchType.Function;
                        index += 9;
                        continue;
                    }

                    if (index + 9 < formula.Length && formula.Substring(index, 9).Equals("ROUNDING("))
                    {
                        operatorStack.Push(Operator.Rounding);
                        lastMatchType = MatchType.Function;
                        index += 9;
                        continue;
                    }

                    if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("MAX("))
                    {
                        operatorStack.Push(Operator.Max);
                        lastMatchType = MatchType.Function;
                        index += 4;
                        continue;
                    }

                    if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("MIN("))
                    {
                        operatorStack.Push(Operator.Min);
                        lastMatchType = MatchType.Function;
                        index += 4;
                        continue;
                    }

                    if (index + 3 < formula.Length && formula.Substring(index, 3).Equals("IF("))
                    {
                        operatorStack.Push(Operator.If);
                        lastMatchType = MatchType.Function;
                        index += 3;
                        continue;
                    }

                    if (index + 3 < formula.Length && formula.Substring(index, 3).Equals("IN("))
                    {
                        operatorStack.Push(Operator.In);
                        lastMatchType = MatchType.Function;
                        index += 3;
                        continue;
                    }

                    if (index + 4 < formula.Length && formula.Substring(index, 4).Equals("AND("))
                    {
                        operatorStack.Push(Operator.FunctionAnd);
                        lastMatchType = MatchType.Function;
                        index += 4;
                        continue;
                    }
                    if (index + 3 < formula.Length && formula.Substring(index, 3).Equals("OR("))
                    {
                        operatorStack.Push(Operator.FunctionOr);
                        lastMatchType = MatchType.Function;
                        index += 3;
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
                        while (Operator.Separator.GetPriority() < topOperator.GetPriority())
                        {
                            var result = TryBinaryBuild(topOperator, expressionStack);
                            if (!result)
                            {
                                throw new ExpressionException("表达式有误：" + formula);
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
                throw new ExpressionException($"An error near index {index} '{formula.Substring(index, count)}' formula: {formula}");
            }

            while (operatorStack.Count > 0)
            {
                Operator op = operatorStack.Pop();
                _ = TryBinaryBuild(op, expressionStack) || TryFunctionBuild(op, expressionStack, operatorStack);
            }

            return expressionStack.Pop();
        }

        #endregion


        #region 静态 公共表达式树构建

        public static ConstantExpression NumConstant(double value)
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
        public static ResultExpression Result(double data)
        {
            return new ResultExpression(data);
        }
        public static ResultExpression Result(string data)
        {
            return new ResultExpression(data);
        }
        public static ResultExpression Result(IList<double> data)
        {
            return new ResultExpression(data);
        }
        public static ResultExpression Result(IList<string> data)
        {
            return new ResultExpression(data);
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

        public static FunctionExpression In(Expression value, IList<Expression> list)
        {
            list.Insert(0, value);
            return MakeFunction(ExpressionType.In, list);
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
                        throw new ExpressionException("分析构建表达式出错");
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

        private static bool TryFunctionBuild(Operator op, Stack<Expression> expressionStack, Stack<Operator> operatorStack)
        {
            // 一元方法
            if (op == Operator.Ceiling)
            {
                Expression data = expressionStack.Pop();
                expressionStack.Push(Ceiling(data));
                return true;
            }

            if (op == Operator.Flooring)
            {
                Expression data = expressionStack.Pop();
                expressionStack.Push(Flooring(data));
                return true;
            }

            if (op == Operator.Rounding)
            {
                Expression data = expressionStack.Pop();
                expressionStack.Push(Rounding(data));
                return true;
            }

            if (op == Operator.Not)
            {
                Expression data = expressionStack.Pop();
                expressionStack.Push(Not(data));
                return true;
            }

            // 多元方法
            if (op == Operator.Separator)
            {
                int separatorCount = 1;
                Operator topOp = operatorStack.Pop();
                while (topOp == Operator.Separator)
                {
                    separatorCount = separatorCount + 1;
                    topOp = operatorStack.Pop();
                }

                if (topOp == Operator.Max)
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

                if (topOp == Operator.Min)
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

                if (topOp == Operator.In)
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

                if (topOp == Operator.If)
                {
                    Expression ifFalse = expressionStack.Pop();
                    Expression ifTrue = expressionStack.Pop();
                    Expression condition = expressionStack.Pop();
                    expressionStack.Push(If(condition, ifTrue, ifFalse));
                    return true;
                }

                if (topOp == Operator.Rounding)
                {
                    var digits = expressionStack.Pop();
                    var value = expressionStack.Pop();
                    expressionStack.Push(Rounding(value, digits));
                    return true;
                }

                if (topOp == Operator.FunctionAnd)
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

                if (topOp == Operator.FunctionOr)
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
            }

            return false;

        }

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

            double value = isNegative
                ? -double.Parse(formula.Substring(startIndex, currentIndex - startIndex))
                : double.Parse(formula.Substring(startIndex, currentIndex - startIndex));

            expressionStack.Push(NumConstant(value));

            return currentIndex;
        }

        private static int TryBooleanBuild(string formula, int index, Stack<Expression> expressionStack)
        {
            if (formula.Length > index + 4 && formula.Substring(index, 4).Equals("true"))
            {
                expressionStack.Push(BoolConstant(true));
                return index + 4;
            }
            if (formula.Length > index + 5 && formula.Substring(index, 5).Equals("false"))
            {
                expressionStack.Push(BoolConstant(false));
                return index + 5;
            }
            return index;
        }

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
                if (ExpressionSetting.Instance.IsStringChar(formula[currentIndex]))
                {
                    ++currentIndex;
                }
                else
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
                if (ExpressionSetting.Instance.StringEndChar.Equals(formula[currentIndex]))
                {
                    ++currentIndex;
                    expressionStack.Push(StringConstant(value));
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
                if (ExpressionSetting.Instance.IsVariableNameChar(formula[currentIndex]))
                {
                    ++currentIndex;
                }
                else
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
                if (ExpressionSetting.Instance.VariableEndChar.Equals(formula[currentIndex]))
                {
                    ++currentIndex;
                    expressionStack.Push(Variable(value));
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

            findIndex = formula.IndexOf(')', index);
            if (findIndex > 0)
            {
                endIndex = Math.Min(endIndex, findIndex);
            }

            if (endIndex == Int32.MaxValue)
            {
                return index;
            }

            var range = formula.Substring(index, endIndex - index + 1);
            if (RangeUtil.IsRange(range))
            {
                expressionStack.Push(RangeConstant(range));
                return endIndex + 1;
            }

            return index;
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

        #endregion
    }
}
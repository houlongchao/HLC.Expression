using HLC.Expression.Segments;
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
                    if (lastMatchType == MatchType.Value || lastMatchType == MatchType.RightBracket || lastMatchType == MatchType.Function)
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
                            lastMatchType = MatchType.Separator;
                            index += 1;
                            continue;
                        }
                    }

                    // 匹配二元运算符
                    if (lastMatchType == MatchType.Value || lastMatchType == MatchType.RightBracket)
                    {
                        var binarySegments = SegmentManager.GetAllBinarySegments();
                        var matchFlag = false;
                        foreach (var segment in binarySegments.OrderByDescending(t => t.MatchLength))
                        {
                            if (index + segment.MatchLength < formula.Length && formula.Substring(index, segment.MatchLength).Equals(segment.MatchString))
                            {
                                AnalysisBuild(segment.MatchOperator, expressionStack, operatorStack);
                                lastMatchType = MatchType.BinaryOperator;
                                index += segment.MatchLength;
                                matchFlag = true;
                                break;
                            }
                        }
                        if (matchFlag)
                        {
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
                throw;
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

        #region 私有解析构建表达式方法

        #region 二元运算构建

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
            var segment = SegmentManager.GetBinarySegment(op);
            if (segment == null)
            {
                return false;
            }
            Expression right = expressionStack.Pop();
            Expression left = expressionStack.Pop();
            var expression = segment.BuildBinaryExpression(left, right);
            if (expression != null)
            {
                expressionStack.Push(expression);
                return true;
            }
            return false;
        }

        #endregion

        #region 函数构建

        /// <summary>
        /// 尝试解析函数运算符
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="index"></param>
        /// <param name="operatorStack"></param>
        /// <returns></returns>
        private static int TryFunctionBuild(string formula, int index, Stack<Operator> operatorStack)
        {
            var functionSegments = SegmentManager.GetAllFunctionSegments();
            foreach (var segment in functionSegments)
            {
                if (index + segment.MatchLength < formula.Length && formula.Substring(index, segment.MatchLength).Equals(segment.MatchString))
                {
                    operatorStack.Push(segment.MatchOperator);
                    return index + segment.MatchLength;
                }
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
            if (Operator.Separator == op)
            {
                int separatorCount = 1;
                Operator topOp = operatorStack.Pop();
                while (topOp == Operator.Separator)
                {
                    separatorCount = separatorCount + 1;
                    topOp = operatorStack.Pop();
                }

                var segment = SegmentManager.GetFunctionSegment(topOp);
                if (segment.IsPairArgs)
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
                    var expression = segment.BuildPairFunctionExpression(value, arrayList);
                    if (expression != null)
                    {
                        expressionStack.Push(expression);
                        return true;
                    }
                    return false;
                }
                else
                {
                    IList<Expression> arrayList = new List<Expression>();
                    while (separatorCount-- >= 0)
                    {
                        arrayList.Add(expressionStack.Pop());
                    }
                    arrayList = arrayList.Reverse().ToList();
                    var expression = segment.BuildFunctionExpression(arrayList);
                    if (expression != null)
                    {
                        expressionStack.Push(expression);
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                var segment = SegmentManager.GetFunctionSegment(op);
                if (segment == null)
                {
                    return false;
                }

                if (segment.IsFreeArg)
                {
                    expressionStack.Push(Expression.MakeFunction(ExpressionType.PI));
                    return true;
                }

                Expression data = expressionStack.Pop();
                var expression = segment.BuildFunctionExpression(data);
                if (expression != null)
                {
                    expressionStack.Push(expression);
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region 数字构建

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

        #endregion

        #region 布尔构建

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

        #endregion

        #region 字符串构建

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

        #endregion

        #region 变量参数构建

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

        #endregion

        #region 数值区间构建

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

        #endregion

        #region 公式注释移除

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

        #endregion
    }
}

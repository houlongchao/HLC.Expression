﻿using System.Collections.Generic;

namespace HLC.Expression
{
    /// <summary>
    /// 表达式树
    /// </summary>
    public abstract partial class Expression
    {
        public ExpressionType Type { get; protected set; }

        /// <summary>
        /// Invoke 执行后的结果
        /// </summary>
        public ResultExpression InvokeResult { get; protected set; }

        protected Expression(ExpressionType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"{Type:G}";
        }

        public abstract IList<string> GetVariableKeys();

        #region 调用表达式树

        /// <summary>
        /// 执行表达式树，获得运算结果
        /// </summary>
        /// <returns></returns>
        public ResultExpression Invoke()
        {
            return Invoke(null);
        }

        /// <summary>
        /// 执行表达式树，获得运算结果
        /// </summary>
        /// <returns></returns>
        public abstract ResultExpression Invoke(Parameters parameters);

        /// <summary>
        /// 检查表达式所需参数是否都在参数列表中
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool CheckParameters(Parameters parameters)
        {
            var variableKeys = GetVariableKeys();
            foreach (var variableKey in variableKeys)
            {
                if (!parameters.ContainsKey(variableKey))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 静态 表达式数构建

        /// <summary>
        /// 从字符串构建表达式树
        /// <code>
        /// 支持：
        /// + - * / % ^ ()
        /// &amp;&amp; ||
        /// &gt; &gt;= &lt; &lt;= == !=
        /// CEILING(_) FLOORING(_)
        /// MAX(_,_,_..) MIN(_,_,_..)
        /// NOT(_) AND(_,_,..) OR(_,_,..)
        /// IF(_,_,_)
        /// IN(_,_,..)
        /// CONCAT(_,..)
        /// SWITCH(_,_:_,_:_,..) SWITCHC(_,_:_,_:_,..)
        /// SUBSTR(_,_,_) SUBNUM(_,_,_)
        /// </code>
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static Expression From(string formula)
        {
            return FromFormula(formula);
        }

        #endregion
    }
}

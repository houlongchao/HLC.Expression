namespace HLC.Expression
{
    /// <summary>
    /// 表达式类型扩展方法
    /// </summary>
    public static class ExpressionTypeExtension
    {
        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        public static int GetPriority(this ExpressionType expressionType)
        {
            if (expressionType == ExpressionType.Power)
            {
                return 100;
            }
            if (expressionType == ExpressionType.Multiply || expressionType == ExpressionType.Divide || expressionType == ExpressionType.Modulo)
            {
                return 50;
            }
            if (expressionType == ExpressionType.Add || expressionType == ExpressionType.Subtract)
            {
                return 40;
            }

            if (expressionType == ExpressionType.Greater ||
                expressionType == ExpressionType.GreaterEqual ||
                expressionType == ExpressionType.Less ||
                expressionType == ExpressionType.LessEqual ||
                expressionType == ExpressionType.Equal ||
                expressionType == ExpressionType.NotEqual)
            {
                return 30;
            }

            if (expressionType == ExpressionType.BooleanAnd)
            {
                return 20;
            }

            if (expressionType == ExpressionType.BooleanOr)
            {
                return 10;
            }
            return 100;
        }
    }
}
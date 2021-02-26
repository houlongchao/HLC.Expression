namespace HLC.Expression
{
    /// <summary>
    /// 操作符扩展方法
    /// </summary>
    public static class OperatorExtension
    {
        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static int GetPriority(this Operator op)
        {
            if (op == Operator.Power)
            {
                return 100;
            }

            if (op == Operator.Multiply || op == Operator.Divide || op == Operator.Modulo)
            {
                return 50;
            }

            if (op == Operator.Add || op == Operator.Subtract)
            {
                return 40;
            }

            if (op == Operator.Greater ||
                op == Operator.GreaterEqual ||
                op == Operator.Less ||
                op == Operator.LessEqual ||
                op == Operator.Equal ||
                op == Operator.NotEqual)
            {
                return 30;
            }

            if (op == Operator.BooleanAnd)
            {
                return 20;
            }

            if (op == Operator.BooleanOr)
            {
                return 10;
            }


            if (op == Operator.Separator ||
                op == Operator.Ceiling ||
                op == Operator.Flooring ||
                op == Operator.Rounding ||
                op == Operator.Max ||
                op == Operator.Min ||
                op == Operator.Not ||
                op == Operator.If ||
                op == Operator.In ||
                op == Operator.FunctionAnd ||
                op == Operator.FunctionOr)
            {
                return 5;
            }

            return 0;
        }

    }
}
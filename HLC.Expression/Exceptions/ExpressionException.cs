using System;

namespace HLC.Expression
{
    /// <summary>
    /// 表达式异常
    /// </summary>
    public class ExpressionException : Exception
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string Formula { get; set; }

        public ExpressionException(string message) : this(0, 0, message, null)
        {
        }

        public ExpressionException(string message, string formula) : this(0, 0, message, formula)
        {
        }

        public ExpressionException(int line, int column, string message, string formula) : base(message)
        {
            Line = line;
            Column = column;
            Formula = formula;
        }

        public override string ToString()
        {
            return $"{Line}-{Column} {Message} [{Formula}]";
        }
    }
}

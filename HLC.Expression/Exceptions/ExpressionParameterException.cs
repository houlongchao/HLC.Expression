using System;

namespace HLC.Expression
{
    public class ExpressionParameterException : Exception
    {
        public string Key { get; set; }

        public ExpressionParameterException(string key, string message) : base(message)
        {
            Key = key;
        }
    }
}

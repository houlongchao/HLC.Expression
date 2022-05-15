namespace HLC.Expression
{
    public class ExpressionParameterNotFoundException : ExpressionParameterException
    {
        public ExpressionParameterNotFoundException(string key) : base(key, $"Not found parameter: {key}")
        {
        }
    }
}

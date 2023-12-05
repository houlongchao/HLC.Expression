namespace HLC.Expression.Definitions
{
    public class ExpressionSymbolDefinitionItem
    {
        public ExpressionSymbolDefinitionItem(string symbol, string desc)
        {
            Name = symbol;
            Desc = desc;
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public string Demo { get; set; }
        public string Details { get; set; }
    }
}

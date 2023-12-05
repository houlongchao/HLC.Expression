namespace HLC.Expression.Definitions
{
    public class ExpressionFunctionDefinitionItem
    {
        public ExpressionFunctionDefinitionItem(string name, string desc)
        {
            Name = name;
            Desc = desc;
        }

        public ExpressionFunctionDefinitionItem(string group, string name, string desc)
        {
            Group = group;
            Name = name;
            Desc = desc;
        }

        public string Group { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Demo { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string Details { get; set; }
    }
}

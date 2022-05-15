using System.Collections.Generic;

namespace HLC.Expression
{
    public static partial class ExpressionSymbolDefinitions
    {
        public static List<Item> Items { get; } =
            new List<Item>()
            {
                new Item("==", "等于"){ Demo = "5==2", Details = "如果左侧为数字，右侧可以是一个区间，如{A}==[1,10]"},
                new Item("!=", "不等于"){ Demo = "5!=2", Details = "如果左侧为数字，右侧可以是一个区间，如{A}==[1,10]"},
                new Item("&&", "与运算"){ Demo = "true && false", Details = "左右只能是Boolean或表达式结果为Boolean"},
                new Item("||", "或运算"){ Demo = "true || false", Details = "左右只能是Boolean或表达式结果为Boolean"},

                new Item(">", "大于"){ Demo = "5>2", Details = "左右只能是数字或表达式结果为数字"},
                new Item(">=", "大于等于"){ Demo = "5>=2", Details = "左右只能是数字或表达式结果为数字"},
                new Item("<", "小于"){ Demo = "5<2", Details = "左右只能是数字或表达式结果为数字"},
                new Item("<=", "小于等于"){ Demo = "5<=2", Details = "左右只能是数字或表达式结果为数字"},
                
                new Item("*", "乘法"){ Demo = "5*2", Details = "左右只能是数字或表达式结果为数字"},
                new Item("/", "除法"){ Demo = "5/2", Details = "左右只能是数字或表达式结果为数字"},
                new Item("+", "加法"){ Demo = "5+2", Details = "左右只能是数字或表达式结果为数字"},
                new Item("-", "减法"){ Demo = "5-2", Details = "左右只能是数字或表达式结果为数字"},
                new Item("%", "取模"){ Demo = "5%2", Details = "左右只能是整数或表达式结果为整数"},
                new Item("^", "指数"){ Demo = "5^2", Details = "左右只能是数字或表达式结果为数字"},
            };
    }

    public static partial class ExpressionSymbolDefinitions
    {
        public class Item
        {
            public Item(string symbol, string desc)
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
}

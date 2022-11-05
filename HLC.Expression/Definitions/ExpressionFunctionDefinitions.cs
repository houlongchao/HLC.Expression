using System.Collections.Generic;

namespace HLC.Expression
{
    public static partial class ExpressionFunctionDefinitions
    {
        public const string Text = "文本函数";
        public const string Number = "数值函数";
        public const string Value = "参数函数";
        public const string If = "判断函数";
        public const string Boolean = "逻辑函数";
        public const string Math = "数学函数";
        public const string Array = "数组函数";

        public static List<Item> Items { get; } = new List<Item>();

        static ExpressionFunctionDefinitions()
        {
            Items.AddRange(GetValue());
            Items.AddRange(GetIf());
            Items.AddRange(GetNumber());
            Items.AddRange(GetText());
            Items.AddRange(GetBoolean());
            Items.AddRange(GetArray());
            Items.AddRange(GetMath());
        }

        /// <summary>
        /// 文本函数
        /// </summary>
        /// <returns></returns>
        private static List<Item> GetText()
        {
            return new List<Item>()
            {
                new Item(Text, "CONCAT()", "字符串拼接")
                {
                    Demo = "CONCAT({OPT:A}, 'str')  CONCAT({OPT:A}, 1, 'str')",
                    Input = "输入参数至少1个，对所有输入参数进行字符串拼接。",
                    Output = "字符串。将所有输入参数当作字符串进行拼接输出。",
                },
                new Item(Text, "SUBSTR()", "取子串")
                {
                    Demo = "SUBSTR({OPT:A}, 1, 2)  SWITCH({OPT:A}, 0, 2)",
                    Input = "输入参数必须3个，第一个参数为要截取的源字符串，第二个为开始截取坐标，第三个为截取长度。",
                    Output = "字符串。输出截取的字符串结果。",
                },
                new Item(Text, "SUBNUM()", "取子串转数值")
                {
                    Demo = "SUBNUM({OPT:A}, 1, 2)  SUBNUM({OPT:A}, 0, 2)",
                    Input = "输入参数必须3个，第一个参数为要截取的源字符串，第二个为开始截取坐标，第三个为截取长度。",
                    Output = "字符串。输出截取的字符串结果转为数值。",
                },
                new Item(Text, "LEFT()", "从左侧取指定长度子串")
                {
                    Demo = "LEFT({OPT:A}, 2)  LEFT('123456', 3)",
                    Input = "输入参数必须2个，第一个参数为要截取的源字符串，第二个为截取长度。",
                    Output = "字符串。输出从左侧截取的字符串结果。",
                },
                new Item(Text, "RIGHT()", "从右侧取指定长度子串")
                {
                    Demo = "RIGHT({OPT:A}, 2)  RIGHT('123456', 3)",
                    Input = "输入参数必须2个，第一个参数为要截取的源字符串，第二个为截取长度。",
                    Output = "字符串。输出从右侧截取的字符串结果。",
                },
                new Item(Text, "REVERSE()", "反转字符串")
                {
                    Demo = "REVERSE({OPT:A})  REVERSE('123456')",
                    Input = "字符串或计算结果为字符串。",
                    Output = "字符串。输出输入字符串的反转字符串。",
                },
                new Item(Text, "FIND()", "获取下标")
                {
                    Demo = "FIND({OPT:A}, '2')  FIND('123456', '23')",
                    Input = "输入参数必须2个，第一个参数为要匹配的源字符串，第二个为匹配字符。",
                    Output = "整数。输出匹配到的位置下标（下标从0开始）。",
                },
                new Item(Text, "LENGTH()", "获取字符串长度")
                {
                    Demo = "LENGTH({OPT:A})  LENGTH('123456')",
                    Input = "字符串或计算结果为字符串。",
                    Output = "整数。输出输入字符串的长度。",
                },
            };
        }

        /// <summary>
        /// 数值函数
        /// </summary>
        /// <returns></returns>
        private static List<Item> GetNumber()
        {
            return new List<Item>()
            {
                new Item(Number, "CEILING()", "向上取整")
                {
                    Demo = "CEILING(1.1)  CEILING(1.1+2.2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "整数。输出比输入参数大的最小整数。"
                },
                new Item(Number, "FLOORING()", "向下取整")
                {
                    Demo = "FLOORING(1.1)  FLOORING(1.1+2.2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "整数。输出比输入参数小的最大整数。"
                },
                new Item(Number, "ROUNDING()", "四舍五入")
                {
                    Demo = "ROUNDING(1.1)  ROUNDING(1.1+2.2, 1)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值。输出保留指定位数的四舍五入值。"
                },
                new Item(Number, "MAX()", "取最大值")
                {
                    Demo = "MAX(1.1, 2, 3)  MAX(1.1+2.2, 3, 4)",
                    Input = "数值，或结果为数值的表达式。输入可以为多个，用逗号分隔。",
                    Output = "数值。输出最大的输入结果。",
                },
                new Item(Number, "MIN()", "取最小值")
                {
                    Demo = "MIN(1.1, 2, 3)  MIN(1.1+2.2, 3, 4)",
                    Input = "数值，或结果为数值的表达式。输入可以为多个，用逗号分隔。",
                    Output = "数值。输出最小的输入结果。",
                },
            };
        }

        /// <summary>
        /// 参数函数
        /// </summary>
        /// <returns></returns>
        private static List<Item> GetValue()
        {
            return new List<Item>()
            {
                new Item(Value, "GET()", "变量为空返回默认值")
                {
                    Demo = "GET({OPT:A}, 'AAA')   GET({OPT:NUM},123)",
                    Input = "输入参数必须为2个，第一个为要获取变量，第二个为变量默认值",
                    Output = "变量值或者默认值",
                },
                new Item(Value, "META()", "获取属性信息")
                {
                    Demo = "META({OPT:A}, 'meta01')   META({OPT:A}, 'meta01', 'bool')",
                    Input = "输入参数必须为2个或3个，第一个为变量，第二个为属性名字符串，第三个可选为转换类型('bool','num', 'txt'),不传或其它输入时默认为txt",
                    Output = "属性值",
                },
                new Item(Value, "TONUM()", "转为数字")
                {
                    Demo = "TONUM({OPT:NUM1})   TONUM('123')",
                    Input = "一个输入参数",
                    Output = "结果数字",
                },
                new Item(Value, "TOSTR()", "转为字符串")
                {
                    Demo = "TOSTR({OPT:DATETIME})   TOSTR({OPT:DATETIME}, 'yyyyMMdd')",
                    Input = "变量和一个可选格式化字符串",
                    Output = "字符串",
                },
                new Item(Value, "DATETIME()", "字符串转日期时间")
                {
                    Demo = "DATETIME('2021-02-01 12:00')  DATETIME('202102011200', 'yyyyMMddHHmm')",
                    Input = "一个可以转换为日期时间的字符串, 第二个参数为可选格式化字符串",
                    Output = "日期类型",
                },
            };
        }

        /// <summary>
        /// 判断函数
        /// </summary>
        /// <returns></returns>
        private static List<Item> GetIf()
        {
            return new List<Item>()
            {
                new Item(If, "IF()", "逻辑判断")
                {
                    Demo = "IF(1>2, true, false)  IF(1 > 2, 2 > 4, false)",
                    Input = "输入参数必须为3个，第一个为逻辑值或逻辑结果公式，第二和第三个参数为任意值或任意公式。",
                    Output = "任意值。当第一个参数为逻辑true时输出第二个参数结果，否则输出第三个参数结果",
                },
                new Item(If, "SWITCH()", "精确开关匹配")
                {
                    Demo = "SWITCH({OPT:A}, 1:'str', 2:'2')  SWITCH({OPT:A}, 'A':1, 'B':2)",
                    Input = "输入参数至少2个，第一个参数为待匹配参数，后面的参数为匹配值和结果值(用冒号分隔)。",
                    Output = "任意值。输出匹配参数中与待匹配参数相同匹配值的结果值。",
                },
                new Item(If, "SWITCHC()", "模糊开关匹配")
                {
                    Demo = "SWITCHC({OPT:A}, '1':'str', '2':'2')  SWITCHC({OPT:A}, 'A':1, 'B':2)",
                    Input = "输入参数至少2个，第一个参数为待匹配参数，后面的参数为匹配值和结果值(用冒号分隔)。",
                    Output = "任意值。输出匹配参数中包含待匹配参数匹配值的结果值。",
                },
            };
        }

        /// <summary>
        /// 逻辑函数
        /// </summary>
        /// <returns></returns>
        private static List<Item> GetBoolean()
        {
            return new List<Item>()
            {
                new Item(Boolean, "NOT()", "逻辑取反")
                {
                    Demo = "NOT(true)  NOT(true && false) NOT(1 > 2)",
                    Input = "Boolean值，或结果为Boolean的表达式。",
                    Output = "逻辑值。输出当前输入的非逻辑。",
                },
                new Item(Boolean, "AND()", "逻辑与运算")
                {
                    Demo = "AND(true, true, false)  AND(1 > 2, 2 > 4, false)",
                    Input = "Boolean值，或结果为Boolean的表达式。输入可以为多个，用逗号分隔。",
                    Output = "逻辑值。输出对所有输入参数的与运算结果。",
                },
                new Item(Boolean, "OR()", "逻辑或运算")
                {
                    Demo = "OR(true, true, false)  OR(1 > 2, 2 > 4, false)",
                    Input = "Boolean值，或结果为Boolean的表达式。输入可以为多个，用逗号分隔。",
                    Output = "逻辑值。输出对所有输入参数的或运算结果。",
                },
                new Item(Boolean, "IN()", "包含判断")
                {
                    Demo = "IN({OPT:A}, 1, 2)  IN({OPT:A}, 1, 2+3)",
                    Input = "输入参数至少2个，判断第一个参数是否在后面参数列表中。",
                    Output = "逻辑值。如果第一个参数在后面参数列表中则为true，否则为false。",
                },
                new Item(Boolean, "HASVALUE()", "是否有数据")
                {
                    Demo = "HASVALUE({OPT:A})  HASVALUE({OPT:B})",
                    Input = "一个参数变量",
                    Output = "逻辑值。输出当前参数变量是否有数据。",
                },
                new Item(Boolean, "ISNUMBER()", "是否为数值")
                {
                    Demo = "ISNUMBER({OPT:A})  ISNUMBER({OPT:A}+{OPT:B})",
                    Input = "参数变量，或一个表达式。",
                    Output = "逻辑值。输出结果是否为数值",
                },
                new Item(Boolean, "ISSTART()", "是否以指定字符串开始")
                {
                    Demo = "ISSTART({OPT:A}, 'a')  ISSTART({OPT:A}, '123')",
                    Input = "输入参数必须为2个，第一个为字符串或计算结果为字符串的表达式，第二个为要匹配的字符串",
                    Output = "逻辑值。输出结果为第一个参数是否以第二个参数开始",
                },
                new Item(Boolean, "ISEND()", "是否以指定字符串结尾")
                {
                    Demo = "ISEND({OPT:A}, 'a')  ISEND({OPT:A}, '123')",
                    Input = "输入参数必须为2个，第一个为字符串或计算结果为字符串的表达式，第二个为要匹配的字符串",
                    Output = "逻辑值。输出结果为第一个参数是否以第二个参数结尾",
                },
                new Item(Boolean, "ISMATCH()", "是否可以用指定正则匹配")
                {
                    Demo = "ISMATCH('123456', '^\\d*$')  ISMATCH('123456', '^1\\d{5}$')",
                    Input = "输入参数必须为2个，第一个为字符串或计算结果为字符串的表达式，第二个为要匹配的正则字符串",
                    Output = "逻辑值。输出结果为第一个参数是否可以用第二个正则表达式进行匹配",
                },
            };
        }

        /// <summary>
        /// 数学函数
        /// </summary>
        /// <returns></returns>
        private static List<Item> GetMath()
        {
            return new List<Item>()
            {
                new Item(Math, "ABS()", "绝对值函数")
                {
                    Demo = "ABS(1.1)  ABS(1.1+2.2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值。输出为输入值的绝对值"
                },
                new Item(Math, "SIN()", "正弦函数")
                {
                    Demo = "SIN(1.1)  SIN(1.1+2.2)",
                    Input = "数值(弧度)，或结果为数值(弧度)的表达式",
                    Output = "数值(正弦值)。输出为输入弧度的正弦值"
                },
                new Item(Math, "ASIN()", "反正弦函数")
                {
                    Demo = "ASIN(1.1)  ASIN(1.1+2.2)",
                    Input = "数值(正弦值)，或结果为数值(正弦值)的表达式",
                    Output = "数值(弧度)。输出为输入正弦值的弧度"
                },
                new Item(Math, "COS()", "余弦函数")
                {
                    Demo = "COS(1.1)  COS(1.1+2.2)",
                    Input = "数值(弧度)，或结果为数值(弧度)的表达式",
                    Output = "数值(余弦值)。输出为输入弧度的余弦值"
                },
                new Item(Math, "ACOS()", "反余弦函数")
                {
                    Demo = "ACOS(1.1)  ACOS(1.1+2.2)",
                    Input = "数值(余弦值)，或结果为数值(余弦值)的表达式",
                    Output = "数值(弧度)。输出为输入余弦值的弧度"
                },
                new Item(Math, "TAN()", "正切函数")
                {
                    Demo = "TAN(1.1)  TAN(1.1+2.2)",
                    Input = "数值(弧度)，或结果为数值(弧度)的表达式",
                    Output = "数值(正切值)。输出为输入弧度的正切值"
                },
                new Item(Math, "ATAN()", "反正切函数")
                {
                    Demo = "ATAN(1.1)  ATAN(1.1+2.2)",
                    Input = "数值(正切值)，或结果为数值(正切值)的表达式",
                    Output = "数值(弧度)。输出为输入正切值的弧度"
                },
                new Item(Math, "ATAN2()", "反余切函数")
                {
                    Demo = "ATAN2(1,2)  ATAN2(1+1,2+2)",
                    Input = "数值(X,Y坐标)，或结果为数值(X,Y坐标)的表达式",
                    Output = "数值(弧度)。输出为输入坐标的反余切弧度"
                },
                new Item(Math, "SINH()", "双曲正弦函数")
                {
                    Demo = "SINH(1.1)  SINH(1.1+2.2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值"
                },
                new Item(Math, "COSH()", "双曲余弦函数")
                {
                    Demo = "COSH(1.1)  COSH(1.1+2.2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值"
                },
                new Item(Math, "TANH()", "双曲正切函数")
                {
                    Demo = "TANH(1.1)  TANH(1.1+2.2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值"
                },
                new Item(Math, "RAD()", "角度转弧度")
                {
                    Demo = "RAD(1.1)  RAD(1.1+2.2)",
                    Input = "数值(角度)，或结果为数值(角度)的表达式",
                    Output = "数值(弧度)。输出为输入角度的弧度值"
                },
                new Item(Math, "DEG()", "弧度转角度")
                {
                    Demo = "DEG(1.1)  DEG(1.1+2.2)",
                    Input = "数值(弧度)，或结果为数值(弧度)的表达式",
                    Output = "数值(角度)。输出为输入弧度的角度值"
                },
                new Item(Math, "LOG()", "自然对数")
                {
                    Demo = "LOG(2)  LOG(2, 5)",
                    Input = "数值，或结果为数值的表达式。支持一个参数或两个参数，第二个参数默认e",
                    Output = "数值。输出为输入值的对数，默认对数底为e"
                },
                new Item(Math, "LOG10()", "以10为底对数")
                {
                    Demo = "LOG10(2)  LOG10(2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值。输出为输入值以10为底的对数"
                },
                new Item(Math, "EXP()", "e的指数次幂")
                {
                    Demo = "EXP(1.1)  EXP(1.1+2.2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值"
                },
                new Item(Math, "FACT()", "阶乘")
                {
                    Demo = "FACT(5)  FACT(1+2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值"
                },
                new Item(Math, "SQRT()", "平方根")
                {
                    Demo = "SQRT(20)  SQRT(1.1+2.2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值"
                },
                new Item(Math, "MOD()", "取余数")
                {
                    Demo = "MOD(20，3)  MOD(10+20, 1+2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值"
                },
                new Item(Math, "POW()", "指数")
                {
                    Demo = "POW(20, 3)  POW(10+20, 1+2)",
                    Input = "数值，或结果为数值的表达式",
                    Output = "数值"
                },
                new Item(Math, "PI()", "圆周率PI")
                {
                    Demo = "PI()",
                    Input = "无",
                    Output = "常量圆周率PI"
                },
            };
        }

        /// <summary>
        /// 参数数组函数
        /// </summary>
        /// <returns></returns>
        private static List<Item> GetArray()
        {
            return new List<Item>()
            {
                new Item(Array, "ASUM()", "数组求和")
                {
                    Demo = "ASUM({Array})",
                    Input = "数值数组参数，或者结果未数值数组的表达式",
                    Output = "数值。输出输入数值数值参数的求和。"
                },
                new Item(Array, "ACOUNT()", "数组长度")
                {
                    Demo = "ACOUNT({Array})  ACOUNT({Array})",
                    Input = "数值数组参数，或者结果未数值数组的表达式",
                    Output = "整数。输出输入数组的长度。"
                },
                new Item(Array, "AINDEX()", "获取指定下标数据")
                {
                    Demo = "AINDEX({Array}, 1)  AINDEX({Array}, 1)",
                    Input = "输入参数必须2个。第一个为数组参数，或结果为数组的表达式，第二个为指定下标（从1开始）",
                    Output = "任意值。输出指定下标位置的值。没有数据返回空字符串。"
                },
                new Item(Array, "AMATCH()", "下标搜索")
                {
                    Demo = "AMATCH({Array}, 100)  AMATCH({Array}, '123')",
                    Input = "输入参数必须2个。第一个为数组参数，或结果为数组的表达式，第二个为要匹配的数据",
                    Output = "整数。输出匹配到的数据下标。没匹配到返回-1 。"
                },
            };
        }
    }

    public static partial class ExpressionFunctionDefinitions
    {
        public class Item
        {
            public Item(string name, string desc)
            {
                Name = name;
                Desc = desc;
            }

            public Item(string group, string name, string desc)
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
}

using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
namespace HLC.Expression.Segments
{
    public class FunctionSegmentCONCAT : FunctionSegment
    {
        public override string MatchString { get; } = "CONCAT(";
        public override Operator MatchOperator { get; } = Operator.CONCAT;
        public override ExpressionType ExpressionType => ExpressionType.CONCAT;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var sb = new StringBuilder();
            foreach (var child in children)
            {
                var result = child.Invoke(parameters);
                if (result.IsList())
                {
                    foreach (var item in result.ListResult)
                    {
                        sb.Append(item);
                    }
                }
                else
                {
                    sb.Append(result.Data);
                }
            }
            return Expression.Result(sb.ToString());
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Text, "CONCAT()", "字符串拼接")
            {
                Demo = "CONCAT({OPT:A}, 'str')  CONCAT({OPT:A}, 1, 'str')",
                Input = "输入参数至少1个，对所有输入参数进行字符串拼接。",
                Output = "字符串。将所有输入参数当作字符串进行拼接输出。",
            };
        }
    }

}

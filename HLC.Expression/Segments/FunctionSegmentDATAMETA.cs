using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentDATAMETA : FunctionSegment
    {
        public override string MatchString { get; } = "DATAMETA(";
        public override Operator MatchOperator { get; } = Operator.DATAMETA;
        public override ExpressionType ExpressionType => ExpressionType.DATAMETA;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var value = children[0] as VariableExpression;
            if (!parameters.ContainsKey(value.Key))
            {
                return Expression.ResultEmpty();
            }
            var parameter = parameters[value.Key];

            var metadata = children[1] as ConstantExpression;

            var formate = "txt";
            if (children.Count > 2)
            {
                formate = (children[2] as ConstantExpression).Value.ToString();
            }

            if (!parameter.DataMetadata.TryGetDataValue(parameter.Data.ToString(), metadata.Value.ToString(), out string v))
            {
                if (ExpressionSetting.Instance.CheckVariableExist)
                {
                    throw new ExpressionParameterException(value.Key, $"Not Found DataMetadata {parameter.Data} {metadata.Value}");
                }

                return Expression.ResultEmpty();
            }

            switch (formate)
            {
                case "bool":
                    {
                        return Expression.Result(Convert.ToBoolean(v));
                    }
                case "num":
                    {
                        return Expression.Result(Convert.ToDecimal(v));
                    }
                case "txt":
                default:
                    {
                        return Expression.Result(v);
                    }
            }
        }

        public override ExpressionFunctionDefinitionItem GetDefinistion()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Value, "DATAMETA()", "获取数据属性信息")
            {
                Demo = "DATAMETA({OPT:A}, 'meta01')   DATAMETA({OPT:A}, 'meta01', 'bool')",
                Input = "输入参数必须为2个或3个，第一个为变量，第二个为属性名字符串，第三个可选为转换类型('bool','num', 'txt'),不传或其它输入时默认为txt",
                Output = "数据的属性值",
            };
        }
    }

}

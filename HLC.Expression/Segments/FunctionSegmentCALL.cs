using HLC.Expression.Definitions;
using System;
using System.Collections.Generic;

namespace HLC.Expression.Segments
{
    public class FunctionSegmentCALL : FunctionSegment
    {
        public override string MatchString { get; } = "CALL(";
        public override Operator MatchOperator { get; } = Operator.CALL;
        public override ExpressionType ExpressionType => ExpressionType.CALL;

        public override ResultExpression Invoke(IList<Expression> children, Parameters parameters)
        {
            var functionName = children[0].Invoke(parameters).StringResult;
            var arg = new List<string>();
            for (int i = 1; i < children.Count; i++)
            {
                 var data = children[i].Invoke(parameters).StringResult;
                 arg.Add(data);
            }

            var func = parameters.GetCallFunc(functionName);
            if (func == null)
            {
                return Expression.ResultEmpty();
            }
            var result = func.Invoke(arg);
            return Expression.Result(result);
        }

        public override ExpressionFunctionDefinitionItem GetDefinition()
        {
            return new ExpressionFunctionDefinitionItem(ExpressionFunctionDefinistionGroups.Number, "CALL()", "调用自定义方法")
            {
                Demo = "CALL('funcName', 'arg1')  CALL('funcName', 'arg1', 'arg2')",
                Input = "第一个参数为自定义函数名称，后面的参数为函数的入参，入参数量不限",
                Output = "字符串",
            };
        }
    }

}

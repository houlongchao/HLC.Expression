using HLC.Expression.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLC.Expression
{
    public class FunctionPairExpression : Expression
    {
        public Expression Value { get; set; }
        public IList<Tuple<Expression, Expression>> Children { get; set; }

        public FunctionPairExpression(ExpressionType type, Expression value, IList<Tuple<Expression, Expression>> children) : base(type)
        {
            Value = value;
            Children = children;
        }

        public override ResultExpression Invoke(Parameters parameters)
        {
            var segment = SegmentManager.GetFunctionSegment(Type);
            if (segment == null)
            {
                return InvokeResult;
            }

            InvokeResult = segment.InvokePair(Value, Children, parameters);
            return InvokeResult;
        }

        public override string ToString()
        {
            var segment = SegmentManager.GetFunctionSegment(Type);
            if (segment == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(segment.MatchString);
            sb.Append(Value);
            foreach (var t in Children)
            {
                sb.Append($", {t.Item1}:{t.Item2}");
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override IList<string> GetVariableKeys()
        {
            IEnumerable<string> variableKeys = Value.GetVariableKeys();
            for (int i = 1; i < Children.Count; i++)
            {
                variableKeys = variableKeys.Concat(Children[i].Item1.GetVariableKeys());
                variableKeys = variableKeys.Concat(Children[i].Item2.GetVariableKeys());
            }
            return variableKeys.ToList();
        }
    }
}

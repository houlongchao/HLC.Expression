using HLC.Expression.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLC.Expression
{
    /// <summary>
    /// 函数表达式
    /// </summary>
    public class FunctionExpression : Expression
    {
        public IList<Expression> Children { get; set; }

        public FunctionExpression(ExpressionType type, IList<Expression> children) : base(type)
        {
            Children = children;
        }

        public override ResultExpression Invoke(Parameters parameters)
        {
            var segment = SegmentManager.GetFunctionSegment(Type);
            if (segment == null)
            {
                return InvokeResult;
            }

            InvokeResult = segment.Invoke(Children, parameters);
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
            if (Children.Count >= 0)
            {
                sb.Append(Children[0]);
                for (int i = 1; i < Children.Count; i++)
                {
                    sb.Append(", " + Children[i]);
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override IList<string> GetVariableKeys()
        {
            IEnumerable<string> result = new List<string>();
            for (int i = 0; i < Children.Count; i++)
            {
                result = result.Concat(Children[i].GetVariableKeys());
            }
            return result.ToList();
        }
    }
}

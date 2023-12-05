using HLC.Expression.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLC.Expression
{
    /// <summary>
    /// 二元运算符表达式
    /// </summary>
    public class BinaryExpression : Expression
    {
        public Expression Left { get; set; }
        public Expression Right { get; set; }

        public BinaryExpression(Expression left, ExpressionType type, Expression right) : base(type)
        {
            Left = left;
            Right = right;
        }

        public override ResultExpression Invoke(Parameters parameters)
        {
            var segment = SegmentManager.GetBinarySegment(Type);
            if (segment == null)
            {
                throw new NotSupportedException($"Not Supported {Type} In BinaryExpression");
            }

            InvokeResult = segment.Invoke(Left, Right, parameters);
            return InvokeResult;
        }

        public override string ToString()
        {
            var segment = SegmentManager.GetBinarySegment(Type);
            if (segment == null)
            {
                return "";
            }
            var sb = new StringBuilder();
            
            if (Left.Type.GetPriority() < Type.GetPriority())
            {
                sb.Append("(");
                sb.Append(Left);
                sb.Append(")");
            }
            else
            {
                sb.Append(Left);
            }
            sb.Append($" {segment.MatchString} ");

            if (Right.Type.GetPriority() < Type.GetPriority())
            {
                sb.Append("(");
                sb.Append(Right);
                sb.Append(")");
            }
            else
            {
                sb.Append(Right);
            }
            return sb.ToString();
        }

        public override IList<string> GetVariableKeys()
        {
            IList<string> leftVariableKeys = Left.GetVariableKeys();
            IList<string> rightVariableKeys = Right.GetVariableKeys();
            return leftVariableKeys.Concat(rightVariableKeys).ToList();
        }

    }
}

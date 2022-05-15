﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLC.Expression
{
    public class SwitchExpression : Expression
    {
        public Expression Value { get; set; }
        public IList<Tuple<Expression, Expression>> Children { get; set; }

        public SwitchExpression(ExpressionType type, Expression value, IList<Tuple<Expression, Expression>> children) : base(type)
        {
            Value = value;
            Children = children;
        }

        public override ResultExpression Invoke(Parameters parameters)
        {
            var value = Value.Invoke(parameters);
            if (value == null)
            {
                return null;
            }
            foreach (var child in Children)
            {
                var item1 = child.Item1.Invoke(parameters);
                if (item1 == null)
                {
                    continue;
                }
                if (value.IsNumber() && item1.IsNumber() && ExpressionSetting.Instance.AreEquals(value.NumberResult, item1.NumberResult))
                {
                    return child.Item2.Invoke(parameters);
                }
                else if(value.IsNumber() && item1.IsRange() && RangeUtils.IsInRange(value.NumberResult, item1.Data.ToString()))
                {
                    return child.Item2.Invoke(parameters);
                }
                else if (Type == ExpressionType.Switch && string.Equals(value.ToString(), item1.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return child.Item2.Invoke(parameters);
                }
                else if (Type == ExpressionType.SwitchC && value.ToString().ToLower().Contains(item1.ToString().ToLower()))
                {
                    return child.Item2.Invoke(parameters);
                }
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Type == ExpressionType.Switch)
            {
                sb.Append("SWITCH(");
            }
            else if (Type == ExpressionType.SwitchC)
            {
                sb.Append("SWITCHC(");
            }
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
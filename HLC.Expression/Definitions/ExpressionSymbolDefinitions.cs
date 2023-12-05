using HLC.Expression.Definitions;
using HLC.Expression.Segments;
using System.Collections.Generic;

namespace HLC.Expression
{
    public static partial class ExpressionSymbolDefinitions
    {
        public static List<ExpressionSymbolDefinitionItem> Items { get; } = new List<ExpressionSymbolDefinitionItem>();

        static ExpressionSymbolDefinitions()
        {
            var segments = SegmentManager.GetAllBinarySegments();
            foreach (var segment in segments)
            {
                var definistion = segment.GetDefinistion();
                if (definistion != null)
                {
                    Items.Add(definistion);
                }
            }
        }
    }
}

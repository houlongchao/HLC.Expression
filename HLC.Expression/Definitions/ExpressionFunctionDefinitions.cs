﻿using HLC.Expression.Definitions;
using HLC.Expression.Segments;
using System.Collections.Generic;

namespace HLC.Expression
{
    public static partial class ExpressionFunctionDefinitions
    {
        public static List<ExpressionFunctionDefinitionItem> Items { get; } = new List<ExpressionFunctionDefinitionItem>();

        static ExpressionFunctionDefinitions()
        {
            var functionSegments = SegmentManager.GetAllFunctionSegments();
            foreach (var functionSegment in functionSegments)
            {
                var definition = functionSegment.GetDefinition();
                if (definition != null)
                {
                    Items.Add(definition);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HLC.Expression.Segments
{
    public static class SegmentManager
    {

        static SegmentManager()
        {
            AddSystemFunctionSegments();
            AddSystemBinarySegments();
        }

        #region FunctionSegment

        private static List<FunctionSegment> _functionSegments = new List<FunctionSegment>();
        private static Dictionary<Operator, FunctionSegment> _functionSegmentOpDict = new Dictionary<Operator, FunctionSegment>();
        private static Dictionary<ExpressionType, FunctionSegment> _functionSegmentExpressionTypeDict = new Dictionary<ExpressionType, FunctionSegment>();

        public static List<FunctionSegment> GetAllFunctionSegments()
        {
            return _functionSegments;
        }

        public static FunctionSegment GetFunctionSegment(Operator op)
        {
            _functionSegmentOpDict.TryGetValue(op, out FunctionSegment functionSegment);
            return functionSegment;
        }

        public static FunctionSegment GetFunctionSegment(ExpressionType type)
        {
            _functionSegmentExpressionTypeDict.TryGetValue(type, out FunctionSegment functionSegment);
            return functionSegment;
        }



        public static void AddFunctionSegment(FunctionSegment functionSegment)
        {
            _functionSegments.Add(functionSegment);
            _functionSegmentOpDict[functionSegment.MatchOperator] = functionSegment;
            _functionSegmentExpressionTypeDict[functionSegment.ExpressionType] = functionSegment;
        }

        private static void AddSystemFunctionSegments()
        {
            var functionSegments = Assembly.GetCallingAssembly().GetTypes().Where(t => t.IsClass && t.IsPublic && !t.IsAbstract && typeof(FunctionSegment).IsAssignableFrom(t));
            foreach (var item in functionSegments)
            {
                AddFunctionSegment(Activator.CreateInstance(item) as FunctionSegment);
            }
        }

        #endregion

        #region BinarySegment

        private static List<BinarySegment> _binarySegments = new List<BinarySegment>();
        private static Dictionary<Operator, BinarySegment> _binarySegmentOpDict = new Dictionary<Operator, BinarySegment>();
        private static Dictionary<ExpressionType, BinarySegment> _binarySegmentExpressionTypeDict = new Dictionary<ExpressionType, BinarySegment>();

        public static List<BinarySegment> GetAllBinarySegments()
        {
            return _binarySegments;
        }

        public static BinarySegment GetBinarySegment(Operator op)
        {
            _binarySegmentOpDict.TryGetValue(op, out BinarySegment binarySegment);
            return binarySegment;
        }

        public static BinarySegment GetBinarySegment(ExpressionType type)
        {
            _binarySegmentExpressionTypeDict.TryGetValue(type, out BinarySegment binarySegment);
            return binarySegment;
        }



        public static void AddBinarySegment(BinarySegment binarySegment)
        {
            _binarySegments.Add(binarySegment);
            _binarySegmentOpDict[binarySegment.MatchOperator] = binarySegment;
            _binarySegmentExpressionTypeDict[binarySegment.ExpressionType] = binarySegment;
        }

        private static void AddSystemBinarySegments()
        {
            var binarySegments = Assembly.GetCallingAssembly().GetTypes().Where(t => t.IsClass && t.IsPublic && !t.IsAbstract && typeof(BinarySegment).IsAssignableFrom(t));
            foreach (var item in binarySegments)
            {
                AddBinarySegment(Activator.CreateInstance(item) as BinarySegment);
            }
        }

        #endregion
    }
}

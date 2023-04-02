using System;
using System.Text.RegularExpressions;

namespace HLC.Expression
{
    public class RangeUtils
    {
        private const string RangePattern = "^(?<leftBacket>[\\(\\[])(?<leftNum>-?\\d+(\\.\\d+)?) ?, ?(?<rightNum>-?\\d+(\\.\\d+)?)(?<rightBacket>[\\)\\]])$";

        /// <summary>
        /// 判断给定的字符串是否为一个区间字符串
        /// </summary>
        /// <param name="range">区间字符串 (0.00,0.00) [0.00,0.00) (0.00,0.00] [0.00,0.00]</param>
        /// <returns></returns>
        public static bool IsRange(string range)
        {
            var match = Regex.Match(range, RangePattern);
            if (match.Success && match.Groups["leftBacket"].Success && match.Groups["leftNum"].Success && match.Groups["rightNum"].Success && match.Groups["rightBacket"].Success)
            {
                return Convert.ToDouble(match.Groups["leftNum"].Value) < Convert.ToDouble(match.Groups["rightNum"].Value);
            }
            return false;
        }

        /// <summary>
        /// 判断给定的数字是否在制定区间中
        /// </summary>
        /// <param name="num">需要判断的数字</param>
        /// <param name="range">区间字符串 (0.00,0.00) [0.00,0.00) (0.00,0.00] [0.00,0.00]</param>
        /// <returns></returns>
        public static bool IsInRange(double num, string range)
        {
            var match = Regex.Match(range, RangePattern);
            if (match.Success && match.Groups["leftBacket"].Success && match.Groups["leftNum"].Success && match.Groups["rightNum"].Success && match.Groups["rightBacket"].Success)
            {
                if (range.StartsWith("(") && Convert.ToDouble(match.Groups["leftNum"].Value) >= num)
                {
                    return false;
                }
                if (range.StartsWith("[") && Convert.ToDouble(match.Groups["leftNum"].Value) > num)
                {
                    return false;
                }
                if (range.EndsWith(")") && num >= Convert.ToDouble(match.Groups["rightNum"].Value))
                {
                    return false;
                }
                if (range.EndsWith("]") && num > Convert.ToDouble(match.Groups["rightNum"].Value))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断给定的数字是否在制定区间中
        /// </summary>
        public static bool IsInRange(decimal num, string range)
        {
            return IsInRange((double)num, range);
        }
    }
}

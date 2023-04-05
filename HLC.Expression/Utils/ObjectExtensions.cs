using System;

namespace HLC.Expression
{
    public static class ObjectExtensions
    {

        #region ToDecimal

        /// <summary>
        /// 将对象转换成<see cref="decimal"/>类型
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object obj)
        {
            if (obj is decimal num)
            {
                return num;
            }

            return decimal.Parse(obj.ToString());
        }

        /// <summary>
        /// 将对象转换成<see cref="decimal"/>类型
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object obj, decimal defaultValue)
        {
            return obj.ToDecimalNullable() ?? defaultValue;
        }

        /// <summary>
        /// 将对象转换成<see cref="Nullable{Decimal}"/>类型
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns></returns>
        public static decimal? ToDecimalNullable(this object obj)
        {
            if (null == obj)
            {
                return null;
            }

            if (obj is decimal num)
            {
                return num;
            }

            if (decimal.TryParse(obj.ToString(), out var d))
            {
                return d;
            }
            return null;
        }

        #endregion ToDouble

    }
}

using System.Collections.Generic;

namespace HLC.Expression
{
    /// <summary>
    /// 元数据
    /// </summary>
    public class Metadata
    {
        private readonly Dictionary<string, string> _dict = new Dictionary<string, string>();

        public string this[string key]
        {
            get
            {
                return _dict[key];
            }
            set
            {
                _dict[key] = value;
            }
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out string value)
        {
            return _dict.TryGetValue(key, out value);
        }
    }

    /// <summary>
    /// 数据元数据
    /// </summary>
    public class DataMetadata
    {
        private readonly Dictionary<string, string> _dict = new Dictionary<string, string>();

        public string this[string data, string key]
        {
            get
            {
                return _dict[$"{data}#{key}"];
            }
            set
            {
                _dict[$"{data}#{key}"] = value;
            }
        }

        /// <summary>
        /// 尝试获取数据值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetDataValue(string data, string key, out string value)
        {
            return _dict.TryGetValue($"{data}#{key}", out value);
        }
    }
}

using System;
using System.Collections.Generic;

namespace HLC.Expression
{
    /// <summary>
    /// 参数字典
    /// </summary>
    public class Parameters : Dictionary<string, Parameter>
    {
        /// <summary>
        /// 自定义函数存储字典
        /// </summary>
        private readonly Dictionary<string, Func<List<string>, string>> _callFunctions = new Dictionary<string, Func<List<string>, string>>();

        /// <summary>
        /// 注册自定义函数
        /// </summary>
        /// <param name="funcName">函数名称，不区分大小写</param>
        /// <param name="func">函数定义，入参为字符串列表，输出为字符串</param>
        public void RegisterCallFunc(string funcName, Func<List<string>, string> func)
        {
            _callFunctions[funcName.ToLower()] = func;
        }

        /// <summary>
        /// 获取自定义函数
        /// </summary>
        /// <param name="funcName">函数名称，不区分大小写</param>
        /// <returns></returns>
        public Func<List<string>, string> GetCallFunc(string funcName)
        {
            if (_callFunctions.TryGetValue(funcName.ToLower(), out var func))
            {
                return func;
            }
            return null;
        }
    }
}
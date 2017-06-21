using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerControlDemo.Helper
{
    public class ConverterHelper
    {
        /// <summary>
        /// 将object对象转换为Json数据
        /// </summary>
        /// <param name="obj">object对象</param>
        /// <returns>转换后的json字符串</returns>
        public static string ObjectToJson(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将Json对象转换为T对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="jsonString">json对象字符串</param>
        /// <returns>由字符串转换得到的T对象</returns>
        public static T JsonToObject<T>(string jsonString)
        {
            if (String.IsNullOrEmpty(jsonString))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
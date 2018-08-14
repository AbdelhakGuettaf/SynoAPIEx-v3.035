using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TM.Library
{
    public static class IntExtensions
    {
        /// <summary>
        /// 拓展 Int转枚举
        /// </summary>
        /// <returns></returns>
        public static T ToEnum<T>(this int val)
        {
            return (T)Enum.ToObject(typeof(T), val);
        }

        /// <summary>
        /// 拓展 获取Int类型转的枚举的描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string ToEnumDescriptionString(this int value, Type enumType)
        {
            //NameValueCollection nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        if (strValue == value.ToString())
                        {
                            return aa.Description;
                        }
                    }
                    //nvc.Add(strValue, strText);
                }
            }
            //return nvc;
            return "";
        }
    }
}

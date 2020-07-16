using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace UserManagement.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Usage: (int).ToEnum<EnumType>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int code) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), code);
        }

        public static string GetDisplayName(this Enum enumType)
        {
            return enumType.GetType()
                        .GetMember(enumType.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .Name;
        }
    }
}

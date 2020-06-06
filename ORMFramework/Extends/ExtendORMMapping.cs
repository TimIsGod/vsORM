using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace 手写ORM.ORMFramework.Extends
{
    public static class ExtendORMMapping
    {
        /**
         * 特性扩展方法需要使用反射类的MemberInfo，用于获取成员属性的信息且提供对成员元数据的访问权限
         */ 
        public  static string GetMappingName<T>(this T t) where T:MemberInfo
        {
            //判断当前对象是否使用了ORMBaseAttribute声明
            if (t.IsDefined(typeof(ORMBaseAttribute),true))
            {
                var attribute = t.GetCustomAttribute<ORMBaseAttribute>();
                return attribute.GetMappingName();
            }
            else
            {
                return t.Name;
            }
        }


    }
}

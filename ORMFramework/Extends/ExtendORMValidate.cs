using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace 手写ORM.ORMFramework.Extends
{
    // 扩展类必须静态声明
    public static class ExtendORMValidate
    {
        public static bool ValidateModel<T>(this T t) //where T:EntityBase,new() 这里会造成循环依赖
        {
            Type type = typeof(T);

            var properties = type.GetProperties();

            foreach (var item in properties)
            {
                if (item.IsDefined(typeof(ORMBaseValidateAttribute),true))
                {
                    //获取当前属性上所有验证特性声明
                    var validateAttributes = item.GetCustomAttributes<ORMBaseValidateAttribute>();
                    object o = item.GetValue(t);
                    foreach (var a in validateAttributes)
                    {
                       if(!a.Validate(o))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}

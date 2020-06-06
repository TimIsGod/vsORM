using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using 手写ORM.ORMFramework.Attributes;

namespace 手写ORM.ORMFramework.Extends
{
    public static class ExtendORMFilter
    {
        //这里必须要返回可遍历类型对象，以便返回结果可用Linq操作
        public static IEnumerable<PropertyInfo> GetPropertiesWithNotPrimayKey(this Type t)
        {
            return t.GetProperties().Where(r => !r.IsDefined(typeof(ORMPrimayKeyAttribute), true));
        }
    }
}

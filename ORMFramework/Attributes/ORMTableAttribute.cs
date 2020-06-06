using System;
using System.Collections.Generic;
using System.Text;

namespace 手写ORM.ORMFramework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ORMTableAttribute:ORMBaseAttribute
    {
        /**
         * 使用父类的方法进行传递
         */
        public ORMTableAttribute(string tableName) : base(tableName) { }
    }
}

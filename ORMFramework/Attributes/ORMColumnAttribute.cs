using System;
using System.Collections.Generic;
using System.Text;

namespace 手写ORM.ORMFramework
{
    //定义Attribute可声明的位置
    [AttributeUsage(AttributeTargets.Property)]
    public class ORMColumnAttribute :ORMBaseAttribute
    {
        /**
         * 使用父类的方法进行传递
         */
        public ORMColumnAttribute(string columnNmae) : base(columnNmae) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace 手写ORM.ORMFramework
{
    // 允许多用，允许继承
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =true,Inherited =true)]
    public abstract class ORMBaseValidateAttribute:Attribute
    {
        /// <summary>
        /// o 是需要验证的值
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public abstract bool Validate(object o);
    }
}

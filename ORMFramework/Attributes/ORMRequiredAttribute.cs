using System;
using System.Collections.Generic;
using System.Text;

namespace 手写ORM.ORMFramework.Attributes
{
    public class ORMRequiredAttribute:ORMBaseValidateAttribute
    {
        public override bool Validate(object o)
        {
            return o != null &&
                !string.IsNullOrEmpty(o.ToString().Trim());
        }
    }
}

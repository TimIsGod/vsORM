using System;
using System.Collections.Generic;
using System.Text;

namespace 手写ORM.ORMFramework
{
    public class ORMBaseAttribute:Attribute
    {
        private string _MappingName = null;

        public ORMBaseAttribute(string mappingName)
        {
            this._MappingName = mappingName;
        }

        public string GetMappingName()
        {
            return this._MappingName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 手写ORM.ORMFramework.Attributes;

namespace 手写ORM.Model
{
    public class EntityBase
    {
        [ORMPrimayKey]
        public long Id { get; set; }
    }
}

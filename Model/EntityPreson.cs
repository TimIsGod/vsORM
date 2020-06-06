using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 手写ORM.ORMFramework;
using 手写ORM.ORMFramework.Attributes;

namespace 手写ORM.Model
{
    [ORMTable("Preson")]
    public class EntityPreson:EntityBase
    {
        [ORMRequired]
        public string Name { get; set; }
        [ORMDoubleRangeValidate(1,80)]
        public int Age { get; set; }

        public string City { get; set; }

        public string Gender { get ; set ; }

        public DateTime Brithday { get; set; }

    }
}

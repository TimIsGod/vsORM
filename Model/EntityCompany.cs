using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 手写ORM.ORMFramework;
using 手写ORM.ORMFramework.Attributes;

namespace 手写ORM.Model
{
    [ORMTable("Company")]
    public class EntityCompany:EntityBase
    {
        [ORMRequired]
        public string Name { get; set; }
        [ORMRequired]
        public string Address { get; set; }
        [ORMRequired]
        public string Number { get; set; }
        [ORMColumn("CreatedTime")]
        public DateTime CreatedDate { get; set; }
    }
}

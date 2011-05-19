using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glass.Sitecore.Mapper.Demo.Application.Domain
{
    public class Company : Address
    {
        public virtual string Name { get; set; }
        public virtual IEnumerable<Employee> Employees { get; set; }
    }
}

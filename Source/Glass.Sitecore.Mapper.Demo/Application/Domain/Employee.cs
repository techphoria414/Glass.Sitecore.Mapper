using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Sitecore.Mapper.FieldTypes;

namespace Glass.Sitecore.Mapper.Demo.Application.Domain
{
    public class Employee : Address
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Image Photo { get; set; }
    }
}

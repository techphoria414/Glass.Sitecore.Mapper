using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Demo.Application.Domain
{
    public class Address
    {
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Town { get; set; }
        public virtual string Postcode { get; set; }
    }

    [SitecoreClass]
    public class SitecoreClass
    {

        [SitecoreField]
        public virtual string Property { get; set; }

        [SitecoreField]
        public virtual int Cost { get; set; }
    }
}

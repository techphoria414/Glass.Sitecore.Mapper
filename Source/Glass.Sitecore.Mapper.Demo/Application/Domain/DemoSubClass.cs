using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Demo.Application.Domain
{
    [SitecoreClass]
    public class DemoSubClass
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }

        [SitecoreField]
        public virtual string Body { get; set; }
    }
}
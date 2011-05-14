using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Sitecore.Persistence.Configuration.Attributes;

namespace Glass.Sitecore.Persistence.Demo.Application.Domain
{
    [SitecoreClass]
    public class DemoClass
    {

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }

        [SitecoreField]
        public virtual string Title { get; set; }

        [SitecoreField]
        public virtual string Body { get; set; }
        
        [SitecoreField]
        public virtual IEnumerable<DemoClass> Links { get; set; }


    }
}

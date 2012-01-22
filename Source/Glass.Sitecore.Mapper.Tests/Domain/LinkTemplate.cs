using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Tests.Domain
{
    [SitecoreClass]
    public class LinkTemplate
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Dashboard.Model.Data
{
    [SitecoreClass]
    public class ItemData
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }

        [SitecoreInfo(SitecoreInfoType.FullPath)]
        public virtual string Path { get; set; }
    }
}
